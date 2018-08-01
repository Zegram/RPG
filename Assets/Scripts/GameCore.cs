using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCore : MonoBehaviour {

    public enum GameMode { FreeRoam, Battle }

    public GameMode currentGameMode = GameMode.FreeRoam;

    //CharacterStats charStats = null;
    ClassLibrary classLibrary = null;
	
    public static GameCore GetResource()
    {
        return GameObject.Find("GlobalData").GetComponent<GameCore>();
    }

    void Awake()
    {
        classLibrary = ClassLibrary.GetResource();
        //charStats = CharacterStats.GetResource();

        //for (int i = 0; i < classLibrary.JobClasses.Count; i++ )
        //{
        //    for(int j = 0; j < classLibrary.JobClasses[i].skills.Count; j++)
        //    {
        //        classLibrary.JobClasses[i].skills[j].unlocked = false;
        //    }
        //}

        //charStats.AnalyzeStats();

    }

    public void ApplySkill(SkillLibrary.Skill unlockedSkill)
    {
        //// If skill is passive.
        //if(unlockedSkill.type == SkillLibrary.skillType.Passive)
        //{
        //    for(int i = 0; i < unlockedSkill.passiveSkill.Count; i++)
        //    {
        //        SkillLibrary.PassiveSkill currentSkill = unlockedSkill.passiveSkill[i];
        //        // If the passive type is stat increase.
        //        if(unlockedSkill.passiveSkill[i].passiveSkillType == SkillLibrary.passiveSkillType.StatIncrease)
        //            charStats.BaseStats.ChangeBaseStatValue(currentSkill.statType, currentSkill.increaseAmount);

        //        else if(unlockedSkill.passiveSkill[i].passiveSkillType == SkillLibrary.passiveSkillType.UnlockWeaponLicense)
        //            charStats.WeaponLicenses.UnlockLicense(currentSkill.weaponType);

        //    }

        //}

        //else if(unlockedSkill.type == SkillLibrary.skillType.Active)
        //{

        //}
    }


    public void StartBattleMode()
    {
        currentGameMode = GameMode.Battle;
        BattleModeCore.GetResource().InitializeBattle();
    }

    public void StartFreeRoam()
    {
        currentGameMode = GameMode.FreeRoam;
        BattleHUD.GetResource().ActivateBattleChoices(false);
        //BattleModeCore.GetResource().InitializeBattle();
    }
}
