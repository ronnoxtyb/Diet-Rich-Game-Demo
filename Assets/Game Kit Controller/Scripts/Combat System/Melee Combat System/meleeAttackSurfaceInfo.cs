﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class meleeAttackSurfaceInfo : MonoBehaviour
{
	[Header ("Main Settings")]
	[Space]

	public bool surfaceEnabled = true;

	public string surfaceName;

	[Space]
	[Header ("Weapon Throw Settings")]
	[Space]

	public bool useOffsetTransformOnWeaponThrow;
	public Transform offsetTransformOnWeaponThrow;

//	public bool useOffsetOnWeaponThrow;
//	public Vector3 offsetOnWeaponThrow;

	public bool useOffsetDistanceOnWeaponThrow;
	public float offsetDistanceOnWeaponThrow;

	public bool disableInstaTeleportOnThisSurface;

	[Space]
	[Header ("Events Settings")]
	[Space]

	public bool useEventOnSurfaceDetected;
	public UnityEvent eventOnSurfaceDeteceted;

	[Space]
	[Header ("Remote Events Settings")]
	[Space]

	public bool useRemoteEvent;
	public List<string> remoteEventNameList = new List<string> ();

	[Space]

	public bool useRemoteEventOnWeapon;
	public List<string> remoteEventOnWeaponNameList = new List<string> ();

	[Space]
	[Header ("Debug")]
	[Space]

	public bool ignoreSurfaceActive;

	string originalSurfaceName;

	void Start ()
	{
		originalSurfaceName = surfaceName;
	}

	public string getSurfaceName ()
	{
		return surfaceName;
	}

	public bool isSurfaceEnabled ()
	{
		return surfaceEnabled && !ignoreSurfaceActive;
	}

	public void setNewSurfaceName (string newSurfaceName)
	{
		surfaceName = newSurfaceName;
	}

	public void setOriginalSurfaceName ()
	{
		setNewSurfaceName (originalSurfaceName);
	}

	public void setIgnoreSurfaceActiveState (bool state)
	{
		ignoreSurfaceActive = state;
	}

	public void setUseRemoteEventState (bool state)
	{
		useRemoteEvent = state;
	}

	public void setUseRemoteEventOnWeaponState (bool state)
	{
		useRemoteEventOnWeapon = state;
	}

	public void checkEventOnSurfaceDetected ()
	{
		if (useEventOnSurfaceDetected) {
			eventOnSurfaceDeteceted.Invoke ();
		}
	}
}