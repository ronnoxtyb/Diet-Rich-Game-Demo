﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System;
using System.Reflection;

using UnityEngine.Events;
using UnityEngine.EventSystems;

public class inventoryManager : MonoBehaviour
{
	public bool inventoryEnabled;
	public bool openInventoryMenuEnabled = true;
	public List<inventoryInfo> inventoryList = new List<inventoryInfo> ();
	public GameObject inventoryPanel;
	public GameObject inventoryListContent;
	public ScrollRect inventoryListScrollRect;
	public GameObject objectIcon;

	public GameObject equipButton;
	public GameObject unequipButton;

	public GameObject examineObjectPanel;

	public Text examineObjectName;
	public Text examineObjectDescription;
	public GameObject takeObjectInExaminePanelButton;

	public bool examiningObject;

	public Text currentObjectName;
	public Text currentObjectInfo;
	public RawImage objectImage;
	public Color buttonUsable;
	public Color buttonNotUsable;
	public bool infiniteSlots;
	public int inventorySlotAmount;
	public bool infiniteAmountPerSlot;
	public int amountPerSlot;

	public bool dropSingleObjectOnInfiniteAmount = true;

	public Camera inventoryCamera;
	public Transform lookObjectsPosition;
	public float rotationSpeed;

	public bool inventoryOpened;

	public GameObject emptyInventoryPrefab;

	public bool showMessageWhenObjectUsed = true;

	public GameObject usedObjectMessage;
	public float usedObjectMessageTime;
	[TextArea (1, 10)]
	public string unableToUseObjectMessage;
	[TextArea (1, 10)]
	public string nonNeededAmountAvaliable;
	[TextArea (1, 10)]
	public string objectNotFoundMessage;
	[TextArea (1, 10)]
	public string cantUseThisObjectHereMessage;

	public GameObject fullInventoryMessage;
	public float fullInventoryMessageTime = 2;

	public GameObject combinedObjectMessage;
	public float combineObjectMessageTime;
	[TextArea (1, 10)]
	public string unableToCombineMessage;
	[TextArea (1, 10)]
	public string notEnoughSpaceToCombineMessage;

	[TextArea (1, 10)]
	public string canBeCombinedButObjectIsFullMessage = "These objects can be combined, but -OBJECTNAME- is full";

	[TextArea (1, 10)]
	public string weightLimitReachedMessage;

	[TextArea (1, 10)]
	public string objectTooMuchHeavyToCarryMessage;

	public bool checkWeightLimitToPickObjects;
	public inventoryWeightManager mainInventoryWeightManager;

	public Scrollbar inventorySlotsScrollbar;
	public Scrollbar inventoryObjectInforScrollbar;

	public bool combineElementsAtDrop;
	public float zoomSpeed;
	public float maxZoomValue;
	public float minZoomValue;
	public inventoryInfo currentInventoryObject;

	public inventoryInfo firstObjectToCombine;
	public inventoryInfo secondObjectToCombine;
	public bool usedByAI;

	public inventoryBankUISystem mainInventoryBankUISystem;

	public vendorUISystem mainVendorUISystem;

	public playerWeaponsManager weaponsManager;
	public meleeWeaponsGrabbedManager mainMeleeWeaponsGrabbedManager;
	public playerController playerControllerManager;
	public menuPause pauseManager;
	public playerInputManager playerInput;
	public inventoryListManager mainInventoryListManager;
	public usingDevicesSystem usingDevicesManager;

	public string mainInventoryManagerName = "Main Inventory Manager";

	public GameObject mainInventoryManagerPrefab;

	public inventoryQuickAccessSlotsSystem mainInventoryQuickAccessSlotsSystem;

	public List<inventoryListElement> inventoryListManagerList = new List<inventoryListElement> ();
	public List<inventoryMenuIconElement> menuIconElementList = new List<inventoryMenuIconElement> ();

	public string[] inventoryManagerListString;
	public List<inventoryManagerStringInfo> inventoryManagerStringInfoList = new List<inventoryManagerStringInfo> ();

	public bool loadCurrentPlayerInventoryFromSaveFile;
	public bool saveCurrentPlayerInventoryToSaveFile;

	public float configureNumberObjectsToUseRate = 0.4f;
	public float fasterNumberObjectsToUseRate = 0.1f;
	public float waitTimeToUseFasterNumberObjectsToUseRate = 1;

	public List<inventoryQuickAccessSlotElement.quickAccessSlotInfo> quickAccessSlotInfoList = new List<inventoryQuickAccessSlotElement.quickAccessSlotInfo> ();

	public bool storePickedWeaponsOnInventory;


	public bool equipWeaponsWhenPicked;
	public bool equipPickedWeaponOnlyItNotPreviousWeaponEquipped;

	public bool changeToMeleeWeaponsModeWhenPickingMeleeWeapon;
	public bool changeToFireWeaponsModeWhenPickingFireWeapon;

	public bool useBlurUIPanel = true;

	public bool examineObjectBeforeStoreEnabled;

	public inventoryMenuPanelsSystem mainInventoryMenuPanelsSystem;

	inventoryInfo duplicateObject;
	GameObject objectInCamera;
	int objectsAmount;

	int loop = 0;

	bool enableRotation;

	bool zoomingIn;
	bool zoomingOut;
	float originalFov;

	GameObject currentUseInventoryGameObject;
	Coroutine resetCameraFov;
	Coroutine inventoryFullCoroutine;

	inventoryInfo currentPickUpObjectInfo;
	int inventoryAmountNotTaken = 0;

	public Image useButtonImage;
	public Image equipButtonImage;
	public Image unequipButtonImage;
	public Image dropButtonImage;
	public Image combineButtonImage;
	public Image examineButtonImage;
	public Image discardButtonImage;
	public Image dropAllUnitsObjectButtonImage;

	public bool selectFirstInventoryObjectWhenOpeningMenu = true;

	bool combiningObjects;

	inventoryInfo firstObjectCombinedOnNewBehavior;
	inventoryInfo secondObjectCombinedOnNewBehavior;

	float lastTimeConfigureNumberOfObjects;

	bool addingObjectToUse;
	bool removinvObjectToUse;
	float lastTimeAddObjectToUse;
	float lastTimeRemoveObjectToUse;
	bool useFasterNumberObjectsToUseRateActive;

	inventoryObject currentInventoryObjectManager;
	float currentMaxZoomValue;
	float currentMinZoomValue;


	bool initializingInventory;
	Vector2 axisValues;

	IKWeaponSystem currentIKWeaponSystem;

	float maxRadiusToInstantiate = 1;

	public bool setTotalAmountWhenDropObject;

	int numberOfObjectsToUse = 1;

	public bool useOnlyWhenNeededAmountToUseObject;
	public bool activeNumberOfObjectsToUseMenu;

	public GameObject numberOfObjectsToUseMenu;
	public RectTransform numberOfObjectsToUseMenuRectTransform;
	public RectTransform numberOfObjectsToUseMenuPosition;
	public RectTransform numberOfObjectsToDropMenuPosition;

	public Text numberOfObjectsToUseText;

	useInventoryObject currentUseInventoryObject;

	Coroutine objectMessageCoroutine;

	GameObject previousMessagePanel;
	pickUpObject currentPickupObject;

	public float distanceToPlaceObjectInCamera = 10;
	public float placeObjectInCameraSpeed = 10;
	public int numberOfRotationsObjectInCamera = 3;
	public float placeObjectInCameraRotationSpeed = 0.02f;
	public float extraCameraFovOnExamineObjects = 20;

	Coroutine objectInCameraPositionCoroutine;
	Coroutine objectInCameraRotationCoroutine;

	bool activatingDualWeaponSlot;
	string currentRighWeaponName;
	string currentLeftWeaponName;

	public bool showInventoryObjectsName = true;

	public bool showObjectAmountIfEqualOne = true;

	public bool useInventoryOptionsOnSlot;
	public RectTransform inventoryOptionsOnSlotPanel;
	public inventorySlotOptionsButtons mainInventorySlotOptionsButtons;

	public bool inventoryOptionsOnSlotPanelActive;
	RectTransform inventoryOptionsOnSlotPanelTargetToFollow;

	public UnityEvent eventOnInventoryInitialized;
	public UnityEvent eventOnClickInventoryChange;
	public UnityEvent eventOnInventorySlotSelected;
	public UnityEvent eventOnInventorySlotUnSelected;
	public UnityEvent eventOnInventoryClosed;
	public UnityEvent eventOnInventoryOpened;

	public UnityEvent eventOnInventoryListChange;

	public bool useAudioSounds;
	public AudioSource mainAudioSource;

	public bool useEventIfSystemDisabled;
	public UnityEvent eventIfSystemDisabled;

	float currentTimeTime;

	int customAmountToUse = 1;

	GameObject lastObjectDropped;

	public bool showDebugPrint;

	//Editor variables
	public bool showElementSettings;

	public bool showAllSettings;
	public bool showWeightSettings;
	public bool showWeaponsSettings;
	public bool showExamineSettings;
	public bool showMessagesSettings;
	public bool showSoundsSettings;
	public bool showOthersSettings;
	public bool showEventSettings;
	public bool showSaveLoadSettings;
	public bool showDebugSettings;

	public void initializeInventoryValues ()
	{
		checkMainInventoryManager ();

		if (!inventoryEnabled) {
			checkEventOnSystemDisabled ();

			return;
		}

		initializingInventory = true;
	}

	public void setNewInventoryListManagerList (List<inventoryListElement> newList)
	{
		inventoryListManagerList = newList;
	}

	void Start ()
	{
		if (!inventoryEnabled) {
			return;
		}

		if (usedByAI) {
			return;
		}

		objectIcon.SetActive (false);

		setInventoryFromInventoryListManager ();

		setInventory (true);

		inventoryPanel.SetActive (false);

		disableCurrentObjectInfo ();

		originalFov = inventoryCamera.fieldOfView;

		inventoryCamera.enabled = false;

		mainInventoryQuickAccessSlotsSystem.initializeQuickAccessSlots ();

		int inventoryListCount = inventoryList.Count;

		if (storePickedWeaponsOnInventory) {
			for (int i = 0; i < inventoryListCount; i++) {
				inventoryInfo currentInventoryInfo = inventoryList [i];

				if (currentInventoryInfo.isEquiped) {
					if (currentInventoryInfo.canBeEquiped) {

						currentInventoryObject = currentInventoryInfo;

						equipCurrentObject ();
					} else {
						currentInventoryInfo.isEquiped = false;
					}
				}
			}
		}

		bool weaponNotFound = false;

		inventoryListCount = inventoryList.Count;

		for (int i = 0; i < inventoryListCount; i++) {
			inventoryInfo currentInventoryInfo = inventoryList [i];

			if (currentInventoryInfo.isWeapon) {
				if (!currentInventoryInfo.isMeleeWeapon) {
					IKWeaponSystem newIKWeaponSystem = weaponsManager.getIKWeaponSystem (currentInventoryInfo.Name);

					if (newIKWeaponSystem != null) {
						currentInventoryInfo.mainWeaponObjectInfo = newIKWeaponSystem;

						if (currentInventoryInfo.projectilesInMagazine > -1) {
							newIKWeaponSystem.setInitialProjectilesInMagazine (currentInventoryInfo.projectilesInMagazine);
						}
					} else {
						currentInventoryInfo.resetInventoryInfo ();

						weaponNotFound = true;
					}
				}

				if (currentInventoryInfo.isMeleeWeapon) {
					currentInventoryInfo.mainWeaponObjectInfo = mainMeleeWeaponsGrabbedManager.getWeaponGrabbedByName (currentInventoryInfo.Name);
				}
			}
		}

		if (weaponNotFound) {
			checkEmptySlots ();

			updateFullInventorySlots ();
		}

		checkIfWeaponUseAmmoFromInventory ();

		initializingInventory = false;

		checkInventoryOptionsOnSlot (false);

		checkEventOnInventoryInitialized ();
	}

	void Update ()
	{
		if (usedByAI) {
			return;
		}

		if (inventoryEnabled) {

			if (inventoryOpened) {
				if (enableRotation) {
					axisValues = playerInput.getPlayerMouseAxis ();
				} else if (examiningObject) {
					axisValues = playerInput.getPlayerMovementAxis ();
				}

				if (enableRotation || examiningObject) {
					objectInCamera.transform.Rotate (inventoryCamera.transform.up, -Mathf.Deg2Rad * rotationSpeed * axisValues.x, Space.World);
					objectInCamera.transform.Rotate (inventoryCamera.transform.right, Mathf.Deg2Rad * rotationSpeed * axisValues.y, Space.World);
				}

				if (currentInventoryObjectManager && currentInventoryObjectManager.useZoomRange) {
					currentMaxZoomValue = currentInventoryObjectManager.maxZoom;
					currentMinZoomValue = currentInventoryObjectManager.minZoom;	
				} else {
					currentMaxZoomValue = maxZoomValue;
					currentMinZoomValue = minZoomValue;
				}

				if (zoomingIn) {
					if (inventoryCamera.fieldOfView > currentMaxZoomValue) {
						inventoryCamera.fieldOfView -= getDeltaTime () * zoomSpeed;
					} else {
						inventoryCamera.fieldOfView = currentMaxZoomValue;
					}
				}

				if (zoomingOut) {
					if (inventoryCamera.fieldOfView < currentMinZoomValue) {
						inventoryCamera.fieldOfView += getDeltaTime () * zoomSpeed;
					} else {
						inventoryCamera.fieldOfView = currentMinZoomValue;
					}
				}

				currentTimeTime = getTimeTime ();

				if (addingObjectToUse) {
					if (!useFasterNumberObjectsToUseRateActive) {
						if (currentTimeTime > configureNumberObjectsToUseRate + lastTimeAddObjectToUse) {

							lastTimeAddObjectToUse = currentTimeTime;

							addObjectToUse ();
						}

						if (currentTimeTime > lastTimeConfigureNumberOfObjects + waitTimeToUseFasterNumberObjectsToUseRate) {
							useFasterNumberObjectsToUseRateActive = true;
						}
					} else {
						if (currentTimeTime > fasterNumberObjectsToUseRate + lastTimeAddObjectToUse) {
							lastTimeAddObjectToUse = currentTimeTime;

							addObjectToUse ();
						}
					}
				}

				if (removinvObjectToUse) {
					if (!useFasterNumberObjectsToUseRateActive) {
						if (currentTimeTime > configureNumberObjectsToUseRate + lastTimeRemoveObjectToUse) {

							lastTimeRemoveObjectToUse = currentTimeTime;

							removeObjectToUse ();
						}

						if (currentTimeTime > lastTimeConfigureNumberOfObjects + waitTimeToUseFasterNumberObjectsToUseRate) {
							useFasterNumberObjectsToUseRateActive = true;
						}
					} else {
						if (currentTimeTime > fasterNumberObjectsToUseRate + lastTimeRemoveObjectToUse) {
							lastTimeRemoveObjectToUse = currentTimeTime;

							removeObjectToUse ();
						}
					}
				}

				mainInventoryQuickAccessSlotsSystem.updateInventoryOpenedState ();

				if (inventoryOptionsOnSlotPanelActive) {
					inventoryOptionsOnSlotPanel.position = inventoryOptionsOnSlotPanelTargetToFollow.position;
				}
			} else {
				mainInventoryQuickAccessSlotsSystem.updateQuickAccessInputKeysState ();
			}
		}
	}

	//START INVENTORY MANAGEMENT ELEMENTS
	public void setInventorySlotAmountValue (int newValue)
	{
		inventorySlotAmount = newValue;
	}

	public void setInfiniteSlotsState (bool state)
	{
		infiniteSlots = state;
	}

	public void setInventoryFromInventoryListManager ()
	{
		int inventoryListManagerListCount = inventoryListManagerList.Count;

		List<inventoryCategoryInfo> inventoryCategoryInfoList = mainInventoryListManager.inventoryCategoryInfoList;

		int inventoryCategoryInfoListCount = inventoryCategoryInfoList.Count;

		for (int i = 0; i < inventoryListManagerListCount; i++) {
			inventoryListElement currentElementInfo = inventoryListManagerList [i];

			if (currentElementInfo.addInventoryObject) {

				bool inventoryInfoLocated = false;

				if (inventoryCategoryInfoListCount > currentElementInfo.categoryIndex) {

					inventoryCategoryInfo currentCategoryInfo = inventoryCategoryInfoList [currentElementInfo.categoryIndex];

					if (currentCategoryInfo.inventoryList.Count > currentElementInfo.elementIndex) {

						inventoryInfo currentInventoryInfo = currentCategoryInfo.inventoryList [currentElementInfo.elementIndex];

						if (currentInventoryInfo != null) {
							inventoryInfo newInventoryInfo = new inventoryInfo (currentInventoryInfo);

							newInventoryInfo.Name = currentElementInfo.Name;

							if (newInventoryInfo.storeTotalAmountPerUnit && newInventoryInfo.amountPerUnit > 0) {
								newInventoryInfo.amountPerUnit = 0;
							} 

							newInventoryInfo.amount = currentElementInfo.amount;

							newInventoryInfo.isEquiped = currentElementInfo.isEquipped;

							newInventoryInfo.projectilesInMagazine = currentElementInfo.projectilesInMagazine;

							inventoryList.Add (newInventoryInfo);
						}

						inventoryInfoLocated = true;
					}
				}

				if (!inventoryInfoLocated) {
					print ("WARNING: The inventory system is trying to load an inventory object with an index higher than the count of the inventory list " +
					"for the object called " + currentElementInfo.Name + ". Make sure to configure the initial inventory properly");
				}
			}
		}

		for (int i = inventoryListManagerList.Count - 1; i >= 0; i--) {
			if (!inventoryListManagerList [i].addInventoryObject) {
				inventoryListManagerList.RemoveAt (i);
			}
		}
	}

	public void setInventory (bool creatingInventoryIcons)
	{
		checkInventoryAmountPerSpace ();

		checkRemainigEmptyInventorySlots ();

		if (creatingInventoryIcons) {
			createInventoryIcons ();
		}
	}

	public void checkInventoryAmountPerSpace ()
	{
		if (infiniteAmountPerSlot) {
			return;
		}

		loop = 0;

		for (int i = 0; i < inventoryList.Count; i++) {
			inventoryInfo currentInventoryInfo = inventoryList [i];

			if (currentInventoryInfo.amount > amountPerSlot) {
				while (currentInventoryInfo.amount > amountPerSlot) {
					loop++;

					if (loop > 100) {
						//print ("loop loop");
						return;
					}

					int amount = 0;

					if (currentInventoryInfo.amount - amountPerSlot > 0) {
						amount = currentInventoryInfo.amount - amountPerSlot;
						currentInventoryInfo.amount = amountPerSlot;
					} else {
						amount = currentInventoryInfo.amount;
					}

					i = reOrderInventoryList (currentInventoryInfo, amount, i);
				}
			}
		}
	}

	public int reOrderInventoryList (inventoryInfo objectInfo, int amount, int index)
	{
		//print ("reordenate");
		int numberOfSlots = inventorySlotAmount;
		if (infiniteSlots) {
			numberOfSlots = inventoryList.Count * numberOfSlots;
		}

		if (inventoryList.Count >= numberOfSlots) {
			//print (amount);
			bool amountAdded = false;

			if (getNumberOfFreeSlots () > 0) {
				for (int i = 0; i < inventoryList.Count; i++) {
					inventoryInfo currentInventoryInfo = inventoryList [i];

					if (currentInventoryInfo.amount == 0 &&
					    currentInventoryInfo.inventoryGameObject != objectInfo.inventoryGameObject &&
					    currentInventoryInfo != objectInfo &&
					    !amountAdded) {

						addObjectToInventory (objectInfo, amount, i);

						inventoryAmountNotTaken -= amount;

						amountAdded = true;
					}
				}
			}

			if (currentPickUpObjectInfo != null) {
				if (currentPickUpObjectInfo.inventoryGameObject == objectInfo.inventoryGameObject) {
					inventoryAmountNotTaken += amount;
					//print (inventoryAmountNotTaken);
				}
			}
		} else {
			int newIndexPosition = 0;
			if (index == inventoryList.Count - 1) {
				newIndexPosition = inventoryList.Count;
			} else {
				newIndexPosition = index + 1;
			}

			index++;
			duplicateObject = new inventoryInfo (objectInfo);
			duplicateObject.amount = amount;
			inventoryList.Insert (newIndexPosition, duplicateObject);
		}

		return index;
	}

	public void checkRemainigEmptyInventorySlots ()
	{
		if (infiniteSlots) {
			return;
		}

		loop = 0;

		if (inventoryList.Count < inventorySlotAmount) {
			while (inventoryList.Count < inventorySlotAmount) {
				loop++;

				if (loop > 100) {
					//print ("loop loop");
					return;
				}

				addNewInventorySlot ();
			}
		}
	}

	public bool isInventoryEmpty ()
	{
		if (inventoryList.Count == 0) {
			return true;
		} else {
			for (int i = 0; i < inventoryList.Count; i++) {
				if (inventoryList [i].amount > 0) {
					return false;
				}
			}
		}

		return true;
	}

	public bool isAnyInventoryWeaponEquipped ()
	{
		if (inventoryList.Count == 0) {
			return false;
		} else {
			for (int i = 0; i < inventoryList.Count; i++) {
				if (inventoryList [i].isEquiped && inventoryList [i].isWeapon) {
					return true;
				}
			}
		}

		return false;
	}

	public void addNewInventorySlot ()
	{
		inventoryInfo newEmptyInventoryObject = new inventoryInfo ();

		newEmptyInventoryObject.Name = "Empty Slot";

		newEmptyInventoryObject.objectInfo = "It is an empty slot";

		inventoryList.Add (newEmptyInventoryObject);
	}

	public void checkEmptySlots ()
	{
		int numberOfObjects = inventoryList.Count;

		int currentObjectIndex = 0;

		for (int i = 0; i < inventoryList.Count; i++) {
			if (numberOfObjects > currentObjectIndex) {

				inventoryInfo currentInventoryInfo = inventoryList [i];

				if (currentInventoryInfo.amount == 0) {
					//move the object to the last element of the list for those that has amount = 0
					inventoryList.Add (currentInventoryInfo);

					inventoryList.RemoveAt (i);

					i--;
				}

				currentObjectIndex++;
			}
		}

		updateFullInventorySlots ();
	}

	public void createInventoryIcons ()
	{
		int inventoryListCount = inventoryList.Count;

		for (int i = 0; i < inventoryListCount; i++) {
			inventoryInfo currentInventoryInfo = inventoryList [i];

			if (currentInventoryInfo.button != null) {
				Destroy (currentInventoryInfo.button.gameObject);
			}
		}

		inventoryListCount = inventoryList.Count;

		for (int i = 0; i < inventoryListCount; i++) {
			createInventoryIcon (inventoryList [i], i);
		}
	}

	public void createInventoryIcon (inventoryInfo currentInventoryInfo, int index)
	{
		GameObject newIconButton = (GameObject)Instantiate (objectIcon, Vector3.zero, Quaternion.identity, inventoryListContent.transform);

		if (!newIconButton.activeSelf) {
			newIconButton.SetActive (true);
		}

		newIconButton.transform.localScale = Vector3.one;
		newIconButton.transform.localPosition = Vector3.zero;

		inventoryMenuIconElement menuIconElement = newIconButton.GetComponent<inventoryMenuIconElement> ();

		if (showInventoryObjectsName) {
			menuIconElement.iconName.text = currentInventoryInfo.Name;
		} else {
			menuIconElement.iconName.text = "";
		}

		if (currentInventoryInfo.inventoryGameObject != null) {
			menuIconElement.icon.texture = currentInventoryInfo.icon;
		} else {
			menuIconElement.icon.texture = null;
		}

		bool slotIsActive = currentInventoryInfo.amount > 0;

		menuIconElement.activeSlotContent.SetActive (slotIsActive);
		menuIconElement.emptySlotContent.SetActive (!slotIsActive);

		bool showRegularAmount = true;

		if (currentInventoryInfo.amountPerUnit > 0) {
			if (currentInventoryInfo.showAmountPerUnitInAmountText) {

				menuIconElement.amount.text = (currentInventoryInfo.amount * currentInventoryInfo.amountPerUnit).ToString ();
				menuIconElement.amountPerUnitPanel.SetActive (false);

				showRegularAmount = false;
			} else {
				menuIconElement.amountPerUnitPanel.SetActive (true);
				menuIconElement.amountPerUnitText.text = currentInventoryInfo.amountPerUnit.ToString ();
			}
		} else {
			menuIconElement.amountPerUnitPanel.SetActive (false);
		}

		if (showRegularAmount) {
			if (currentInventoryInfo.infiniteAmount) {
				menuIconElement.amount.text = "";

				menuIconElement.infiniteAmountIcon.SetActive (true);
			} else {
				if (currentInventoryInfo.amount > 1 || showObjectAmountIfEqualOne) {
					menuIconElement.amount.text = currentInventoryInfo.amount.ToString ();
				} else {
					menuIconElement.amount.text = "";
				}

				if (menuIconElement.infiniteAmountIcon.activeSelf) {
					menuIconElement.infiniteAmountIcon.SetActive (false);
				}
			}
		}

		menuIconElement.pressedIcon.SetActive (false);
		newIconButton.name = "Inventory Object-" + (index + 1);
		Button button = menuIconElement.button;

		currentInventoryInfo.button = button;
		currentInventoryInfo.menuIconElement = menuIconElement;
		menuIconElementList.Add (menuIconElement);
	}

	public bool existInventoryInfoFromName (string objectName)
	{
		return mainInventoryListManager.existInventoryInfoFromName (objectName);
	}

	public bool existInPlayerInventoryFromName (string objectName)
	{
		int inventoryListCount = inventoryList.Count;

		for (int i = 0; i < inventoryListCount; i++) {
			if (inventoryList [i].Name.Equals (objectName)) {
				return true;
			}
		}

		return false;
	}

	public inventoryInfo getInventoryInfoByName (string objectName)
	{
		return mainInventoryListManager.getInventoryInfoFromName (objectName);
	}

	public void setCustomAmountToUse (float customAmount)
	{
		customAmountToUse = (int)customAmount;
	}

	public void tryToPickUpObjectByName (string objectName)
	{
		inventoryInfo inventoryObjectToPick = mainInventoryListManager.getInventoryInfoFromName (objectName);

		if (inventoryObjectToPick != null) {
			if (showDebugPrint) {
				print (customAmountToUse);
			}

			inventoryObjectToPick.amount = customAmountToUse;

			tryToPickUpObject (inventoryObjectToPick);

			customAmountToUse = 1;
		}
	}

	public int tryToPickUpObjectByName (string objectName, int objectAmount)
	{
		inventoryInfo inventoryObjectToPick = mainInventoryListManager.getInventoryInfoFromName (objectName);

		if (inventoryObjectToPick != null) {
			inventoryObjectToPick.amount = objectAmount;

			return tryToPickUpObject (inventoryObjectToPick);
		}

		return -1;
	}

	public int tryToPickUpObject (inventoryInfo inventoryObjectToPickup)
	{
		lastInventoryObjectPickedName = "";

		inventoryAmountNotTaken = 0;

		currentPickUpObjectInfo = new inventoryInfo (inventoryObjectToPickup);

		if (showDebugPrint) {
			print ("current object to pick " + currentPickUpObjectInfo.Name + " with amount " + currentPickUpObjectInfo.amount);
		}

		int amountToTake = currentPickUpObjectInfo.amount;

		bool canCheckToPickupObject = false;

		//Check for the weight of the objects to pick
		if (checkWeightLimitToPickObjects && mainInventoryWeightManager != null) {
			int amountWhichCanBeTaken = mainInventoryWeightManager.checkIfCanCarryObjectWeight (currentPickUpObjectInfo);

			if (showDebugPrint) {
				print ("amount which can be taken " + amountWhichCanBeTaken);
			}

			bool objectNotAbleToBeingCarried = false;

			string objectWeightMessage = "";

			//If the amount that can be taken from the total amount is higher than 0, get the exact amount to pick
			if (amountWhichCanBeTaken > 0) {
				if (amountWhichCanBeTaken != amountToTake) {
					amountToTake = amountWhichCanBeTaken;
				}

				inventoryAmountNotTaken = currentPickUpObjectInfo.amount - amountToTake;

				if (showDebugPrint) {
					print ("amount not taken " + inventoryAmountNotTaken);
				}

				canCheckToPickupObject = true;

				if (!mainInventoryWeightManager.checkIfCanCarrySingleObjectWeight (currentPickUpObjectInfo)) {
					objectNotAbleToBeingCarried = true;

					canCheckToPickupObject = false;

					if (showDebugPrint) {
						print ("object too much heavy");
					}

					objectWeightMessage = objectTooMuchHeavyToCarryMessage;
				}

				if (showDebugPrint) {
					print ("total amount taken " + amountToTake);
				}
			} else {
				objectNotAbleToBeingCarried = true;

				objectWeightMessage = weightLimitReachedMessage;
			}

			if (objectNotAbleToBeingCarried) {
				//Else, no amount is picked
				inventoryAmountNotTaken = currentPickUpObjectInfo.amount;

				if (objectWeightMessage != "") {
					showObjectMessage (objectWeightMessage, usedObjectMessageTime, usedObjectMessage);
				}

				if (showDebugPrint) {
					print ("this object can't be carried");
				}
			}
		} else {
			canCheckToPickupObject = true;
		}

		if (canCheckToPickupObject) {
			if (infiniteAmountPerSlot) {
				addAmountToInventorySlot (currentPickUpObjectInfo, -1, amountToTake);
			} else {
				int freeSpaceInInventorySlot = freeSpaceInSlot (currentPickUpObjectInfo.inventoryGameObject);

				if (freeSpaceInInventorySlot == 0 || freeSpaceInInventorySlot > 0) {
					if (showDebugPrint) {
						print ("same slot type, full or with space");
					}

					int inventoryAmountInSlot = amountPerSlot - freeSpaceInInventorySlot;

					addAmountToInventorySlot (currentPickUpObjectInfo, inventoryAmountInSlot, amountToTake);
				} else {
					if (getNumberOfFreeSlots () >= 1) {
						if (showDebugPrint) {
							print ("empty slots available");
						}

						addObjectToInventory (currentPickUpObjectInfo, amountToTake, -1);
					} else {
						if (showDebugPrint) {
							print ("no slots available");
						}

						inventoryAmountNotTaken = currentPickUpObjectInfo.amount;
					}
				}
			}
		}

		setInventory (false);

		int inventoryAmountPicked = currentPickUpObjectInfo.amount - inventoryAmountNotTaken;

		if (showDebugPrint) {
			print ("total amount picked " + inventoryAmountPicked + " of inventory object " + currentPickUpObjectInfo.Name + " which has " + currentPickUpObjectInfo.amount);
		}

//		print ("total amount picked " + inventoryAmountPicked + " of inventory object " + currentPickUpObjectInfo.Name + " which has " + currentPickUpObjectInfo.amount);

		if (inventoryAmountPicked > 0) {
			lastInventoryObjectPickedName = currentPickUpObjectInfo.Name;
		}

		currentPickUpObjectInfo = null;

		if (infiniteSlots) {

			int inventoryWithoutSlotAssigned = 0;

			for (int i = 0; i < inventoryList.Count; i++) {
				if (inventoryList [i].menuIconElement == null) {
					inventoryWithoutSlotAssigned++;
				}
			}

			inventoryWithoutSlotAssigned = inventoryList.Count - inventoryWithoutSlotAssigned;

			for (int i = inventoryWithoutSlotAssigned; i < inventoryList.Count; i++) {
				createInventoryIcon (inventoryList [i], i);
			}

			checkEmptySlots ();
		}

		updateAmountsInventoryPanel ();

		checkCurrentUseInventoryObject ();

		//manage weapons stored in the inventory
		if (equipWeaponsWhenPicked) {

			bool equipNewWeapon = true;

			if (equipPickedWeaponOnlyItNotPreviousWeaponEquipped) {
				if (inventoryObjectToPickup.isWeapon) {
					if (!inventoryObjectToPickup.isMeleeWeapon) {
						if (weaponsManager.isUsingWeapons ()) {
							equipNewWeapon = false;
						}
					}

					if (inventoryObjectToPickup.isMeleeWeapon) {
						if (mainMeleeWeaponsGrabbedManager.characterIsCarryingWeapon ()) {
							equipNewWeapon = false;
						}
					}
				}
			}

			if (equipNewWeapon) {
				if (inventoryAmountPicked > 0 && inventoryObjectToPickup.canBeEquiped) {
					if (inventoryObjectToPickup.isWeapon) {

						if (!inventoryObjectToPickup.isMeleeWeapon) {
							if (weaponsManager.drawWeaponWhenPicked) {

								currentInventoryObject = new inventoryInfo (inventoryObjectToPickup);

								weaponsManager.setEquippingPickedWeaponActiveState (true);

								equipCurrentObject ();

								weaponsManager.setEquippingPickedWeaponActiveState (false);
							}
						}

						if (inventoryObjectToPickup.isMeleeWeapon) {
							currentInventoryObject = new inventoryInfo (inventoryObjectToPickup);

							equipCurrentObject ();
						}
					}
				}
			}
		}

		if (examiningObject) {
			examineCurrentObject (false);
		}

		checkEventOnClickInventoryChange ();

		checkIfWeaponUseAmmoFromInventory ();

		if (mainInventoryQuickAccessSlotsSystem.isShowQuickAccessSlotsAlwaysActive ()) {
			updateAllWeaponSlotAmmo ();
		}

		checkEventOnInventoryListChange ();

		return inventoryAmountPicked;
	}

	string lastInventoryObjectPickedName;

	public string getLastInventoryObjectPickedName ()
	{
		return lastInventoryObjectPickedName;
	}

	//add an object grabbed by the player to the current inventory
	public void addObjectToInventory (inventoryInfo objectToAdd, int amountToTake, int index)
	{
		if (infiniteSlots) {
			addNewInventorySlot ();

			for (int i = 0; i < inventoryList.Count; i++) {
				inventoryInfo temporalInventoryInfo = inventoryList [i];

				if (temporalInventoryInfo.button == null) {
					createInventoryIcon (temporalInventoryInfo, i);
					//print ("new slot added");
				}
			}
		}

		inventoryInfo currentInventoryInfo = new inventoryInfo ();

		if (index > -1) {
			currentInventoryInfo = inventoryList [index];
		} else {
			bool inventoryInfoFound = false;

			for (int i = 0; i < inventoryList.Count; i++) {
				if (!inventoryInfoFound) {

					inventoryInfo temporalInventoryInfo = inventoryList [i];

					if (temporalInventoryInfo.amount == 0) {
						currentInventoryInfo = temporalInventoryInfo;
						inventoryInfoFound = true;
					}
				}
			}
		}

		inventoryInfo inventoryObject = new inventoryInfo (mainInventoryListManager.getInventoryInfoFromInventoryGameObject (objectToAdd.inventoryGameObject));

		if (inventoryObject != null) {

			currentInventoryInfo = currentInventoryInfo.copyInventoryObject (currentInventoryInfo, inventoryObject);

			currentInventoryInfo.amount = amountToTake;

			currentInventoryInfo.amountPerUnit = objectToAdd.amountPerUnit;

			if (currentInventoryInfo.storeTotalAmountPerUnit && currentInventoryInfo.amountPerUnit > 0) {
				currentInventoryInfo.amountPerUnit = 0;
			}

			inventoryMenuIconElement currentIconElement = currentInventoryInfo.menuIconElement;

			if (currentInventoryInfo.isEquiped) {
				currentIconElement.equipedIcon.SetActive (true);
			} else {
				currentIconElement.equipedIcon.SetActive (false);
			}

			if (currentInventoryInfo.isWeapon) {
				if (!currentInventoryInfo.isMeleeWeapon) {
					currentInventoryInfo.mainWeaponObjectInfo = weaponsManager.getIKWeaponSystem (currentInventoryInfo.Name);
				}

				if (currentInventoryInfo.isMeleeWeapon) {
					currentInventoryInfo.mainWeaponObjectInfo = mainMeleeWeaponsGrabbedManager.getWeaponGrabbedByName (currentInventoryInfo.Name);
				}
			} else {
				if (currentInventoryInfo.isMeleeWeapon) {
					
				} else {

				}

				currentInventoryInfo.mainWeaponObjectInfo = null;
			}

			if (showInventoryObjectsName) {
				currentIconElement.iconName.text = currentInventoryInfo.Name;
			} else {
				currentIconElement.iconName.text = "";
			}

			bool slotIsActive = amountToTake > 0;

			currentIconElement.activeSlotContent.SetActive (slotIsActive);
			currentIconElement.emptySlotContent.SetActive (!slotIsActive);

			bool showRegularAmount = true;

			if (currentInventoryInfo.amountPerUnit > 0) {
				if (currentInventoryInfo.showAmountPerUnitInAmountText) {

					currentIconElement.amount.text = (currentInventoryInfo.amount * currentInventoryInfo.amountPerUnit).ToString ();
					currentIconElement.amountPerUnitPanel.SetActive (false);

					showRegularAmount = false;
				} else {
					currentIconElement.amountPerUnitPanel.SetActive (true);
					currentIconElement.amountPerUnitText.text = currentInventoryInfo.amountPerUnit.ToString ();
				}
			} else {
				currentIconElement.amountPerUnitPanel.SetActive (false);
			}

			if (showRegularAmount) {
				if (currentInventoryInfo.infiniteAmount) {
					currentIconElement.amount.text = "";

					currentIconElement.infiniteAmountIcon.SetActive (true);
				} else {
					if (currentInventoryInfo.amount > 1 || showObjectAmountIfEqualOne) {
						currentIconElement.amount.text = currentInventoryInfo.amount.ToString ();
					} else {
						currentIconElement.amount.text = "";
					}

					if (currentIconElement.infiniteAmountIcon.activeSelf) {
						currentIconElement.infiniteAmountIcon.SetActive (false);
					}
				}
			}
		}
	}

	public void updateFullInventorySlots ()
	{
		int inventoryListCount = inventoryList.Count;

		for (int i = 0; i < inventoryListCount; i++) {
			inventoryInfo currentInventoryInfo = inventoryList [i];

			currentInventoryInfo.menuIconElement = menuIconElementList [i];

			inventoryMenuIconElement currentIconElement = currentInventoryInfo.menuIconElement;

			currentInventoryInfo.button = menuIconElementList [i].button;

			if (showInventoryObjectsName) {
				currentIconElement.iconName.text = currentInventoryInfo.Name;
			} else {
				currentIconElement.iconName.text = "";
			}

			currentIconElement.icon.texture = currentInventoryInfo.icon;

			bool slotIsActive = currentInventoryInfo.amount > 0;

			currentIconElement.activeSlotContent.SetActive (slotIsActive);
			currentIconElement.emptySlotContent.SetActive (!slotIsActive);

			bool showRegularAmount = true;

			if (currentInventoryInfo.amountPerUnit > 0) {
				if (currentInventoryInfo.showAmountPerUnitInAmountText) {

					currentIconElement.amount.text = (currentInventoryInfo.amount * currentInventoryInfo.amountPerUnit).ToString ();
					currentIconElement.amountPerUnitPanel.SetActive (false);

					showRegularAmount = false;
				} else {
					currentIconElement.amountPerUnitPanel.SetActive (true);
					currentIconElement.amountPerUnitText.text = currentInventoryInfo.amountPerUnit.ToString ();
				}
			} else {
				currentIconElement.amountPerUnitPanel.SetActive (false);
			}

			if (showRegularAmount) {
				if (currentInventoryInfo.infiniteAmount) {
					currentIconElement.amount.text = "";

					currentIconElement.infiniteAmountIcon.SetActive (true);
				} else {
					if (currentInventoryInfo.amount > 1 || showObjectAmountIfEqualOne) {
						currentIconElement.amount.text = currentInventoryInfo.amount.ToString ();
					} else {
						currentIconElement.amount.text = "";
					}

					if (currentIconElement.infiniteAmountIcon.activeSelf) {
						currentIconElement.infiniteAmountIcon.SetActive (false);
					}
				}
			}

//			print (currentInventoryInfo.Name + " " + inventoryList [i].isEquiped + " " + currentInventoryInfo.isEquiped);

//			currentInventoryInfo.isEquiped = currentInventoryInfo.isEquiped;
		
			if (currentInventoryInfo.isEquiped) {
				currentIconElement.equipedIcon.SetActive (true);
			} else {
				currentIconElement.equipedIcon.SetActive (false);
			}

//			currentInventoryInfo.isWeapon = currentInventoryInfo.isWeapon;
//			currentInventoryInfo.isMeleeWeapon = currentInventoryInfo.isMeleeWeapon;

			if (currentInventoryInfo.isWeapon) {
				if (!currentInventoryInfo.isMeleeWeapon) {
					currentInventoryInfo.mainWeaponObjectInfo = weaponsManager.getIKWeaponSystem (currentInventoryInfo.Name);
				}

				if (currentInventoryInfo.isMeleeWeapon) {
					currentInventoryInfo.mainWeaponObjectInfo = mainMeleeWeaponsGrabbedManager.getWeaponGrabbedByName (currentInventoryInfo.Name);
				}
			} else {
				if (currentInventoryInfo.isMeleeWeapon) {

				}

				if (!currentInventoryInfo.isMeleeWeapon) {

				}

				currentInventoryInfo.mainWeaponObjectInfo = null;
			}
		}

		if (infiniteSlots) {
			for (int i = 0; i < inventoryList.Count; i++) {
				inventoryInfo currentInventoryInfo = inventoryList [i];

				if (currentInventoryInfo.amount == 0) {
					//print (i);
					Destroy (currentInventoryInfo.button.gameObject);

					inventoryList.RemoveAt (i);

					menuIconElementList.RemoveAt (i);

					i--;
				}
			}
		}
	}

	public void addInventoryExtraSpace (int amount)
	{
		if (infiniteSlots) {
			return;
		}

		inventorySlotAmount += amount;

		checkRemainigEmptyInventorySlots ();

		for (int i = 0; i < inventorySlotAmount; i++) {
			inventoryInfo currentInventoryInfo = inventoryList [i];

			if (currentInventoryInfo.button == null) {
				createInventoryIcon (currentInventoryInfo, i);
			}
		}
	}

	public void getPressedButton (Button buttonObj)
	{
		int inventoryListCount = inventoryList.Count;

		for (int i = 0; i < inventoryListCount; i++) {
			inventoryInfo currentInventoryInfo = inventoryList [i];

			if (currentInventoryInfo.button == buttonObj) {
				if (currentInventoryObject != null) {
					if (currentInventoryObject == currentInventoryInfo && currentInventoryObject.menuIconElement.pressedIcon.activeSelf) {

						checkInventoryOptionsOnSlot (true);

						if (combiningObjects) {
							if (showDebugPrint) {
								print ("stop combine objects");
							}

							combiningObjects = false;

							disableObjectsToCombineIcon ();

							firstObjectToCombine = null;
						}

						return;
					}
				}

				setObjectInfo (currentInventoryInfo);

				adjustCameraFovToSeeInventoryObject ();

				checkInventoryOptionsOnSlot (true);

				checkEventOnInventorySlotSelected ();

				return;
			}
		}
	}

	public void setCurrenObjectByPrefab (GameObject objectToSearch, string inventoryObjectName)
	{
		bool objectFound = false;

		if (inventoryObjectName != "") {
			//			print (inventoryObjectName);

			int currentIndex = inventoryList.FindIndex (s => s.Name == inventoryObjectName);

			if (currentIndex > -1) {
				currentInventoryObject = inventoryList [currentIndex];

				objectFound = true;

				return;
			}
		} 

		if (!objectFound) {
			if (objectToSearch != null) {
				int inventoryListCount = inventoryList.Count;

				for (int i = 0; i < inventoryListCount; i++) {
					inventoryInfo currentInventoryInfo = inventoryList [i];

					if (currentInventoryInfo.inventoryGameObject == objectToSearch) {
						currentInventoryObject = currentInventoryInfo;

						return;
					}
				}
			}
		}

		currentInventoryObject = null;
	}

	public void setCurrenInventoryInfoByPickup (inventoryInfo newObject)
	{
		setCurrentInventoryObject (newObject);
	}

	public bool inventoryContainsObject (GameObject objectToCheck)
	{
		int inventoryListCount = inventoryList.Count;

		for (int i = 0; i < inventoryListCount; i++) {
			inventoryInfo currentInventoryInfo = inventoryList [i];

			if (currentInventoryInfo.inventoryGameObject == objectToCheck) {
				if (currentInventoryInfo.amount > 0) {
					return true;
				}
			}
		}

		return false;
	}

	public void searchForObjectNeed (GameObject obj)
	{
		setCurrentUseInventoryGameObject (obj);

		useCurrentObject ();
	}

	public void useInventoryObjectByName (string objectName, int amountToUse)
	{
		int inventoryListCount = inventoryList.Count;

		for (int i = 0; i < inventoryListCount; i++) {
			inventoryInfo currentInventoryInfo = inventoryList [i];

			if (currentInventoryInfo.amount > 0) {
				if (currentInventoryInfo.Name.Equals (objectName)) {

					currentInventoryObject = currentInventoryInfo;

					numberOfObjectsToUse = amountToUse;

					useCurrentObject ();

					return;
				}
			}
		}
	}

	public void setCurrentUseInventoryGameObject (GameObject obj)
	{
		currentUseInventoryGameObject = obj;

		if (currentUseInventoryGameObject) {
			currentUseInventoryObject = currentUseInventoryGameObject.GetComponent<useInventoryObject> ();
		} else {
			currentUseInventoryObject = null;
		}
	}

	public void removeCurrentInventoryObject ()
	{
		currentInventoryObject = null;
	}

	public void removeCurrentUseInventoryObject ()
	{
		currentUseInventoryObject = null;
	}

	public void useCurrentObjectByButton (RectTransform menuPosition)
	{
		bool enabledPreviously = false;

		if (isCurrentObjectNotNull () && currentInventoryObject.amount > 1) {
			if (activeNumberOfObjectsToUseMenu) {
				if (numberOfObjectsToUseMenu.activeSelf && numberOfObjectsToUseMenuRectTransform.anchoredPosition == menuPosition.anchoredPosition) {
					enabledPreviously = true;
				}

				if (!enabledPreviously) {
					enableNumberOfObjectsToUseMenu (menuPosition);
					return;
				}
			}
		}

		useCurrentObject ();

		if (enabledPreviously) {
			disableNumberOfObjectsToUseMenu ();
		}
	}

	public void checkCurrentUseInventoryObject ()
	{
		if (currentUseInventoryObject) {
			currentUseInventoryObject.selectObjectOnInventory ();
		}
	}

	bool usingCurrentObjectFromQuickAccessSlotsInventory;

	public void setUsingCurrentObjectFromQuickAccessSlotsInventoryState (bool state)
	{
		usingCurrentObjectFromQuickAccessSlotsInventory = state;
	}

	public void useCurrentObject ()
	{
		bool objectFound = false;

		if (isCurrentObjectNotNull ()) {
			if (currentInventoryObject.canBeUsed) {

				if (currentInventoryObject.useNewBehaviorOnUse) {
					inventoryObject currentInventoryObjectToUse = getInventoryObjectComponentByInventoryGameObject (currentInventoryObject.inventoryGameObject);

					int amountToUse = numberOfObjectsToUse;

					currentInventoryObjectToUse.useObjectOnNewBehavior (gameObject, amountToUse);

					return;
				} else {
					if (currentUseInventoryGameObject && currentUseInventoryObject != null) {
						//currentUseInventoryObject = currentUseInventoryGameObject.GetComponent<useInventoryObject> ();
						if (currentUseInventoryObject.inventoryObjectNeededListContainsObject (currentInventoryObject.inventoryGameObject, currentInventoryObject.Name)) {
							bool amountNeededAvaliable = false;

							int amountToUse = numberOfObjectsToUse;
							int amountNeededForInventoryObject = currentUseInventoryObject.getInventoryObjectNeededAmound (currentInventoryObject.inventoryGameObject);

							if (useOnlyWhenNeededAmountToUseObject) {
								amountToUse = amountNeededForInventoryObject;
							} else {
								if (amountToUse > amountNeededForInventoryObject) {
									amountToUse = amountNeededForInventoryObject;
								}
							}

							if (amountToUse > 1) {
								if (currentInventoryObject.amount >= amountToUse || currentInventoryObject.infiniteAmount) {
									amountNeededAvaliable = true;
								}
							} else {
								amountNeededAvaliable = true;
							}

							bool setToNullCurrentInventoryObject = true;

							if (amountNeededAvaliable) {
								int amountUsed = amountToUse;
								if (currentInventoryObject.amountPerUnit > 0) {
									amountUsed *= currentInventoryObject.amountPerUnit;
								}

								currentUseInventoryObject.useObject (amountUsed, currentInventoryObject.Name, currentInventoryObject.inventoryGameObject);

								objectFound = true;

								currentInventoryObject.amount -= amountToUse;

								if (currentInventoryObject.infiniteAmount && currentInventoryObject.amount <= 0) {
									currentInventoryObject.amount = 1;
								}

								updateAmount (currentInventoryObject, currentInventoryObject.amount);

								if (currentInventoryObject.useSoundOnUseObject) {
									playSound (currentInventoryObject.soundOnUseObject);
								}
									
								if (!usingCurrentObjectFromQuickAccessSlotsInventory && currentInventoryObject != null) {
									mainInventoryQuickAccessSlotsSystem.updateQuickAccessSlotAmountByName (currentInventoryObject.Name);
								}

								if (currentInventoryObject.amount == 0) {
									removeButton (currentInventoryObject);
								}

								if (showMessageWhenObjectUsed) {
									string objectUsedMessage = currentUseInventoryObject.getObjectUsedMessage ();

									if (objectUsedMessage != "") {
										showObjectMessage (objectUsedMessage, usedObjectMessageTime, usedObjectMessage);
									}
								}

								if (inventoryOpened) {
									openOrCloseInventory (false);
								}

								setInventory (false);

								checkEmptySlots ();

								setToNullCurrentInventoryObject = currentUseInventoryObject.setToNullCurrentInventoryObject ();

								checkEventOnInventoryListChange ();
							} else {
								if (nonNeededAmountAvaliable != "") {
									showObjectMessage (nonNeededAmountAvaliable, usedObjectMessageTime, usedObjectMessage);
								}
							}

							if (setToNullCurrentInventoryObject) {
								currentInventoryObject = null;
							}

							//check the state of the use inventory object to load the new inventory elements to use if there are elements to use yet
							if (currentUseInventoryObject) {
								if (currentUseInventoryObject.objectUsed) {
									removeCurrentUseInventoryObject ();

									currentInventoryObject = null;
								} else {
									currentUseInventoryObject.updateUseInventoryObjectState ();
								}
							}

							checkEventOnClickInventoryChange ();
						}

						if (!objectFound) {
							if (unableToUseObjectMessage != "") {
								showObjectMessage (currentInventoryObject.Name + " " + unableToUseObjectMessage, usedObjectMessageTime, usedObjectMessage);
							}
						}
					} else {
						if (cantUseThisObjectHereMessage != "") {
							showObjectMessage (cantUseThisObjectHereMessage, usedObjectMessageTime, usedObjectMessage);
						}
					}
				}
			}
		} else {
			if (objectNotFoundMessage != "") {
				showObjectMessage (objectNotFoundMessage, usedObjectMessageTime, usedObjectMessage);
			}
		}
	}

	public void setUseObjectWithNewBehaviorResult (int amountPicked)
	{
		if (amountPicked > 0) {
			currentInventoryObject.amount -= amountPicked;

			if (currentInventoryObject.infiniteAmount && currentInventoryObject.amount <= 0) {
				currentInventoryObject.amount = 1;
			}

			updateAmount (currentInventoryObject, currentInventoryObject.amount);

			if (showMessageWhenObjectUsed) {
				string objectUsedMessage = currentInventoryObject.newBehaviorOnUseMessage;

				if (objectUsedMessage != "") {
					if (currentInventoryObject.storeTotalAmountPerUnit) {
						objectUsedMessage = objectUsedMessage.Replace ("-AMOUNT-", (1).ToString ());
					} else {
						objectUsedMessage = objectUsedMessage.Replace ("-AMOUNT-", (currentInventoryObject.amountPerUnit * amountPicked).ToString ());
					}

					showObjectMessage (objectUsedMessage, usedObjectMessageTime, usedObjectMessage);
				}
			}

			if (currentInventoryObject.useSoundOnUseObject) {
				playSound (currentInventoryObject.soundOnUseObject);
			}

			if (!usingCurrentObjectFromQuickAccessSlotsInventory && currentInventoryObject != null) {
				mainInventoryQuickAccessSlotsSystem.updateQuickAccessSlotAmountByName (currentInventoryObject.Name);
			}

			if (currentInventoryObject.amount == 0) {
				removeButton (currentInventoryObject);
			}

			checkEventOnInventoryListChange ();
		} else {
			if (showMessageWhenObjectUsed) {
				string objectUsedMessage = currentInventoryObject.newBehaviorOnUnableToUseMessage;

				if (objectUsedMessage != "") {
					showObjectMessage (objectUsedMessage, usedObjectMessageTime, usedObjectMessage);
				}
			}
		}

		setInventory (false);

		checkEmptySlots ();

		disableNumberOfObjectsToUseMenu ();

		checkEventOnClickInventoryChange ();
	}

	public bool isCurrentObjectNotNull ()
	{
		if (currentInventoryObject != null) {
			if (currentInventoryObject.inventoryGameObject == null) {
				return false;
			}
		} else {
			return false;
		}

		return true;
	}

	public void useEquippedObjectAction (string objectName)
	{
		int inventoryListCount = inventoryList.Count;

		for (int i = 0; i < inventoryListCount; i++) {

			inventoryInfo currentInventoryInfo = inventoryList [i];

			if (currentInventoryInfo.Name.Equals (objectName)) {
				inventoryObject currentInventoryObjectToUse = getInventoryObjectComponentByInventoryGameObject (currentInventoryInfo.inventoryGameObject);

				currentInventoryObject = currentInventoryInfo;

				currentInventoryObjectToUse.useObjectOnNewBehavior (gameObject, 1);

				return;
			}
		}
	}


	//START FUNCTIONS TO COMBINE OBJECTS
	public void equipCurrentObject ()
	{
		if (isUsingGenericModelActive ()) {
			return;
		}

		if (isCurrentObjectNotNull ()) {
			if (currentInventoryObject.canBeEquiped) {
				bool equippedCorrectly = false;

				bool objectIsWeapon = currentInventoryObject.isWeapon;

				bool objectIsMeleeWeapon = currentInventoryObject.isMeleeWeapon;

				if (objectIsWeapon) {
					if (!objectIsMeleeWeapon) {
						currentIKWeaponSystem = weaponsManager.equipWeapon (currentInventoryObject.Name, initializingInventory, activatingDualWeaponSlot, currentRighWeaponName, currentLeftWeaponName);

						if (currentIKWeaponSystem != null) {
							equippedCorrectly = true;

							//print (currentIKWeaponSystem.getWeaponSystemName () + " equipped " + activatingDualWeaponSlot);
						} else {
							if (showDebugPrint) {
								print ("WARNING: weapon " + currentInventoryObject.Name + " not found in the player weapons manager when player has tried to equip it from the inventory, " +
								"make sure the name of the weapon is the same on inventory as well");
							}
						}
					}

					if (objectIsMeleeWeapon) {
						equippedCorrectly = mainMeleeWeaponsGrabbedManager.equipMeleeWeapon (currentInventoryObject.Name, false);

						//grabPhysicalObjectMeleeAttackSystem currentGrabPhysicalObjectMeleeAttackSystem = mainMeleeWeaponsGrabbedManager.getWeaponGrabbedByName (currentInventoryObject.Name);
					}
				} else {
					if (showDebugPrint) {
						print ("equipping regular object, like the future armor/cloth system");
					}

					inventoryObject currentInventoryObjectToUse = getInventoryObjectComponentByInventoryGameObject (currentInventoryObject.inventoryGameObject);

					equippedCorrectly = currentInventoryObjectToUse.setObjectEquippedStateOnInventoryOnNewBehavior (gameObject, true);

					//here check for armor and any other kind of inventory object
					///////////////////////////
					////////////////////////////// 

				}

				if (equippedCorrectly) {
					if (currentInventoryObject.menuIconElement == null) {
						bool inventoryObjectFound = false;

						int inventoryListCount = inventoryList.Count;

						for (int i = 0; i < inventoryListCount; i++) {

							inventoryInfo currentInventoryInfo = inventoryList [i];

							if (!inventoryObjectFound && currentInventoryInfo.Name.Equals (currentInventoryObject.Name)) {
								currentInventoryObject = currentInventoryInfo;
								inventoryObjectFound = true;
							}
						}

						if (!inventoryObjectFound) {
							if (showDebugPrint) {
								print ("WARNING: inventory object called " + currentInventoryObject.Name + " not found, please check the inventory manager" +
								" settings to assign a proper name to this inventory object");
							}
						}
					} 

					if (currentInventoryObject.menuIconElement != null) {
						currentInventoryObject.menuIconElement.equipedIcon.SetActive (true);
					}

					currentInventoryObject.isEquiped = true;

					bool updateQuickAccessInventorySlots = false;

					if (objectIsWeapon) {
						if (!objectIsMeleeWeapon) {
							if (currentIKWeaponSystem.weaponGameObject != null) {

								currentInventoryObject.mainWeaponObjectInfo = currentIKWeaponSystem;

								//update the content in the weapon slot list, according to the weapons equipped
								if (storePickedWeaponsOnInventory) {
									updateQuickAccessInventorySlots = true;
								}
							}
						}

						if (objectIsMeleeWeapon) {
							currentInventoryObject.mainWeaponObjectInfo = mainMeleeWeaponsGrabbedManager.getWeaponGrabbedByName (currentInventoryObject.Name);
						
							updateQuickAccessInventorySlots = true;
						} 
					} else {
						if (showDebugPrint) {
							print ("assign the object from the future armor/cloth system");
						}

						updateQuickAccessInventorySlots = true;

					}

					if (updateQuickAccessInventorySlots) {
						bool slotAlreadyAdded = false;

						quickAccessSlotInfoList = mainInventoryQuickAccessSlotsSystem.quickAccessSlotInfoList;

						int quickAccessSlotInfoListCount = quickAccessSlotInfoList.Count;

						for (int i = 0; i < quickAccessSlotInfoListCount; i++) {
							if (quickAccessSlotInfoList [i].slotActive && quickAccessSlotInfoList [i].Name.Equals (currentInventoryObject.Name)) {
								slotAlreadyAdded = true;
							}
						}

						if (!slotAlreadyAdded) {
							bool quickAccessSlotFound = false;

							for (int i = 0; i < quickAccessSlotInfoList.Count; i++) {
								if (!quickAccessSlotFound && !quickAccessSlotInfoList [i].slotActive && quickAccessSlotInfoList [i].Name != currentInventoryObject.Name) {
									updateWeaponSlotInfo (i, currentInventoryObject, null, null);

									if (objectIsWeapon) {
										if (!objectIsMeleeWeapon) {
											weaponsManager.checkToUpdateInventoryWeaponAmmoText ();
										} else {
											updateQuickAccesSlotAmount (i);
										}
									} else {
										updateQuickAccesSlotAmount (i);
									}

									quickAccessSlotFound = true;
								}
							}
						}

						if (objectIsWeapon) {
							if (objectIsMeleeWeapon) {
								showWeaponSlotsParentWhenWeaponSelectedByName (currentInventoryObject.Name);
							}
						}
					}

					setEquipButtonState (false);

					checkInventoryOptionsOnSlot (false);

					if (objectIsWeapon && !initializingInventory) {
						//COMENTAR ESTA PARTE PARA QUE SE ACTUALIZE SI ES UN ARMA; MELEE O FIRE
						bool activateChangeOfQuickAccessSlot = false;

						if (objectIsMeleeWeapon) {
							if (changeToMeleeWeaponsModeWhenPickingMeleeWeapon) {
								activateChangeOfQuickAccessSlot = true;
							}
						} else {
							if (changeToFireWeaponsModeWhenPickingFireWeapon) {
								activateChangeOfQuickAccessSlot = true;
							}
						}

						if (activateChangeOfQuickAccessSlot) {
							mainInventoryQuickAccessSlotsSystem.checkQuickAccessSlotToSelectByName (currentInventoryObject.Name);
						}
					}
				}
			}
		}
	}

	public void unEquipCurrentObject ()
	{
		if (isCurrentObjectNotNull ()) {
			if (currentInventoryObject.canBeEquiped) {
				bool unequippedCorrectly = false;

				bool objectIsWeapon = currentInventoryObject.isWeapon;

				bool objectIsMeleeWeapon = currentInventoryObject.isMeleeWeapon;

				bool updateQuickAccessInventorySlots = false;

				if (objectIsWeapon) {
					unequippedCorrectly = true;

					if (!objectIsMeleeWeapon) {
						weaponsManager.unequipWeapon (currentInventoryObject.Name, false);

						if (storePickedWeaponsOnInventory) {
							updateQuickAccessInventorySlots = true;
						}
					}

					if (objectIsMeleeWeapon) {
						mainMeleeWeaponsGrabbedManager.unEquipMeleeWeapon (currentInventoryObject.Name, false);

						updateQuickAccessInventorySlots = true;
					} 				
				} else {
					if (showDebugPrint) {
						print ("unequipping an object of the future armor/cloth system");
					}

					updateQuickAccessInventorySlots = true;

					inventoryObject currentInventoryObjectToUse = getInventoryObjectComponentByInventoryGameObject (currentInventoryObject.inventoryGameObject);

					unequippedCorrectly = currentInventoryObjectToUse.setObjectEquippedStateOnInventoryOnNewBehavior (gameObject, false);

					//here check for armor and any other kind of inventory object
				}

				if (unequippedCorrectly) {
					if (updateQuickAccessInventorySlots) {
						bool quickAccessSlotFound = false;

						quickAccessSlotInfoList = mainInventoryQuickAccessSlotsSystem.quickAccessSlotInfoList;

						for (int i = 0; i < quickAccessSlotInfoList.Count; i++) {
							if (!quickAccessSlotFound && quickAccessSlotInfoList [i].slotActive && quickAccessSlotInfoList [i].Name.Equals (currentInventoryObject.Name)) {
								updateWeaponSlotInfo (-1, null, quickAccessSlotInfoList [i], null);

								quickAccessSlotFound = true;
							}
						}
					}

					currentInventoryObject.menuIconElement.equipedIcon.SetActive (false);

					currentInventoryObject.isEquiped = false;

					setEquipButtonState (true);

					checkInventoryOptionsOnSlot (false);
				}
			}
		}
	}

	public void unEquipObjectByName (string objectToUnequipName)
	{
		int inventoryListCount = inventoryList.Count;

		for (int i = 0; i < inventoryListCount; i++) {
			inventoryInfo currentInventoryInfo = inventoryList [i];

			if (currentInventoryInfo.canBeEquiped && currentInventoryInfo.isEquiped) {
				if (currentInventoryInfo.Name.Equals (objectToUnequipName)) {
//					print ("unequip " + objectToUnequipName);

					inventoryInfo inventoryObjectToUnequip = currentInventoryInfo;
					inventoryObjectToUnequip.menuIconElement.equipedIcon.SetActive (false);
					inventoryObjectToUnequip.isEquiped = false;

					setEquipButtonState (true);

					bool updateQuickAccessInventorySlots = false;

					if (inventoryObjectToUnequip.isWeapon) {
						if (!inventoryObjectToUnequip.isMeleeWeapon) {
							if (storePickedWeaponsOnInventory) {
								updateQuickAccessInventorySlots = true;
							}
						}

						if (inventoryObjectToUnequip.isMeleeWeapon) {
							updateQuickAccessInventorySlots = true;
						}
					} else {
						updateQuickAccessInventorySlots = true;
					}

					if (updateQuickAccessInventorySlots) {
						bool quickAccessSlotFound = false;

						quickAccessSlotInfoList = mainInventoryQuickAccessSlotsSystem.quickAccessSlotInfoList;

						for (i = 0; i < quickAccessSlotInfoList.Count; i++) {
							if (!quickAccessSlotFound && quickAccessSlotInfoList [i].slotActive && quickAccessSlotInfoList [i].Name.Equals (inventoryObjectToUnequip.Name)) {
								updateWeaponSlotInfo (-1, null, quickAccessSlotInfoList [i], null);

								quickAccessSlotFound = true;
							}
						}
					}

					return;
				}
			}
		}
	}

	public void equipObjectByName (string objectName)
	{
		int inventoryListCount = inventoryList.Count;

		for (int i = 0; i < inventoryListCount; i++) {
			inventoryInfo currentInventoryInfo = inventoryList [i];

			if (!currentInventoryInfo.isEquiped) {
				if (currentInventoryInfo.canBeEquiped && currentInventoryInfo.Name.Equals (objectName)) {
					
					currentInventoryObject = currentInventoryInfo;

					equipCurrentObject ();

					return;
				}
			}
		}
	}
	//END FUNCTIONS TO EQUIP OBJECTS


	//START FUNCTIONS TO DROP OBJECTS
	public void dropAllUnitsObjectAtOnce ()
	{
		if (currentInventoryObject == null) {
			return;
		}

		numberOfObjectsToUse = currentInventoryObject.amount;

		dropCurrentObject (true);
	}

	public void discardCurrentObject ()
	{
		if (currentInventoryObject == null) {
			return;
		}

		numberOfObjectsToUse = currentInventoryObject.amount;

		dropCurrentObject (false);
	}

	public void dropCurrentObject (RectTransform menuPosition)
	{
		bool enabledPreviously = false;

		if (activeNumberOfObjectsToUseMenu && (currentInventoryObject == null || currentInventoryObject.amount > 1)) {
			if (numberOfObjectsToUseMenu.activeSelf && numberOfObjectsToUseMenuRectTransform.anchoredPosition == menuPosition.anchoredPosition) {
				enabledPreviously = true;
			}

			if (!enabledPreviously) {
				enableNumberOfObjectsToUseMenu (menuPosition);
				return;
			}
		}

		dropCurrentObject (true);

		if (enabledPreviously) {
			disableNumberOfObjectsToUseMenu ();
		}
	}

	public void dropCurrentObject (bool instantiateInventoryObjectPrefab)
	{
		//print ("drop " + currentInventoryObject.Name + " " + currentInventoryObject.amount);
		if (currentInventoryObject != null && currentInventoryObject.amount > 0) {

			currentInventoryObject.amount -= numberOfObjectsToUse;

			if (currentInventoryObject.infiniteAmount && currentInventoryObject.amount <= 0) {
				if (dropSingleObjectOnInfiniteAmount) {
					currentInventoryObject.amount = 0;
				} else {
					currentInventoryObject.amount = 1;
				}
			}

			GameObject currentInventoryObjectPrefab = emptyInventoryPrefab;

			GameObject inventoryObjectPrefabObtained = mainInventoryListManager.getInventoryPrefab (currentInventoryObject.inventoryGameObject);

			if (inventoryObjectPrefabObtained != null) {
				currentInventoryObjectPrefab = inventoryObjectPrefabObtained;
			}

			Vector3 positionToInstantiate = transform.position + transform.forward + transform.up;

			if (instantiateInventoryObjectPrefab) {
				if (setTotalAmountWhenDropObject) {
					dropObject (currentInventoryObjectPrefab, positionToInstantiate, numberOfObjectsToUse);
				} else {
					for (int i = 0; i < numberOfObjectsToUse; i++) {
						dropObject (currentInventoryObjectPrefab, positionToInstantiate, 1);
					}
				}
			}

			if (currentInventoryObject.amount > 0) {

				updateAmount (currentInventoryObject, currentInventoryObject.amount);

				if (combineElementsAtDrop && !infiniteAmountPerSlot) {
					//combine same objects when the amount of an object is lower than amountPerSlot and there is another group equal to that object
					//for example if I have 10 cubes and 4 cubes with a amountPerSlot of 10, and you drop 1 cube of the first group, this combines the other cubes
					//so after that, you have 9 cubes and 4 cubes, and then, this changes that into 10 cubes and 3 cubes
					//this only checks the next objects after current object
					int currentIndex = inventoryList.IndexOf (currentInventoryObject) + 1;

					int index = -1;

					for (int i = currentIndex; i < inventoryList.Count; i++) {
						inventoryInfo currentInventoryInfo = inventoryList [i];

						if (currentInventoryInfo.Name.Equals (currentInventoryObject.Name)) {
							if (currentInventoryInfo.amount < amountPerSlot && index == -1) {
								index = i;
							}
						}
					}

					//if there are more objects equals to the current object dropped, then check their remaining amount to combine their values
					if (index != -1) {
						int amountToChange = amountPerSlot - currentInventoryObject.amount;

						inventoryInfo currentInventoryInfo = inventoryList [index];

						if (amountToChange < currentInventoryInfo.amount) {
							currentInventoryInfo.amount -= amountToChange;

							currentInventoryObject.amount += amountToChange;
						} else if (amountToChange >= currentInventoryInfo.amount) {
							currentInventoryObject.amount += currentInventoryInfo.amount;

							currentInventoryInfo.amount -= currentInventoryInfo.amount;

							removeButton (currentInventoryInfo);
						}
					} else {
						//if  all the objects equal to this has the max amount per space, search the last one of them to drop an object
						//from its amount
						currentIndex = inventoryList.IndexOf (currentInventoryObject);

						for (int i = inventoryList.Count - 1; i >= currentIndex; i--) {

							inventoryInfo currentInventoryInfo = inventoryList [i];

							if (currentInventoryInfo.Name.Equals (currentInventoryObject.Name)) {
								//|| i == inventoryList.Count - 1
								if ((currentInventoryInfo.amount == amountPerSlot) && index == -1) {
									index = i;
								}
							}
						}

						if (index != -1) {
							int amountToChange = amountPerSlot - currentInventoryObject.amount;

							inventoryInfo currentInventoryInfo = inventoryList [index];

							if (amountToChange < currentInventoryInfo.amount) {
								currentInventoryInfo.amount -= amountToChange;

								currentInventoryObject.amount += amountToChange;
							} else if (amountToChange >= currentInventoryInfo.amount) {

								currentInventoryObject.amount += currentInventoryInfo.amount;

								currentInventoryInfo.amount -= currentInventoryInfo.amount;

								removeButton (currentInventoryInfo);
							}
						}
					}

					updateAmountsInventoryPanel ();
				}
			} else {
				if (currentInventoryObject.canBeEquiped && currentInventoryObject.isEquiped) {
					if (currentInventoryObject.isWeapon) {
						if (!currentInventoryObject.isMeleeWeapon) {
							weaponsManager.unequipWeapon (currentInventoryObject.Name, false);
						}

						if (currentInventoryObject.isMeleeWeapon) {
							mainMeleeWeaponsGrabbedManager.unEquipMeleeWeapon (currentInventoryObject.Name, false);

							unEquipObjectByName (currentInventoryObject.Name);
						}

					} else {
						if (showDebugPrint) {
							print ("function to unequip the future armor/cloth system");
						}

						inventoryObject currentInventoryObjectToUse = getInventoryObjectComponentByInventoryGameObject (currentInventoryObject.inventoryGameObject);

						currentInventoryObjectToUse.setObjectEquippedStateOnInventoryOnNewBehavior (gameObject, false);
					}

					currentInventoryObject.isEquiped = false;

					if (currentInventoryObject.menuIconElement != null) {
						currentInventoryObject.menuIconElement.equipedIcon.SetActive (false);
					}
				}

				removeButton (currentInventoryObject);
			}

			setInventory (false);

			checkEmptySlots ();

			resetAndDisableNumberOfObjectsToUseMenu ();

			checkCurrentUseInventoryObject ();

			checkEventOnClickInventoryChange ();

			checkIfWeaponUseAmmoFromInventory ();

			updateAllWeaponSlotAmmo ();
		}
	}

	public void checkObjectDroppedEvent (GameObject currentObjectDropped)
	{
		inventoryObject currentInventoryObjectToUse = currentObjectDropped.GetComponentInChildren<inventoryObject> ();

		currentInventoryObjectToUse.eventOnDropObjectNewBehaviour (gameObject);
	}

	public void dropObject (GameObject currentInventoryObjectPrefab, Vector3 positionToInstantiate, int amount)
	{
		if (numberOfObjectsToUse > 1) {
			positionToInstantiate = transform.position + transform.forward + transform.up + UnityEngine.Random.insideUnitSphere * maxRadiusToInstantiate;
		}

		GameObject inventoryObjectClone = (GameObject)Instantiate (currentInventoryObjectPrefab, positionToInstantiate, Quaternion.identity);

		lastObjectDropped = inventoryObjectClone;

		inventoryObject inventoryObjectManager = inventoryObjectClone.GetComponentInChildren<inventoryObject> ();

		if (inventoryObjectManager) {
			inventoryObjectManager.inventoryObjectInfo = new inventoryInfo (currentInventoryObject);

			if (inventoryObjectManager.inventoryObjectInfo.storeTotalAmountPerUnit) {
				inventoryObjectManager.inventoryObjectInfo.amountPerUnit = 1;
			}

			pickUpObject currentPickupObject = inventoryObjectClone.GetComponent<pickUpObject> ();
			currentPickupObject.amountPerUnit = currentInventoryObject.amountPerUnit;

			if (currentPickupObject.amountPerUnit > 0 && !inventoryObjectManager.inventoryObjectInfo.storeTotalAmountPerUnit) {
				currentPickupObject.useAmountPerUnit = true;
			}
			currentPickupObject.setPickUpAmount (amount);

			inventoryObjectClone.name = inventoryObjectManager.inventoryObjectInfo.Name + " (inventory)";
			inventoryObjectClone.GetComponentInChildren<deviceStringAction> ().deviceName = inventoryObjectManager.inventoryObjectInfo.Name;

			inventoryObjectManager.eventOnDropObjectNewBehaviour (gameObject);

			checkEventOnInventoryListChange ();
		}
	}

	public void dropEquipByName (string equipName, int amount, bool instantiateInventoryObjectPrefab)
	{
		int inventoryListCount = inventoryList.Count;

		for (int i = 0; i < inventoryListCount; i++) {
			inventoryInfo currentInventoryInfo = inventoryList [i];

			if (currentInventoryInfo.canBeEquiped && currentInventoryInfo.isEquiped) {
				if (currentInventoryInfo.Name.Equals (equipName)) {
					currentInventoryObject = currentInventoryInfo;
				}
			}
		}

		numberOfObjectsToUse = amount;

		dropCurrentObject (instantiateInventoryObjectPrefab);
	}

	public void carryPhysicalObjectFromInventory ()
	{
		if (isCurrentObjectNotNull ()) {

			dropAllUnitsObjectAtOnce ();

			openOrCloseInventory (false);

			inventoryObject currentInventoryObjectToUse = lastObjectDropped.GetComponentInChildren<inventoryObject> ();

			currentInventoryObjectToUse.carryPhysicalObjectFromInventory (gameObject);
		}
	}

	public void dropAllInventory ()
	{
		if (Application.isPlaying) {

			for (int i = 0; i < inventoryList.Count; i++) {
				loop = 0;

				while (inventoryList [i].amount > 0) {
					currentInventoryObject = inventoryList [i];

					dropCurrentObject (true);

					loop++;

					if (loop > 100) {
						//print ("loop loop");
						return;
					}
				}
			}
		}
	}
	//END FUNCTIONS TO DROP OBJECTS


	//START FUNCTIONS TO COMBINE OBJECTS
	public void disableObjectsToCombineIcon ()
	{
		if (firstObjectToCombine != null && firstObjectToCombine.inventoryGameObject != null) {
			setCombineIcontPressedState (false, firstObjectToCombine);
		}

		if (secondObjectToCombine != null && secondObjectToCombine.inventoryGameObject != null) {
			setCombineIcontPressedState (false, secondObjectToCombine);
		}
	}

	public void setCombineIcontPressedState (bool state, inventoryInfo inventoryObjectToCombine)
	{
		if (inventoryObjectToCombine != null && (inventoryObjectToCombine.inventoryGameObject != null || !state)) {
			if (inventoryObjectToCombine.menuIconElement != null) {
				inventoryObjectToCombine.menuIconElement.combineIcon.SetActive (state);
			}
		}
	}

	public void setCombiningObjectsState (bool state)
	{
		if (isCurrentObjectNotNull ()) {
			if (currentInventoryObject.canBeCombined) {
				combiningObjects = state;

				setCombineIcontPressedState (state, currentInventoryObject);

				checkInventoryOptionsOnSlot (false);
			} 
		}
	}

	public void combineCurrentObject ()
	{
		combiningObjects = false;

		bool stopCombineObjects = false;

		if (firstObjectToCombine != null && secondObjectToCombine != null) {
			if (firstObjectToCombine.inventoryGameObject != null && secondObjectToCombine.inventoryGameObject != null && firstObjectToCombine != secondObjectToCombine) {

				bool canBeCombined = false;
				inventoryInfo combinedInventoryObject = new inventoryInfo ();

				inventoryObject currentInventoryObjectToCombine = null;

				firstObjectCombinedOnNewBehavior = new inventoryInfo ();
				secondObjectCombinedOnNewBehavior = new inventoryInfo ();

				bool useNewBehaviorOnCombine = firstObjectToCombine.useNewBehaviorOnCombine || secondObjectToCombine.useNewBehaviorOnCombine;

				if (firstObjectToCombine.canBeCombined && secondObjectToCombine.canBeCombined) {
					if (firstObjectToCombine.objectToCombine == secondObjectToCombine.inventoryGameObject ||
					    firstObjectToCombine.inventoryGameObject == secondObjectToCombine.objectToCombine ||
					    useNewBehaviorOnCombine) {

						if (useNewBehaviorOnCombine) {
							if (firstObjectToCombine.objectToCombine && secondObjectToCombine.objectToCombine) {

								if (firstObjectToCombine.useNewBehaviorOnCombine) {
									currentInventoryObjectToCombine = secondObjectToCombine.objectToCombine.GetComponentInChildren<inventoryObject> ();

									firstObjectCombinedOnNewBehavior = firstObjectToCombine;
									secondObjectCombinedOnNewBehavior = secondObjectToCombine;
								} else {
									currentInventoryObjectToCombine = firstObjectToCombine.objectToCombine.GetComponentInChildren<inventoryObject> ();

									firstObjectCombinedOnNewBehavior = secondObjectToCombine;
									secondObjectCombinedOnNewBehavior = firstObjectToCombine;
								}
							} else {

								if (firstObjectToCombine.useNewBehaviorOnCombine) {
									currentInventoryObjectToCombine = getInventoryObjectComponentByInventoryGameObject (firstObjectToCombine.inventoryGameObject);

									firstObjectCombinedOnNewBehavior = firstObjectToCombine;
									secondObjectCombinedOnNewBehavior = secondObjectToCombine;
								} else {
									currentInventoryObjectToCombine = getInventoryObjectComponentByInventoryGameObject (secondObjectToCombine.inventoryGameObject);

									firstObjectCombinedOnNewBehavior = secondObjectToCombine;
									secondObjectCombinedOnNewBehavior = firstObjectToCombine;
								}
							}

							if (currentInventoryObjectToCombine != null) {
								canBeCombined = true;
							}
						} else {
							if (showDebugPrint) {
								print (firstObjectToCombine.Name);
							}

							combinedInventoryObject = firstObjectToCombine.combinedObject.GetComponentInChildren<inventoryObject> ().inventoryObjectInfo;

							canBeCombined = true;
						}
					}
				}

				if (canBeCombined) {
					bool enoughSpace = false;

					if (infiniteSlots) {
						enoughSpace = true;
					} else {
						int amountFromFirstObject = firstObjectToCombine.amount;
						int amountFromSecondObject = secondObjectToCombine.amount;

						if (amountFromFirstObject == 1 || amountFromSecondObject == 1) {
							enoughSpace = true;
						} else {
							if (getNumberOfFreeSlots () >= 1) {
								enoughSpace = true;
							}
						}
					}

					if (enoughSpace) {
						if (secondObjectToCombine.combinedObjectMessage != "") {
							showObjectMessage (secondObjectToCombine.combinedObjectMessage, combineObjectMessageTime, combinedObjectMessage);
						}

						setCombineIcontPressedState (false, firstObjectToCombine);

						if (useNewBehaviorOnCombine) {
							currentInventoryObjectToCombine.combineObjectsOnNewBehavior (gameObject, firstObjectCombinedOnNewBehavior);

							return;
						} else {
							firstObjectToCombine.amount -= 1;

							if (firstObjectToCombine.infiniteAmount && firstObjectToCombine.amount <= 0) {
								firstObjectToCombine.amount = 1;
							}

							updateAmount (firstObjectToCombine, firstObjectToCombine.amount);

							if (firstObjectToCombine.amount == 0) {
								removeButton (firstObjectToCombine);
							}

							secondObjectToCombine.amount -= 1;

							if (secondObjectToCombine.infiniteAmount && secondObjectToCombine.amount <= 0) {
								secondObjectToCombine.amount = 1;
							}

							updateAmount (secondObjectToCombine, secondObjectToCombine.amount);

							if (secondObjectToCombine.amount == 0) {
								removeButton (secondObjectToCombine);
							}

							addObjectToInventory (combinedInventoryObject, 1, -1);
						}

						if (useNewBehaviorOnCombine) {
							addObjectToInventory (combinedInventoryObject, 1, -1);
						}

						firstObjectToCombine = null;

						secondObjectToCombine = null;

						if (!useNewBehaviorOnCombine) {
							bool iconPressed = false;
							//set as current inventory object the combined element
							int inventoryListCount = inventoryList.Count;

							for (int i = 0; i < inventoryListCount; i++) {

								inventoryInfo currentInventoryInfo = inventoryList [i];

								if (!iconPressed && currentInventoryInfo.inventoryGameObject == combinedInventoryObject.inventoryGameObject) {
									getPressedButton (currentInventoryInfo.button);

									iconPressed = true;
								}
							}
						}

						checkEventOnClickInventoryChange ();

						checkEventOnInventoryListChange ();
					} else {
						if (notEnoughSpaceToCombineMessage != "") {
							showObjectMessage (notEnoughSpaceToCombineMessage, combineObjectMessageTime, combinedObjectMessage);
						}
					}
				} else {
					if (unableToCombineMessage != "") {
						showObjectMessage (unableToCombineMessage, combineObjectMessageTime, combinedObjectMessage);
					}

					stopCombineObjects = true;
				}

				checkCurrentUseInventoryObject ();
			} else {
				stopCombineObjects = true;
			}

			if (stopCombineObjects) {
				disableObjectsToCombineIcon ();

				firstObjectToCombine = null;
				secondObjectToCombine = null;
			}
		}
	}

	public void setCombineObjectsWithNewBehaviorResult (int amountPicked, bool combinedSuccessfully)
	{
		if (showDebugPrint) {
			print (firstObjectCombinedOnNewBehavior.Name + " " + amountPicked + " " + combinedSuccessfully);
		}

		if (combinedSuccessfully) {
			if (amountPicked > 0) {
				string objectUsedMessage = firstObjectCombinedOnNewBehavior.newBehaviorOnCombineMessage;

				if (objectUsedMessage != "") {
					if (showDebugPrint) {
						print (secondObjectCombinedOnNewBehavior.Name);
					}

					objectUsedMessage = objectUsedMessage.Replace ("-OBJECT-", secondObjectCombinedOnNewBehavior.Name);

					if (firstObjectCombinedOnNewBehavior.useOneUnitOnNewBehaviourCombine) {
						objectUsedMessage = objectUsedMessage.Replace ("-AMOUNT-", firstObjectCombinedOnNewBehavior.amountPerUnit.ToString ());
					} else {
						objectUsedMessage = objectUsedMessage.Replace ("-AMOUNT-", amountPicked.ToString ());
					}

					showObjectMessage (objectUsedMessage, usedObjectMessageTime, usedObjectMessage);
				}

				if (firstObjectCombinedOnNewBehavior.useOneUnitOnNewBehaviourCombine) {
					firstObjectCombinedOnNewBehavior.amount -= 1;
				} else {
					firstObjectCombinedOnNewBehavior.amount -= amountPicked;
				}

				if (firstObjectCombinedOnNewBehavior.infiniteAmount && firstObjectCombinedOnNewBehavior.amount <= 0) {
					firstObjectCombinedOnNewBehavior.amount = 1;
				}

				updateAmount (firstObjectCombinedOnNewBehavior, firstObjectCombinedOnNewBehavior.amount);

				if (firstObjectCombinedOnNewBehavior.amount == 0) {
					removeButton (firstObjectCombinedOnNewBehavior);
				}
			} else {
				string objectUsedMessage = canBeCombinedButObjectIsFullMessage;

				if (objectUsedMessage != "") {

					objectUsedMessage = objectUsedMessage.Replace ("-OBJECTNAME-", secondObjectCombinedOnNewBehavior.Name);

					showObjectMessage (objectUsedMessage, usedObjectMessageTime, usedObjectMessage);
				}
			}

			checkEventOnInventoryListChange ();
		} else {
			if (unableToCombineMessage != "") {
				showObjectMessage (unableToCombineMessage, usedObjectMessageTime, usedObjectMessage);
			}
		}

		firstObjectToCombine = null;

		secondObjectToCombine = null;

		checkEventOnClickInventoryChange ();
	}

	public inventoryInfo getCurrentInventoryObjectInfo ()
	{
		return currentInventoryObject;
	}

	public inventoryInfo getCurrentFirstObjectToCombine ()
	{
		return firstObjectToCombine;
	}

	public inventoryInfo getCurrentSecondObjectToCombine ()
	{
		return secondObjectToCombine;
	}
	//END FUNCTIONS TO COMBINE OBJECTS


	public void setCurrentPickupObject (pickUpObject newPickupObject)
	{
		currentPickupObject = newPickupObject;
	}

	public void setObjectInfo (inventoryInfo currentInventoryObjectInfo)
	{
		resetAndDisableNumberOfObjectsToUseMenu ();

		setMenuIconElementPressedState (false);

		if (isCurrentObjectNotNull ()) {
			firstObjectToCombine = currentInventoryObject;
		}

		currentInventoryObject = currentInventoryObjectInfo;

		GameObject currentInventoryObjectPrefab = emptyInventoryPrefab;

		bool objectFound = false;

		GameObject inventoryObjectPrefabObtained = mainInventoryListManager.getInventoryPrefab (currentInventoryObject.inventoryGameObject);

		if (inventoryObjectPrefabObtained != null) {
			currentInventoryObjectPrefab = inventoryObjectPrefabObtained;

			objectFound = true;
		}

		if (objectFound) {
			currentInventoryObjectManager = currentInventoryObjectPrefab.GetComponentInChildren<inventoryObject> ();

			secondObjectToCombine = currentInventoryObject;

			setMenuIconElementPressedState (true);
			currentObjectName.text = currentInventoryObject.Name;
			currentObjectInfo.text = currentInventoryObject.objectInfo;

			resetScroll (inventoryObjectInforScrollbar);

			if (currentInventoryObject.canBeUsed) {
				useButtonImage.color = buttonUsable;
			} else {
				useButtonImage.color = buttonNotUsable;
			}

			if (currentInventoryObject.canBeEquiped) {
				if (currentInventoryObject.isEquiped) {
					setEquipButtonState (false);
				} else {
					setEquipButtonState (true);
				}
			} else {
				equipButtonImage.color = buttonNotUsable;
				unequipButtonImage.color = buttonNotUsable;

				equipButton.SetActive (true);
				unequipButton.SetActive (false);
			}

			if (currentInventoryObject.canBeDropped) {
				dropButtonImage.color = buttonUsable;
				dropAllUnitsObjectButtonImage.color = buttonUsable;
			} else {
				dropButtonImage.color = buttonNotUsable;
				dropAllUnitsObjectButtonImage.color = buttonUsable;
			}

			if (currentInventoryObject.canBeDiscarded) {
				discardButtonImage.color = buttonUsable;
			} else {
				discardButtonImage.color = buttonNotUsable;
			}

			if (currentInventoryObject.canBeCombined) {
				combineButtonImage.color = buttonUsable;
			} else {
				combineButtonImage.color = buttonNotUsable;
			}

			examineButtonImage.color = buttonUsable;

			objectImage.enabled = true;
			destroyObjectInCamera ();

			if (currentInventoryObject.inventoryGameObject) {
				objectInCamera = (GameObject)Instantiate (currentInventoryObject.inventoryGameObject, 
					lookObjectsPosition.transform.position, Quaternion.identity, lookObjectsPosition);
			}

			if (combiningObjects) {
				combineCurrentObject ();
			}
		} else {
			print ("WARNING: the inventory object to search " + currentInventoryObjectInfo.Name + " hasn't been found on the inventory list. " +
			"Please make sure that the component Inventory List Manager is properly configured and with the inventory list updated");
		}
	}

	public GameObject getInventoryPrefab (GameObject inventoryGameObject)
	{
		return mainInventoryListManager.getInventoryPrefab (inventoryGameObject);
	}

	public inventoryObject getInventoryObjectComponentByInventoryGameObject (GameObject inventoryGameObjectToSearch)
	{
		return mainInventoryListManager.getInventoryObjectComponentByInventoryGameObject (inventoryGameObjectToSearch);
	}

	public bool isInventoryFull ()
	{
		if (infiniteSlots || infiniteAmountPerSlot) {
			return false;
		}

		bool isFull = true;

		int inventoryListCount = inventoryList.Count;

		for (int i = 0; i < inventoryListCount; i++) {
			if (inventoryList [i].amount < amountPerSlot) {
				isFull = false;
			}
		}

		return isFull;
	}

	public int getNumberOfFreeSlots ()
	{
		if (infiniteSlots) {
			return 1;
		}

		int numberOfSlots = 0;

		int inventoryListCount = inventoryList.Count;

		for (int i = 0; i < inventoryListCount; i++) {
			if (inventoryList [i].amount == 0) {
				numberOfSlots++;
			}
		}

		return numberOfSlots;
	}

	public int getRealAmountOfFreeSlots ()
	{
		int numberOfSlots = 0;

		int inventoryListCount = inventoryList.Count;

		for (int i = 0; i < inventoryListCount; i++) {
			if (inventoryList [i].amount == 0) {
				numberOfSlots++;
			}
		}

		return numberOfSlots;
	}

	public int freeSpaceInSlot (GameObject inventoryObjectMesh)
	{
		int freeSpaceAmount = -1;

		int inventoryListCount = inventoryList.Count;

		for (int i = 0; i < inventoryListCount; i++) {
			inventoryInfo currentInventoryInfo = inventoryList [i];

			if (currentInventoryInfo.inventoryGameObject == inventoryObjectMesh) {
				freeSpaceAmount = amountPerSlot - currentInventoryInfo.amount;
			}
		}

		return freeSpaceAmount;
	}

	public bool checkIfObjectCanBeStored (GameObject inventoryObjectMesh, int amountToStore)
	{
		if (infiniteSlots) {
			//if the inventory has infinite slots, it can be stored
			return true;
		}

		//get the number of free slots in the inventory
		int numberOfFreeSlots = getRealAmountOfFreeSlots ();

		//check if the object to store is already in the inventory, to obtain that amount
		int freeSpaceAmountOnSlot = freeSpaceInSlot (inventoryObjectMesh);

		bool hasSameObject = false;
		if (freeSpaceAmountOnSlot > 0) {
			hasSameObject = true;
		}

		//if the number of free slots is 0, then 
		if (numberOfFreeSlots == 0) {

			//if there is no at least an slot with the object to store, the object has not space to be stored
			if (!hasSameObject) {
				return false;
			} 

			//if there is at least an slot with the object to store, but amount to store is higher than the regular amount per slot, the object has not space to be stored
			else if (amountToStore > amountPerSlot) {
				return false;
			} 

			//if there is not enough free space in that slot, the object can't be stored
			else if (amountToStore > freeSpaceAmountOnSlot) {
				return false;
			}
		}

		//if there are free slots or the a same type of object already stored and infinite units can be stored in an slot, the object can be stored
		if (infiniteAmountPerSlot && (numberOfFreeSlots > 0 || hasSameObject)) {
			return true;
		}

		//get the total amount of units that can be stored in the inventory to compare it with the amount to store from the current object to get
		int freeUnits = numberOfFreeSlots * amountPerSlot + freeSpaceAmountOnSlot;

		if (freeUnits > amountToStore) {
			return true;
		}

		return false;
	}

	public void addAmountToInventorySlot (inventoryInfo objectInfo, int currentSlotAmount, int extraAmount)
	{
		bool amountAdded = false;

		int inventoryListCount = inventoryList.Count;

		for (int i = 0; i < inventoryListCount; i++) {
			inventoryInfo currentInventoryInfo = inventoryList [i];

			if (currentInventoryInfo.inventoryGameObject == objectInfo.inventoryGameObject &&
			    (currentInventoryInfo.amount == currentSlotAmount || infiniteAmountPerSlot) &&
			    !amountAdded) {

				currentInventoryInfo.amount += extraAmount;

				if (currentInventoryInfo.storeTotalAmountPerUnit && objectInfo.amountPerUnit > 0) {
					currentInventoryInfo.amountPerUnit = 0;
				} else {
					currentInventoryInfo.amountPerUnit = objectInfo.amountPerUnit;
				}

				updateAmount (currentInventoryInfo, currentInventoryInfo.amount);

				amountAdded = true;
			}
		}

		if (amountAdded) {
			//print ("object amount increased");
		} else {
			//print ("object not found on inventory, added new one");
			addObjectToInventory (objectInfo, extraAmount, -1);
		}
	}

	public float getTimeTime ()
	{
		if (pauseManager.isRegularTimeScaleActive ()) {
			return Time.time;
		}

		return Time.unscaledTime;
	}

	public float getDeltaTime ()
	{
		if (pauseManager.isRegularTimeScaleActive ()) {
			return Time.deltaTime;
		}

		return Time.unscaledDeltaTime;
	}

	public void addObjectAmountToInventoryByName (string objectName, int amountToMove)
	{
		inventoryInfo inventoryInfoToCheck = getInventoryInfoByName (objectName);

		if (inventoryInfoToCheck != null) {
			inventoryInfo newObjectToAdd = new inventoryInfo (inventoryInfoToCheck);
			newObjectToAdd.amount = amountToMove;

			tryToPickUpObject (newObjectToAdd);
		}
	}

	public void removeObjectAmountFromInventoryByName (string objectName, int amountToMove)
	{
		int inventoryListCount = inventoryList.Count;

		for (int i = 0; i < inventoryListCount; i++) {
			if (inventoryList [i].Name.Equals (objectName)) {
				removeObjectAmountFromInventory (i, amountToMove);

				return;
			}
		}
	}

	public void sellObjectOnVendorSystem (int objectToMoveIndex, int amountToMove)
	{
		removeObjectAmountFromInventory (objectToMoveIndex, amountToMove);

		checkIfWeaponUseAmmoFromInventory ();

		updateAllWeaponSlotAmmo ();
	}

	public void removeObjectAmountFromInventory (int objectToMoveIndex, int amountToMove)
	{
		bool amountNeededAvaliable = false;

		currentInventoryObject = inventoryList [objectToMoveIndex];

		if (amountToMove > 1) {
			if (currentInventoryObject.amount >= amountToMove) {
				amountNeededAvaliable = true;
			}
		} else {
			amountNeededAvaliable = true;
		}

		if (amountNeededAvaliable) {
			//			print ("amount removed from " + currentInventoryObject.Name + " is " + amountToMove);

			currentInventoryObject.amount -= amountToMove;

			updateAmount (currentInventoryObject, currentInventoryObject.amount);

			if (currentInventoryObject.amount == 0) {
				if (currentInventoryObject.canBeEquiped) {
					if (currentInventoryObject.isEquiped) {
//						if (currentInventoryObject.isWeapon) {
						unEquipCurrentObject ();
//						}
					}
				}

				removeButton (currentInventoryObject);
			}

			setInventory (false);

			checkEmptySlots ();
		} else {
			if (nonNeededAmountAvaliable != "") {
				showObjectMessage (nonNeededAmountAvaliable, usedObjectMessageTime, usedObjectMessage);
			}
		}

		currentInventoryObject = null;
	}

	public void setCurrentInventoryObject (inventoryInfo newObject)
	{
		currentInventoryObject = newObject;
	}

	public int getInventoryObjectIndexByName (string inventoryObjectName)
	{
		int inventoryListCount = inventoryList.Count;

		for (int i = 0; i < inventoryListCount; i++) {
			if (inventoryList [i].Name.Equals (inventoryObjectName)) {
				return i;
			}
		}

		return -1;
	}

	public int getInventoryObjectAmountByName (string inventoryObjectName)
	{
		int totalAmount = 0;

		int inventoryListCount = inventoryList.Count;

		for (int i = 0; i < inventoryListCount; i++) {
			inventoryInfo currentInventoryInfo = inventoryList [i];

			if (currentInventoryInfo.Name.Equals (inventoryObjectName)) {
				totalAmount += currentInventoryInfo.amount;
			}
		}

		if (totalAmount > 0) {
			return totalAmount;
		}

		return -1;
	}

	public int getInventoryObjectAmountPerUnitByName (string inventoryObjectName)
	{
		return mainInventoryListManager.getInventoryObjectAmountPerUnitByName (inventoryObjectName);
	}

	public bool checkIfInventoryObjectEquipped (string inventoryObjectName)
	{
		int inventoryListCount = inventoryList.Count;

		for (int i = 0; i < inventoryListCount; i++) {
			inventoryInfo currentInventoryInfo = inventoryList [i];

			if (currentInventoryInfo.Name.Equals (inventoryObjectName)) {
				if (currentInventoryInfo.isEquiped) {
					return true;
				}
			}
		}

		return false;
	}

	public void setUsedByAIState (bool state)
	{
		usedByAI = state;
	}

	public void setInventoryEnabledState (bool state)
	{
		inventoryEnabled = state;
	}

	public bool playerIsBusy ()
	{
		if (!playerControllerManager.isUsingDevice () &&
		    !playerControllerManager.isUsingSubMenu () &&
		    !playerControllerManager.isPlayerMenuActive () &&
		    playerControllerManager.canPlayerMove ()) {
			return false;
		}

		return true;
	}

	public bool isUsingGenericModelActive ()
	{
		return playerControllerManager.isUsingGenericModelActive ();
	}

	public List<inventoryInfo> getInventoryList ()
	{
		return inventoryList;
	}

	//START FUNCTIONS USED TO EXAMINE INVENTORY OBJECTS
	public void examineCurrentPickupObject (inventoryInfo inventoryObjectToPickup)
	{
		openOrCloseInventory (true);

		currentInventoryObject = new inventoryInfo (inventoryObjectToPickup);

		setObjectInfo (currentInventoryObject);

		examineCurrentObject (true);

		takeObjectInExaminePanelButton.SetActive (true);

		adjustCameraFovToSeeInventoryObject ();

		examineObjectName.text = currentInventoryObject.Name + " x " + currentInventoryObject.amount;
	}

	public void confirmToPickCurrentObjectInExaminingPanel ()
	{
		currentPickupObject.pickObjectByButton ();

		examineCurrentObject (false);
	}

	public void examineCurrentObject (bool state)
	{
		if (isCurrentObjectNotNull ()) {
			examiningObject = state;
			examineObjectPanel.SetActive (examiningObject);

			examineObjectName.text = currentInventoryObject.Name;
			examineObjectDescription.text = currentInventoryObject.objectInfo;

			if (examiningObject) {
				placeObjectInCameraPosition ();
				placeObjectInCameraRotation ();

				float currentCameraFov = originalFov;

				if (currentInventoryObjectManager) {
					if (currentInventoryObjectManager.useZoomRange) {
						currentCameraFov = currentInventoryObjectManager.initialZoom;
					}
				}

				currentCameraFov += extraCameraFovOnExamineObjects;
				inventoryCamera.fieldOfView = currentCameraFov;

				pauseManager.openOrClosePlayerMenu (inventoryOpened, examineObjectPanel.transform, useBlurUIPanel);

			} else {
				stopObjectInCameraPositionMovevement ();
				stopObjectInCameraRotationMovevement ();

				float currentCameraFov = originalFov;

				if (currentInventoryObjectManager) {
					if (currentInventoryObjectManager.useZoomRange) {
						currentCameraFov = currentInventoryObjectManager.initialZoom;
					}
				}

				inventoryCamera.fieldOfView = currentCameraFov;
				objectInCamera.transform.localPosition = Vector3.zero;
				objectInCamera.transform.localRotation = Quaternion.identity;

				pauseManager.openOrClosePlayerMenu (inventoryOpened, inventoryPanel.transform, useBlurUIPanel);

				if (currentPickupObject != null) {
					openOrCloseInventory (false);

					currentPickupObject = null;
					currentInventoryObject = null;
				}
			}

			takeObjectInExaminePanelButton.SetActive (false);

			checkInventoryOptionsOnSlot (false);
		}
	}

	public bool playerIsExaminingInventoryObject ()
	{
		return examiningObject;
	}
	//END FUNCTIONS USED TO EXAMINE INVENTORY OBJECTS


	//START FUNCTIONS USED ON THE INVENTORY BANK
	public void setNewInventoryList (List<inventoryInfo> newInventoryList)
	{
		int inventoryListCount = inventoryList.Count;

		for (int i = 0; i < inventoryListCount; i++) {
			inventoryList [i] = new inventoryInfo (newInventoryList [i]);
		}

		checkEmptySlots ();

		updateFullInventorySlots ();
	}

	public void moveObjectToBank (int objectToMoveIndex, int amountToMove)
	{
		removeObjectAmountFromInventory (objectToMoveIndex, amountToMove);

		checkIfWeaponUseAmmoFromInventory ();

		updateAllWeaponSlotAmmo ();
	}

	public void setCurrentInventoryBankSystem (GameObject inventoryBankGameObject)
	{
		mainInventoryBankUISystem.setCurrentInventoryBankSystem (inventoryBankGameObject);
	}

	public void setCurrentVendorSystem (GameObject inventoryBankGameObject)
	{
		mainVendorUISystem.setCurrentInventoryBankSystem (inventoryBankGameObject);
	}

	public void openOrCloseInventoryBankMenu (bool state)
	{
		mainInventoryBankUISystem.openOrCloseInventoryBankMenu (state);
	}
	//END FUNCTIONS USED ON THE INVENTORY BANK

	public void instantiateMainInventoryManagerPrefab ()
	{
		if (mainInventoryListManager == null) {
			GKC_Utils.instantiateMainManagerOnSceneWithType (mainInventoryManagerName, typeof(inventoryListManager));

			mainInventoryListManager = FindObjectOfType<inventoryListManager> ();

			if (mainInventoryListManager == null) {
				GameObject newMainInventoryManager = (GameObject)Instantiate (mainInventoryManagerPrefab, Vector3.zero, Quaternion.identity);
				newMainInventoryManager.name = mainInventoryManagerPrefab.name;

				mainInventoryListManager = newMainInventoryManager.GetComponentInChildren<inventoryListManager> ();
			}
		} else {
			print ("Main Inventory Manager is already on the scene");
		}
	}

	public void selectMainInventoryManagerOnScene ()
	{
		if (mainInventoryListManager == null) {
			instantiateMainInventoryManagerPrefab ();
		}

		if (mainInventoryListManager != null) {
			GKC_Utils.setActiveGameObjectInEditor (mainInventoryListManager.gameObject);
		}
	}

	bool mainInventoryManagerFound;

	void getMainInventoryManager ()
	{
		if (mainInventoryListManager != null) {
			mainInventoryManagerFound = true;
		} else {
			if (mainInventoryListManager == null) {
				mainInventoryListManager = FindObjectOfType<inventoryListManager> ();
			}

			if (mainInventoryListManager != null) {
				mainInventoryManagerFound = true;
			}
		}
	}

	void checkMainInventoryManager ()
	{
		if (!mainInventoryManagerFound) {
			instantiateMainInventoryManagerPrefab ();

			getMainInventoryManager ();

			if (!mainInventoryManagerFound) {
				print ("WARNING: No Main Inventory Manager Prefab found, make sure to drop it on the scene");

				return;
			}
		}
	}


	//END INVENTORY MANAGEMENT ELEMENTS


	//INPUT FUNCTIONS
	public void inputConfirmToPickCurrentObjectInExaminingPanel ()
	{
		if (currentPickupObject != null && examiningObject) {
			confirmToPickCurrentObjectInExaminingPanel ();

			usingDevicesManager.setPauseUseDeviceButtonForDurationState (0.5f);
		}
	}

	public void inputOpenOrCloseInventory ()
	{
		if (pauseManager.isOpenOrClosePlayerOpenMenuByNamePaused ()) {
			return;
		}

		if (inventoryEnabled && openInventoryMenuEnabled) {
			openOrCloseInventory (!inventoryOpened);
		}

	}

	public void inputSetZoomInState (bool state)
	{
		if (inventoryEnabled && inventoryOpened) {
			if (state) {
				zoomInEnabled ();
			} else {
				zoomInDisabled ();
			}
		}
	}

	public void inputSetZoomOutState (bool state)
	{
		if (inventoryEnabled && inventoryOpened) {
			if (state) {
				zoomOutEnabled ();
			} else {
				zoomOutDisabled ();
			}
		}
	}

	//START WEAPON ELEMENTS FROM INVENTORY
	public void useAmmoFromInventory (string weaponName, int amountToUse)
	{
		removeObjectAmountFromInventoryByName (weaponName, amountToUse);
	}

	public void checkIfWeaponUseAmmoFromInventory ()
	{
		if (storePickedWeaponsOnInventory) {
			int inventoryListCount = inventoryList.Count;

			for (int i = 0; i < inventoryListCount; i++) {
				inventoryInfo currentInventoryInfo = inventoryList [i];

				if (currentInventoryInfo.isWeapon) {
					if (!currentInventoryInfo.isMeleeWeapon) {
						if (currentInventoryInfo.mainWeaponObjectInfo != null) {
							if (currentInventoryInfo.mainWeaponObjectInfo.isWeaponUseRemainAmmoFromInventoryActive ()) {

								int currentAmmoFromInventory = getInventoryObjectAmountByName (currentInventoryInfo.mainWeaponObjectInfo.getWeaponAmmoName ());

								if (currentAmmoFromInventory < 0) {
									currentAmmoFromInventory = 0;
								}

								currentInventoryInfo.mainWeaponObjectInfo.setWeaponRemainAmmoAmount (currentAmmoFromInventory);
							}
						}
					}
				}
			}
		}
	}

	public void setActivatingDualWeaponSlotState (bool state)
	{
		activatingDualWeaponSlot = state;
	}

	public void setCurrentRighWeaponNameValue (string newValue)
	{
		currentRighWeaponName = newValue;
	}

	public void setCurrentLeftWeaponNameValue (string newValue)
	{
		currentLeftWeaponName = newValue;
	}
	//END WEAPON ELEMENTS FROM INVENTORY


	//START UI ELEMENTS OF INVENTORY
	public void adjustCameraFovToSeeInventoryObject ()
	{
		float currentCameraFov = originalFov;

		if (currentInventoryObjectManager) {
			if (currentInventoryObjectManager.useZoomRange) {
				currentCameraFov = currentInventoryObjectManager.initialZoom;
			}
		}

		if (inventoryCamera.fieldOfView != currentCameraFov) {
			checkResetCameraFov (currentCameraFov);
		}
	}

	public void disableCurrentObjectInfo ()
	{
		currentObjectName.text = "";
		currentObjectInfo.text = "";
		useButtonImage.color = buttonNotUsable;

		equipButtonImage.color = buttonNotUsable;
		unequipButtonImage.color = buttonNotUsable;

		equipButton.SetActive (true);
		unequipButton.SetActive (false);

		discardButtonImage.color = buttonNotUsable;

		dropAllUnitsObjectButtonImage.color = buttonNotUsable;

		dropButtonImage.color = buttonNotUsable;
		combineButtonImage.color = buttonNotUsable;
		examineButtonImage.color = buttonNotUsable;
		objectImage.enabled = false;

		checkInventoryOptionsOnSlot (false);
	}

	public void checkInventoryOptionsOnSlot (bool state)
	{
		if (useInventoryOptionsOnSlot) {
			if (isCurrentObjectNotNull ()) {
				setInventoryOptionsOnSlotPanelActiveState (state);

				if (state) {
					inventoryOptionsOnSlotPanelTargetToFollow = currentInventoryObject.menuIconElement.inventoryOptionsOnSlotPanelPosition;
					inventoryOptionsOnSlotPanel.position = inventoryOptionsOnSlotPanelTargetToFollow.position;

					bool useState = currentInventoryObject.canBeUsed;
					bool equipState = currentInventoryObject.canBeEquiped && !currentInventoryObject.isEquiped;
					bool unEquipState = currentInventoryObject.canBeEquiped && currentInventoryObject.isEquiped;
					bool dropState = currentInventoryObject.canBeDropped;
					bool combineState = currentInventoryObject.canBeCombined;
					bool examineState = true;
					bool holdState = currentInventoryObject.canBeHeld;
					bool discardState = currentInventoryObject.canBeDiscarded;

					mainInventorySlotOptionsButtons.setButtonsState (useState, equipState, unEquipState, dropState, combineState, examineState, holdState, discardState);
				}

				inventoryOptionsOnSlotPanelActive = state;
			} else {
				setInventoryOptionsOnSlotPanelActiveState (false);

				inventoryOptionsOnSlotPanelActive = false;
			}
		}
	}

	public void setInventoryOptionsOnSlotPanelActiveState (bool state)
	{
		inventoryOptionsOnSlotPanel.gameObject.SetActive (state);
	}

	public void setEquipButtonState (bool state)
	{
		if (state) {
			unequipButtonImage.color = buttonNotUsable;
			unequipButton.SetActive (false);
			equipButtonImage.color = buttonUsable;
			equipButton.SetActive (true);
		} else {
			unequipButtonImage.color = buttonUsable;
			unequipButton.SetActive (true);
			equipButtonImage.color = buttonNotUsable;
			equipButton.SetActive (false);
		}
	}

	public void setAllAmountObjectToUse ()
	{
		numberOfObjectsToUse = currentInventoryObject.amount;
		setNumberOfObjectsToUseText (numberOfObjectsToUse);
	}

	public void addObjectToUse ()
	{
		numberOfObjectsToUse++;

		if (numberOfObjectsToUse > currentInventoryObject.amount) {
			numberOfObjectsToUse = currentInventoryObject.amount;
		}

		setNumberOfObjectsToUseText (numberOfObjectsToUse);
	}

	public void removeObjectToUse ()
	{
		numberOfObjectsToUse--;

		if (numberOfObjectsToUse < 1) {
			numberOfObjectsToUse = 1;
		}

		setNumberOfObjectsToUseText (numberOfObjectsToUse);
	}

	public void startToAddObjectToUse ()
	{
		addingObjectToUse = true;

		lastTimeConfigureNumberOfObjects = getTimeTime ();
	}

	public void stopToAddObjectToUse ()
	{
		addingObjectToUse = false;

		lastTimeAddObjectToUse = 0;

		useFasterNumberObjectsToUseRateActive = false;
	}

	public void startToRemoveObjectToUse ()
	{
		removinvObjectToUse = true;

		lastTimeConfigureNumberOfObjects = getTimeTime ();
	}

	public void stopToRemoveObjectToUse ()
	{
		removinvObjectToUse = false;

		lastTimeRemoveObjectToUse = 0;

		useFasterNumberObjectsToUseRateActive = false;
	}

	public void setNumberOfObjectsToUseText (int amount)
	{
		numberOfObjectsToUseText.text = amount.ToString ();
	}

	public void enableNumberOfObjectsToUseMenu (RectTransform menuPosition)
	{
		if (activeNumberOfObjectsToUseMenu) {
			if (isCurrentObjectNotNull ()) {
				if ((currentInventoryObject.canBeUsed && menuPosition == numberOfObjectsToUseMenuPosition) ||
				    (currentInventoryObject.canBeDropped && menuPosition == numberOfObjectsToDropMenuPosition)) {

					numberOfObjectsToUseMenu.SetActive (true);
					numberOfObjectsToUseMenuRectTransform.anchoredPosition = menuPosition.anchoredPosition;
					numberOfObjectsToUse = 1;

					setNumberOfObjectsToUseText (numberOfObjectsToUse);
				}
			}
		}
	}

	public void disableNumberOfObjectsToUseMenu ()
	{
		numberOfObjectsToUse = 1;
		numberOfObjectsToUseMenu.SetActive (false);
	}

	public void resetNumberOfObjectsToUse ()
	{
		numberOfObjectsToUse = 1;
		setNumberOfObjectsToUseText (numberOfObjectsToUse);
	}

	public void resetAndDisableNumberOfObjectsToUseMenu ()
	{
		disableNumberOfObjectsToUseMenu ();

		resetNumberOfObjectsToUse ();
	}

	public void playSound (AudioClip newClip)
	{
		if (useAudioSounds) {
			mainAudioSource.PlayOneShot (newClip);
		}
	}

	public void showObjectMessage (string message, float messageDuration, GameObject messagePanel)
	{
		if (message == "") {
			return;
		}

		if (objectMessageCoroutine != null) {
			StopCoroutine (objectMessageCoroutine);
		}

		objectMessageCoroutine = StartCoroutine (showObjectMessageCoroutine (message, messageDuration, messagePanel));
	}

	IEnumerator showObjectMessageCoroutine (string info, float messageDuration, GameObject messagePanel)
	{
		usingDevicesManager.checkDeviceName ();

		if (previousMessagePanel) {
			previousMessagePanel.SetActive (false);
		}

		messagePanel.SetActive (true);

		previousMessagePanel = messagePanel;

		messagePanel.GetComponentInChildren<Text> ().text = info;

		yield return new WaitForSecondsRealtime (messageDuration);

		messagePanel.SetActive (false);
	}

	public void showObjectMessage (string message, float messageDuration)
	{
		showObjectMessage (message, messageDuration, usedObjectMessage);
	}

	public void enableOrDisableHUD (bool state)
	{
		mainInventoryQuickAccessSlotsSystem.enableOrDisableHUD (state);
	}

	public void enableOrDisableWeaponSlotsParentOutOfInventory (bool state)
	{
		mainInventoryQuickAccessSlotsSystem.enableOrDisableQuickAccessSlotsParentOutOfInventory (state);
	}

	public void checkToEnableOrDisableWeaponSlotsParentOutOfInventory (bool state)
	{
		mainInventoryQuickAccessSlotsSystem.checkToEnableOrDisableQuickAccessSlotsParentOutOfInventory (state);
	}

	public void setShowWeaponSlotsPausedState (bool state)
	{
		mainInventoryQuickAccessSlotsSystem.setShowQuickAccessSlotsPausedState (state);
	}

	public void updateAmountsInventoryPanel ()
	{
		int inventoryListCount = inventoryList.Count;

		for (int i = 0; i < inventoryListCount; i++) {
			inventoryInfo currentInventoryInfo = inventoryList [i];

			updateAmount (currentInventoryInfo, currentInventoryInfo.amount);
		}
	}

	public void updateAmount (inventoryInfo inventoryInfoToUpdate, int amount)
	{
		bool showRegularAmount = true;

		inventoryMenuIconElement currentIconElement = inventoryInfoToUpdate.menuIconElement;

		bool currentIconLocated = currentIconElement != null;

		if (inventoryInfoToUpdate.amountPerUnit > 0) {
			if (inventoryInfoToUpdate.showAmountPerUnitInAmountText) {

				if (currentIconLocated) {
					currentIconElement.amount.text = (amount * inventoryInfoToUpdate.amountPerUnit).ToString ();
					currentIconElement.amountPerUnitPanel.SetActive (false);
				}

				showRegularAmount = false;
			} else {
				if (currentIconLocated) {
					currentIconElement.amountPerUnitPanel.SetActive (true);
					currentIconElement.amountPerUnitText.text = inventoryInfoToUpdate.amountPerUnit.ToString ();
				}
			}
		} else {
			if (currentIconLocated) {
				currentIconElement.amountPerUnitPanel.SetActive (false);
			}
		}

		if (showRegularAmount) {
			if (inventoryInfoToUpdate.infiniteAmount) {
				if (currentIconLocated) {
					currentIconElement.amount.text = "";

					currentIconElement.infiniteAmountIcon.SetActive (true);
				}
			} else {
				if (currentIconLocated) {
					if (inventoryInfoToUpdate.amount > 1 || showObjectAmountIfEqualOne) {
						currentIconElement.amount.text = amount.ToString ();
					} else {
						currentIconElement.amount.text = "";
					}

					if (currentIconElement.infiniteAmountIcon.activeSelf) {
						currentIconElement.infiniteAmountIcon.SetActive (false);
					}
				}
			}
		}
	}

	public void removeButton (inventoryInfo currentObj)
	{
		disableCurrentObjectInfo ();

		if (currentObj.menuIconElement != null) {
			currentObj.menuIconElement.activeSlotContent.SetActive (false);
			currentObj.menuIconElement.emptySlotContent.SetActive (true);

			currentObj.menuIconElement.amountPerUnitPanel.SetActive (false);
		}

		currentObj.resetInventoryInfo ();

		enableRotation = false;

		destroyObjectInCamera ();

		setMenuIconElementPressedState (false);

		currentInventoryObject = null;

		currentInventoryObjectManager = null;

		checkEventOnInventorySlotUnSelected ();
	}

	public void setMenuIconElementPressedState (bool state)
	{
		if (currentInventoryObject != null) {
			if (currentInventoryObject.menuIconElement != null) {
				currentInventoryObject.menuIconElement.pressedIcon.SetActive (state);
			}
		}
	}

	public void enableObjectRotation ()
	{
		if (objectInCamera) {
			enableRotation = true;
		}
	}

	public void disableObjectRotation ()
	{
		enableRotation = false;
	}

	public void destroyObjectInCamera ()
	{
		if (objectInCamera) {
			Destroy (objectInCamera);
		}
	}

	public void placeObjectInCameraPosition ()
	{
		stopObjectInCameraPositionMovevement ();

		objectInCameraPositionCoroutine = StartCoroutine (placeObjectInCameraPositionCoroutine ());
	}

	public void stopObjectInCameraPositionMovevement ()
	{
		if (objectInCameraPositionCoroutine != null) {
			StopCoroutine (objectInCameraPositionCoroutine);
		}
	}

	IEnumerator placeObjectInCameraPositionCoroutine ()
	{
		if (objectInCamera) {
			objectInCamera.transform.localPosition = lookObjectsPosition.forward * distanceToPlaceObjectInCamera;

			Vector3 targetPosition = Vector3.zero;

			while (objectInCamera.transform.localPosition != targetPosition) {
				objectInCamera.transform.localPosition = Vector3.MoveTowards (objectInCamera.transform.localPosition, targetPosition, getDeltaTime () * placeObjectInCameraSpeed);
				yield return null;
			}
		}
	}

	public void placeObjectInCameraRotation ()
	{
		stopObjectInCameraRotationMovevement ();

		objectInCameraRotationCoroutine = StartCoroutine (placeObjectInCameraRotationCoroutine ());
	}

	public void stopObjectInCameraRotationMovevement ()
	{
		if (objectInCameraRotationCoroutine != null) {
			StopCoroutine (objectInCameraRotationCoroutine);
		}
	}

	IEnumerator placeObjectInCameraRotationCoroutine ()
	{
		if (objectInCamera) {
			objectInCamera.transform.localRotation = Quaternion.identity;
			Vector3 targetRotation = new Vector3 (0, 360 * numberOfRotationsObjectInCamera, 0);

			float currentLerpTime = 0;
			float lerpTime = placeObjectInCameraRotationSpeed;

			while (targetRotation != Vector3.zero) {
				currentLerpTime += getDeltaTime ();

				if (currentLerpTime > lerpTime) {
					currentLerpTime = lerpTime;
				}

				float perc = currentLerpTime / lerpTime;
				targetRotation = Vector3.Lerp (targetRotation, Vector3.zero, perc);

				objectInCamera.transform.localEulerAngles = targetRotation;
				yield return null;
			}
		}
	}

	public void setInventoryPanelByName (string panelInfo)
	{
		mainInventoryMenuPanelsSystem.setInventoryPanelByName (panelInfo);
	}

	public void showInventoryFullMessage ()
	{
		if (inventoryFullCoroutine != null) {
			StopCoroutine (inventoryFullCoroutine);
		}

		inventoryFullCoroutine = StartCoroutine (showInventoryFullMessageCoroutine ());
	}

	IEnumerator showInventoryFullMessageCoroutine ()
	{
		fullInventoryMessage.SetActive (true);

		yield return new WaitForSecondsRealtime (fullInventoryMessageTime);

		fullInventoryMessage.SetActive (false);
	}

	public void zoomInEnabled ()
	{
		zoomingIn = true;
	}

	public void zoomInDisabled ()
	{
		zoomingIn = false;
	}

	public void zoomOutEnabled ()
	{
		zoomingOut = true;
	}

	public void zoomOutDisabled ()
	{
		zoomingOut = false;
	}

	public void checkResetCameraFov (float targetValue)
	{
		if (resetCameraFov != null) {
			StopCoroutine (resetCameraFov);
		}

		resetCameraFov = StartCoroutine (resetCameraFovCorutine (targetValue));
	}

	IEnumerator resetCameraFovCorutine (float targetValue)
	{
		while (inventoryCamera.fieldOfView != targetValue) {
			inventoryCamera.fieldOfView = Mathf.MoveTowards (inventoryCamera.fieldOfView, targetValue, getDeltaTime () * zoomSpeed);
			yield return null;
		}
	}

	public bool isInventoryMenuOpened ()
	{
		return inventoryOpened;
	}

	public void resetScroll (Scrollbar scrollBarToReset)
	{
		StartCoroutine (resetScrollCoroutine (scrollBarToReset));
	}

	IEnumerator resetScrollCoroutine (Scrollbar scrollBarToReset)
	{
		yield return new WaitForEndOfFrame ();
		yield return new WaitForEndOfFrame ();

		scrollBarToReset.value = 1;
	}

	public void resetInventorySlotsRectTransform ()
	{
		StartCoroutine (resetRectTransformCoroutine ());
	}

	IEnumerator resetRectTransformCoroutine ()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate (inventoryListContent.GetComponent<RectTransform> ());

		yield return new WaitForEndOfFrame ();
		yield return new WaitForEndOfFrame ();

		if (inventoryListScrollRect) {
			inventoryListScrollRect.verticalNormalizedPosition = 1;
		}
	}

	public void enableOrDisableInventoryMenu (bool state)
	{
		inventoryPanel.SetActive (state);
	}

	public void openOrClosePlayerMenuFromUseInventoryObject (bool state)
	{
		inventoryOpened = state;

		pauseManager.openOrClosePlayerMenu (inventoryOpened, inventoryPanel.transform, false);

		inventoryPanel.SetActive (inventoryOpened);

		pauseManager.setIngameMenuOpenedState ("Inventory Manager", inventoryOpened, false);

		setOpenOrCloseInventoryMenuState ();
	}

	public void openOrCloseInventory (bool state)
	{
		if ((!playerControllerManager.isPlayerMenuActive () || inventoryOpened) && !playerControllerManager.isUsingDevice () && !pauseManager.isGamePaused ()) {
			inventoryOpened = state;

			pauseManager.openOrClosePlayerMenu (inventoryOpened, inventoryPanel.transform, useBlurUIPanel);

			inventoryPanel.SetActive (inventoryOpened);

			pauseManager.setIngameMenuOpenedState ("Inventory Manager", inventoryOpened, true);

			setOpenOrCloseInventoryMenuState ();
		}
	}

	public void setOpenOrCloseInventoryMenuState ()
	{
		setMenuIconElementPressedState (false);

		disableObjectsToCombineIcon ();

		pauseManager.enableOrDisablePlayerMenu (inventoryOpened, true);

		if (examiningObject) {
			stopObjectInCameraPositionMovevement ();
			stopObjectInCameraRotationMovevement ();

			examiningObject = false;
			examineObjectPanel.SetActive (false);
		}

		destroyObjectInCamera ();

		if (inventoryOpened) {
			resetScroll (inventorySlotsScrollbar);
			resetScroll (inventoryObjectInforScrollbar);

			resetInventorySlotsRectTransform ();

			if (mainInventoryQuickAccessSlotsSystem.isShowQuickAccessSlotsAlwaysActive () || mainInventoryQuickAccessSlotsSystem.showQuickAccessSlotsWhenChangingSlot) {

				stopShowWeaponSlotsParentWhenWeaponSelectedCoroutuine ();

				moveWeaponSlotsToInventory ();

				updateWeaponCurrentlySelectedIcon (-1, false);
			}

			updateAllWeaponSlotAmmo ();
		} else {
			disableCurrentObjectInfo ();

			if (mainInventoryQuickAccessSlotsSystem.isShowQuickAccessSlotsAlwaysActive ()) {
				moveWeaponSlotsOutOfInventory ();

				weaponsManager.updateCurrentChoosedDualWeaponIndex ();

				mainInventoryQuickAccessSlotsSystem.checkModeToUpdateWeaponCurrentlySelectedIcon ();
			}
		}

		resetAndDisableNumberOfObjectsToUseMenu ();

		inventoryCamera.fieldOfView = originalFov;

		currentInventoryObject = null;

		currentInventoryObjectManager = null;

		if (currentUseInventoryObject) {
			currentUseInventoryObject.updateUseInventoryObjectState ();
		}

		inventoryCamera.enabled = inventoryOpened;

		unequipButton.SetActive (false);

		checkInventoryOptionsOnSlot (false);

		if (inventoryOpened) {
			checkEventOnInventoryOpened ();
		} else {
			checkEventOnInventoryClosed ();
		}

		if (inventoryOpened) {
			if (selectFirstInventoryObjectWhenOpeningMenu) {
				if (!isInventoryEmpty ()) {
					getPressedButton (inventoryList [0].button);

					setInventoryOptionsOnSlotPanelActiveState (false);
				}
			}
		}

		mainInventoryQuickAccessSlotsSystem.setOpenOrCloseInventoryMenuState (inventoryOpened);
	}

	public void openOrCLoseInventoryFromTouch ()
	{
		openOrCloseInventory (!inventoryOpened);
	}
	//END UI ELEMENTS OF INVENTORY


	//START OF QUICK ACCESS SLOTS ELEMENTS
	public void resetDragAndDropWeaponSlotState ()
	{
		mainInventoryQuickAccessSlotsSystem.resetDragAndDropSlotState ();
	}

	public void updateSingleWeaponSlotInfo (string newRighWeaponName, string newLeftWeaponName)
	{
		mainInventoryQuickAccessSlotsSystem.updateSingleWeaponSlotInfo (newRighWeaponName, newLeftWeaponName);
	}

	public void updateSingleWeaponSlotInfoWithoutAddingAnotherSlot (string newRighWeaponName)
	{
		mainInventoryQuickAccessSlotsSystem.updateSingleWeaponSlotInfoWithoutAddingAnotherSlot (newRighWeaponName);
	}

	public void updateDualWeaponSlotInfo (string newRighWeaponName, string newLeftWeaponName)
	{
		mainInventoryQuickAccessSlotsSystem.updateDualWeaponSlotInfo (newRighWeaponName, newLeftWeaponName);
	}

	public void updateWeaponSlotInfo (int weaponSlotIndex, inventoryInfo currentWeaponSlotToMove, inventoryQuickAccessSlotElement.quickAccessSlotInfo weaponSlotToUnEquip, playerWeaponSystem secondaryWeaponToEquip)
	{
		mainInventoryQuickAccessSlotsSystem.updateQuickAccessSlotInfo (weaponSlotIndex, currentWeaponSlotToMove, weaponSlotToUnEquip, secondaryWeaponToEquip);
	}

	public void updateQuickAccesSlotAmount (int weaponSlotIndex)
	{
		mainInventoryQuickAccessSlotsSystem.updateQuickAccesSlotAmount (weaponSlotIndex);
	}

	public void updateAllWeaponSlotAmmo ()
	{
		mainInventoryQuickAccessSlotsSystem.updateAllQuickAccessSlotsAmount ();
	}

	public string getFirstSingleWeaponSlot (string weaponNameToAvoid)
	{
		return mainInventoryQuickAccessSlotsSystem.getFirstSingleWeaponSlot (weaponNameToAvoid);
	}

	public void showWeaponSlotsParentWhenWeaponSelectedByName (string objectName)
	{
		mainInventoryQuickAccessSlotsSystem.showQuickAccessSlotsParentWhenSlotSelectedByName (objectName);
	}

	public void showWeaponSlotsParentWhenWeaponSelected (int weaponSlotIndex)
	{
		mainInventoryQuickAccessSlotsSystem.showQuickAccessSlotsParentWhenSlotSelected (weaponSlotIndex);
	}

	public void stopShowWeaponSlotsParentWhenWeaponSelectedCoroutuine ()
	{
		mainInventoryQuickAccessSlotsSystem.stopShowQuickAccessSlotsParentWhenSlotSelectedCoroutuine ();
	}

	public bool isShowWeaponSlotsAlwaysActive ()
	{
		return mainInventoryQuickAccessSlotsSystem.isShowQuickAccessSlotsAlwaysActive ();
	}

	public void updateWeaponCurrentlySelectedIcon (int weaponSlotIndex, bool activeCurrentWeaponSlot)
	{
		mainInventoryQuickAccessSlotsSystem.updateCurrentlySelectedSlotIcon (weaponSlotIndex, activeCurrentWeaponSlot);
	}

	public void disableCurrentlySelectedIcon ()
	{
		mainInventoryQuickAccessSlotsSystem.disableCurrentlySelectedIcon ();
	}

	public void selectWeaponByPressingSlotWeapon (GameObject buttonToCheck)
	{
		mainInventoryQuickAccessSlotsSystem.selectQuickAccessSlotByPressingSlot (buttonToCheck);
	}

	public void moveWeaponSlotsOutOfInventory ()
	{
		mainInventoryQuickAccessSlotsSystem.moveQuickAccessSlotsOutOfInventory ();
	}

	public void moveWeaponSlotsToInventory ()
	{
		mainInventoryQuickAccessSlotsSystem.moveQuickAccessSlotsToInventory ();
	}

	public void setWeaponSlotsBackgroundColorAlphaValue (float newAlphaValue)
	{
		mainInventoryQuickAccessSlotsSystem.setQuickAccessSlotsBackgroundColorAlphaValue (newAlphaValue);
	}

	public void changeToMeleeWeapons (string meleeWeaponName)
	{
		mainInventoryQuickAccessSlotsSystem.changeToMeleeWeapons (meleeWeaponName);
	}

	public void checkQuickAccessSlotToSelectByName (string objectOnSlotName)
	{
		mainInventoryQuickAccessSlotsSystem.checkQuickAccessSlotToSelectByName (objectOnSlotName);
	}
	//END OF QUICK ACCESS SLOTS ELEMENTS


	//START INVENTORY EVENTS ELEMENTS
	public void checkEventOnSystemDisabled ()
	{
		if (useEventIfSystemDisabled) {
			eventIfSystemDisabled.Invoke ();
		}
	}

	public void checkEventOnInventoryInitialized ()
	{
		eventOnInventoryInitialized.Invoke ();
	}

	public void checkEventOnClickInventoryChange ()
	{
		eventOnClickInventoryChange.Invoke ();
	}

	public void checkEventOnInventorySlotSelected ()
	{
		eventOnInventorySlotSelected.Invoke ();
	}

	public void checkEventOnInventorySlotUnSelected ()
	{
		eventOnInventorySlotUnSelected.Invoke ();
	}

	public void checkEventOnInventoryClosed ()
	{
		eventOnInventoryClosed.Invoke ();
	}

	public void checkEventOnInventoryOpened ()
	{
		eventOnInventoryOpened.Invoke ();
	}

	public void checkEventOnInventoryListChange ()
	{
		eventOnInventoryListChange.Invoke ();
	}
	//END INVENTORY EVENTS ELEMENTS


	//START EDITOR FUNCTIONS
	public void addNewInventoryObjectToInventoryListManagerList ()
	{
		inventoryListElement newInventoryListElement = new inventoryListElement ();
		newInventoryListElement.Name = "New Object";

		newInventoryListElement.amount = 1;

		inventoryListManagerList.Add (newInventoryListElement);

		updateComponent ();
	}

	public void addNewInventoryObject ()
	{
		inventoryInfo newObject = new inventoryInfo ();
		inventoryList.Add (newObject);

		updateComponent ();
	}

	public void getInventoryListManagerList ()
	{
		checkMainInventoryManager ();

		List<inventoryCategoryInfo> inventoryCategoryInfoList = mainInventoryListManager.inventoryCategoryInfoList;

		inventoryManagerListString = new string[inventoryCategoryInfoList.Count];

		for (int i = 0; i < inventoryManagerListString.Length; i++) {
			inventoryManagerListString [i] = inventoryCategoryInfoList [i].Name;
		}

		inventoryManagerStringInfoList.Clear ();

		for (int i = 0; i < inventoryCategoryInfoList.Count; i++) {

			inventoryManagerStringInfo newInventoryManagerStringInfoo = new inventoryManagerStringInfo ();
			newInventoryManagerStringInfoo.Name = inventoryCategoryInfoList [i].Name;

			List<inventoryInfo> currentInventoryListByCategories = inventoryCategoryInfoList [i].inventoryList;

			newInventoryManagerStringInfoo.inventoryManagerListString = new string[currentInventoryListByCategories.Count];

			for (int j = 0; j < currentInventoryListByCategories.Count; j++) {
				string newName = currentInventoryListByCategories [j].Name;
				newInventoryManagerStringInfoo.inventoryManagerListString [j] = newName;
			}

			inventoryManagerStringInfoList.Add (newInventoryManagerStringInfoo);
		}

		updateComponent ();
	}

	public void setInventoryObjectListNames ()
	{
		for (int i = 0; i < inventoryListManagerList.Count; i++) {
			inventoryListManagerList [i].Name = inventoryListManagerList [i].inventoryObjectName;
		}

		updateComponent ();
	}

	public void saveCurrentInventoryListToFileFromEditor ()
	{
		pauseManager.saveGameInfoFromEditor ("Player Inventory");

		print ("Inventory list saved");

		updateComponent ();
	}

	public void setAddAllObjectEnabledState (bool state)
	{
		for (int i = 0; i < inventoryListManagerList.Count; i++) {
			inventoryListManagerList [i].addInventoryObject = state;
		}

		updateComponent ();
	}

	public void removeAllWeaponsFromInitialInventoryList ()
	{
		checkMainInventoryManager ();

		for (int i = inventoryListManagerList.Count - 1; i >= 0; i--) {

			string currentInventoryObjectName = inventoryListManagerList [i].Name;

			inventoryInfo inventoryInfoToCheck = getInventoryInfoByName (currentInventoryObjectName);

			print ("remove " + inventoryInfoToCheck.Name + " " + inventoryInfoToCheck.isWeapon);

			if (inventoryInfoToCheck.isWeapon) {
				inventoryListManagerList.RemoveAt (i);

				print (inventoryInfoToCheck.Name + " weapon removed");
			}
		}

		updateComponent ();
	}

	public void removeAllObjectsFromInitialInventoryList ()
	{
		inventoryListManagerList.Clear ();

		updateComponent ();
	}

	public void setAddObjectEnabledState (int objectIndex)
	{
		inventoryListManagerList [objectIndex].addInventoryObject = !inventoryListManagerList [objectIndex].addInventoryObject;

		updateComponent ();
	}

	public void setEquippedObjectState (int objectIndex, bool state)
	{
		inventoryListManagerList [objectIndex].isEquipped = state;

		updateComponent ();
	}

	public void setEquippedObjectState (int objectIndex)
	{
		inventoryListManagerList [objectIndex].isEquipped = !inventoryListManagerList [objectIndex].isEquipped;

		updateComponent ();
	}

	public void updateComponent ()
	{
		GKC_Utils.updateComponent (this);
	}
	//END EDITOR FUNCTIONS

	[System.Serializable]
	public class inventoryManagerStringInfo
	{
		public string Name;
		public string[] inventoryManagerListString;
	}
}