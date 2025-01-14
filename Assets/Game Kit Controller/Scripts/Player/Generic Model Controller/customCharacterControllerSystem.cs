﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customCharacterControllerSystem : customCharacterControllerBase
{
	[Header ("Custom Settings")]
	[Space]

	public string horizontalAnimatorName = "Horizontal";
	public string verticalAnimatorName = "Vertical";
	public string stateAnimatorName = "State";

	public string groundedStateAnimatorName = "Grounded";
	public string movementAnimatorName = "Movement";
	public string speedMultiplierAnimatorName = "SpeedMultiplier";

	//	public string stateFloatAnimatorName = "StateFloat";
	//	public string lastStateAnimatorName = "LastState";
	//	public string stateAnimatorName = "StateStatus";
	//	public string stateAnimatorName = "Mode";
	//	public string stateAnimatorName = "ModeStatus";
	//	public string deltaAngleAnimatorName = "DeltaAngle";

	[Space]
	[Header ("Other Settings")]
	[Space]

	public int jumpState = 2;
	public int movementState = 1;
	public int fallState = 3;
	public int deathState = 10;

	public int currentState;

	int horizontalAnimatorID;
	int verticalAnimatorID;

	int stateAnimatorID;
	int groundedStateAnimatorID;

	int movementAnimatorID;

	bool valuesInitialized;

	//	int speedMultiplierAnimatorID;
	//	int deltaAngleAnimatorID;

	void initializeValues ()
	{
		if (!valuesInitialized) {
			horizontalAnimatorID = Animator.StringToHash (horizontalAnimatorName);
			verticalAnimatorID = Animator.StringToHash (verticalAnimatorName);

			stateAnimatorID = Animator.StringToHash (stateAnimatorName);
			groundedStateAnimatorID = Animator.StringToHash (groundedStateAnimatorName);
			movementAnimatorID = Animator.StringToHash (movementAnimatorName);

//			speedMultiplierAnimatorID = Animator.StringToHash (speedMultiplierAnimatorName);
//			deltaAngleAnimatorID = Animator.StringToHash (deltaAngleAnimatorName);

			valuesInitialized = true;
		}
	}

	public override void updateCharacterControllerState ()
	{
		updateAnimatorFloatValueLerping (horizontalAnimatorID, turnAmount, animatorTurnInputLerpSpeed, Time.fixedDeltaTime);

		updateAnimatorFloatValueLerping (verticalAnimatorID, forwardAmount, animatorForwardInputLerpSpeed, Time.fixedDeltaTime);

		updateAnimatorBoolValue (groundedStateAnimatorID, onGround);

		updateAnimatorBoolValue (movementAnimatorID, playerUsingInput);
	}

	public override void updateCharacterControllerAnimator ()
	{
		
	}

	public override void updateMovementInputValues (Vector3 newValues)
	{

	}

	public override void updateHorizontalVerticalInputValues (Vector2 newValues)
	{

	}

	public override void activateJumpAnimatorState ()
	{
		updateAnimatorIntegerValue (stateAnimatorID, jumpState);

		currentState = jumpState;
	}

	public override void updateOnGroundValue (bool state)
	{
		base.updateOnGroundValue (state);

		if (currentState == 1) {
			if (!onGround) {
				updateAnimatorIntegerValue (stateAnimatorID, 3);

				currentState = 3;
			}
		} else {
			if (onGround) {
				updateAnimatorIntegerValue (stateAnimatorID, 1);

				currentState = 1;
			} else {

//				if (currentState == 2) {
//					updateAnimatorIntegerValue (stateAnimatorID, 20);
//
//					currentState = 20;
//				}
			}
		}
	}

	public override void setCharacterControllerActiveState (bool state)
	{
		base.setCharacterControllerActiveState (state);

		if (state) {
			initializeValues ();
		}
	}
}
