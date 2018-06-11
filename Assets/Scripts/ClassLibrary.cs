using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using System.Collections.Generic;

[System.Serializable]
public class ClassLibrary : ScriptableObject
{

    [System.Serializable]
    public class JobSkill
    {
        public SkillLibrary.Skill skill = null;
        public int unlockedAtLevel = 0;

        public JobSkill() { }

        public JobSkill(SkillLibrary.Skill skill)
        {
            this.skill = skill;
        }

        //Editor
        public bool showSkillStats = false;
    }

    [System.Serializable]
    public class JobClass
    {
        public string className = "New Class";
        public Sprite classIcon = null;
        //public int unlockRequirement = 200;
        //public bool unlocked = true;
        public int level = 0;

        public List<JobSkill> skills = new List<JobSkill>();

        public JobClass() { }

        // Editor
        [HideInInspector]
        public bool show = false;
        public bool showSkills = false;

    }


    static readonly string assetName = "ClassLibrary";
    static readonly string resourcePath = "Libraries/";
    static ClassLibrary instance = null;

    public static ClassLibrary GetResource()
    {

        if (instance == null)

            instance = Resources.Load<ClassLibrary>(resourcePath + assetName);

        return instance;

    }

    public List<JobClass> JobClasses = new List<JobClass>();
}
