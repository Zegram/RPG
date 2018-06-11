//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class CharacterSheet : MonoBehaviour
//{
//    public GameObject statsPage, skillPage, classPage, SkillTree = null;
//    CharacterStats cStats = null;
//    SelectionData sData = null;

//    void Awake()
//    {
//        cStats = CharacterStats.GetResource();
//        sData = SelectionData.GetResource();

//        statsPage.SetActive(true);
//        skillPage.SetActive(false);
//        classPage.SetActive(false);
//    }

//    public void ChangeToStatsPage(bool toggle)
//    {
//        statsPage.SetActive(true);
//        skillPage.SetActive(false);
//        classPage.SetActive(false);

//    }

//    public void ChangeToSkillsPage(bool toggle)
//    {
//        statsPage.SetActive(false);
//        skillPage.SetActive(true);
//        classPage.SetActive(false);
//    }

//    public void ChangeToClassPage1(bool toggle)
//    {
//        if (toggle == true)
//        {
//            sData.sClass = SelectionData.SelectedClass.Class1;
//            if (cStats.Class1.unlocked)
//            {

                
//                SkillTree.GetComponent<SkillTree>().preview = false;
//                SkillTree.SetActive(true);
//                statsPage.SetActive(false);

//            }

//            else
//            {
//                statsPage.SetActive(false);
//                skillPage.SetActive(false);
//                classPage.SetActive(true);
//            }
//        }

//    }

//    public void ChangeToClassPage2(bool toggle)
//    {
//        if (toggle == true)
//        {
//            sData.sClass = SelectionData.SelectedClass.Class2;
//            if (cStats.Class2.unlocked)
//            {
                
//                SkillTree.GetComponent<SkillTree>().preview = false;
//                SkillTree.SetActive(true);
//                statsPage.SetActive(false);
//            }

//            else
//            {
//                statsPage.SetActive(false);
//                skillPage.SetActive(false);
//                classPage.SetActive(true);
//            }

//        }
//    }


//}
