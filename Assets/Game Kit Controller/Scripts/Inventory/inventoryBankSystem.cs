﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class inventoryBankSystem : MonoBehaviour
{
	public bool usingInventoryBank;

	public float openBankDelay;

	public string animationName;
	public AudioClip openSound;
	public AudioClip closeSound;

	public List<inventoryListElement> inventoryListManagerList = new List<inventoryListElement> ();
	public List<inventoryInfo> bankInventoryList = new List<inventoryInfo> ();

	public string[] inventoryManagerListString;
	public List<inventoryManagerStringInfo> inventoryManagerStringInfoList = new List<inventoryManagerStringInfo> ();

	public bool useInventoryFromThisBank;

	public bool useAsVendorSystem;

	public bool useGeneralBuyPriceMultiplier;
	public float generalBuyPriceMultiplayerPercentage;

	public bool useGeneralSellPriceMultiplier;
	public float generalSellPriceMultiplayerPercentage;

	public bool infiniteVendorAmountAvailable;

	public bool attachToTransformActive;
	public Transform transformToAttach;
	public Vector3 localOffset;

	public Transform positionToSpawnObjects;

	public bool useEventOnInventoryEmpty;
	public UnityEvent eventOnInventoryEmpty;
	public bool repeatEventOnInventoryEmpty;
	bool eventOnInventoryEmptyActivated;

	public Animation mainAnimation;
	public AudioSource mainAudioSource;

	public inventoryListManager mainInventoryManager;

	public playerController playerControllerManager;

	GameObject currentPlayer;

	inventoryManager currentPlayerInventoryManager;

	vendorUISystem currentPlayerVendorUISystem;

	Coroutine openCoroutine;
	bool firstAnimationPlay = true;

	usingDevicesSystem usingDevicesManager;

	playerComponentsManager mainPlayerComponentsManager;

	void Start ()
	{
		if (mainAnimation == null) {
			mainAnimation = GetComponent<Animation> ();
		}

		if (mainAudioSource == null) {
			mainAudioSource = GetComponent<AudioSource> ();
		}

		checkGetMainInventoryManager ();

		setInventoryFromInventoryListManager ();

		if (attachToTransformActive && transformToAttach != null) {
			transform.SetParent (transformToAttach);
			transform.localPosition = Vector3.zero + localOffset;
		}
	}

	public void setUsingInventoryBankState (bool state)
	{
		usingInventoryBank = state;

		if (!usingInventoryBank) {
			checkEventOnInventoryEmpty ();
		}
	}

	public void activateInventoryBank ()
	{
		if (openCoroutine != null) {
			StopCoroutine (openCoroutine);
		}

		openCoroutine = StartCoroutine (openOrCloseInventoryBank ());
	}

	IEnumerator openOrCloseInventoryBank ()
	{
		usingInventoryBank = !usingInventoryBank;

		playAnimation (usingInventoryBank);

		playSound (usingInventoryBank);

		playerControllerManager.setUsingDeviceState (usingInventoryBank);

		if (usingInventoryBank) {
			yield return new WaitForSeconds (openBankDelay);
		}
			
		if (useAsVendorSystem) {
			bool cancelOpenVendorMenu = false;

			if (usingInventoryBank) {
				currentPlayerInventoryManager.setCurrentVendorSystem (gameObject);

				if (usingDevicesManager != null) {
					if (!usingDevicesManager.existInDeviceList (gameObject)) {
						activateInventoryBank ();
						cancelOpenVendorMenu = true;
					}
				}
			}

			if (!cancelOpenVendorMenu) {
				currentPlayerVendorUISystem.openOrCloseVendorMenu (usingInventoryBank);
			}
		} else {
			bool cancelOpenInventoryBank = false;

			if (usingInventoryBank) {
				currentPlayerInventoryManager.setCurrentInventoryBankSystem (gameObject);

				if (usingDevicesManager != null) {
					if (!usingDevicesManager.existInDeviceList (gameObject)) {
						activateInventoryBank ();
						cancelOpenInventoryBank = true;
					}
				}
			}

			if (!cancelOpenInventoryBank) {
				currentPlayerInventoryManager.openOrCloseInventoryBankMenu (usingInventoryBank);
			}
		}

		if (!usingInventoryBank) {
			checkEventOnInventoryEmpty ();
		}
	}

	public void checkEventOnInventoryEmpty ()
	{
		if (useEventOnInventoryEmpty) {
			if (!eventOnInventoryEmptyActivated || repeatEventOnInventoryEmpty) {
				if (bankInventoryList.Count == 0) {
					eventOnInventoryEmpty.Invoke ();

					eventOnInventoryEmptyActivated = true;
				}
			}
		}
	}

	public void playAnimation (bool playForward)
	{
		if (mainAnimation != null) {
			if (playForward) {
				if (!mainAnimation.IsPlaying (animationName)) {
					mainAnimation [animationName].normalizedTime = 0;
				}

				mainAnimation [animationName].speed = 1;
			} else {
				if (!mainAnimation.IsPlaying (animationName)) {
					mainAnimation [animationName].normalizedTime = 1;
				}

				mainAnimation [animationName].speed = -1; 
			}

			if (firstAnimationPlay) {
				mainAnimation.Play (animationName);
				firstAnimationPlay = false;
			} else {
				mainAnimation.CrossFade (animationName);
			}
		}
	}

	public void playSound (bool state)
	{
		if (mainAudioSource != null) {
			GKC_Utils.checkAudioSourcePitch (mainAudioSource);
			if (state) {
				mainAudioSource.PlayOneShot (openSound);
			} else {
				mainAudioSource.PlayOneShot (closeSound);
			}
		}
	}

	public void setCurrentPlayer (GameObject player)
	{
		currentPlayer = player;

		if (currentPlayer != null) {

			mainPlayerComponentsManager = currentPlayer.GetComponent<playerComponentsManager> ();

			currentPlayerInventoryManager = mainPlayerComponentsManager.getInventoryManager ();

			playerControllerManager = mainPlayerComponentsManager.getPlayerController ();

			usingDevicesManager = mainPlayerComponentsManager.getUsingDevicesSystem ();

			currentPlayerVendorUISystem = mainPlayerComponentsManager.getVendorUISystem ();
		}
	}

	public void setInventoryFromInventoryListManager ()
	{
		int inventoryListManagerListCount = inventoryListManagerList.Count;

		List<inventoryCategoryInfo> inventoryCategoryInfoList = mainInventoryManager.inventoryCategoryInfoList;

		int inventoryCategoryInfoListCount = inventoryCategoryInfoList.Count;

		for (int i = 0; i < inventoryListManagerListCount; i++) {

			inventoryListElement currentElement = inventoryListManagerList [i];

			if (currentElement.addObjectToList) {

				bool inventoryInfoLocated = false;
				
				if (inventoryCategoryInfoListCount > currentElement.categoryIndex) {

					inventoryCategoryInfo currentCategoryInfo = inventoryCategoryInfoList [currentElement.categoryIndex];

					if (currentCategoryInfo.inventoryList.Count > currentElement.elementIndex) {

						inventoryInfo currentInventoryInfo = currentCategoryInfo.inventoryList [currentElement.elementIndex];

						if (currentInventoryInfo != null) {
							inventoryInfo newInventoryInfo = new inventoryInfo (currentInventoryInfo);
							newInventoryInfo.Name = currentInventoryInfo.Name;
							newInventoryInfo.amount = currentElement.amount;

							if (useAsVendorSystem) {
								float buyPrice = currentElement.vendorPrice;

								if (useGeneralBuyPriceMultiplier) {
									buyPrice = currentInventoryInfo.vendorPrice * generalBuyPriceMultiplayerPercentage;
								}
								newInventoryInfo.vendorPrice = buyPrice;

								float sellPrice = currentElement.sellPrice;

								if (useGeneralBuyPriceMultiplier) {
									sellPrice = currentInventoryInfo.sellPrice * generalSellPriceMultiplayerPercentage;
								}
								newInventoryInfo.sellPrice = sellPrice;

								newInventoryInfo.infiniteVendorAmountAvailable = infiniteVendorAmountAvailable || currentElement.infiniteAmount;

								if (currentElement.useMinLevelToBuy) {
									newInventoryInfo.useMinLevelToBuy = true;
									newInventoryInfo.minLevelToBuy = currentElement.minLevelToBuy;
								}

								newInventoryInfo.spawnObject = currentElement.spawnObject;
							}

							bankInventoryList.Add (newInventoryInfo);
						}

						inventoryInfoLocated = true;
					} 
				}

				if (!inventoryInfoLocated) {
					print ("WARNING: The inventory bank system configured on the object " + gameObject.name + " hasn't the proper inventory objects to pick by the player properly configured. " +
					"Make sure the inventory list on this component has the correct fields ");
				}
			}
		}
	}

	public string mainInventoryManagerName = "Main Inventory Manager";

	bool mainInventoryManagerLocated;

	void checkGetMainInventoryManager ()
	{
		if (!mainInventoryManagerLocated) {
			if (mainInventoryManager == null) {
				GKC_Utils.instantiateMainManagerOnSceneWithType (mainInventoryManagerName, typeof(inventoryListManager));

				mainInventoryManager = FindObjectOfType<inventoryListManager> ();

				mainInventoryManagerLocated = true;
			}
		}
	}

	public void getInventoryListManagerList ()
	{
		checkGetMainInventoryManager ();

		if (mainInventoryManager != null) {
			inventoryManagerListString = new string[mainInventoryManager.inventoryCategoryInfoList.Count];

			for (int i = 0; i < inventoryManagerListString.Length; i++) {
				inventoryManagerListString [i] = mainInventoryManager.inventoryCategoryInfoList [i].Name;
			}

			inventoryManagerStringInfoList.Clear ();

			for (int i = 0; i < mainInventoryManager.inventoryCategoryInfoList.Count; i++) {

				inventoryManagerStringInfo newInventoryManagerStringInfoo = new inventoryManagerStringInfo ();
				newInventoryManagerStringInfoo.Name = mainInventoryManager.inventoryCategoryInfoList [i].Name;

				newInventoryManagerStringInfoo.inventoryManagerListString = new string[mainInventoryManager.inventoryCategoryInfoList [i].inventoryList.Count];

				for (int j = 0; j < mainInventoryManager.inventoryCategoryInfoList [i].inventoryList.Count; j++) {
					string newName = mainInventoryManager.inventoryCategoryInfoList [i].inventoryList [j].Name;
					newInventoryManagerStringInfoo.inventoryManagerListString [j] = newName;
				}

				inventoryManagerStringInfoList.Add (newInventoryManagerStringInfoo);
			}

			updateComponent ();
		}
	}

	public void setInventoryObjectListNames ()
	{
		for (int i = 0; i < inventoryListManagerList.Count; i++) {
			inventoryListManagerList [i].Name = inventoryListManagerList [i].inventoryObjectName;
		}

		updateComponent ();
	}

	public void addNewInventoryObjectToInventoryListManagerList ()
	{
		inventoryListElement newInventoryListElement = new inventoryListElement ();

		newInventoryListElement.Name = "New Object";

		inventoryListManagerList.Add (newInventoryListElement);

		updateComponent ();
	}

	public Transform getPositionToSpawnObjects ()
	{
		return positionToSpawnObjects;
	}

	public List<inventoryInfo> getBankInventoryList ()
	{
		return bankInventoryList;
	}

	public void updateComponent ()
	{
		GKC_Utils.updateComponent (this);
	}

	[System.Serializable]
	public class inventoryManagerStringInfo
	{
		public string Name;
		public string[] inventoryManagerListString;
	}
}