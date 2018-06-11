//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class SkillTree : MonoBehaviour
//{

//    SelectionData sData = null;
//    ClassLibrary cLibrary = null;
//    CharacterStats cStats = null;
//    public Toggle stats, inventory, class1, class2 = null;

//    [Header("General")]
//    public bool preview = true;
//    public Text className = null;

//    [Header("Skill Tiers")]
//    public List<GameObject> tierSkills = new List<GameObject>();

//    [Header("Description")]
//    public Text skillName = null;
//    public Text description = null;
//    public Image image = null;
//    public GameObject unlockButton = null;
//    public Text expNeeded = null;

//    void Awake()
//    {
//        sData = SelectionData.GetResource();
//        cLibrary = ClassLibrary.GetResource();
//        cStats = CharacterStats.GetResource();
        
//    }


//    void OnEnable()
//    {

//        if (preview)
//            PreviewClassSkills();

//        else
//            ShowOwnedClassSkills();
//    }

//    void ShowOwnedClassSkills()
//    {
//        UpdateOwnedSkills();
//    }

//    void PreviewClassSkills()
//    {
//        unlockButton.SetActive(false);

//        className.text = cLibrary.JobClasses[sData.classId].className;

//        // Update SkillTrees Sprites + Pictures by Tiers.
//        for(int j = 0; j < tierSkills.Count; j++)
//        {
//            // Scan current tiers skills from library.
//            List<SkillLibrary.Skill> currentTierSkills = new List<SkillLibrary.Skill>();
//            for(int k = 0; k < cLibrary.JobClasses[sData.classId].skills.Count; k++)
//            {
//                if(cLibrary.JobClasses[sData.classId].skills[k].tier == j)
//                    currentTierSkills.Add(cLibrary.JobClasses[sData.classId].skills[k]);
//            }

//            // Insert the scanned tiers into the SkillTree.
//                for (int i = 0; i < currentTierSkills.Count; i++ )
//                {
                    
//                    tierSkills[j].transform.GetChild(i).GetComponent<SkillButton>().Skill = currentTierSkills[i];

//                    SkillLibrary.Skill currentSkill = tierSkills[j].transform.GetChild(i).GetComponent<SkillButton>().Skill;

//                    tierSkills[j].transform.GetChild(i).GetComponent<Image>().sprite = currentSkill.skillIcon;
//                    tierSkills[j].transform.GetChild(i).GetChild(0).GetComponent<Text>().text = currentSkill.name;
//                }

//            currentTierSkills.Clear();
//        }
//    }

//    void UpdateOwnedSkills()
//    {
//        unlockButton.SetActive(false);
//        ClassLibrary.JobClass selectedJob = null;

//        if (sData.sClass == SelectionData.SelectedClass.Class1)
//        {
//            selectedJob = cStats.Class1;
//            className.text = cStats.Class1.className;
//        }

//        else
//        {
//            selectedJob = cStats.Class2;
//            className.text = cStats.Class2.className;
//        }

//        for (int j = 0; j < tierSkills.Count; j++)
//        {
//            // Scan current tiers skills from character.
//            List<SkillLibrary.Skill> currentTierSkills = new List<SkillLibrary.Skill>();

//            for (int k = 0; k < selectedJob.skills.Count; k++)
//            {
//                if (selectedJob.skills[k].tier == j)
//                    currentTierSkills.Add(selectedJob.skills[k]);
//            }

//            for (int i = 0; i < currentTierSkills.Count; i++)
//            {
//                tierSkills[j].transform.GetChild(i).GetComponent<SkillButton>().Skill = currentTierSkills[i];

//                SkillLibrary.Skill currentSkill = tierSkills[j].transform.GetChild(i).GetComponent<SkillButton>().Skill;

//                tierSkills[j].transform.GetChild(i).GetComponent<Image>().sprite = currentSkill.skillIcon;
//                tierSkills[j].transform.GetChild(i).GetChild(0).GetComponent<Text>().text = currentSkill.name;

//            }

//            currentTierSkills = new List<SkillLibrary.Skill>();
//        }
//    }

//    public void UpdateDescription(SkillButton button)
//    {
//        sData.skillId = button.Skill.id;
//        image.sprite = button.Skill.skillIcon;
//        skillName.text = button.Skill.name;
//        description.text = button.Skill.description;

//        if(!preview)
//        {
//            if (!button.Skill.unlocked)
//            {
//                expNeeded.text = button.Skill.unlockRequirement.ToString();
//                unlockButton.SetActive(true);
//            }

//        }

//    }

//    public void UnlockSkill()
//    {   
//        ClassLibrary.JobClass currentClass = null;

//        if(sData.sClass == SelectionData.SelectedClass.Class1)
//            currentClass = cStats.Class1;

//        else
//            currentClass = cStats.Class2;

//        SkillLibrary.Skill currentSkill = null;

//        for (int i = 0; i < currentClass.skills.Count; i++ )
//        {
//            if (currentClass.skills[i].id == sData.skillId)
//                currentSkill = currentClass.skills[i];
//        }


//        if (cStats.EXP >= currentSkill.unlockRequirement)
//        {
//            if (currentSkill.unlocked != true)
//            {
//                cStats.EXP -= currentSkill.unlockRequirement;

//                if (sData.sClass == SelectionData.SelectedClass.Class1)
//                {
//                    for (int j = 0; j < cStats.Class1.skills.Count; j++)
//                    {
//                        if (cStats.Class1.skills[j].id == sData.skillId)
//                            cStats.Class1.skills[j].unlocked = true;
//                    }
//                }

//                else
//                {
//                    for (int j = 0; j < cStats.Class2.skills.Count; j++)
//                    {
//                        if (cStats.Class2.skills[j].id == sData.skillId)
//                            cStats.Class2.skills[j].unlocked = true;
//                    }
//                }
//            }
//            // Apply the skill to the character.
//            GameCore.GetResource().ApplySkill(currentSkill);
//        }
//        UpdateOwnedSkills();
//    }

//    public void Exit()
//    {
//        class1.isOn = false;
//        class2.isOn = false;
//        stats.isOn = true;
//        gameObject.SetActive(false);
//    }
//}

