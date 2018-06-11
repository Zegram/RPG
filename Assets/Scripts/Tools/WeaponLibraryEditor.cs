using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[CustomEditor(typeof(WeaponLibrary))]
public class WeaponLibraryEditor : Editor 
{
    Color[] typeColors = { new Color(0.5f, 1.0f, 0.5f), new Color(0.5f, 1.0f, 1.0f), new Color(1f, 0.92f, 0.016f) };
    WeaponLibrary.Weapon[] weapons;

    public override void OnInspectorGUI()
    {
        WeaponLibrary library = target as WeaponLibrary;

        Undo.RecordObject(library, null);

        if(GUILayout.Button("Create Weapon"))
        {
            WeaponLibrary.Weapon tempWeapon = new WeaponLibrary.Weapon();
            library.weapons.Add(tempWeapon);
            EditorUtility.SetDirty(library);
            return;   
        }

        EditorGUILayout.Space();

        if (library.weapons == null)
        {
            library.weapons = new System.Collections.Generic.List<WeaponLibrary.Weapon>();
        }

        for(int i = 0; i < library.weapons.Count; i++)
        {
            WeaponLibrary.Weapon weapon = library.weapons[i];
            EditorGUILayout.BeginHorizontal();

            GUI.color = typeColors[0];

            if (GUILayout.Button("[" + weapon.rarity.ToString() + "] " + weapon.name))
            {
                weapon.show = !weapon.show;
            }

            EditorGUILayout.BeginVertical(GUILayout.Width(25), GUILayout.Height(20));
            //Up
            if (GUILayout.Button("▲", GUILayout.Height(10)))
            {
                if (i > 0)
                {
                    WeaponLibrary.Weapon tempSkill = library.weapons[i - 1];
                    library.weapons[i - 1] = library.weapons[i];
                    library.weapons[i] = tempSkill;
                    EditorUtility.SetDirty(library);
                    return;
                }
            }

            if (GUILayout.Button("▼", GUILayout.Height(10)))
            {
                if (i < library.weapons.Count - 1)
                {
                    WeaponLibrary.Weapon tempSkill = library.weapons[i + 1];
                    library.weapons[i + 1] = library.weapons[i];
                    library.weapons[i] = tempSkill;
                    EditorUtility.SetDirty(library);
                    return;
                }
            }
            EditorGUILayout.EndVertical();

            // Remove button
            GUI.color = Color.red;
            if (GUILayout.Button("X", GUILayout.Width(25)))
            {
                if (EditorUtility.DisplayDialog("Remove Weapon", "Do you really want to remove the weapon?", "Yes", "No"))
                {
                    library.weapons.RemoveAt(i);
                    EditorUtility.SetDirty(library);
                    return;
                }

            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();

            if (weapon.show)
            {
                // Name
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Weapon Name: ");
                weapon.name = GUILayout.TextField(weapon.name);
                EditorGUILayout.EndHorizontal();

                // Sprite
                weapon.weaponSprite = (Sprite)EditorGUILayout.ObjectField("Sprite", weapon.weaponSprite, typeof(Sprite), true);

                // Type
                EditorGUILayout.BeginHorizontal();
                weapon.type = (WeaponLibrary.weaponType)EditorGUILayout.EnumPopup("Weapon Type: ", weapon.type);
                EditorGUILayout.EndHorizontal();

                // Rarity
                EditorGUILayout.BeginHorizontal();
                weapon.rarity = (WeaponLibrary.rarity)EditorGUILayout.EnumPopup("Rarity: ", weapon.rarity);
                EditorGUILayout.EndHorizontal();

                // Damage
                EditorGUILayout.BeginHorizontal();
                weapon.damage = EditorGUILayout.IntField("Damage: ", weapon.damage);
                EditorGUILayout.EndHorizontal();

                // Damage
                EditorGUILayout.BeginHorizontal();
                weapon.range = EditorGUILayout.IntField("Range: ", weapon.range);
                EditorGUILayout.EndHorizontal();

                // Gold Cost
                EditorGUILayout.BeginHorizontal();
                weapon.goldCost = EditorGUILayout.IntField("Gold Cost: ", weapon.goldCost);
                EditorGUILayout.EndHorizontal();
            }
        }
    }


}
