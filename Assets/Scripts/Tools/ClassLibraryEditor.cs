using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[CustomEditor(typeof(ClassLibrary))]
public class ClassLibraryEditor : Editor
{

    Color[] typeColors = { new Color(0.5f, 1.0f, 0.5f), new Color(0.5f, 1.0f, 1.0f) };

    ClassLibrary.JobClass[] jobClasses;

    int index = 0;

    public override void OnInspectorGUI()
    {

        ClassLibrary library = target as ClassLibrary;

        Undo.RecordObject(library, null);



        if (GUILayout.Button("Create Class"))
        {
            ClassLibrary.JobClass tempClass = new ClassLibrary.JobClass();

            library.JobClasses.Add(tempClass);
            EditorUtility.SetDirty(library);
            return;
        }

        EditorGUILayout.Space();

        if (library.JobClasses == null)
        {

            library.JobClasses = new System.Collections.Generic.List<ClassLibrary.JobClass>();

        }

        for (int i = 0; i < library.JobClasses.Count; i++)
        {
            ClassLibrary.JobClass jobClass = library.JobClasses[i];
            EditorGUILayout.BeginHorizontal();
            GUI.color = typeColors[0];
            if (GUILayout.Button(jobClass.className))
            {
                jobClass.show = !jobClass.show;
            }

            EditorGUILayout.BeginVertical(GUILayout.Width(25), GUILayout.Height(20));
            //Up
            if (GUILayout.Button("▲", GUILayout.Height(10)))
            {
                if (i > 0)
                {
                    ClassLibrary.JobClass tempJobClass = library.JobClasses[i - 1];
                    library.JobClasses[i - 1] = library.JobClasses[i];
                    library.JobClasses[i] = tempJobClass;
                    EditorUtility.SetDirty(library);
                    return;
                }
            }

            if (GUILayout.Button("▼", GUILayout.Height(10)))
            {
                if (i < library.JobClasses.Count - 1)
                {
                    ClassLibrary.JobClass tempJobClass = library.JobClasses[i + 1];
                    library.JobClasses[i + 1] = library.JobClasses[i];
                    library.JobClasses[i] = tempJobClass;
                    EditorUtility.SetDirty(library);
                    return;
                }
            }
            EditorGUILayout.EndVertical();

            // Remove button
            GUI.color = Color.red;
            if (GUILayout.Button("X", GUILayout.Width(25)))
            {
                if (EditorUtility.DisplayDialog("Remove Class", "Do you really want to remove the class?", "Yes", "No"))
                {
                    library.JobClasses.RemoveAt(i);
                    EditorUtility.SetDirty(library);
                    return;
                }

            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();

            if (jobClass.show)
            {
                // Name
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Class Name: ");
                jobClass.className = GUILayout.TextField(jobClass.className);
                EditorGUILayout.EndHorizontal();

                // Sprite
                jobClass.classIcon = (Sprite)EditorGUILayout.ObjectField("Icon", jobClass.classIcon, typeof(Sprite), true);

                jobClass.showSkills = EditorGUILayout.Foldout(jobClass.showSkills, "Class Skills");

                if (jobClass.showSkills)
                {                   
                        EditorGUILayout.BeginHorizontal();
                        GUI.color = typeColors[0];

                        SkillLibrary skillLibrary = SkillLibrary.GetResource();

                    if(skillLibrary == null)
                    {
                        Debug.Log("Skill Library is null");
                        return;
                    }

                    string[] skillOptions = new string[skillLibrary.Skills.Count];
                    for (int y = 0; y < skillLibrary.Skills.Count; y++)
                    {
                        //string tier = "[Tier " + skillLibrary.Skills[y].tier.ToString() + "]";
                        skillOptions[y] = skillLibrary.Skills[y].name;
                    }


                    index = EditorGUILayout.Popup(index, skillOptions);

                        if (GUILayout.Button("Add Skill"))
                        {
                            ClassLibrary.JobSkill tempSkill = new ClassLibrary.JobSkill(skillLibrary.Skills[index]);
                            library.JobClasses[i].skills.Add(tempSkill);
                            EditorUtility.SetDirty(library);
                            return;
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();

                        if (library.JobClasses[i].skills == null)
                            library.JobClasses[i].skills = new System.Collections.Generic.List<ClassLibrary.JobSkill>();

                       
                        for (int j = 0; j < library.JobClasses[i].skills.Count; j++)
                        {
                            ClassLibrary.JobSkill skill = library.JobClasses[i].skills[j];
                            EditorGUILayout.BeginHorizontal();

                            //string tier = "[TIER " + skill.tier + "]";
                            //string type = skill.type == SkillLibrary.skillType.Active ? "[ACTIVE] " : "[PASSIVE] ";

                            GUI.color = Color.white;
                            if (GUILayout.Button("[" + skill.unlockedAtLevel.ToString() + "] " + skill.skill.name))
                            {
                                skill.showSkillStats = !skill.showSkillStats;
                            }
                            
                            if(skill.showSkillStats)
                            {
                                skill.unlockedAtLevel = EditorGUILayout.IntField("Unlocked at Level: ", skill.unlockedAtLevel);
                            }

                            // Remove button
                            GUI.color = Color.red;
                            if (GUILayout.Button("X", GUILayout.Width(25)))
                            {
                                if (EditorUtility.DisplayDialog("Remove Skill", "Do you really want to remove the skill?", "Yes", "No"))
                                {

                                    library.JobClasses[i].skills.RemoveAt(j);
                                    EditorUtility.SetDirty(library);
                                    return;
                                }

                            }
                            EditorGUILayout.EndHorizontal();


                        }
                   
                }


            }


        }


    }
}
