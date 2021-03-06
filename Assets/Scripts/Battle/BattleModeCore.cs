﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// BattleMode Core
// Hosts everything battle related, such as:
// BattleHUD    -  
// MapData      - 
// TurnTable    - 


public class BattleModeCore : MonoBehaviour
{
    /* 
    == GAMESTATE ==
    Initialize = Start of Battle, adds combatants to turn order, and into the fight.
    Game =
    End =

    == ROUNDSTATE ==
    Initialize =
    Action = 
    End = Used to clean everything and afterwards signal next turn.
    */
    public enum GameState { Initialize, Game, End }
    public enum RoundState { Initialize, Action, End }

    public enum Animations { Idle, Move, Attack, Death}


    // BOOLS
    public bool showingMovementOptions = false;
    public bool showingAttackOptions = false;
    public bool showingSpecialAttackOptions = false;

    // Start with initialize
    public GameState gameState = GameState.Initialize;
    public RoundState roundState = RoundState.Initialize;

    // Different gameobjects.
    public GameObject movementGlow = null;
    public GameObject attackGlow = null;
    public GameObject turnTag = null;
    GameObject activeTurnTag = null;

    public CharacterInfo leftCharInfo = null;
    public CharacterInfo rightCharInfo = null;

    public GameObject damageNumber = null;

    public BattleHUD battleHUD = null;
    public MapData mapData = null;
    public TurnTable turnTable = null;
    public BattleMovement bMovement = null;
    BattleCamera bCamera = null;
    SelectionData sData = null;

    //DEBUG
    public List<TileData> movementTiles = new List<TileData>();
    public List<TileData> attackingTiles = new List<TileData>();
    public List<TileData> specialattackTiles = new List<TileData>();
    public GameObject startBattleButton = null;
    public GameObject GiveClassButton = null;
    public GameObject DebugClone = null;

    // START //

    void OnEnable()
    {
        gameState = GameState.Initialize;
        mapData = MapData.GetResource();
        turnTable = TurnTable.GetResource();
        battleHUD = BattleHUD.GetResource();
        bMovement = BattleMovement.GetResource();
        sData = SelectionData.GetResource();

        //leftCharInfo = GameObject.Find("LInfo").GetComponent<CharacterInfo>();
        //rightCharInfo = GameObject.Find("RInfo").GetComponent<CharacterInfo>();

    }

    // UPDATE //

    void Update()
    {
        UpdateGameState();
    }

    void UpdateGameState()
    {
        //if (gameState == GameState.Initialize)
        //    InitializeBattle();

        if (gameState == GameState.Game)
        {
            BattleOptions();
            UpdateRoundState();
        }

    }

    void UpdateRoundState()
    {
        // Player
        if (turnTable.currentCharacterTurn.charType == BattleCharacter.CharacterType.Player)
        {
            if (roundState == RoundState.Initialize)
            {
                if (turnTable.currentCharacterTurn.stats != null)
                {
                    turnTable.currentCharacterTurn.stats.currentMovement = turnTable.currentCharacterTurn.stats.movement;
                    turnTable.currentCharacterTurn.stats.readyToAttack = true;

                    if(leftCharInfo.gameObject.activeInHierarchy != true)
                    leftCharInfo.gameObject.SetActive(true);

                    //leftCharInfo.UpdateCharInfo();
                    battleHUD.UpdateMovementText();
                    UpdateTurnTag();

                    if (turnTable.currentCharacterTurn.team != BattleCharacter.Team.PlayerForces)
                        BattleHUD.GetResource().ActivateBattleChoices(false);

                    else
                        BattleHUD.GetResource().ActivateBattleChoices(true);

                    //bCamera.SetCameraToCharacter(turnTable.currentCharacterTurn);

                    roundState = RoundState.Action;
                }
            }


            else if (roundState == RoundState.Action)
            {

                if (!showingMovementOptions && !bMovement.characterIsMoving)
                {
                    if (Input.GetButtonDown("Fire1"))
                    {
                        RaycastHit hit;
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.collider.GetComponent<TileData>().occupied == true)
                            {
                                rightCharInfo.gameObject.SetActive(true);

                                for (int i = 0; i < turnTable.characters.Count; i++)
                                {
                                    if (turnTable.characters[i].currentPos == hit.collider.GetComponent<TileData>())
                                    {
                                        rightCharInfo.UpdateStats(turnTable.characters[i]);
                                    }
                                }
                            }
                            else
                                rightCharInfo.gameObject.SetActive(false);
                        }
                    }
                }

                // While showing attack options, hovering over is enough.
                if (showingAttackOptions)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.GetComponent<TileData>() != null)
                        {
                            if (hit.collider.GetComponent<TileData>().occupied == true)
                            {
                                rightCharInfo.gameObject.SetActive(true);

                                for (int i = 0; i < turnTable.characters.Count; i++)
                                {
                                    if (turnTable.characters[i].currentPos == hit.collider.GetComponent<TileData>())
                                    {
                                       rightCharInfo.UpdateStats(turnTable.characters[i]);
                                    }
                                }
                            }
                        }
                    }
                }


            }

            else if (roundState == RoundState.End)
            {
                if (rightCharInfo.gameObject.activeInHierarchy)
                 rightCharInfo.gameObject.SetActive(false);

                if (showingMovementOptions)
                {
                    ClearMovement();
                    ClearTiles();
                }

                if (showingAttackOptions)
                    ClearAttack();

                turnTable.NextTurn();
                roundState = RoundState.Initialize;
            }
        }

        //AI
        else if (turnTable.currentCharacterTurn.charType == BattleCharacter.CharacterType.NPC)
        {
            BattleHUD.GetResource().ActivateBattleChoices(false);
            CombatAI AI = turnTable.currentCharacterTurn.transform.GetComponent<CombatAI>();

            if (roundState == RoundState.Initialize)
            {
                if (turnTable.currentCharacterTurn.stats != null)
                {
                    turnTable.currentCharacterTurn.stats.currentMovement = turnTable.currentCharacterTurn.stats.movement;
                    turnTable.currentCharacterTurn.stats.readyToAttack = true;

                    UpdateTurnTag();

                    AI.state = CombatAI.aiState.targeting;
                    roundState = RoundState.Action;
                }
            }

            else if (roundState == RoundState.Action)
            {
                // Simple AI
                // 1. Choose Target
                // 2. Movement
                // 3. Attacking
                if (AI.state == CombatAI.aiState.targeting)
                {
                    AI.ChooseTarget();

                    rightCharInfo.UpdateStats(AI.target);
                    rightCharInfo.gameObject.SetActive(true);
                }

                if (AI.state == CombatAI.aiState.moving)
                    AI.Movement();

                if (AI.state == CombatAI.aiState.attacking)
                    AI.Attack();

                if (AI.state == CombatAI.aiState.end)
                {
                    rightCharInfo.gameObject.SetActive(false);
                    roundState = RoundState.End;
                }

            }

            else if (roundState == RoundState.End)
            {
                turnTable.NextTurn();
                roundState = RoundState.Initialize;
            }
        }
    }

    public void EndTurn()
    {
        roundState = RoundState.End;
    }

    // Initialize map & characters
    public void InitializeBattle()
    {


        for (int i = 0; i < mapData.characters.Count; i++)
        {
            // Move characters to closest tile
            float closestPos = Mathf.Infinity;
            TileData closestTile = new TileData();

            BattleCharacter currCharacter = mapData.characters[i];
            for (int j = 0; j < mapData.mapTiles.Count; j++)
            {
                TileData currTile = mapData.mapTiles[j];
                float dist = Vector2.Distance(new Vector2(currCharacter.transform.localPosition.x / 2, currCharacter.transform.localPosition.z / 2), new Vector2(currTile.x, currTile.z));

                if (dist < closestPos && !currTile.unpassable && !currTile.occupied)
                {
                    closestPos = dist;
                    closestTile = currTile;
                }
            }
            currCharacter.currentPos = closestTile;
            TileData currentPos = mapData.characters[i].currentPos;
            currCharacter.stats = mapData.characters[i].GetComponent<CharacterStats>();
            currCharacter.transform.localPosition = new Vector3(currentPos.x * 2, currentPos.height + 1, currentPos.z * 2);
            mapData.UpdateOccupation();

        }
        mapData.UpdateOccupation();
        turnTable.InitializeTable();
        battleHUD.UpdateMovementText();
        

        activeTurnTag = Instantiate(turnTag);

        gameState = GameState.Game;
    }

    void BattleOptions()
    {

    }

    // HUD SCRIPTS //

    // Called when clicking ATTACK on HUD.
    public void OnAttackClick()
    {
        if (showingMovementOptions)
        {
            ClearMovement();
            ClearTiles();
        }

        if (showingSpecialAttackOptions)
            ClearAttack();

        if (turnTable.currentCharacterTurn.stats.readyToAttack == true)
        {
            attackingTiles = BattleAttacking.ShowAttackOptions();

            // Glow for attackTiles
            if (!showingAttackOptions)
            {
                for (int i = 0; i < attackingTiles.Count; i++)
                {
                    GameObject instance = Instantiate(attackGlow);
                    instance.transform.SetParent(GameObject.Find("Glows").transform);
                    instance.transform.localPosition = attackingTiles[i].transform.localPosition + new Vector3(0f, 0.1f, 0f);
                }
                showingAttackOptions = true;
            }

            else
            {
                ClearAttack();
            }
        }

        else
        {
            Debug.Log("Out of Attacks!");
            // OUT OF ATTACKS
        }
    }

    // -
    public void OnSpecialAttackClick()
    {
        SkillLibrary.Skill skill = turnTable.currentCharacterTurn.stats.Class.skills[sData.selectedSkillIndex].skill;

        // Not enough SP
        if (turnTable.currentCharacterTurn.stats.currentSpecialPoints < skill.spCost || turnTable.currentCharacterTurn.stats.readyToAttack == false)
        {
            Debug.Log("Not enough Special Points or Attacks");
            return;
        }

        if (showingMovementOptions)
        {
            ClearTiles();
            ClearMovement();
        }

        specialattackTiles = BattleAttacking.ShowAttackOptions(skill);

        // Glow for attackTiles
        if (!showingSpecialAttackOptions)
        {
            for (int i = 0; i < specialattackTiles.Count; i++)
            {
                GameObject instance = Instantiate(attackGlow);
                instance.transform.SetParent(GameObject.Find("Glows").transform);
                instance.transform.localPosition = specialattackTiles[i].transform.localPosition + new Vector3(0f, 0.1f, 0f);
            }
            showingSpecialAttackOptions = true;
        }

        else
        {
            ClearAttack();
        }

    }

    public void OnMovementClick()
    {
        if (showingAttackOptions || showingSpecialAttackOptions)
            ClearAttack();

        movementTiles = bMovement.ShowMovementOptions();

        // Glow for the movementOption tiles.
        if (!showingMovementOptions)
        {
            for (int i = 0; i < movementTiles.Count; i++)
            {
                GameObject instance = Instantiate(movementGlow);
                instance.transform.SetParent(GameObject.Find("Glows").transform);
                instance.transform.localPosition = movementTiles[i].transform.localPosition + new Vector3(0f, 0.1f, 0f);
                movementTiles[i].glow = instance;
            }
            showingMovementOptions = true;
        }

        else
        {
            ClearMovement();
            ClearTiles();
        }
    }



    // HELPERS //

    public void ClearMovement()
    {
        // Tile Tags
        for (int i = 0; i < mapData.mapTiles.Count; i++)
        {
            if (mapData.mapTiles[i].tag == "Moveable")
            {
                mapData.mapTiles[i].tag = "Untagged";
            }
        }

        foreach (Transform child in GameObject.Find("Glows").transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        showingMovementOptions = false;
    }


    public void ClearTiles()
    {
        BattleModeCore bCore = BattleModeCore.GetResource();

        for (int j = 0; j < bCore.movementTiles.Count; j++)
        {
            bCore.movementTiles[j].movementPath.Clear();
        }

        bCore.movementTiles.Clear();
    }

    public void ClearAttack()
    {
        // Tile Tags
        for (int i = 0; i < mapData.mapTiles.Count; i++)
        {
            if (mapData.mapTiles[i].tag == "Attackable")
                mapData.mapTiles[i].tag = "Untagged";
        }

        foreach (Transform child in GameObject.Find("Glows").transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        BattleModeCore.GetResource().attackingTiles.Clear();
        showingAttackOptions = false;
        showingSpecialAttackOptions = false;
    }
    public void UpdateTurnTag()
    {
        activeTurnTag.transform.parent = turnTable.currentCharacterTurn.transform;
        activeTurnTag.transform.localPosition = new Vector3(0f, 6f, 0f);
    }

    // Play an animation & stop playing every other animation. Good for animations that can potentially loop.
    public void PlayAnimation(BattleCharacter character, Animations animation)
    {
        if (character.GetComponent<Animator>() == null)
        {
            Debug.Log(character.gameObject.name + " has no animator!");
            return;
        }

        Animator a = character.GetComponent<Animator>();

        foreach(Animations val in System.Enum.GetValues(typeof(Animations))) 
        {
            if (val != animation)
                a.SetBool(val.ToString(), false);

            else
                a.SetBool(val.ToString(), true);
        }

    }

    // Triggers are used when you want to play an animation once and then return back to the last animation. Good for ex. Attack animation.
    public void PlayAnimationTrigger(BattleCharacter character, Animations animationTrigger)
    {
        if (character.GetComponent<Animator>() == null)
        {
            Debug.Log(character.gameObject.name + " has no animator!");
            return;
        }

        Animator a = character.GetComponent<Animator>();

        foreach (Animations val in System.Enum.GetValues(typeof(Animations)))
        {
            if (val == animationTrigger)
                a.SetTrigger(val.ToString());
        }
    }

    // DEBUG //

    public void StartBattle()
    {
        //Destroy(startBattleButton);
        GameCore.GetResource().StartBattleMode();
    }

    public void SpawnEnemy()
    {
        GameObject enemy = Instantiate(DebugClone) as GameObject;
        enemy.transform.SetParent(GameObject.Find("Map").transform);
        enemy.transform.localPosition = new Vector3(-6f, 2f, 0f);
        mapData.characters.Add(enemy.GetComponent<BattleCharacter>());
    }

    public void GivePlayerClass(BattleCharacter character)
    {
        Destroy(GiveClassButton);
        character.stats.Class = ClassLibrary.GetResource().JobClasses[0];
    }

    public void UpdateDeaths()
    {
        for(int i = 0; i < mapData.characters.Count; i++)
        {

            if (mapData.characters[i].stats.currentHitPoints <= 0)
            {
                // This character has died
                mapData.characters[i].stats.DEAD = true;
                PlayAnimationTrigger(mapData.characters[i], Animations.Death);
                StartCoroutine("PlayDeathAnimation", mapData.characters[i]);
            }

            else
                mapData.characters[i].stats.DEAD = false;
        }

        //Check if everyone from either of the teams are dead.

        //EnemyForces
        List<BattleCharacter> teamList = new List<BattleCharacter>();
        for(int j = 0; j < turnTable.characters.Count; j++)
        {
            if(turnTable.characters[j].team == BattleCharacter.Team.EnemyForces)
                if(turnTable.characters[j].stats.DEAD == false)
                {
                    teamList.Add(turnTable.characters[j]);
                }
        }

        if(teamList.Count == 0)
        {
            // All enemies dead!
            Debug.Log("All enemies are dead.");
            ClearBattleMode();
            GameCore.GetResource().StartFreeRoam();
            return;
        }

        teamList.Clear();

        //PlayerForces
        for (int j = 0; j < turnTable.characters.Count; j++)
        {
            if (turnTable.characters[j].team == BattleCharacter.Team.PlayerForces)
                if (turnTable.characters[j].stats.DEAD == false)
                {
                    teamList.Add(turnTable.characters[j]);
                }
        }

        if (teamList.Count == 0)
        {
            // All players dead!
            ClearBattleMode();
            GameCore.GetResource().StartFreeRoam();
            Debug.Log("All playerforces are dead.");
        }
    }

    void ClearBattleMode()
    {
        gameState = GameState.Initialize;
        roundState = RoundState.Initialize;

        leftCharInfo.gameObject.SetActive(false);
        rightCharInfo.gameObject.SetActive(false);
    }

    IEnumerator PlayDeathAnimation(BattleCharacter character)
    {
        while (true)
        {
            if (character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                character.gameObject.SetActive(false);
                yield break;
            }
            yield return null;
        }
    }

    public static BattleModeCore GetResource()
    {
        return GameObject.Find("GlobalBattleData").GetComponent<BattleModeCore>();
    }
}
