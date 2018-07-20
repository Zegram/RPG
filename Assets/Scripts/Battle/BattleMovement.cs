using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stuff regarding movement & rotation during battles can be found here
public class BattleMovement : MonoBehaviour
{

    // DoubleClick
    float catchTime = 0.3f;
    float lastClickTime = 0f;
    BattleModeCore battleCore = null;
    int i = 0;
    public float MovementSpeed = 10f;

    public bool characterIsMoving = false;
    TileData tileToMove = new TileData();

    void Awake()
    {
        battleCore = BattleModeCore.GetResource();
    }

    public List<TileData> ShowMovementOptions()
    {
        BattleCharacter character = TurnTable.GetResource().currentCharacterTurn;
        List<TileData> avaibleTiles = new List<TileData>();


        if (character.stats.currentMovement <= 0)
            return null;

        int movement = 1;

        List<TileData> currTiles = new List<TileData>();
        currTiles.Add(character.currentPos);
        for (int i = 0; i < character.stats.currentMovement; i++)
        {
            for (int j = 0; j < currTiles.Count; j++)
            {
                for (int k = 0; k < currTiles[j].neighborTiles.Count; k++)
                {
                    TileData cTile = currTiles[j].neighborTiles[k];
                    if (!avaibleTiles.Contains(cTile) && cTile != character.currentPos && !cTile.unpassable && !cTile.occupied)
                    {

                        if (currTiles[j].height < cTile.height || currTiles[j].height > cTile.height)
                        {
                            if (cTile.height < currTiles[j].height)
                            {
                                if (currTiles[j].height - cTile.height <= character.stats.jump)
                                {
                                    currTiles[j].neighborTiles[k].movementCost = movement;
                                    currTiles[j].neighborTiles[k].tag = "Moveable";

                                    //Movement Path                                  
                                    // Add last tiles path
                                    // Add last tile
                                    // Add Itself                                 
                                    for (int x = 0; x < currTiles[j].movementPath.Count; x++)
                                        currTiles[j].neighborTiles[k].movementPath.Add(currTiles[j].movementPath[x]);

                                    currTiles[j].neighborTiles[k].movementPath.Add(currTiles[j].neighborTiles[k]);

                                    avaibleTiles.Add(currTiles[j].neighborTiles[k]);
                                }
                            }

                            else if (cTile.height > currTiles[j].height)
                            {
                                if (cTile.height - currTiles[j].height <= character.stats.jump)
                                {
                                    currTiles[j].neighborTiles[k].movementCost = movement;
                                    currTiles[j].neighborTiles[k].tag = "Moveable";

                                    //Movement Path

                                    for (int x = 0; x < currTiles[j].movementPath.Count; x++)
                                        currTiles[j].neighborTiles[k].movementPath.Add(currTiles[j].movementPath[x]);

                                    currTiles[j].neighborTiles[k].movementPath.Add(currTiles[j].neighborTiles[k]);

                                    avaibleTiles.Add(currTiles[j].neighborTiles[k]);
                                }
                            }
                        }
                        else
                        {
                            currTiles[j].neighborTiles[k].movementCost = movement;
                            currTiles[j].neighborTiles[k].tag = "Moveable";

                            //Movement Path                          
                            for (int x = 0; x < currTiles[j].movementPath.Count; x++)
                            {
                                currTiles[j].neighborTiles[k].movementPath.Add(currTiles[j].movementPath[x]);
                            }

                            currTiles[j].neighborTiles[k].movementPath.Add(currTiles[j].neighborTiles[k]);


                            avaibleTiles.Add(currTiles[j].neighborTiles[k]);
                        }
                    }
                }
            }
            for (int j = 0; j < avaibleTiles.Count; j++)
                currTiles.Add(avaibleTiles[j]);

            movement++;
        }

        return avaibleTiles;
    }

    void Update()
    {
        if (battleCore.showingMovementOptions)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Hovering over with mouse.
            if (Physics.Raycast(ray, out hit))
            {

            }

            //Clicking with mouse
            if (Input.GetButtonDown("Fire1"))
            {
                // Doubleclick
                if (Time.time - lastClickTime < catchTime)
                {
                    if (hit.collider.tag == "Moveable")
                    {
                        MoveCharacterTo(hit.transform.GetComponent<TileData>());
                    }
                }
                // Singleclick
                else
                {
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.tag == "Moveable")
                        {
                            List<TileData> tilePath = hit.transform.GetComponent<TileData>().movementPath;

                            for (int j = 0; j < battleCore.movementTiles.Count; j++)
                            {
                                battleCore.movementTiles[j].glow.GetComponent<Renderer>().material.SetColor("_TintColor", Color.yellow);
                            }

                            for (int i = 0; i < tilePath.Count; i++)
                            {
                                // Change Color
                                tilePath[i].glow.GetComponent<Renderer>().material.SetColor("_TintColor", Color.red);
                            }
                        }
                    }
                }
                lastClickTime = Time.time;
            }
        }

    }


    public void MoveCharacterTo(TileData tileData)
    {
        BattleHUD.GetResource().ActivateBattleChoices(false);
        tileToMove = new TileData(tileData);
        characterIsMoving = true;
        BattleCharacter currentCharacter = battleCore.turnTable.currentCharacterTurn;

        // Coroutine for movement
        StartCoroutine("MoveBetweenTiles", tileToMove);

        // Animation
        battleCore.PlayAnimation(currentCharacter, BattleModeCore.Animations.Move);

        // CLEAN
        battleCore.ClearMovement();

        // Reduce Movement
        currentCharacter.stats.currentMovement -= tileData.movementCost;

        // Set Characters current tile to tile we are moving to + Update occupations.
        currentCharacter.currentPos = tileData;
        battleCore.mapData.UpdateOccupation();

        // Update HUD while were at it
        battleCore.battleHUD.UpdateMovementText();
    }

    IEnumerator MoveBetweenTiles(TileData tileToMove)
    {
        bool readyToRotate = true;
        while (true)
        {
            //Transform currChar = battleCore.turnTable.currentCharacterTurn.transform;
            BattleCharacter currChar = battleCore.turnTable.currentCharacterTurn;
            float step = MovementSpeed * Time.deltaTime;

            if (readyToRotate)
            {
                // Rotation
                TileData currTile = battleCore.turnTable.currentCharacterTurn.currentPos;
                RotateCharacter(currChar, currTile, tileToMove.movementPath[i]);
                //int xDir = currTile.x.CompareTo(tileToMove.movementPath[i].x);
                //int zDir = currTile.z.CompareTo(tileToMove.movementPath[i].z);
                //float rotation = 0;

                //if (xDir == 0 && zDir > 0)
                //    rotation = 0;
                //else if (xDir > 0 && zDir == 0)
                //    rotation = 90;
                //else if (xDir == 0 && zDir < 0)
                //    rotation = 180;
                //else if (xDir < 0 && zDir == 0)
                //    rotation = 270;

                //currChar.localRotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                readyToRotate = false;
            }

            Vector3 target = new Vector3(tileToMove.movementPath[i].transform.localPosition.x, tileToMove.movementPath[i].transform.localPosition.y + 1, tileToMove.movementPath[i].transform.localPosition.z);
            currChar.transform.localPosition = Vector3.MoveTowards(currChar.transform.localPosition, target, step);

            //2
            if (currChar.transform.localPosition == target)
            {
                battleCore.turnTable.currentCharacterTurn.currentPos = tileToMove.movementPath[i];
                i++;             
                readyToRotate = true;

                // Reached the end
                if (i >= tileToMove.movementPath.Count)
                {

                    battleCore.ClearTiles();
                    characterIsMoving = false;
                    i = 0;

                    // Start playing idle again
                    battleCore.PlayAnimation(battleCore.turnTable.currentCharacterTurn, BattleModeCore.Animations.Idle);

                    if (battleCore.turnTable.currentCharacterTurn.charType == BattleCharacter.CharacterType.Player)
                        BattleHUD.GetResource().ActivateBattleChoices(true);

                    if (battleCore.turnTable.currentCharacterTurn.charType == BattleCharacter.CharacterType.NPC)
                        battleCore.turnTable.currentCharacterTurn.transform.GetComponent<CombatAI>().state = CombatAI.aiState.attacking;

                    yield break;
                }
            }

            yield return null;
        }
    }

    // Turns character in current tile to face the target tile.
    public void RotateCharacter(BattleCharacter character, TileData currTile, TileData targetTile)
    {
        Transform currChar = character.transform;

        int xDir = currTile.x.CompareTo(targetTile.x);
        int zDir = currTile.z.CompareTo(targetTile.z);
        float rotation = 0;

        if (xDir == 0 && zDir > 0)
            rotation = 0;
        else if (xDir > 0 && zDir == 0)
            rotation = 90;
        else if (xDir == 0 && zDir < 0)
            rotation = 180;
        else if (xDir < 0 && zDir == 0)
            rotation = 270;

        currChar.localRotation = Quaternion.Euler(0.0f, rotation, 0.0f);
    }

    public static BattleMovement GetResource()
    {
        return GameObject.Find("GlobalBattleData").GetComponent<BattleMovement>();
    }
}
