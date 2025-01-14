﻿using UnityEngine;
using System.Collections;

public class laserConnector : laser
{
	[Space]
	[Header ("Main Settings")]
	[Space]

	public LayerMask layer;
	GameObject currentLaser;
	GameObject cubeRefractionLaser;
	GameObject raycast;
	GameObject laser2;
	RaycastHit hit;
	GameObject receiver;

	//the laser connector is activated when a laser device is deflected
	void Start ()
	{
		StartCoroutine (laserAnimation ());
	}

	void Update ()
	{
		//check if the laser connector hits a lasers receiver, or any other object to disable the laser connection
		if (Physics.Raycast (transform.position, transform.forward, out hit, Mathf.Infinity, layer)) {
			if (!hit.collider.GetComponent<laserReceiver>() && !hit.collider.GetComponent<refractionCube>()) {
				disableRefractionState ();
			} else {
				if (!laser2 && cubeRefractionLaser) {
					//get the laser inside the refraction cube
					laser2 = cubeRefractionLaser.transform.GetChild (0).gameObject;
					laser2.SetActive (true);
					//set the color of the laser connector according to the laser beam deflected
					if (laser2.GetComponent<Renderer> ()) {
						laser2.GetComponent<Renderer> ().material.SetColor ("_TintColor", cubeRefractionLaser.GetComponent<Renderer> ().material.GetColor ("_Color"));
					}
				}
				laserDistance = hit.distance;
				if (!receiver) {
					receiver = hit.collider.gameObject;
				}
			}
		} else {
			laserDistance = 1000;
		}
		//set the laser size according to the hit position
		lRenderer.SetPosition (1, (laserDistance * Vector3.forward));
		animateLaser ();
	}

	public void disableRefractionState ()
	{
		//if the player touchs the laser connector, disable the reflected laser
		if (laser2) {
			if (laser2.activeSelf) {
				laser2.SetActive (false);
				cubeRefractionLaser.GetComponent<refractionCube>().setRefractingLaserState (false);
				cubeRefractionLaser = null;
				laser2 = null;
			}
		}
		currentLaser.GetComponent<laserDevice> ().setAssignLaserState (false);
		gameObject.SetActive (false);
		if (receiver) {
			if (receiver.GetComponent<laserReceiver> ()) {
				receiver.GetComponent<laserReceiver> ().laserDisconnected ();
			}
			receiver = null;
		}
	}

	//set the color of the laser beam
	public void setColor ()
	{
		Color c = currentLaser.GetComponent<Renderer> ().material.GetColor ("_TintColor");
		mainRenderer.material.SetColor ("_TintColor", c);
	}

	public void setCurrentLaser(GameObject laser){
		currentLaser = laser;
	}

	public void setCubeRefractionLaser(GameObject cube){
		cubeRefractionLaser = cube;
	}
}