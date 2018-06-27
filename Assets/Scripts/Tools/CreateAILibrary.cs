using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateAILibrary : MonoBehaviour
{

    static string defaultPath = "Assets/Libraries/AILibrary";

    [UnityEditor.MenuItem("Tools/Create AI Library")]
    public static void CreateAsset()
    {
        AILibrary asset = ScriptableObject.CreateInstance<AILibrary>();
        AssetDatabase.CreateAsset(asset, defaultPath + ".asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
