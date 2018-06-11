using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    // STATS
    public enum BaseStatTypes { HP, SP, ATT, DEF, MAG, RES, SPD, MOV, JMP };
    public enum WeaponType { Fists, Sword, Staff };
    public enum JobType { Monk };

    [Header("Statistics")]
    public int hitPoints = 0;
    public int specialPoints = 0;

    public int attack = 0;
    public int magic = 0;

    public int defence = 0;
    public int resistance = 0;

    public int speed = 0;
    public int movement = 3;
    public int jump = 1;

    public int range = 1;

    public bool readyToAttack = true;

    [Header("Current")]
    public int currentHitPoints = 0;
    public int currentSpecialPoints = 0;
    public int currentMovement = 3;

    public ClassLibrary.JobClass Class = null;
    WeaponType weapon = WeaponType.Fists;

    // EXP
    [Header("Other")]
    public int LEVEL = 0;
    public int EXPToNextLevel = 0;
    public int EXP = 0;

    // OTHER
    public Sprite charIcon = null;

    // RIP
    public bool DEAD = false;

    public CharacterStats() { }

    public void ChangeBaseStatValue(BaseStatTypes type, int amount)
    {
        if (type == BaseStatTypes.HP)
            hitPoints += amount;

        else if (type == BaseStatTypes.SP)
            specialPoints += amount;

        else if (type == BaseStatTypes.ATT)
            attack += amount;

        else if (type == BaseStatTypes.MAG)
            attack += amount;

        else if (type == BaseStatTypes.DEF)
            defence += amount;

        else if (type == BaseStatTypes.RES)
            resistance += amount;

        else if (type == BaseStatTypes.SPD)
            speed += amount;

        else if (type == BaseStatTypes.MOV)
            movement += amount;

        else if (type == BaseStatTypes.JMP)
            jump += amount;

        else
            return;
    }

    public int GetStat(BaseStatTypes type)
    {
        if (type == BaseStatTypes.HP)
            return hitPoints;

        else if (type == BaseStatTypes.SP)
            return specialPoints;

        else if (type == BaseStatTypes.ATT)
            return attack;

        else if (type == BaseStatTypes.DEF)
            return defence;

        else if (type == BaseStatTypes.MAG)
            return magic;

        else if (type == BaseStatTypes.RES)
            return resistance;

        else if (type == BaseStatTypes.SPD)
            return speed;

        else if (type == BaseStatTypes.MOV)
            return movement;

        else if (type == BaseStatTypes.JMP)
            return jump;

        else
            return 0;
    }
    //public static CharacterStats GetResource()
    //{
    //    return GameObject.Find("GlobalData").GetComponent<CharacterStats>();
    //}
}
