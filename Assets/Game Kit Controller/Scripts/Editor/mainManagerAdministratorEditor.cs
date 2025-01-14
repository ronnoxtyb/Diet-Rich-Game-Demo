﻿using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor (typeof(mainManagerAdministrator))]
public class mainManagerAdministratorEditor : Editor
{
	mainManagerAdministrator manager;

	void OnEnable ()
	{
		manager = (mainManagerAdministrator)target;
	}

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

		EditorGUILayout.Space ();

		if (GUILayout.Button ("Add Main Managers to Scene")) {
			manager.addAllMainManagersToScene ();
		}

		EditorGUILayout.Space ();

		if (GUILayout.Button ("Update Main Managers Info To Prefabs")) {
			manager.updateMainManagersInfoToPrefabs ();
		}

		EditorGUILayout.Space ();
	}
}
#endif