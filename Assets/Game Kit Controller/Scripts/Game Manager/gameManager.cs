﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class gameManager : MonoBehaviour
{
	[Header ("Main Settings")]
	[Space]

	public bool loadEnabled;

	public bool limitFps;

	public bool updateLimitFpsIfChangeDetected;

	public int targetFps = 60;

	[Space]
	[Header ("Save Info")]
	[Space]

	public float playTime;
	public string chapterInfo;

	public bool useRelativePath;

	[Space]
	[Header ("Save File Names Settings")]
	[Space]

	public string versionNumber = "3-01";

	public string saveGameFolderName = "Save";
	public string saveFileName = "Save State";

	public string saveCaptureFolder = "Captures";
	public string saveCaptureFileName = "Photo";

	public string touchControlsPositionFolderName = "Touch Buttons Position";
	public string touchControlsPositionFileName = "Touch Positions";

	public string saveInputFileFolderName = "Input";
	public string saveInputFileName = "Input File";

	public string defaultInputSaveFileName = "Default Input File";

	public string fileExtension = ".txt";

	public int slotBySaveStation;

	public bool saveCameraCapture = true;

	public LayerMask layer;

	[Space]
	[Header ("Debug Settings")]
	[Space]

	public int saveNumberToLoad = -1;

	public string currentPersistentDataPath;

	public string currentSaveDataPath;

	[Space]
	[Header ("Current Game State")]
	[Space]

	public bool gamePaused;

	public bool useTouchControls = false;

	public bool touchPlatform;

	[Space]
	[Header ("Game Manager Elements")]
	[Space]

	public playerCharactersManager charactersManager;

	public Camera mainCamera;

	public GameObject mainPlayerGameObject;
	public GameObject mainPlayerCameraGameObject;
	public playerCamera currentPlayerCamera;
	public GameObject prefabsManagerPrefab;

	List<saveGameSystem.saveStationInfo> saveList = new List<saveGameSystem.saveStationInfo> ();

	RaycastHit hit;

	int lastSaveNumber = -1;

	bool FPSLimitAdjusted;

	void Awake ()
	{
		if (limitFps) {
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = targetFps;
		} else {
			Application.targetFrameRate = -1;
		}

		touchPlatform = touchJoystick.checkTouchPlatform ();

		if (touchPlatform) {
			useRelativePath = false;
		}

		checkGameInfoToLoad ();
	}

	void checkGameInfoToLoad ()
	{
		if (isLoadGameEnabled () && getLastSaveNumber () > -1) {

			string extraFileString = " " + getVersionNumber () + getFileExtension ();

			lastSaveNumber = -1;
			if (PlayerPrefs.HasKey ("saveNumber")) {
				lastSaveNumber = PlayerPrefs.GetInt ("saveNumber");
			} else if (saveNumberToLoad >= 0) {
				lastSaveNumber = saveNumberToLoad;
			}

			currentSaveDataPath = getDataPath ();

			if (charactersManager != null) {
				charactersManager.checkGameInfoToLoad (lastSaveNumber, currentSaveDataPath, extraFileString);
			}
		} else {
			if (charactersManager != null) {
				charactersManager.checkComponentsToInitialize ();
			}
		}
	}

	public void saveGameInfoFromEditor (string infoTypeName)
	{
		if (isLoadGameEnabled () && saveNumberToLoad > -1) {

			string extraFileString = " " + getVersionNumber () + getFileExtension ();

			charactersManager.saveGameInfoFromEditor (saveNumberToLoad, infoTypeName, extraFileString);
		}
	}

	public void saveGameWhenReturningHomeMenu ()
	{
		if (isLoadGameEnabled ()) {
			charactersManager.saveGameWhenReturningHomeMenu ();
		}
	}

	void Start ()
	{
		currentPersistentDataPath = Application.persistentDataPath;

		if (loadEnabled && mainPlayerGameObject != null) {
			if (PlayerPrefs.HasKey ("chapterInfo")) {
				chapterInfo = PlayerPrefs.GetString ("chapterInfo");
			}

			if (PlayerPrefs.HasKey ("loadingGame")) {
				if (PlayerPrefs.GetInt ("loadingGame") == 1) {

					Vector3 newPlayerPosition = Vector3.zero;
					Quaternion newPlayerRotation = Quaternion.identity;

					bool savingGameToChangeScene = false;

					if (PlayerPrefs.GetInt ("savingGameToChangeScene") == 1) {

						int levelManagerIDToLoad = PlayerPrefs.GetInt ("levelManagerIDToLoad");
						bool targetFound = false;

						levelManagerIngame[] levelManagerIngameList = FindObjectsOfType<levelManagerIngame> ();

						foreach (levelManagerIngame currentLevelManagerIngame in levelManagerIngameList) {
							if (!targetFound && currentLevelManagerIngame.levelManagerID == levelManagerIDToLoad) {
								newPlayerPosition = currentLevelManagerIngame.spawPlayerPositionTransform.position;
								newPlayerRotation = currentLevelManagerIngame.spawPlayerPositionTransform.rotation;

								targetFound = true;

								savingGameToChangeScene = true;
							}
						}
					} else {
						newPlayerPosition = 
							new Vector3 (PlayerPrefs.GetFloat ("saveStationPositionX"), PlayerPrefs.GetFloat ("saveStationPositionY"), PlayerPrefs.GetFloat ("saveStationPositionZ"));
						
						newPlayerRotation =	
							Quaternion.Euler (PlayerPrefs.GetFloat ("saveStationRotationX"), PlayerPrefs.GetFloat ("saveStationRotationY"), PlayerPrefs.GetFloat ("saveStationRotationZ"));
					}

					if (PlayerPrefs.GetInt ("useRaycastToPlacePlayer") == 1) {
						if (Physics.Raycast (newPlayerPosition + Vector3.up, -Vector3.up, out hit, Mathf.Infinity, layer)) {
							newPlayerPosition = hit.point;
						}
					}
						
					//set player position and rotation
					mainPlayerGameObject.transform.position = newPlayerPosition;
					mainPlayerGameObject.transform.rotation = newPlayerRotation;

					Quaternion newCameraRotation = newPlayerRotation;

					if (savingGameToChangeScene) {
						newCameraRotation = newPlayerRotation;
					} else {
						if (PlayerPrefs.GetInt ("usePlayerCameraOrientation") == 1) {
							newCameraRotation =
								Quaternion.Euler (PlayerPrefs.GetFloat ("playerCameraRotationX"), PlayerPrefs.GetFloat ("playerCameraRotationY"), PlayerPrefs.GetFloat ("playerCameraRotationZ"));

							float playerCameraPivotRotationX = PlayerPrefs.GetFloat ("playerCameraPivotRotationX");

							float newLookAngle = playerCameraPivotRotationX;

							if (newLookAngle > 180) {
								newLookAngle -= 360;
							}
							Vector2 newPivotRotation = new Vector2 (0, newLookAngle);

							currentPlayerCamera.setLookAngleValue (newPivotRotation);

							currentPlayerCamera.getPivotCameraTransform ().localRotation = Quaternion.Euler (playerCameraPivotRotationX, 0, 0);
						}
					}
						
					mainPlayerCameraGameObject.transform.position = newPlayerPosition;
					mainPlayerCameraGameObject.transform.rotation = newCameraRotation;

					if (PlayerPrefs.GetInt ("isPlayerDriving") == 1) {
						string vehicleName = PlayerPrefs.GetString ("currentVehicleName");
						print ("PLAYER PREVIOUSLY DRIVING " + vehicleName);

						prefabsManager newPrefabsManager = FindObjectOfType<prefabsManager> ();

						if (newPrefabsManager == null) {

							GameObject newPrefabsManagerGameObject = (GameObject)Instantiate (prefabsManagerPrefab, Vector3.zero, Quaternion.identity);

							newPrefabsManagerGameObject.name = "Prefabs Manager";

							newPrefabsManagerGameObject.transform.position = Vector3.zero;
							newPrefabsManagerGameObject.transform.rotation = Quaternion.identity;

							newPrefabsManager = newPrefabsManagerGameObject.GetComponent<prefabsManager> ();
						}

						GameObject vehiclePrefab = newPrefabsManager.getPrefabGameObject (vehicleName);

						if (vehiclePrefab != null) {
							mainPlayerGameObject.transform.position = newPlayerPosition + Vector3.up * 200;

							Vector3 vehiclePosition = Vector3.zero;

							if (Physics.Raycast (newPlayerPosition + Vector3.up * 20, -Vector3.up, out hit, Mathf.Infinity, layer)) {
								vehiclePosition = hit.point;
							}

							vehiclePosition += Vector3.up * newPrefabsManager.getPrefabSpawnOffset (vehicleName);

							GameObject newVehicle = (GameObject)Instantiate (vehiclePrefab, vehiclePosition, newPlayerRotation);

							IKDrivingSystem currentIKDrivingSystem = newVehicle.GetComponent<IKDrivingSystem> ();

							currentIKDrivingSystem.setPlayerToStartGameOnThisVehicle (mainPlayerGameObject);
						} else {
							print ("WARNING: No prefab of the vehicle called " + vehicleName + " was found. Make sure a prefab with that name is configured in the prefabs manager");
						}
					}

					PlayerPrefs.DeleteAll ();
				}
			}
		} else {
			PlayerPrefs.DeleteAll ();
		}
	}

	void Update ()
	{
		playTime += Time.deltaTime;
	
		if (limitFps) {
			if (updateLimitFpsIfChangeDetected || !FPSLimitAdjusted) {
				if (Application.targetFrameRate != targetFps) {
					Application.targetFrameRate = targetFps;
				}

				if (!updateLimitFpsIfChangeDetected) {
					FPSLimitAdjusted = true;
				}
			}
		}
	}

	public void getPlayerPrefsInfo (saveGameSystem.saveStationInfo save)
	{
		PlayerPrefs.SetInt ("loadingGame", 1);

		PlayerPrefs.SetInt ("saveNumber", save.saveNumber);

		PlayerPrefs.SetInt ("currentSaveStationId", save.id);

		PlayerPrefs.SetFloat ("saveStationPositionX", save.saveStationPositionX);
		PlayerPrefs.SetFloat ("saveStationPositionY", save.saveStationPositionY);
		PlayerPrefs.SetFloat ("saveStationPositionZ", save.saveStationPositionZ);
		PlayerPrefs.SetFloat ("saveStationRotationX", save.saveStationRotationX);
		PlayerPrefs.SetFloat ("saveStationRotationY", save.saveStationRotationY);
		PlayerPrefs.SetFloat ("saveStationRotationZ", save.saveStationRotationZ);

		if (save.usePlayerCameraOrientation) {
			PlayerPrefs.SetInt ("usePlayerCameraOrientation", 1);
		} else {
			PlayerPrefs.SetInt ("usePlayerCameraOrientation", 0);
		}

		PlayerPrefs.SetFloat ("playerCameraRotationX", save.playerCameraRotationX);
		PlayerPrefs.SetFloat ("playerCameraRotationY", save.playerCameraRotationY);
		PlayerPrefs.SetFloat ("playerCameraRotationZ", save.playerCameraRotationZ);
		PlayerPrefs.SetFloat ("playerCameraPivotRotationX", save.playerCameraPivotRotationX);

		if (save.useRaycastToPlacePlayer) {
			PlayerPrefs.SetInt ("useRaycastToPlacePlayer", 1);
		}

		if (save.savingGameToChangeScene) {
			PlayerPrefs.SetInt ("savingGameToChangeScene", 1);
		} else {
			PlayerPrefs.SetInt ("savingGameToChangeScene", 0);
		}

		PlayerPrefs.SetInt ("levelManagerIDToLoad", save.checkpointID);

		if (save.isPlayerDriving) {
			PlayerPrefs.SetInt ("isPlayerDriving", 1);
			PlayerPrefs.SetString ("currentVehicleName", save.currentVehicleName);
		} else {
			PlayerPrefs.SetInt ("isPlayerDriving", 0);
		}

		SceneManager.LoadScene (save.saveStationScene);
	}

	public string getDataPath ()
	{
		string dataPath = "";
		if (useRelativePath) {
			dataPath = saveGameFolderName;
		} else {
			dataPath = Application.persistentDataPath + "/" + saveGameFolderName;
		}

		if (!Directory.Exists (dataPath)) {
			Directory.CreateDirectory (dataPath);
		}

		dataPath += "/";

		return dataPath;
	}

	public string getDataName ()
	{
		return saveFileName + " " + versionNumber;
	}

	public string getVersionNumber ()
	{
		return versionNumber;
	}

	public string getFileExtension ()
	{
		return fileExtension;
	}

	public List<saveGameSystem.saveStationInfo> getCurrentSaveList ()
	{
		return saveList;
	}

	public void setSaveList (List<saveGameSystem.saveStationInfo> currentList)
	{
		saveList = currentList;
	}

	public int getLastSaveNumber ()
	{
		lastSaveNumber = -1;

		if (PlayerPrefs.HasKey ("saveNumber")) {
			lastSaveNumber	= PlayerPrefs.GetInt ("saveNumber");
		} else if (saveNumberToLoad >= 0) {
			lastSaveNumber = saveNumberToLoad;
		}

		return lastSaveNumber;
	}

	public Camera getMainCamera ()
	{
		return mainCamera;
	}

	public void setMainCamera (Camera cameraToConfigure)
	{
		mainCamera = cameraToConfigure;
	}

	public void setGamePauseState (bool state)
	{
		gamePaused = state;
	}

	public bool isGamePaused ()
	{
		return gamePaused;
	}

	public bool isUsingTouchControls ()
	{
		return useTouchControls;
	}

	public void setUseTouchControlsState (bool state)
	{
		useTouchControls = state;
	}

	public void setUseTouchControlsStateFromEditor (bool state)
	{
		useTouchControls = state;

		updateComponent ();
	}

	public void setCharactersManagerPauseState (bool state)
	{
		charactersManager.setCharactersManagerPauseState (state);
	}

	public playerController getMainPlayerController ()
	{
		return charactersManager.getMainPlayerController ();
	}

	public bool anyCharacterDead ()
	{
		return charactersManager.anyCharacterDead ();
	}

	public string getSaveCaptureFolder ()
	{
		return saveCaptureFolder;
	}

	public string getSaveCaptureFileName ()
	{
		return saveCaptureFileName;
	}

	public string getTouchControlsPositionFolderName ()
	{
		return touchControlsPositionFolderName;
	}

	public string getTouchControlsPositionFileName ()
	{
		return touchControlsPositionFileName + " " + versionNumber;
	}

	public bool isLoadGameEnabled ()
	{
		return loadEnabled;
	}

	public void setCurrentPersistentDataPath ()
	{
		currentPersistentDataPath = Application.persistentDataPath;

		updateComponent ();
	}

	public void deletePlayerPrefs ()
	{
		PlayerPrefs.DeleteAll ();

		print ("Player Prefs Deleted");
	}

	public void getCurrentDataPath ()
	{
		setCurrentPersistentDataPath ();

		currentSaveDataPath = getDataPath ();

		updateComponent ();
	}

	public void updateComponent ()
	{
		GKC_Utils.updateComponent (this);
	}
}
