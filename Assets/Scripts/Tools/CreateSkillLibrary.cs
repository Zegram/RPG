using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateSkillLibrary : MonoBehaviour {

    static string defaultPath = "Assets/Libraries/SkillLibrary";

    [UnityEditor.MenuItem("Tools/Create Skill Library")]
    public static void CreateAsset()
    {
        SkillLibrary asset = ScriptableObject.CreateInstance<SkillLibrary>();
        AssetDatabase.CreateAsset(asset, defaultPath + ".asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
