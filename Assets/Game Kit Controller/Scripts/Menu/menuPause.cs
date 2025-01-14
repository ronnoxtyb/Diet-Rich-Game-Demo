﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class menuPause : MonoBehaviour
{
	[Header ("Main Settings")]
	[Space]

	public bool menuPauseEnabled;

	public bool fadeScreenActive = true;
	public float fadeScreenSpeed = 2;

	public bool activateDeathMenuWhenPlayeDies = true;

	public bool closeOnlySubMenuIfOpenOnEscape;

	public int sceneNumberForHomeMenu = 0;

	public bool saveGameAutomaticallyOnReturnToHome;

	public bool pauseAudioListenerOnGamePaused = true;

	public float timeScaleOnGamePaused = 0;

	public bool pauseAIWhenPauseMenuOpened;

	[Space]
	[Header ("Canvas Group Menu Settings")]
	[Space]

	public bool useCanvasGroup;
	public float fadeOnPanelSpeed;
	public float fadeOffPanelSpeed;

	[Space]
	[Header ("Menu Settings")]
	[Space]

	public List<submenuInfo> submeneInfoList = new List<submenuInfo> ();

	public List<playerMenuInfo> playerMenuInfoList = new List<playerMenuInfo> ();

	public List<ingameMenuInfo> ingameMenuInfoList = new List<ingameMenuInfo> ();

	public bool useDefaultPlayerMenuNameToOpen;
	public string defaultPlayerMenuNameToOpen;

	public List<GameObject> touchZoneList = new List<GameObject> ();

	public bool changeBetweenMenusInputEnabled = true;

	public string mainPauseMenuName = "Pause Menu";

	public bool usePreviousMenuManagement;

	[Space]
	[Header ("Help Menu Settings")]
	[Space]

	public bool inputHelpMenuEnabled;
	public GameObject inputHelpMenuGameObject;
	bool inputHelpMenuEnabledState;

	[Space]
	[Header ("Mechanic Menu Settings")]
	[Space]

	public bool mechanicsHelpMenuEnabled;
	public GameObject mechanicsHelpMenuGameObject;
	bool mechanicsHelpMenuEnabledState;

	[Space]
	[Header ("Blur Background Settings")]
	[Space]

	public bool blurPanelEnabled = true;
	public bool useBlurUIPanelOnPlayerMenu;
	public bool useBlurUIPanelOnPause;
	public GameObject blurUIPanel;
	public Material blurUIPanelMaterial;
	public Image blurUIPanelImage;
	public float blurUIPanelValue = 4.4f;
	public float blurUIPanelSpeed = 10;
	public float blurUIPanelAlphaSpeed = 10;
	public float blurUIPanelAlphaValue = 120;
	public Transform blurUIPanelParent;

	[Space]
	[Header ("Event Settings")]
	[Space]

	public eventParameters.eventToCallWithBool eventToEnableOrDisableAllPlayerHUD;
	public eventParameters.eventToCallWithBool eventToEnableOrDisableSecondaryPlayerHUD;
	public eventParameters.eventToCallWithBool eventToEnableOrDisableAllVehicleHUD;

	public bool useEventsOnPauseResume;
	public UnityEvent eventOnPauseGame;
	public UnityEvent eventOnResumeGame;

	[Space]
	[Header ("Menu Pause Elements")]
	[Space]

	public List<canvasInfo> canvasInfoList = new List<canvasInfo> ();

	[Space]
	[Header ("Pause Manager State/Debug")]
	[Space]

	public cursorStateInfo cursorState;

	public int currentPlayerMenuInfoIndex;

	public bool gamePaused = false;

	public bool subMenuActive;
	public bool usingSubMenu;

	public bool playerMenuActive;

	public bool useTouchControls = false;
	public bool usingTouchControlsPreviously;

	public bool usingDevice;
	public bool dead;

	public bool ignoreChangeHUDElements;

	public bool ignoreDisableTouchZoneList;

	public bool pauseGameInputPaused;
	public bool changeBetweenIngameMenuPaused;
	public bool openOrClosePlayerOpenMenuByNamePaused;

	public bool regularTimeScaleActive = true;

	public bool openOrClosePauseMenuExternallyPaused;

	[Space]
	[Header ("Components")]
	[Space]

	public GameObject hudAndMenus;
	public GameObject touchPanel;
	public GameObject pauseMenu;
	public Transform pauseMenuParent;
	public GameObject dieMenu;

	public Image blackBottomImage;

	public GameObject dynamicUIElementsParent;

	public gameManager mainGameManager;
	public playerInputManager playerInput;
	public playerHUDManager mainPlayerHUDManager;
	public Canvas mainCanvas;
	public RectTransform mainCanvasRectTransform;
	public CanvasScaler mainCanvasScaler;
	public Camera mainCanvasCamera;
	public inputManager input;
	public timeBullet timeManager;
	public playerWeaponsManager mainWeaponsManager;
	public playerHealthBarManagementSystem mainPlayerHealthBarManager;
	public playerScreenObjectivesSystem playerScreenObjectivesManager;
	public playerPickupIconManager mainPlayerPickupIconManager;
	public mouseCursorController mouseCursorControllerManager;
	public playerController playerControllerManager;
	public playerCamera playerCameraManager;
	public editControlPosition editControlPositionManager;




	bool usingPointAndClick;
	bool showGUI = false;

	Color alpha;

	Coroutine blurUIPanelCoroutine;
	bool blurUIPanelActive;

	bool mouseButtonPressPaused;

	float timeToResetCursorAfterDisable = 0.1f;
	float lastTimeCursorDisabled = 10;

	bool checkLockCursorBetweenMenus;

	int blurSizeID = -1;

	float fadeScreenDirection = -1;

	float fadeScreenTarget = 0;

	bool ignoreHideCursorOnClickActive;


	void Start ()
	{
		AudioListener.pause = false;

		if (blackBottomImage != null) {
			blackBottomImage.enabled = true;
		}

		if (!mainGameManager.isUsingTouchControls ()) {
			showOrHideCursor (false);
		} else {
			enableOrDisableTouchControls (true);
		}
			
		editControlPositionManager.getTouchButtons ();

		touchPanel.SetActive (useTouchControls);

		alpha.a = 1;

		setTimeScale (1);

		//if the fade of the screen is disabled, just set the alpha of the black panel to 0
		if (!fadeScreenActive) {
			alpha.a = 0;

			if (blackBottomImage != null) {
				blackBottomImage.color = alpha;
				blackBottomImage.enabled = false;
			}
		}

		pauseMenu.SetActive (false);

		disableMenuList ();

		setCurrentCameraState (true);

		checkBlurSizeID ();
			
		blurUIPanelMaterial.SetFloat (blurSizeID, 0); 
	}

	public void activateFadePanelBackgroundIn ()
	{
		alpha.a = 1;

		blackBottomImage.color = alpha;

		blackBottomImage.enabled = true;

		fadeScreenActive = true;

		fadeScreenDirection = -1;

		fadeScreenTarget = 0;
	}

	public void activateFadePanelBackgroundOut ()
	{
		alpha.a = 0;

		blackBottomImage.color = alpha;

		blackBottomImage.enabled = true;

		fadeScreenActive = true;

		fadeScreenDirection = 1;

		fadeScreenTarget = 1;
	}

	void Update ()
	{
		//if the fade is enabled, decrease the value of alpha to get a nice fading effect at the beginning of the game
		if (fadeScreenActive) {
			alpha.a += (getDeltaTime () / fadeScreenSpeed) * fadeScreenDirection;

			blackBottomImage.color = alpha;

			if ((fadeScreenTarget == 0 && alpha.a <= 0) || (fadeScreenTarget == 1 && alpha.a >= 1)) {
				blackBottomImage.enabled = false;
				fadeScreenActive = false;
			}
		}

		//if the mouse is showed, press in the screen to lock it again
		if (!gamePaused && !mouseButtonPressPaused && !useTouchControls) {
			//check that the touch controls are disabled, the player is not dead, the powers is not being editing or selecting, the player is not using a device
			//or the cursor is visible
			if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1)) {
				bool canHideCursor = true;

				if (ignoreHideCursorOnClickActive) {
					canHideCursor = false;
				}

				if (useTouchControls) {
					canHideCursor = false;
				}

				if (dead || mainGameManager.anyCharacterDead ()) {
					canHideCursor = false;
				}

				if (!inGameWindowOpened ()) {
					canHideCursor = false;
				}

				if (Cursor.lockState != CursorLockMode.None && !Cursor.visible) {
					canHideCursor = false;
				}

				if (playerInput.isUsingGamepad () && mouseCursorControllerManager.getCurrentJoystickNumber () != playerInput.getPlayerID ()) {
					canHideCursor = false;
				}

				if (canHideCursor) {
					showOrHideCursor (false);
				}
			}
		}

		if (inputHelpMenuEnabled) {
			if (Input.GetKeyDown (KeyCode.F1)) {
				if (inputHelpMenuGameObject != null) {
					inputHelpMenuEnabledState = !inputHelpMenuEnabledState;
					inputHelpMenuGameObject.SetActive (inputHelpMenuEnabledState);
				}
			}
		}

		if (mechanicsHelpMenuEnabled) {
			if (Input.GetKeyDown (KeyCode.F2)) {
				if (mechanicsHelpMenuGameObject != null) {
					mechanicsHelpMenuEnabledState = !mechanicsHelpMenuEnabledState;
					mechanicsHelpMenuGameObject.SetActive (mechanicsHelpMenuEnabledState);
				}
			}
		}
	}

	void OnDisable ()
	{
		if (blurUIPanelActive) {
			if (blurUIPanelMaterial != null) {
				checkBlurSizeID ();

				blurUIPanelMaterial.SetFloat (blurSizeID, 0); 
			}
		}
	}

	public void disableMenuList ()
	{
		for (int i = 0; i < submeneInfoList.Count; i++) {	
			if (submeneInfoList [i].menuGameObject != null && submeneInfoList [i].menuGameObject.activeSelf != false) {
				submeneInfoList [i].menuGameObject.SetActive (false);
			}

			if (submeneInfoList [i].menuOpened) {
				submeneInfoList [i].menuOpened = false;

				if (submeneInfoList [i].useEventOnClose) {
					if (submeneInfoList [i].eventOnClose.GetPersistentEventCount () > 0) {
						submeneInfoList [i].eventOnClose.Invoke ();
					}
				}
			}
		}
	}

	public void setOpenOrClosePauseMenuExternallyPausedState (bool state)
	{
		openOrClosePauseMenuExternallyPaused = state;
	}

	public void openPauseMenuByName (string menuName)
	{
		if (openOrClosePauseMenuExternallyPaused) {
			return;
		}

		openOrClosePauseMenuByName (menuName, true);
	}

	public void closePauseMenuByName (string menuName)
	{
		if (openOrClosePauseMenuExternallyPaused) {
			return;
		}

		openOrClosePauseMenuByName (menuName, false);
	}

	public void openOrClosePauseMenuByName (string menuName, bool state)
	{
		int pauseMenuIndex = submeneInfoList.FindIndex (s => s.Name.Equals (menuName));

		if (pauseMenuIndex > -1) {
			if (state) {
				for (int i = 0; i < submeneInfoList.Count; i++) {	
					if (pauseMenuIndex != i) {
						if (submeneInfoList [i].menuGameObject != null && submeneInfoList [i].menuGameObject.activeSelf != false) {
							submeneInfoList [i].menuGameObject.SetActive (false);
						}

						if (submeneInfoList [i].menuOpened) {
							submeneInfoList [i].menuOpened = false;

							if (submeneInfoList [i].useEventOnClose) {
								if (submeneInfoList [i].eventOnClose.GetPersistentEventCount () > 0) {
									submeneInfoList [i].eventOnClose.Invoke ();
								}
							}

							if (submeneInfoList [i].isSubMenu) {	
								exitSubMenu ();
							}
						}
					}
				}
			}

			submenuInfo currentSubmenuInfo = submeneInfoList [pauseMenuIndex];
		
			if (state) {
				currentSubmenuInfo.eventOnOpen.Invoke ();
			} else {
				currentSubmenuInfo.eventOnClose.Invoke ();
			}

			if (currentSubmenuInfo.menuGameObject != null && currentSubmenuInfo.menuGameObject.activeSelf != state) {
				currentSubmenuInfo.menuGameObject.SetActive (state);
			}

			currentSubmenuInfo.menuOpened = state;

			if (currentSubmenuInfo.isSubMenu) {	
				if (state) {
					enterSubMenu ();
				} else {
					exitSubMenu ();
				}
			}
		}
	}

	public void setPauseScreenWithoutPausingGameState (bool state)
	{
		mouseButtonPressPaused = state;
		
		changeCameraState (!state);
		
		//check if the touch controls were enabled
		if (!useTouchControls) {
			showOrHideCursor (state);
		}
		
		//pause game
		if (state) {
			alpha.a = 0.5f;
		
			//fade a little to black an UI panel
			blackBottomImage.enabled = true;
			blackBottomImage.color = alpha;
		
			//disable the event triggers in the touch buttons
			editControlPositionManager.changeButtonsState (false);
		}
		
		//resume game
		if (!state) {
			alpha.a = 0;
		
			//fade to transparent the UI panel
			blackBottomImage.enabled = false;
			blackBottomImage.color = alpha;
		
			//enable the event triggers in the touch buttons
			editControlPositionManager.changeButtonsState (true);
		}
		
		fadeScreenActive = false;
		
		if (useBlurUIPanelOnPause) {
			changeBlurUIPanelValue (state, pauseMenuParent, useBlurUIPanelOnPause);
		}
		
		playerControllerManager.setGamePausedState (state);
		
		mainWeaponsManager.setGamePausedState (state);
		
		if (playerInput.isUsingGamepad ()) {
			mouseCursorControllerManager.showOrHideMouseController (playerInput, state, playerInput.getPlayerID (), true);
		} else {
			mouseCursorControllerManager.showOrHideMouseController (state, true);
		}
	}

	//save the previous and the current visibility of the cursor, to enable the mouse cursor correctly when the user enables the touch controls, or using a device
	//or editing the powers, or open the menu, or any action that enable and disable the mouse cursor
	void setCurrentCursorState (bool curVisible)
	{
		cursorState.currentVisible = curVisible;
	}

	void setPreviousCursorState (bool prevVisible)
	{
		cursorState.previousVisible = prevVisible;
	}

	//like the mouse, save the state of the camera, to prevent rotate it when a menu is enabled, or using a device, or the player is dead, etc...
	void setCurrentCameraState (bool curCamera)
	{
		cursorState.currentCameraEnabled = curCamera;
	}

	void setPreviousCameraState (bool prevCamera)
	{
		cursorState.previousCameraEnabled = prevCamera;
	}

	public bool inGameWindowOpened ()
	{
		if ((!usingDevice || (playerControllerManager.isPlayerDriving () && !playerMenuActive)) && !usingPointAndClick) {

			for (int i = 0; i < ingameMenuInfoList.Count; i++) {
				if (ingameMenuInfoList [i].menuOpened) {
					return false;
				}
			}

			return true;
		}

		return false;
	}

	public void setIngameMenuOpenedState (string ingameMenuName, bool state, bool activateEvents)
	{
		for (int i = 0; i < ingameMenuInfoList.Count; i++) {
			if (ingameMenuInfoList [i].Name.Equals (ingameMenuName)) {
				ingameMenuInfoList [i].menuOpened = state;

				if (activateEvents && ingameMenuInfoList [i].useEventOnOpenClose) {
					
					if (state) {
						ingameMenuInfoList [i].eventOnOpen.Invoke ();
					} else {
						ingameMenuInfoList [i].eventOnClose.Invoke ();
					}
				}

				fadeInGameMenuPanel (ingameMenuInfoList [i], !state);

				if (ingameMenuInfoList [i].setCustomTimeScale) {
					if (state) {
						setTimeScale (ingameMenuInfoList [i].customTimeScale);
					} else {
						setTimeScale (1);
					}
				}

				if (ingameMenuInfoList [i].pauseAIWhenOpenMenu) {
					GKC_Utils.pauseOrResumeAIOnScene (state);
				}
			}
		}

		for (int i = 0; i < playerMenuInfoList.Count; i++) {
			if (playerMenuInfoList [i].Name.Equals (ingameMenuName)) {
				currentPlayerMenuInfoIndex = i;
				return;
			}
		}
	}

	public void fadeInGameMenuPanel (ingameMenuInfo newIngameMenuInfo, bool fadingPanel)
	{
		if (!useCanvasGroup) {
			return;
		}

		if (newIngameMenuInfo.useMenuCanvasGroup && newIngameMenuInfo.menuCanvasGroup) {
			if (newIngameMenuInfo.canvasGroupCoroutine != null) {
				StopCoroutine (newIngameMenuInfo.canvasGroupCoroutine);
			}

			newIngameMenuInfo.canvasGroupCoroutine = StartCoroutine (fadeInGameMenuPanelCoroutine (newIngameMenuInfo, fadingPanel));
		}
	}

	IEnumerator fadeInGameMenuPanelCoroutine (ingameMenuInfo newIngameMenuInfo, bool fadingPanel)
	{
		float targetValue = 1;

		float fadeSpeed = fadeOffPanelSpeed;

		if (fadingPanel) {
			targetValue = 0;

			fadeSpeed = fadeOnPanelSpeed;
		}

		if (fadingPanel) {
			if (newIngameMenuInfo.menuCanvasGroup.alpha > 0) {
				newIngameMenuInfo.menuPanelGameObject.SetActive (true);
			}
		} else {
			if (newIngameMenuInfo.menuCanvasGroup.alpha == 1) {
				newIngameMenuInfo.menuCanvasGroup.alpha = 0;
			}
		}

		while (newIngameMenuInfo.menuCanvasGroup.alpha != targetValue) {
			newIngameMenuInfo.menuCanvasGroup.alpha = Mathf.MoveTowards (newIngameMenuInfo.menuCanvasGroup.alpha, targetValue, getDeltaTime () * fadeSpeed);

			yield return null;
		}

		if (fadingPanel) {
			newIngameMenuInfo.menuPanelGameObject.SetActive (false);
		}
	}

	public void checkTouchControlsWithoutDisableThem (bool state)
	{
		if (!state) {
			usingTouchControlsPreviously = useTouchControls;
		}

		if (usingTouchControlsPreviously) {
			playerInput.changeControlsType (state);

			if (state && usingTouchControlsPreviously) {
				showOrHideCursor (true);
			}
		}
	}

	public void enableOrDisableTouchControlsExternally (bool state)
	{
		playerInput.enableOrDisableTouchControls (state);
	}

	public void enableOrDisablePlayerMenu (bool state, bool pausePlayerMovement)
	{
		checkLockCursorBetweenMenus = true;

		//set to visible the cursor
		showOrHideCursor (state);

		//disable the touch controls
		checkTouchControlsWithoutDisableThem (!state);

		//disable the camera rotation
		changeCameraState (!state);

		if (!playerControllerManager.isPlayerDriving ()) {
			if (pausePlayerMovement) {
				if (playerControllerManager.isActionActive ()) {
					playerControllerManager.setChangeScriptStateAfterFinishActionState (state);
				} else {
					playerControllerManager.setChangeScriptStateAfterFinishActionState (false);

//					playerControllerManager.changeScriptState (!state);

					playerControllerManager.setCanMoveState (!state);
					playerControllerManager.resetPlayerControllerInput ();
					playerControllerManager.resetOtherInputFields ();
				}
			}
		}

		usingSubMenuState (state);

		if (!ignoreChangeHUDElements) {
			enableOrDisableDynamicElementsOnScreen (!state);

			playerInput.enableOrDisableScreenActionParent (!state);
		}

		checkEnableOrDisableTouchZoneList (!state);

		if (playerInput.isUsingGamepad ()) {
			showOrHideMouseCursorController (playerInput, state, false);
		} else {
			showOrHideMouseCursorController (state);
		}

//		showOrHideMouseCursorController (state);

		playerInput.setUsingPlayerMenuState (state);
	}

	public void checkEnableOrDisableTouchZoneList (bool state)
	{
		if (!ignoreDisableTouchZoneList || state) {
			for (int i = 0; i < touchZoneList.Count; i++) {
				if (touchZoneList [i] != null) {
					touchZoneList [i].SetActive (state);
				}
			}
		}
	}

	public void setIgnoreDisableTouchZoneListState (bool state)
	{
		ignoreDisableTouchZoneList = state;
	}

	//set in the player is using a device like a computer or a text device
	public void usingDeviceState (bool state)
	{
		usingDevice = state;
	}

	public void usingPointAndClickState (bool state)
	{
		usingPointAndClick = state;
	}

	public void usingSubMenuState (bool state)
	{
		usingSubMenu = state;

		playerControllerManager.setUsingSubMenuState (usingSubMenu);
	}

	public void pauseGame ()
	{
		if (!subMenuActive) {
			//if the main pause menu is the current place, resuem the game

			pause ();

			return;
		} else {
			if (!dead) {
				//else, the current menu place is a submenu, so disable all the submenus and set the main menu window
				if (usePreviousMenuManagement) {
					exitSubMenu ();

					pauseMenu.SetActive (true);

					disableMenuList ();
				} else {

					openOrClosePauseMenuByName (mainPauseMenuName, true);
				}
			}
		}

		//disable the edition of the touch button position if the player backs from that menu option
		editControlPositionManager.disableEdit ();
	
		if (input.isEditingInput ()) {
			input.cancelEditingInput ();
		}
	}

	bool pauseCalledFromGameManager;

	public void setMenuPausedState (bool state)
	{
		if (transform.parent.gameObject.activeSelf) {
			gamePaused = !state;
		
			pauseCalledFromGameManager = true;

			pause ();
		}
	}

	public void pause ()
	{
		//check if the game is going to be paused or resumed
		if (!dead && menuPauseEnabled) {
			//if the player pauses the game and he is editing the powers or selecting them, disable the power manager menu

			bool playerMenuActivePreviously = playerMenuActive;

			for (int i = 0; i < ingameMenuInfoList.Count; i++) {
				if (ingameMenuInfoList [i].menuOpened) {
					ingameMenuInfoList [i].closeMenuEvent.Invoke ();
				}
			}

			if (playerMenuActivePreviously && closeOnlySubMenuIfOpenOnEscape) {
				return;
			}

			gamePaused = !gamePaused;

			mainGameManager.setGamePauseState (gamePaused);

			if (!pauseCalledFromGameManager) {
				mainGameManager.setCharactersManagerPauseState (gamePaused);
			}

			showGUI = !showGUI;

			if (pauseAudioListenerOnGamePaused) {
				AudioListener.pause = gamePaused;
			}

			//enable or disable the main pause menu
			if (usePreviousMenuManagement) {
				pauseMenu.SetActive (gamePaused);
			} else {
				openOrClosePauseMenuByName (mainPauseMenuName, gamePaused);
			}

			//change the camera state
			changeCameraState (!gamePaused);

			//check if the touch controls were enabled
			if (!useTouchControls) {
				showOrHideCursor (gamePaused);
			}

			input.setPauseState (gamePaused);

			//pause game
			if (gamePaused) {
				timeManager.disableTimeBullet ();

				setTimeScale (timeScaleOnGamePaused);

				alpha.a = 0.5f;

				//fade a little to black an UI panel
				blackBottomImage.enabled = true;
				blackBottomImage.color = alpha;

				//disable the event triggers in the touch buttons
				editControlPositionManager.changeButtonsState (false);
			}

			//resume game
			if (!gamePaused) {
				setTimeScale (1);

				alpha.a = 0;

				//fade to transparent the UI panel
				blackBottomImage.enabled = false;
				blackBottomImage.color = alpha;

				//enable the event triggers in the touch buttons
				editControlPositionManager.changeButtonsState (true);
				timeManager.reActivateTime ();
			}

			fadeScreenActive = false;

			if (useBlurUIPanelOnPause) {
				changeBlurUIPanelValue (gamePaused, pauseMenuParent, useBlurUIPanelOnPause);
			}

			playerControllerManager.setGamePausedState (gamePaused);

			if (timeScaleOnGamePaused > 0) {
				playerControllerManager.setCanMoveState (!gamePaused);
				playerControllerManager.resetPlayerControllerInput ();
			}

			mainWeaponsManager.setGamePausedState (gamePaused);

			if (playerInput.isUsingGamepad ()) {
				mouseCursorControllerManager.showOrHideMouseController (playerInput, gamePaused, playerInput.getPlayerID (), true);
			} else {
				mouseCursorControllerManager.showOrHideMouseController (gamePaused, true);
			}

			if (!gamePaused) {
				pauseCalledFromGameManager = false;
			}

			//print ("final " + transform.parent.name);

			checkEventOnPauseResumeGame (gamePaused);

			if (pauseAIWhenPauseMenuOpened) {
				GKC_Utils.pauseOrResumeAIOnScene (gamePaused);
			}
		}
	}

	public void checkEventOnPauseResumeGame (bool state)
	{
		if (useEventsOnPauseResume) {
			if (state) {
				eventOnPauseGame.Invoke ();
			} else {
				eventOnResumeGame.Invoke ();
			}
		}
	}

	public void setTimeScale (float newValue)
	{
		Time.timeScale = newValue;

		if (newValue != 0) {
			Time.fixedDeltaTime = newValue * 0.02f;
		}

		regularTimeScaleActive = newValue == 1;
	}

	public float getTimeTime ()
	{
		if (regularTimeScaleActive) {
			return Time.time;
		}

		return Time.unscaledTime;
	}

	public float getDeltaTime ()
	{
		if (regularTimeScaleActive) {
			return Time.deltaTime;
		}

		return Time.unscaledDeltaTime;
	}

	public bool isRegularTimeScaleActive ()
	{
		return regularTimeScaleActive;
	}

	public void setMouseButtonPressPausedState (bool state)
	{
		mouseButtonPressPaused = state;
	}

	public void showOrHideMouseCursorController (bool state)
	{
		mouseCursorControllerManager.showOrHideMouseController (state, false);
	}

	public void showOrHideMouseCursorController (playerInputManager newInput, bool state, bool pausingGame)
	{
		mouseCursorControllerManager.showOrHideMouseController (newInput, state, playerInput.getPlayerID (), pausingGame);
	}

	public void setMouseCursorControllerSpeedOnGameValue (float newValue)
	{
		mouseCursorControllerManager.setMouseCursorControllerSpeedOnGameValue (newValue);
	}

	//set the state of the cursor, according to if the touch controls are enabled, if the game is pause, if the powers manager menu is enabled, etc...
	//so the cursor is always locked and not visible correctly and vice versa
	public void showOrHideCursor (bool value)
	{
		if (cursorState.currentVisible && cursorState.previousVisible) {
			setPreviousCursorState (false);
			setCurrentCursorState (true);

			return;
		}

		if (cursorState.currentVisible && useTouchControls) {
			setPreviousCursorState (false);
			setCurrentCursorState (true);

			return;
		}

		if (value) {
			Cursor.lockState = CursorLockMode.None;
		} else {
			if (!checkLockCursorBetweenMenus || getTimeTime () < lastTimeCursorDisabled + timeToResetCursorAfterDisable) {
				Cursor.lockState = CursorLockMode.Confined;
				Cursor.lockState = CursorLockMode.Locked;
			}
		}

		checkLockCursorBetweenMenus = false;

		lastTimeCursorDisabled = getTimeTime ();

		setPreviousCursorState (Cursor.visible);

		Cursor.visible = value;

		setCurrentCursorState (Cursor.visible);
	}
		
	public void setIgnoreHideCursorOnClickActiveState(bool state)
	{
		ignoreHideCursorOnClickActive = state;
	}

	//check if the touch controls have to be enable and disable and change the cursor visibility according to that
	public void checkTouchControls (bool state)
	{
		if (!state) {
			usingTouchControlsPreviously = useTouchControls;
		}

		if (usingTouchControlsPreviously) {
			enableOrDisableTouchControls (state);

			if (state && usingTouchControlsPreviously) {
				showOrHideCursor (true);
			}
		}
	}

	public void updateTouchButtonsComponents ()
	{
		editControlPositionManager.getTouchButtonsComponents ();
	}

	//the player dies, so enable the death menu to ask the player to play again
	public void death ()
	{
		dead = true;

		if (activateDeathMenuWhenPlayeDies) {
			showOrHideCursor (true);

			dieMenu.SetActive (true);
		}

		changeCameraState (false);

		if (playerInput.isUsingGamepad ()) {
			showOrHideMouseCursorController (playerInput, true, false);
		} else {
			showOrHideMouseCursorController (true);
		}
	}

	//the player chooses to play again
	public void getUp ()
	{
		dead = false;

		dieMenu.SetActive (false);

		showOrHideMouseCursorController (false);

		if (!useTouchControls) {
			showOrHideCursor (false);
		}

		changeCameraState (true);
	}

	//restart the scene
	public void restart ()
	{
		pause ();

		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	public static Scene getCurrentActiveScene ()
	{
		return SceneManager.GetActiveScene ();
	}

	//change the camera state according to if the player pauses the game or uses a device, etc... so the camera is enabled correctly according to every situation
	public void changeCameraState (bool state)
	{
		if (playerCameraManager != null) {
			//if the player paused the game using a device, then resume again with the camera disable to keep using that device
			if (!cursorState.currentCameraEnabled && !cursorState.previousCameraEnabled) {
				setPreviousCameraState (true);
			}
			//else save the current and previous state of the camera and set the state of the camera according to the current situation
			else {
				setPreviousCameraState (playerCameraManager.cameraCanBeUsed);
				playerCameraManager.pauseOrPlayCamera (state);
				setCurrentCameraState (playerCameraManager.cameraCanBeUsed);
			}
		}
	}

	public void changeCursorState (bool state)
	{
		if (playerCameraManager != null) {
			//if the player paused the game using a device, then resume again with the camera disable to keep using that device
			if (!cursorState.currentCameraEnabled && !cursorState.previousCameraEnabled) {
				setPreviousCameraState (true);
			}
			//else save the current and previous state of the camera and set the state of the camera according to the current situation
			else {
				setPreviousCameraState (state);
				setCurrentCameraState (!state);
			}
		}
	}

	public void openOrClosePlayerMenu (bool state, Transform blurUIParent, bool useBlurUIPanel)
	{
		playerMenuActive = state;

		playerControllerManager.setPlayerMenuActiveState (playerMenuActive);

		playerControllerManager.setLastTimeMoved ();

		mainWeaponsManager.setLastTimeMoved ();

		if (useBlurUIPanelOnPlayerMenu && useBlurUIPanel) {
			changeBlurUIPanelValue (playerMenuActive, blurUIParent, useBlurUIPanel);
		}
	}

	public void openPlayerMenuWithBlurPanel (Transform blurUIParent)
	{
		playerControllerManager.setLastTimeMoved ();

		if (useBlurUIPanelOnPlayerMenu) {
			changeBlurUIPanelValue (playerMenuActive, blurUIParent, true);
		}
	}


	public void setHeadBobPausedState (bool state)
	{
		playerControllerManager.setHeadBobPausedState (state);
	}

	//the player is in a submenu, so disable the main menu
	public void enterSubMenu ()
	{
		showGUI = false;
		subMenuActive = true;
	}

	//the player backs from a submenu, so enable the main menu
	public void exitSubMenu ()
	{
		showGUI = true;
		subMenuActive = false;
	}

	//switch between touch controls and the keyboard
	public void switchControls ()
	{
		useTouchControls = !useTouchControls;

		enableOrDisableTouchControls (useTouchControls);

		pause ();

		mainGameManager.setUseTouchControlsState (useTouchControls);

		input.setUseTouchControlsState (useTouchControls);
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

	public bool isUsingTouchControls ()
	{
		return useTouchControls;
	}

	//exit from the game
	public void confirmExit ()
	{
		Application.Quit ();
	}

	public void confirmGoToHomeMenu ()
	{
		if (saveGameAutomaticallyOnReturnToHome) {
			mainGameManager.saveGameWhenReturningHomeMenu ();
		}

		SceneManager.LoadScene (sceneNumberForHomeMenu);
	}

	//enable or disable the joysticks and the touch buttons in the HUD
	public void enableOrDisableTouchControls (bool state)
	{
		input.setKeyboardControls (!state);
	}

	public void reloadStart ()
	{
		Start ();
	}

	public void resetPauseMenuBlurPanel ()
	{
		changeBlurUIPanelValue (true, pauseMenuParent, true);
	}

	public void changeBlurUIPanelValue (bool state, Transform blurUIParent, bool useBlurUIPanel)
	{
		if (!blurPanelEnabled) {
			return;
		}

		if (!useBlurUIPanel) {
			return;
		}

		if (blurUIPanel == null || blurUIPanelParent == null) {
			return;
		}

		if (state) {
			if (blurUIParent != null) {
				blurUIPanel.transform.SetParent (blurUIParent);
				blurUIPanel.transform.SetSiblingIndex (0);
			}
		} else {
			blurUIPanel.transform.SetParent (blurUIPanelParent);
		}

		if (blurUIPanelCoroutine != null) {
			StopCoroutine (blurUIPanelCoroutine);
		}

		blurUIPanelCoroutine = StartCoroutine (changeBlurUIPanelValueCoroutine (state));
	}

	IEnumerator changeBlurUIPanelValueCoroutine (bool state)
	{
		blurUIPanelActive = state;

		if (blurUIPanelActive) {
			blurUIPanel.SetActive (true);
		}

		float blurUIPanelValueTarget = 0;
		float blurUIPanelAlphaTarget = 0;

		if (blurUIPanelActive) {
			blurUIPanelValueTarget = blurUIPanelValue;
			blurUIPanelAlphaTarget = blurUIPanelAlphaValue;
		}

		checkBlurSizeID ();

		float currentBlurUiPanelValue = blurUIPanelMaterial.GetFloat (blurSizeID);
		float currentBlurUIPanelAlphaValue = blurUIPanelImage.color.a * 255;
		Color currentColor = blurUIPanelImage.color;

		if (Time.timeScale > 0) {
			while (currentBlurUiPanelValue != blurUIPanelValueTarget || currentBlurUIPanelAlphaValue != blurUIPanelAlphaTarget) {
				currentBlurUiPanelValue = Mathf.MoveTowards (currentBlurUiPanelValue, blurUIPanelValueTarget, getDeltaTime () * blurUIPanelSpeed);
				blurUIPanelMaterial.SetFloat (blurSizeID, currentBlurUiPanelValue); 

				currentBlurUIPanelAlphaValue = Mathf.MoveTowards (currentBlurUIPanelAlphaValue, blurUIPanelAlphaTarget, getDeltaTime () * blurUIPanelAlphaSpeed);

				currentColor.a = currentBlurUIPanelAlphaValue / 255;

				blurUIPanelImage.color = currentColor;

				yield return null;
			}
		} else {
			blurUIPanelMaterial.SetFloat (blurSizeID, blurUIPanelValueTarget);

			currentColor.a = blurUIPanelAlphaTarget / 255;

			blurUIPanelImage.color = currentColor;

			yield return null;
		}

		if (!blurUIPanelActive) {
			
			blurUIPanelMaterial.SetFloat (blurSizeID, 0); 

			blurUIPanel.SetActive (false);
		}
	}

	public void checkBlurSizeID ()
	{
		if (blurSizeID == -1) {
			blurSizeID = Shader.PropertyToID ("_Size");
		}
	}

	public void setIgnoreChangeHUDElementsState (bool state)
	{
		ignoreChangeHUDElements = state;
	}

	public void enableOrDisableDynamicElementsOnScreen (bool state)
	{
		pauseOrResumeShowHealthSliders (!state);

		pauseOrResumeShowObjectives (!state);

		pauseOrResumeShowIcons (!state);
	
		enableOrDisableDynamicUIElementsParent (state);
	}

	public void pauseOrResumeShowHealthSliders (bool state)
	{
		mainPlayerHealthBarManager.pauseOrResumeShowHealthSliders (state);
	}

	public void pauseOrResumeShowObjectives (bool state)
	{
		playerScreenObjectivesManager.pauseOrResumeShowObjectives (state);
	}

	public void pauseOrResumeShowIcons (bool state)
	{
		mainPlayerPickupIconManager.pauseOrResumeShowIcons (state);
	}

	public void enableOrDisableDynamicUIElementsParent (bool state)
	{
		dynamicUIElementsParent.SetActive (state);
	}

	public void inputPauseGame ()
	{
		if (pauseGameInputPaused) {
			return;
		}

		if (pauseCalledFromGameManager) {
			return;
		}

		if (openMenuSystemAssigned) {
			currentOpenMenuSystem.closeMenu ();

			return;
		}

		pauseGame ();
	}

	public void inputOpenPreviousPlayerOpenMenu ()
	{
		if (changeBetweenMenusInputEnabled) {
			openPreviousPlayerOpenMenu ();
		}
	}

	public void inputOpenNextPlayerOpenMenu ()
	{
		if (changeBetweenMenusInputEnabled) {
			openNextPlayerOpenMenu ();
		}
	}

	public void inputOpenOrClosePlayerMenu ()
	{
		if (changeBetweenIngameMenuPaused) {
			return;
		}

		openOrClosePlayerMenu ();
	}

	public void setPauseGameInputPausedState (bool state)
	{
		pauseGameInputPaused = state;
	}

	public void openOrClosePlayerMenu ()
	{
		if (!playerMenuActive) {

			if (playerControllerManager.isUsingDevice ()) {
				return;
			}

			bool indexFound = false;

			for (int i = 0; i < playerMenuInfoList.Count; i++) {
				if (!indexFound && playerMenuInfoList [i].menuCanBeUsed) {
					if (useDefaultPlayerMenuNameToOpen) {
						if (playerMenuInfoList [i].Name.Equals (defaultPlayerMenuNameToOpen)) {
							currentPlayerMenuInfoIndex = i;

							indexFound = true;
						}
					} else {
						currentPlayerMenuInfoIndex = i;

						indexFound = true;
					}
				}
			}
		}

		playerMenuInfoList [currentPlayerMenuInfoIndex].currentMenuEvent.Invoke ();
	}

	public void openNextPlayerOpenMenu ()
	{
		if (changeBetweenIngameMenuPaused) {
			return;
		}

		if (!playerMenuActive) {
			return;
		}

		if (playerControllerManager.isPlayerDriving ()) {
			return;
		}

		playerMenuInfoList [currentPlayerMenuInfoIndex].currentMenuEvent.Invoke ();

		bool exit = false;
		int max = 0;
		while (!exit) {
			max++;
			if (max > 100) {
				return;
			}

			currentPlayerMenuInfoIndex++;

			if (currentPlayerMenuInfoIndex > playerMenuInfoList.Count - 1) {
				currentPlayerMenuInfoIndex = 0;
			}

			if (playerMenuInfoList [currentPlayerMenuInfoIndex].menuCanBeUsed) {
				exit = true;
			}
		}

		playerMenuInfoList [currentPlayerMenuInfoIndex].currentMenuEvent.Invoke ();
	}

	public void openPreviousPlayerOpenMenu ()
	{
		if (changeBetweenIngameMenuPaused) {
			return;
		}

		if (!playerMenuActive) {
			return;
		}

		if (playerControllerManager.isPlayerDriving ()) {
			return;
		}

		playerMenuInfoList [currentPlayerMenuInfoIndex].currentMenuEvent.Invoke ();

		bool exit = false;
		int max = 0;
		while (!exit) {
			max++;
			if (max > 100) {
				return;
			}

			currentPlayerMenuInfoIndex--;
			if (currentPlayerMenuInfoIndex < 0) {
				currentPlayerMenuInfoIndex = playerMenuInfoList.Count - 1;
			}

			if (playerMenuInfoList [currentPlayerMenuInfoIndex].menuCanBeUsed) {
				exit = true;
			}
		}

		playerMenuInfoList [currentPlayerMenuInfoIndex].currentMenuEvent.Invoke ();
	}

	public void openPlayerOpenMenuByName (string menuName)
	{
		if (openOrClosePlayerOpenMenuByNamePaused) {
			return;
		}

		if (playerMenuActive) {
			playerMenuInfoList [currentPlayerMenuInfoIndex].currentMenuEvent.Invoke ();

			if (menuName.Equals (playerMenuInfoList [currentPlayerMenuInfoIndex].Name)) {
				return;
			}
		}

		for (int i = 0; i < playerMenuInfoList.Count; i++) {

			if (playerMenuInfoList [i].Name.Equals (menuName) && playerMenuInfoList [i].menuCanBeUsed) {
				currentPlayerMenuInfoIndex = i;

				playerMenuInfoList [i].currentMenuEvent.Invoke ();
			}
		}
	}

	public void closePlayerMenuByName (string menuName)
	{
		if (openOrClosePlayerOpenMenuByNamePaused) {
			return;
		}

		for (int i = 0; i < playerMenuInfoList.Count; i++) {

			if (playerMenuInfoList [i].Name.Equals (menuName) && playerMenuInfoList [i].menuCanBeUsed) {

				playerMenuInfoList [i].currentMenuEvent.Invoke ();

				return;
			}
		}
	}

	public void setEnabledMenuCanBeUsedState (string menuNameToSearch)
	{
		setMenuCanBeUsedState (menuNameToSearch, true);
	}

	public void setDisabledMenuCanBeUsedState (string menuNameToSearch)
	{
		setMenuCanBeUsedState (menuNameToSearch, false);
	}

	public void setMenuCanBeUsedState (string menuNameToSearch, bool state)
	{
		for (int i = 0; i < playerMenuInfoList.Count; i++) {

			if (playerMenuInfoList [i].Name.Equals (menuNameToSearch)) {
				playerMenuInfoList [i].menuCanBeUsed = state;

				updateComponent ();

				return;
			}
		}
	}

	public void setOpenOrClosePlayerOpenMenuByNamePausedState (bool state)
	{
		openOrClosePlayerOpenMenuByNamePaused = state;
	}

	public bool isOpenOrClosePlayerOpenMenuByNamePaused ()
	{
		return openOrClosePlayerOpenMenuByNamePaused;
	}

	public void setChangeBetweenIngameMenuPausedState (bool state)
	{
		changeBetweenIngameMenuPaused = state;
	}

	public void enableOrDisablePlayerHUD (bool state)
	{
		eventToEnableOrDisableAllPlayerHUD.Invoke (state);
	}

	public void enableOrDisableSecondaryPlayerHUD (bool state)
	{
		eventToEnableOrDisableSecondaryPlayerHUD.Invoke (state);
	}

	public void enableOrDisableVehicleHUD (bool state)
	{
		eventToEnableOrDisableAllVehicleHUD.Invoke (state);
	}

	public Vector2 getMainCanvasSizeDelta ()
	{
		return mainCanvasRectTransform.sizeDelta;
	}

	public Canvas getMainCanvas ()
	{
		return mainCanvas;
	}

	public List<canvasInfo> getCanvasInfoList ()
	{
		return canvasInfoList;
	}

	public void addNewCanvasToList (Canvas newCanvas)
	{
		canvasInfo newCanvasInfo = new canvasInfo ();

		newCanvasInfo.Name = newCanvas.name;
		newCanvasInfo.mainCanvas = newCanvas;
		newCanvasInfo.mainCanvasScaler = newCanvas.GetComponent<CanvasScaler> ();

		canvasInfoList.Add (newCanvasInfo);

		updateComponent ();
	}

	public bool isUsingScreenSpaceCamera ()
	{
		return mainCanvas.renderMode == RenderMode.ScreenSpaceCamera;
	}

	public CanvasScaler getMainCanvasScaler ()
	{
		return mainCanvasScaler;
	}

	public Camera getMainCanvasCamera ()
	{
		return mainCanvasCamera;
	}

	public void setCanvasInfo (bool creatingCharactersOnEditor)
	{
		playerCameraManager.setCanvasInfo (getMainCanvasSizeDelta (), isUsingScreenSpaceCamera (), creatingCharactersOnEditor);
	}

	public bool isMenuPaused ()
	{
		return gamePaused;
	}

	public bool isGamePaused ()
	{
		return mainGameManager.isGamePaused ();
	}

	public bool isLoadGameEnabled ()
	{
		return mainGameManager.isLoadGameEnabled ();
	}

	public void saveGameInfoFromEditor (string saveInfoName)
	{
		mainGameManager.saveGameInfoFromEditor (saveInfoName);
	}

	public void setGameVolume (float newValue)
	{
		AudioListener.volume = newValue;
	}

	public void setSelectedUIGameObject (GameObject newGameObject)
	{
		if (input.isUsingGamepad ()) {
			EventSystem.current.SetSelectedGameObject (newGameObject);
		} else {
			EventSystem.current.SetSelectedGameObject (null);
		}
	}

	bool openMenuSystemAssigned;

	openMenuSystem currentOpenMenuSystem;

	public void setCurrentOpenMenuSystem (openMenuSystem newOpenMenuSystem)
	{
		currentOpenMenuSystem = newOpenMenuSystem;

		openMenuSystemAssigned = currentOpenMenuSystem != null;
	}

	public void stopEditorOrGame ()
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		confirmExit();
		#endif
	}

	public void updateComponent ()
	{
		GKC_Utils.updateComponent (this);
	}
		
	//a class to save the current and previous state of the mouse visibility and the state of the camera, to enable and disable them correctly according to every
	//type of situation
	[System.Serializable]
	public class cursorStateInfo
	{
		public bool currentVisible;
		public bool previousVisible;
		public bool currentCameraEnabled;
		public bool previousCameraEnabled;
	}

	[System.Serializable]
	public class submenuInfo
	{
		public string Name;
		public GameObject menuGameObject;

		public bool isSubMenu;

		[Space]

		public bool menuOpened;

		[Space]

		public bool useEventOnOpen;
		public UnityEvent eventOnOpen;

		public bool useEventOnClose;
		public UnityEvent eventOnClose;
	}

	[System.Serializable]
	public class playerMenuInfo
	{
		public string Name;
		public bool menuCanBeUsed = true;

		[Space]

		public UnityEvent currentMenuEvent;
	}

	[System.Serializable]
	public class ingameMenuInfo
	{
		public string Name;
		public bool menuOpened;

		[Space]

		public UnityEvent closeMenuEvent;

		[Space]

		public bool useEventOnOpenClose;
		public UnityEvent eventOnOpen;
		public UnityEvent eventOnClose;

		[Space]

		public bool useMenuCanvasGroup;
		public CanvasGroup menuCanvasGroup;
		public Coroutine canvasGroupCoroutine;
		public GameObject menuPanelGameObject;

		[Space]

		public bool setCustomTimeScale;
		public float customTimeScale;

		public bool pauseAIWhenOpenMenu;
	}

	[System.Serializable]
	public class canvasInfo
	{
		public string Name;
		public Canvas mainCanvas;
		public CanvasScaler mainCanvasScaler;
	}
}