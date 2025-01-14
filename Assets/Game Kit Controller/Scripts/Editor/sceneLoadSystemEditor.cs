﻿using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor (typeof(sceneLoadSystem))]
public class sceneLoadSystemEditor : Editor
{
	SerializedProperty mainMenuContent;
	SerializedProperty sceneLoadMenu;
	SerializedProperty sceneSlotPrefab;
	SerializedProperty sceneSlotsParent;
	SerializedProperty mainScrollbar;
	SerializedProperty mainScrollRect;
	SerializedProperty startingSceneIndex;
	SerializedProperty addSceneNumberToName;
	SerializedProperty sceneInfoList;

	bool expanded;
	sceneLoadSystem manager;

	void OnEnable ()
	{
		mainMenuContent = serializedObject.FindProperty ("mainMenuContent");
		sceneLoadMenu = serializedObject.FindProperty ("sceneLoadMenu");
		sceneSlotPrefab = serializedObject.FindProperty ("sceneSlotPrefab");
		sceneSlotsParent = serializedObject.FindProperty ("sceneSlotsParent");
		mainScrollbar = serializedObject.FindProperty ("mainScrollbar");
		mainScrollRect = serializedObject.FindProperty ("mainScrollRect");
		startingSceneIndex = serializedObject.FindProperty ("startingSceneIndex");
		addSceneNumberToName = serializedObject.FindProperty ("addSceneNumberToName");
		sceneInfoList = serializedObject.FindProperty ("sceneInfoList");

		manager = (sceneLoadSystem)target;
	}

	public override void OnInspectorGUI ()
	{
		GUILayout.BeginVertical ();

		EditorGUILayout.Space ();

		GUILayout.BeginVertical ("Main Settings", "window", GUILayout.Height (30));
		EditorGUILayout.PropertyField (mainMenuContent);
		EditorGUILayout.PropertyField (sceneLoadMenu);
		EditorGUILayout.PropertyField (sceneSlotPrefab);
		EditorGUILayout.PropertyField (sceneSlotsParent);
		EditorGUILayout.PropertyField (mainScrollbar);
		EditorGUILayout.PropertyField (mainScrollRect);
		EditorGUILayout.PropertyField (startingSceneIndex);
		EditorGUILayout.PropertyField (addSceneNumberToName);
		GUILayout.EndVertical ();

		EditorGUILayout.Space ();

		GUILayout.BeginVertical ("Scene List", "window", GUILayout.Height (30));
		showSceneList (sceneInfoList);
		GUILayout.EndVertical ();

		GUILayout.EndVertical ();

		if (GUI.changed) {
			serializedObject.ApplyModifiedProperties ();
		}

		EditorGUILayout.Space ();
	}

	void showSceneList (SerializedProperty list)
	{
		EditorGUILayout.PropertyField (list, false);
		if (list.isExpanded) {
			GUILayout.BeginVertical ("box");
			EditorGUILayout.Space ();
			GUILayout.Label ("Number Of Scenes: \t" + list.arraySize);

			EditorGUILayout.Space ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Add")) {
				list.arraySize++;
			}
			if (GUILayout.Button ("Clear")) {
				list.ClearArray ();
			}
			GUILayout.EndHorizontal ();

			EditorGUILayout.Space ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Expand All")) {
				for (int i = 0; i < list.arraySize; i++) {
					list.GetArrayElementAtIndex (i).isExpanded = true;
				}
			}
			if (GUILayout.Button ("Collapse All")) {
				for (int i = 0; i < list.arraySize; i++) {
					list.GetArrayElementAtIndex (i).isExpanded = false;
				}
			}
			GUILayout.EndHorizontal ();

			EditorGUILayout.Space ();

			if (GUILayout.Button ("Set Scene Number In Order")) {
				manager.setSceneNumberInOrder ();
			}

			EditorGUILayout.Space ();

			if (GUILayout.Button ("Add All Scenes")) {
				manager.enableAddSceneToListByIndex (0);
			}

			EditorGUILayout.Space ();

			if (GUILayout.Button ("Disable All Scenes")) {
				manager.disableAddSceneToListByIndex (0);
			}

			EditorGUILayout.Space ();

			for (int i = 0; i < list.arraySize; i++) {
				expanded = false;
				GUILayout.BeginHorizontal ();
				GUILayout.BeginHorizontal ("box");
				EditorGUILayout.Space ();
				if (i < list.arraySize && i >= 0) {
					EditorGUILayout.BeginVertical ();

					string addScene = "-";
					if (list.GetArrayElementAtIndex (i).FindPropertyRelative ("addSceneToList").boolValue) {
						addScene = "+";
					}

					EditorGUILayout.PropertyField (list.GetArrayElementAtIndex (i), 
						new GUIContent (list.GetArrayElementAtIndex (i).displayName + " - " + list.GetArrayElementAtIndex (i).FindPropertyRelative ("sceneNumber").intValue + " " + addScene));

					if (list.GetArrayElementAtIndex (i).isExpanded) {
						showSceneListElement (list.GetArrayElementAtIndex (i));
						expanded = true;
					}
					GUILayout.EndVertical ();
				}
				GUILayout.EndHorizontal ();
				if (expanded) {
					GUILayout.BeginVertical ();
				} else {
					GUILayout.BeginHorizontal ();
				}
				if (GUILayout.Button ("x")) {
					list.DeleteArrayElementAtIndex (i);
				}
				if (GUILayout.Button ("v")) {
					if (i >= 0) {
						list.MoveArrayElement (i, i + 1);
					}
				}
				if (GUILayout.Button ("^")) {
					if (i < list.arraySize) {
						list.MoveArrayElement (i, i - 1);
					}
				}
				if (GUILayout.Button ("+")) {
					manager.enableAddSceneToListByIndex (i);
				}
				if (GUILayout.Button ("-")) {
					manager.disableAddSceneToListByIndex (i);
				}
				if (GUILayout.Button ("º")) {
					manager.setSceneNumberInOrderByIndex (i);
				}
				if (expanded) {
					GUILayout.EndVertical ();
				} else {
					GUILayout.EndHorizontal ();
				}
				GUILayout.EndHorizontal ();
			}
			EditorGUILayout.Space ();
			GUILayout.EndVertical ();
		}       
	}

	void showSceneListElement (SerializedProperty list)
	{
		GUILayout.BeginVertical ("box");
		EditorGUILayout.PropertyField (list.FindPropertyRelative ("Name"));
		EditorGUILayout.PropertyField (list.FindPropertyRelative ("sceneDescription"));
		EditorGUILayout.PropertyField (list.FindPropertyRelative ("sceneNumber"));
		EditorGUILayout.PropertyField (list.FindPropertyRelative ("sceneImage"));
		EditorGUILayout.PropertyField (list.FindPropertyRelative ("addSceneToList"));
		EditorGUILayout.PropertyField (list.FindPropertyRelative ("fontSize"));
		GUILayout.EndVertical ();
	}
}
#endif