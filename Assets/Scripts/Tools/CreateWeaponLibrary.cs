using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateWeaponLibrary {

    static string defaultPath = "Assets/Resources/Libraries/WeaponLibrary";

    [UnityEditor.MenuItem("Tools/Create WeaponLibrary")]
    public static void CreateAsset()
    {
        WeaponLibrary asset = ScriptableObject.CreateInstance<WeaponLibrary>();
        AssetDatabase.CreateAsset(asset, defaultPath + ".asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
