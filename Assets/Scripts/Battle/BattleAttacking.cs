using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAttacking : MonoBehaviour {

    BattleModeCore battleCore = null;
    SelectionData sData = null;

    void Awake()
    {
        battleCore = BattleModeCore.GetResource();
        sData = SelectionData.GetResource();
    }

    public static List<TileData> ShowAttackOptions()
    {
        BattleCharacter character = TurnTable.GetResource().currentCharacterTurn;
        List<TileData> avaibleTiles = new List<TileData>();
        MapData mapData = MapData.GetResource();

        //TileData currPos = character.currentPos;
        int CurrRange = 1;

        // STRAIGHT LINE ATTACKING
        for(int i = 0; i < character.stats.range; i++)
        {
            // -X || Z
            if (mapData.GetTile(character.currentPos.x - CurrRange, character.currentPos.z) != null)
                avaibleTiles.Add(mapData.GetTile(character.currentPos.x - CurrRange, character.currentPos.z));

            // +X || Z
            if (mapData.GetTile(character.currentPos.x + CurrRange, character.currentPos.z) != null)
                avaibleTiles.Add(mapData.GetTile(character.currentPos.x + CurrRange, character.currentPos.z));    
         
            // X || -Z
            if (mapData.GetTile(character.currentPos.x, character.currentPos.z - CurrRange) != null)
                avaibleTiles.Add(mapData.GetTile(character.currentPos.x, character.currentPos.z - CurrRange));

            // X || +Z
            if (mapData.GetTile(character.currentPos.x, character.currentPos.z + CurrRange) != null)
                avaibleTiles.Add(mapData.GetTile(character.currentPos.x, character.currentPos.z + CurrRange));

            CurrRange++;
        }

        for (int i = 0; i < avaibleTiles.Count; i++)
            avaibleTiles[i].tag = "Attackable";

        return avaibleTiles;
    }

    public static List<TileData> ShowAttackOptions(SkillLibrary.Skill skill)
    {
        TileData pos = TurnTable.GetResource().currentCharacterTurn.currentPos;
        List<TileData> avaibleTiles = new List<TileData>();
        MapData mapData = MapData.GetResource();

        if(skill.FreeCastOnRange)
        {
            // Since its freecast we need all tiles from every direction
            for(int i = 0; i < skill.range; i++)
            {
                // X Z
                for(int j = 0; j < skill.range; j++)
                {
                    for(int k = 0; k < skill.range; k++)
                    {
                        if (mapData.GetTile(pos.x + j, pos.z + k) != null)
                            avaibleTiles.Add(mapData.GetTile(pos.x + j, pos.z + k));
                    }
                }

                // X -Z
                for (int j = 0; j < skill.range; j++)
                {
                    for (int k = 0; k < skill.range; k++)
                    {
                        if (mapData.GetTile(pos.x + j,pos.z + -k) != null)
                            avaibleTiles.Add(mapData.GetTile(pos.x + j, pos.z + -k));
                    }
                }

                // -X Z
                for (int j = 0; j < skill.range; j++)
                {
                    for (int k = 0; k < skill.range; k++)
                    {
                        if (mapData.GetTile(pos.x + -j, pos.z + k) != null)
                            avaibleTiles.Add(mapData.GetTile(pos.x + -j, pos.z + k));
                    }
                }

                // -X -Z
                for (int j = 0; j < skill.range; j++)
                {
                    for (int k = 0; k < skill.range; k++)
                    {
                        if (mapData.GetTile(pos.x + -j, pos.z + -k) != null)
                            avaibleTiles.Add(mapData.GetTile(pos.x + -j, pos.z + -k));
                    }
                }
            }

        }

        for (int i = 0; i < avaibleTiles.Count; i++ )
            avaibleTiles[i].tag = "Attackable";

            return avaibleTiles;
    }

    void Update()
    {
        if (battleCore.showingAttackOptions)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Attackable")
                    {
                        TileData cTile = hit.transform.GetComponent<TileData>();
                        BattleCharacter target = null;
                        if (cTile.occupied)
                        {
                            for (int i = 0; i < battleCore.mapData.characters.Count; i++)
                            {
                                if (battleCore.mapData.characters[i].currentPos == cTile)
                                    target = battleCore.mapData.characters[i];
                            }

                            if (target != null)
                                AttackCharacter(target);
                        }
                    }
                }
            }
        }

            if (battleCore.showingSpecialAttackOptions)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.tag == "Attackable")
                        {
                            TileData cTile = hit.transform.GetComponent<TileData>();
                            BattleCharacter target = null;
                            if (cTile.occupied)
                            {
                                for (int i = 0; i < battleCore.mapData.characters.Count; i++)
                                {
                                    if (battleCore.mapData.characters[i].currentPos == cTile)
                                        target = battleCore.mapData.characters[i];
                                }

                                if (target != null)
                                    SpecialAttackCharacter(target);
                            }
                        }
                    }
                }
            }
        }

    void AttackCharacter(BattleCharacter target)
    {
        // CLEAN
        battleCore.showingAttackOptions = false;
        battleCore.ClearAttack();

        // Reduce Attack Times
        battleCore.turnTable.currentCharacterTurn.stats.readyToAttack = false;

        // 1337 damage calculations (TODO: make better)
        target.stats.currentHitPoints -= battleCore.turnTable.currentCharacterTurn.stats.attack;

        DisplayStringOnTarget(battleCore.turnTable.currentCharacterTurn.stats.attack.ToString(), target, Color.red);

        //GameObject instance = Instantiate(battleCore.damageNumber);
        //instance.GetComponent<TextMesh>().text = battleCore.turnTable.currentCharacterTurn.stats.attack.ToString();
        //instance.transform.SetParent(GameObject.Find("Map").transform);
        //instance.transform.localPosition = target.transform.localPosition + new Vector3(0f, 2.0f, 0f);

        // Update turntable after every attack. (see if dead)
        battleCore.turnTable.UpdateTable();
        //battleCore.leftCharInfo.UpdateLeftCharInfo();

    }

    void SpecialAttackCharacter(BattleCharacter target)
    {
        // CLEAN
        battleCore.showingSpecialAttackOptions = false;
        battleCore.ClearAttack();

        // Reduce Attack Times
        battleCore.turnTable.currentCharacterTurn.stats.readyToAttack = false;

        // 1337 damage calculations (TODO: make better)
        SkillLibrary.Skill cSkill = battleCore.turnTable.currentCharacterTurn.stats.Class.skills[sData.selectedSkillIndex].skill;
        int totalDamage = 0;
        int totalHeal = 0;

        // All active effects.
        for(int i = 0; i < cSkill.activeSkill.Count; i++)
        {
            if (cSkill.activeSkill[i].activeSkillType == SkillLibrary.activeSkillType.Attack)
            {
                // Active skills hitamount
                for (int j = 0; j < cSkill.activeSkill[i].hitAmount; j++)
                {
                    totalDamage += (cSkill.activeSkill[i].powerPercent / 100) * battleCore.turnTable.currentCharacterTurn.stats.GetStat(cSkill.activeSkill[i].statTypeModifier);

                    DisplayStringOnTarget(((cSkill.activeSkill[i].powerPercent / 100) * battleCore.turnTable.currentCharacterTurn.stats.GetStat(cSkill.activeSkill[i].statTypeModifier)).ToString(), target, Color.red);
                }
            }

            else if(cSkill.activeSkill[i].activeSkillType == SkillLibrary.activeSkillType.Heal)
            {
                // Active skills hitamount
                for (int j = 0; j < cSkill.activeSkill[i].hitAmount; j++)
                {
                    totalHeal += (cSkill.activeSkill[i].powerPercent / 100) * battleCore.turnTable.currentCharacterTurn.stats.GetStat(cSkill.activeSkill[i].statTypeModifier);

                        DisplayStringOnTarget(((cSkill.activeSkill[i].powerPercent / 100) * battleCore.turnTable.currentCharacterTurn.stats.GetStat(cSkill.activeSkill[i].statTypeModifier)).ToString(), target, Color.green);
                }
            }

            else if(cSkill.activeSkill[i].activeSkillType == SkillLibrary.activeSkillType.Buff)
            {

            }

            else if (cSkill.activeSkill[i].activeSkillType == SkillLibrary.activeSkillType.Debuff)
            {

            }


            

        }

        battleCore.turnTable.currentCharacterTurn.stats.currentSpecialPoints -= cSkill.spCost;

        if(totalDamage != 0)
        target.stats.currentHitPoints -= totalDamage;

        if (totalHeal != 0)
        {
            target.stats.currentHitPoints += totalHeal;
            if (target.stats.currentHitPoints > target.stats.hitPoints)
                target.stats.currentHitPoints = target.stats.hitPoints;
        }

        battleCore.leftCharInfo.UpdateLeftCharInfo();
    }

    void DisplayStringOnTarget(string text, BattleCharacter target, Color textColor)
    {
        GameObject instance = Instantiate(battleCore.damageNumber);
        instance.GetComponent<TextMesh>().text = text;
        instance.GetComponent<TextMesh>().color = textColor;
        instance.transform.SetParent(GameObject.Find("Map").transform);
        instance.transform.localPosition = target.transform.localPosition + new Vector3(0f, 2.0f, 0f);
    }
}
