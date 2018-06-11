using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateClassLibrary
{

    static string defaultPath = "Assets/Libraries/ClassLibrary";

    [UnityEditor.MenuItem("Tools/Create Class Library")]
    public static void CreateAsset()
    {
        ClassLibrary asset = ScriptableObject.CreateInstance<ClassLibrary>();
        AssetDatabase.CreateAsset(asset, defaultPath + ".asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }



}
