using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillLibrary : ScriptableObject
{

    public enum skillType { Active, Passive };
    public enum passiveSkillType { StatIncrease, UnlockWeaponLicense };
    public enum activeSkillType { Attack, Heal, Buff, Debuff}

    [System.Serializable]
    public class PassiveSkill
    {
        //public VariableStats.BaseStatTypes statType;
        public int increaseAmount;
        //public VariableStats.WeaponLicenseTypes weaponType;
        public passiveSkillType passiveSkillType;
        public bool show = false;

        public PassiveSkill(passiveSkillType type) { passiveSkillType = type; }
        public PassiveSkill() { }
    }

    [System.Serializable]
    public class ActiveSkill
    {
        // All
        public activeSkillType activeSkillType = activeSkillType.Attack;


        //public bool restrictedToWeapon = false;
        //public VariableStats.WeaponLicenseTypes restrictedWeaponType = VariableStats.WeaponLicenseTypes.Sword;

        // Attack & Heal share this
        public int powerPercent = 0;
        public CharacterStats.BaseStatTypes statTypeModifier = CharacterStats.BaseStatTypes.ATT;
        public int hitAmount = 1;



        // Buff/Debuff skills
        public CharacterStats.BaseStatTypes statType = CharacterStats.BaseStatTypes.ATT;
        public int amount = 0;
        public int turnAmount = 0;

        // Editor
        [HideInInspector]
        public bool show = false;

        public ActiveSkill(activeSkillType type) { activeSkillType = type; }
        
    }


    [System.Serializable]
    public class Skill
    {
        public string name = "";
        public Sprite skillIcon = null;
        public string description = "";

        public bool unlocked = false;
        public int id = 0;
        public skillType type = skillType.Active;

        public int spCost = 0;
        public int range = 0;
        public bool OnlyToPoint = false;
        public bool FreeCastOnRange = false;

        // X & Z
        public List<Vector2> AoE = new List<Vector2>(); 

        public List<ActiveSkill> activeSkill = new List<ActiveSkill>();
        //public List<PassiveSkill> passiveSkill = new List<PassiveSkill>();

        public Skill() { }
        // Editor
        [HideInInspector]
        public bool show = false;
        public bool showAoE = false;
        public bool showSkillTypeInfo = false;
    }

    static readonly string assetName = "SkillLibrary";
    static readonly string resourcePath = "Libraries/";

    static SkillLibrary instance = null;
    public static SkillLibrary GetResource()
    {

        if (instance == null)

            instance = Resources.Load<SkillLibrary>(resourcePath + assetName);

        return instance;

    }

    public List<Skill> Skills = new List<Skill>();

}
