using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialAttackWindow : MonoBehaviour
{

    public Text skillName = null;
    public Text spCost = null;
    public Text skillNumber = null;
    public Text power = null;
    public Text modifierStat = null;
    public Text skillType = null;

    public int selectedSkill = 0;
    BattleModeCore bCore = null;
    SelectionData sData = null;


    List<ClassLibrary.JobSkill> skills = new List<ClassLibrary.JobSkill>();

    void OnEnable()
    {
        bCore = BattleModeCore.GetResource();
        sData = SelectionData.GetResource();
        sData.selectedSkillIndex = 0;
        skills.Clear();

        BattleCharacter currentChar = bCore.turnTable.currentCharacterTurn;
        for (int i = 0; i < currentChar.stats.Class.skills.Count; i++)
        {
            // Lets just add all the skills for now.
            //if (currentChar.stats.Class.skills[i].skill.unlocked)
            skills.Add(currentChar.stats.Class.skills[i]);
        }

        UpdateWindow();
    }

    public void UpdateWindow()
    {
        BattleCharacter currentChar = bCore.turnTable.currentCharacterTurn;
        SkillLibrary.Skill cSkill = currentChar.stats.Class.skills[sData.selectedSkillIndex].skill;

        if (cSkill != null)
        {
            skillName.text = cSkill.name;
            spCost.text = "Cost: " + cSkill.spCost.ToString() + " SP";
            int skillnumber = sData.selectedSkillIndex + 1;
            skillNumber.text = skillnumber.ToString() + " / " + skills.Count.ToString();
            power.text = "Power: " + cSkill.activeSkill[0].powerPercent.ToString() + "%";
            modifierStat.text = "Stat Type: " + cSkill.activeSkill[0].statTypeModifier.ToString();
            skillType.text = cSkill.activeSkill[0].activeSkillType.ToString();
        }
        
    }

    public void OpenWindow()
    {
        SelectionData.GetResource().selectedSkillIndex = 0;

        if (BattleModeCore.GetResource().turnTable.currentCharacterTurn.stats.Class.skills[SelectionData.GetResource().selectedSkillIndex].skill != null)
        gameObject.SetActive(true);
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);

        bCore.ClearAttack();
    }

    public void SkillForward()
    {
        if(sData.selectedSkillIndex + 1 < bCore.turnTable.currentCharacterTurn.stats.Class.skills.Count)
        sData.selectedSkillIndex++;

        UpdateWindow();
    }

    public void SkillBackward()
    {
        if(sData.selectedSkillIndex > 0)
        sData.selectedSkillIndex--;

        UpdateWindow();
    }
}
