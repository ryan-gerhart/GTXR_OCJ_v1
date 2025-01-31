using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Fusion;

public class LanceController : NetworkBehaviour
{
	//SettingsTweaker settingsTweaker;

	private Vector3 previousPosition;
	private Vector3 velocity;

	// Start is called before the first frame update
	void Start()
	{
		//settingsTweaker = GameObject.Find("SettingsTweaker").GetComponent<SettingsTweaker>();
		//if (settingsTweaker == null)
		//{
		//    LogStatus("SettingsTweaker not found");
		//}

		// Initialize previous position
		previousPosition = transform.position;
	}

	// Update is called once per frame
	void Update()
	{
		CalculateVelocity();
	}

	private void CalculateVelocity()
	{
		// Calculate the velocity based on the change in position over time
		velocity = (transform.position - previousPosition) / Time.deltaTime;

		// Update the previous position
		previousPosition = transform.position;

		// Log the velocity for debugging purposes
		Debug.Log($"Lance Velocity: {velocity}");
	}

	public void UpdateLanceHeight(float newHeight)
	{
		//gameObject.transform.localScale = new Vector3(1, newHeight, 1);
	}
	
	public Vector3 GetVelocity()
	{
		return velocity;
	}
}