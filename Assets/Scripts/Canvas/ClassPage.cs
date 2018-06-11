//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class ClassPage : MonoBehaviour {

//    CharacterStats cStats = null;
//    ClassLibrary classLibrary = null;
//    SelectionData selectData = null;
//    public Toggle stats, inventory, class1, class2 = null;

//    [Header("General")]
//    public GameObject classDescription = null;
//    public Transform classPictures = null;
//    public List<GameObject> classImages = new List<GameObject>();
//    //public SkillTree skillTree = null;

//   [Header("Description")]
//    public Text className = null;
//    public Image descriptionImage = null;
//    public Text description = null;

//    void Awake()
//    {
//        cStats = CharacterStats.GetResource();
//        classLibrary = ClassLibrary.GetResource();
//        selectData = SelectionData.GetResource();
//    }

//    void Start () {


//        for (int i = 0; i < classImages.Count; i++)
//        {
//            if (classImages[i] != null && classLibrary.JobClasses[i].classIcon != null)
//            {
//                classImages[i].GetComponent<Image>().sprite = classLibrary.JobClasses[i].classIcon;
//                classImages[i].transform.FindChild("ClassName").GetComponent<Text>().text = classLibrary.JobClasses[i].className;
//            }
//        }
//        UpdateDescription();

//    }

//    void OnEnable()
//    {
//        selectData.classId = 0;
//        UpdateDescription();
//    }
	
//    void Update () 
//    {
//        UpdateID();

//    }

//    public void UpdateID()
//    {
//        for(int i = 0; i < classImages.Count; i++)
//        {
//            Toggle currentToggle = classImages[i].GetComponent<Toggle>();
//            if (currentToggle.isOn)
//            {
//                selectData.classId = i;
//                UpdateDescription();
//            }
//        }
//    }

//    void UpdateDescription()
//    {
//        className.text = classLibrary.JobClasses[selectData.classId].className;
//        descriptionImage.sprite = classLibrary.JobClasses[selectData.classId].classIcon;
//    }

//    public void UnlockClass()
//    {
//        if (cStats.EXP >= classLibrary.JobClasses[selectData.classId].unlockRequirement)
//        {
//            cStats.EXP -= classLibrary.JobClasses[selectData.classId].unlockRequirement;
            
//            if (selectData.sClass == SelectionData.SelectedClass.Class1)
//            {
//                cStats.Class1.unlocked = true;
//                cStats.Class1 = classLibrary.JobClasses[selectData.classId];
//            }

//            if (selectData.sClass == SelectionData.SelectedClass.Class2)
//            {
//                cStats.Class2.unlocked = true;
//                cStats.Class2 = classLibrary.JobClasses[selectData.classId];
//            }
//            class1.isOn = false;
//            class2.isOn = false;
//            stats.isOn = true;
//        }

//        else
//            Debug.Log("Not enough EXP");
//    }

//    public void ShowCaseSkills()
//    {
//        skillTree.preview = true;
//        skillTree.gameObject.SetActive(true);        
//    }
//}
