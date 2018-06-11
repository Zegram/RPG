using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionData : MonoBehaviour {
    public enum SelectedClass { Class1, Class2 }

    public int classId = 0;
    public SelectedClass sClass = SelectedClass.Class1;
    public int skillId = 0;

    // Battle
    public int selectedSkillIndex = 0;
    

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static SelectionData GetResource()
    {
        return GameObject.Find("GlobalData").GetComponent<SelectionData>();
    }
}
