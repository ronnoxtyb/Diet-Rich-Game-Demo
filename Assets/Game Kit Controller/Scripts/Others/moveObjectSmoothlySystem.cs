﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveObjectSmoothlySystem : MonoBehaviour
{
	public List<objectPositionStateInfo> objectPositionStateInfoList = new List<objectPositionStateInfo> ();

	objectPositionStateInfo currentObjectPositionStateInfo;

	Coroutine movementCoroutine;

	public void moveObjectToPosition (string positionName)
	{
		stopMoveObjectToPositionCoroutine ();

		movementCoroutine = StartCoroutine (moveObjectToPositionCoroutine (positionName));
	}

	public void stopMoveObjectToPositionCoroutine ()
	{
		if (movementCoroutine != null) {
			StopCoroutine (movementCoroutine);
		}
	}

	IEnumerator moveObjectToPositionCoroutine (string positionName)
	{
		bool isNewPosition = true;

		int positionStateIndex = -1;

		for (int i = 0; i < objectPositionStateInfoList.Count; i++) {
			if (objectPositionStateInfoList [i].Name == positionName) {
				if (objectPositionStateInfoList [i].isCurrentPosition) {
					isNewPosition = false;
				} else {
					objectPositionStateInfoList [i].isCurrentPosition = true;
				}

				positionStateIndex = i;

				currentObjectPositionStateInfo = objectPositionStateInfoList [i];
			} else {
				objectPositionStateInfoList [i].isCurrentPosition = false;
			}
		}

		if (isNewPosition && positionStateIndex > -1) {

			float dist = GKC_Utils.distance (transform.localPosition, currentObjectPositionStateInfo.targetPosition);
			float duration = dist / currentObjectPositionStateInfo.movementSpeed;
			float translateTimer = 0;

			float teleportTimer = 0;

			bool targetReached = false;

			while (!targetReached) {
				translateTimer += Time.deltaTime / duration;
				transform.localPosition = Vector3.Lerp (transform.localPosition, currentObjectPositionStateInfo.targetPosition, translateTimer);

				teleportTimer += Time.deltaTime;

				if ((GKC_Utils.distance (transform.localPosition, currentObjectPositionStateInfo.targetPosition) < 0.03f) || teleportTimer > (duration + 1)) {
					targetReached = true;
				}

				yield return null;
			}
		}
	}

	[System.Serializable]
	public class objectPositionStateInfo
	{
		public string Name;
		public Vector3 targetPosition;
		public float movementSpeed;
		public bool isCurrentPosition;
	}
}
