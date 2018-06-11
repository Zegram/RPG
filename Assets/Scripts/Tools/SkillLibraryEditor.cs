using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[CustomEditor(typeof(SkillLibrary))]
public class SkillLibraryEditor : Editor
{
    Color[] typeColors = { new Color(0.5f, 1.0f, 0.5f), new Color(0.5f, 1.0f, 1.0f), new Color(1f, 0.92f, 0.016f) };
    SkillLibrary.Skill[] skills;


    public override void OnInspectorGUI()
    {

        SkillLibrary library = target as SkillLibrary;

        Undo.RecordObject(library, null);

        if (GUILayout.Button("Create Skill"))
        {
            SkillLibrary.Skill tempSkill = new SkillLibrary.Skill();
            tempSkill.unlocked = false;
            tempSkill.id = Random.Range(0, 1000);
            bool loop = true;

            while (loop)
            {
                for (int i = 0; i < library.Skills.Count; i++)
                {
                    if (library.Skills[i].id == tempSkill.id)
                    {
                        tempSkill.id = Random.Range(0, 1000);
                        i = 0;
                    }
                }
                loop = false;
            }


            library.Skills.Add(tempSkill);
            EditorUtility.SetDirty(library);
            return;
        }

        EditorGUILayout.Space();

        if (library.Skills == null)
        {
            library.Skills = new System.Collections.Generic.List<SkillLibrary.Skill>();
        }

        for (int i = 0; i < library.Skills.Count; i++)
        {
            SkillLibrary.Skill skill = library.Skills[i];
            EditorGUILayout.BeginHorizontal();
            GUI.color = typeColors[0];

            if (GUILayout.Button(skill.name))
            {
                skill.show = !skill.show;
            }

            EditorGUILayout.BeginVertical(GUILayout.Width(25), GUILayout.Height(20));
            //Up
            if (GUILayout.Button("▲", GUILayout.Height(10)))
            {
                if (i > 0)
                {
                    SkillLibrary.Skill tempSkill = library.Skills[i - 1];
                    library.Skills[i - 1] = library.Skills[i];
                    library.Skills[i] = tempSkill;
                    EditorUtility.SetDirty(library);
                    return;
                }
            }

            if (GUILayout.Button("▼", GUILayout.Height(10)))
            {
                if (i < library.Skills.Count - 1)
                {
                    SkillLibrary.Skill tempSkill = library.Skills[i + 1];
                    library.Skills[i + 1] = library.Skills[i];
                    library.Skills[i] = tempSkill;
                    EditorUtility.SetDirty(library);
                    return;
                }
            }
            EditorGUILayout.EndVertical();

            // Remove button
            GUI.color = Color.red;
            if (GUILayout.Button("X", GUILayout.Width(25)))
            {
                if (EditorUtility.DisplayDialog("Remove Skill", "Do you really want to remove the skill?", "Yes", "No"))
                {
                    library.Skills.RemoveAt(i);
                    EditorUtility.SetDirty(library);
                    return;
                }

            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();

            if (skill.show)
            {
                // Name
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Skill Name: ");
                skill.name = GUILayout.TextField(skill.name);
                EditorGUILayout.EndHorizontal();

                // Icon
                skill.skillIcon = (Sprite)EditorGUILayout.ObjectField("Icon", skill.skillIcon, typeof(Sprite), true);

                // Description
                GUILayout.Label("Description: ");
                skill.description = GUILayout.TextField(skill.description);

                // SPCost
                skill.spCost = EditorGUILayout.IntField("SP Cost: ", skill.spCost);

                // Range
                skill.range = EditorGUILayout.IntField("Range: ", skill.range);

                // OnlyToPoint
                skill.OnlyToPoint = EditorGUILayout.Toggle("Only To Point ", skill.OnlyToPoint);

                // OnlyToPoint
                skill.FreeCastOnRange = EditorGUILayout.Toggle("Free Cast on Range ", skill.FreeCastOnRange);

                skill.showAoE = EditorGUILayout.Foldout(skill.showAoE, "Skill AoE");

                if (skill.showAoE)
                {
                    if (GUILayout.Button("Add AoE"))
                    {
                        Vector2 tempAoE = new Vector2(0, 0);

                        skill.AoE.Add(tempAoE);
                        EditorUtility.SetDirty(library);
                        return;
                    }

                    EditorGUILayout.Space();

                    //if (skill.AoE == null)
                    //{
                    //    skill.AoE = new System.Collections.Generic.List<Vector2>();
                    //}

                    for (int r = 0; r < skill.AoE.Count; r++)
                    {
                        //string buttonName = "(" + skill.AoE[r].x.ToString() + ", " + skill.AoE[r].y.ToString() + ")";

                        EditorGUILayout.BeginHorizontal();

                        skill.AoE[r] = EditorGUILayout.Vector2Field("AoE Location: ", skill.AoE[r]);
                        //if (GUILayout.Button(buttonName))
                        //{
                            
                        //}

                        // Remove button
                        GUI.color = Color.red;
                        if (GUILayout.Button("X", GUILayout.Width(25)))
                        {
                            if (EditorUtility.DisplayDialog("Remove AoE", "Do you really want to remove the skill?", "Yes", "No"))
                            {

                                skill.AoE.RemoveAt(r);
                                EditorUtility.SetDirty(library);
                                return;
                            }

                        }
                        GUI.color = Color.white;
                        EditorGUILayout.EndHorizontal();
                    }
                }

                // Type
                EditorGUILayout.BeginHorizontal();
                skill.type = (SkillLibrary.skillType)EditorGUILayout.EnumPopup("Skill Type: ", skill.type);
                EditorGUILayout.EndHorizontal();

                // ID
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("ID :");
                skill.id = EditorGUILayout.IntField(skill.id);
                EditorGUILayout.EndHorizontal();

                skill.showSkillTypeInfo = EditorGUILayout.Foldout(skill.showSkillTypeInfo, "Skill Information");




                if (skill.showSkillTypeInfo)
                {

                    //ACTIVE
                    if (skill.type == SkillLibrary.skillType.Active)
                    {
                        if (GUILayout.Button("Create Active"))
                        {
                            SkillLibrary.ActiveSkill tempActive = new SkillLibrary.ActiveSkill(SkillLibrary.activeSkillType.Attack);

                            skill.activeSkill.Add(tempActive);
                            EditorUtility.SetDirty(library);
                            return;
                        }
                        EditorGUILayout.Space();

                        for (int k = 0; k < skill.activeSkill.Count; k++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUI.color = typeColors[0];

                            string buttonName = "[" + skill.activeSkill[k].activeSkillType.ToString() + "] ";

                            if (GUILayout.Button(buttonName))
                            {
                                skill.activeSkill[k].show = !skill.activeSkill[k].show;
                            }

                            // Remove button
                            GUI.color = Color.red;
                            if (GUILayout.Button("X", GUILayout.Width(25)))
                            {
                                if (EditorUtility.DisplayDialog("Remove Skill", "Do you really want to remove the skill?", "Yes", "No"))
                                {

                                    skill.activeSkill.RemoveAt(k);
                                    EditorUtility.SetDirty(library);
                                    return;
                                }

                            }

                            EditorGUILayout.EndHorizontal();
                            GUI.color = Color.white;
                            if (skill.activeSkill[k].show)
                            {
                                EditorGUILayout.BeginHorizontal();
                                skill.activeSkill[k].activeSkillType = (SkillLibrary.activeSkillType)EditorGUILayout.EnumPopup("Active Type: ", skill.activeSkill[k].activeSkillType);
                                EditorGUILayout.EndHorizontal();

                                //EditorGUILayout.BeginHorizontal();
                                //skill.activeSkill[k].manaCost = EditorGUILayout.IntField("Mana Cost: ", skill.activeSkill[k].manaCost);
                                //EditorGUILayout.EndHorizontal();

                                //EditorGUILayout.BeginHorizontal();
                                //skill.activeSkill[k].restrictedToWeapon = EditorGUILayout.Toggle("Restricted To Weapon? ", skill.activeSkill[k].restrictedToWeapon);
                                //EditorGUILayout.EndHorizontal();

                                //if (skill.activeSkill[k].restrictedToWeapon)
                                //{
                                //    EditorGUILayout.BeginHorizontal();
                                //    //skill.activeSkill[k].restrictedWeaponType = (VariableStats.WeaponLicenseTypes)EditorGUILayout.EnumPopup("Weapon Type: ", skill.activeSkill[k].restrictedWeaponType);
                                //    EditorGUILayout.EndHorizontal();
                                //}
                                EditorGUILayout.Space();


                                //What kind of Active Skill? Damage, Buff or Debuff?
                                if (skill.activeSkill[k].activeSkillType == SkillLibrary.activeSkillType.Attack || skill.activeSkill[k].activeSkillType == SkillLibrary.activeSkillType.Heal)
                                {

                                    EditorGUILayout.BeginHorizontal();
                                    skill.activeSkill[k].statTypeModifier = (CharacterStats.BaseStatTypes)EditorGUILayout.EnumPopup("StatType Modifier: ", skill.activeSkill[k].statTypeModifier);
                                    EditorGUILayout.EndHorizontal();

                                    EditorGUILayout.BeginHorizontal();
                                    skill.activeSkill[k].powerPercent = EditorGUILayout.IntField("Power (%): ", skill.activeSkill[k].powerPercent);
                                    EditorGUILayout.EndHorizontal();

                                    EditorGUILayout.BeginHorizontal();
                                    skill.activeSkill[k].hitAmount = EditorGUILayout.IntField("Hit Amount: ", skill.activeSkill[k].hitAmount);
                                    EditorGUILayout.EndHorizontal();

                                }

                                else if (skill.activeSkill[k].activeSkillType == SkillLibrary.activeSkillType.Buff || skill.activeSkill[k].activeSkillType == SkillLibrary.activeSkillType.Debuff)
                                {
                                    EditorGUILayout.BeginHorizontal();
                                    //skill.activeSkill[k].statType = (VariableStats.BaseStatTypes)EditorGUILayout.EnumPopup("Stat Type: ", skill.activeSkill[k].statType);
                                    EditorGUILayout.EndHorizontal();

                                    EditorGUILayout.BeginHorizontal();
                                    skill.activeSkill[k].amount = EditorGUILayout.IntField("Increase Amount: ", skill.activeSkill[k].amount);
                                    EditorGUILayout.EndHorizontal();

                                    EditorGUILayout.BeginHorizontal();
                                    skill.activeSkill[k].turnAmount = EditorGUILayout.IntField("Turn Amount: ", skill.activeSkill[k].turnAmount);
                                    EditorGUILayout.EndHorizontal();
                                }
                            }

                        }
                    }



                    //PASSIVE
                    //else if (skill.type == SkillLibrary.skillType.Passive)
                    //{
                    //    if (GUILayout.Button("Create Passive"))
                    //    {
                    //        SkillLibrary.PassiveSkill tempPassive = new SkillLibrary.PassiveSkill(SkillLibrary.passiveSkillType.StatIncrease);

                    //        skill.passiveSkill.Add(tempPassive);
                    //        EditorUtility.SetDirty(library);
                    //        return;
                    //    }
                    //    EditorGUILayout.Space();

                    //    for (int j = 0; j < skill.passiveSkill.Count; j++)
                    //    {
                    //        EditorGUILayout.BeginHorizontal();
                    //        GUI.color = typeColors[0];

                    //        //string buttonName = skill.passiveSkill[j].passiveSkillType == SkillLibrary.passiveSkillType.StatIncrease ? skill.passiveSkill[j].statType.ToString() + " Increase by: " + skill.passiveSkill[j].increaseAmount.ToString() : "Unlock: " + skill.passiveSkill[j].weaponType.ToString();

                    //        //if (GUILayout.Button(buttonName))
                    //        //{
                    //        //    skill.passiveSkill[j].show = !skill.passiveSkill[j].show;
                    //        //}

                    //        // Remove button
                    //        GUI.color = Color.red;
                    //        if (GUILayout.Button("X", GUILayout.Width(25)))
                    //        {
                    //            if (EditorUtility.DisplayDialog("Remove Passive?", "Do you really want to remove the skill?", "Yes", "No"))
                    //            {

                    //                skill.passiveSkill.RemoveAt(j);
                    //                EditorUtility.SetDirty(library);
                    //                return;
                    //            }
                    //        }
                    //        EditorGUILayout.EndHorizontal();
                    //        GUI.color = Color.white;
                    //        if (skill.passiveSkill[j].show)
                    //        {
                    //            // Type
                    //            EditorGUILayout.BeginHorizontal();
                    //            skill.passiveSkill[j].passiveSkillType = (SkillLibrary.passiveSkillType)EditorGUILayout.EnumPopup("Passive Type: ", skill.passiveSkill[j].passiveSkillType);
                    //            EditorGUILayout.EndHorizontal();

                    //            //What kind of Passive skill is this? Stat Increase or weapon/armor unlock?
                    //            if (skill.passiveSkill[j].passiveSkillType == SkillLibrary.passiveSkillType.StatIncrease)
                    //            {
                    //                //skill.passiveSkill[j].statType = (VariableStats.BaseStatTypes)EditorGUILayout.EnumPopup("Stat Type: ", skill.passiveSkill[j].statType);

                    //                EditorGUILayout.BeginHorizontal();
                    //                GUILayout.Label("Increase Amount: ");
                    //                skill.passiveSkill[j].increaseAmount = EditorGUILayout.IntField(skill.passiveSkill[j].increaseAmount);
                    //                EditorGUILayout.EndHorizontal();
                    //            }

                    //            else if (skill.passiveSkill[j].passiveSkillType == SkillLibrary.passiveSkillType.UnlockWeaponLicense)
                    //            {
                    //                EditorGUILayout.BeginHorizontal();
                    //                //skill.passiveSkill[j].weaponType = (VariableStats.WeaponLicenseTypes)EditorGUILayout.EnumPopup("Weapon Type: ", skill.passiveSkill[j].weaponType);
                    //                EditorGUILayout.EndHorizontal();
                    //            }
                    //        }

                    //    }
                    //}

                }
            }
        }
    }
}
