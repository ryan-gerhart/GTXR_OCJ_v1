using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class DemoManager : NetworkBehaviour
{
	CostumeManager costumeManager;
	
	
	// Velocities
	public float LightHit = 5;
	public float MediumHit = 10;
	public float HeavyHit = 15;
	
	public bool debugStartGame = false;
	
	TextMeshPro statusText;
	
	// Start is called before the first frame update
	void Start()
	{
		statusText = GameObject.Find("StatusText").GetComponent<TextMeshPro>();
		if (statusText == null)
		{
			LogStatus("StatusText not found");
		}
		
		costumeManager = GameObject.Find("CostumeManager").GetComponent<CostumeManager>();
		if (costumeManager == null)
		{
			LogStatus("CostumeManager not found");
		}
		
	}

	// Update is called once per frame
	void Update()
	{
		if(debugStartGame)
		{
			debugStartGame = false;
			StartGame();
		}
	}
	
	public void StartGame()
	{
		if(costumeManager.CheckIfPlayersReady())
		{
			LogStatus("Players are ready!");
		}
		else
		{
			LogStatus("Players are not ready!");
			return;
		}
		
		LogStatus("Starting game...");
	}
	
	
	
	void LogStatus(string message)
	{
		Debug.Log(message);
		if (statusText != null)
		{
			statusText.text = message + "\n" + statusText.text;

			// Split the text into lines and check the number of lines
			string[] lines = statusText.text.Split('\n');
			if (lines.Length > 50) // Assuming maxLines is 50
			{
				// Keep only the most recent 50 lines
				statusText.text = string.Join("\n", lines, 0, 50);
			}
		}
	}
}
