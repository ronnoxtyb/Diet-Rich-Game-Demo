﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class getInventoryObjectSystem : MonoBehaviour
{
	[Header ("Main Settings")]
	public string inventoryObjectName;
	public int objectAmount;

	public bool giveInventoryObjectList;
	public List<inventoryElementInfo> inventoryElementInfoList = new List<inventoryElementInfo> ();

	public bool useEventIfObjectStored;
	public UnityEvent eventIfObjectStored;

	public Transform positionToPlaceInventoryObject;

	public float maxRadiusToInstantiate;
	public float forceAmount;
	public float forceRadius;
	public ForceMode inventoryObjectForceMode;

	[Header ("Player Settings")]
	public GameObject currentPlayer;

	public void getCurrentPlayer (GameObject player)
	{
		currentPlayer = player;
	}

	public void giveObjectToPlayer ()
	{
		if (currentPlayer == null) {
			print ("WARNING: Make sure to configure a player or configure the events to send the player gameObject, so he can receive the object");
			return;
		}

		if (inventoryObjectName.Equals ("")) {
			print ("WARNING: Make sure to configure an inventory object name in order to find its info on the inventory list manager");
			return;
		}

		if (positionToPlaceInventoryObject == null) {
			positionToPlaceInventoryObject = transform;
		}
			
		if (giveInventoryObjectList) {
			for (int i = 0; i < inventoryElementInfoList.Count; i++) {
				applyDamage.giveInventoryObjectToCharacter (currentPlayer, inventoryElementInfoList [i].Name, inventoryElementInfoList [i].inventoryObjectAmount, 
					positionToPlaceInventoryObject, forceAmount, maxRadiusToInstantiate, inventoryObjectForceMode, forceRadius);
			}
		} else {
			if (applyDamage.giveInventoryObjectToCharacter (currentPlayer, inventoryObjectName, objectAmount, 
				    positionToPlaceInventoryObject, forceAmount, maxRadiusToInstantiate, inventoryObjectForceMode, forceRadius)) {

				if (useEventIfObjectStored) {
					eventIfObjectStored.Invoke ();
				}
			}
		}
	}

	[System.Serializable]
	public class inventoryElementInfo
	{
		public string Name;
		public int inventoryObjectAmount;
	}
}
