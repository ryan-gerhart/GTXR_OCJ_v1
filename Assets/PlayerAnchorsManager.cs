using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerAnchorsManager : MonoBehaviour
{
	[Header("Player Anchors")]
	public GameObject playerHead;
	public GameObject playerHandLeft;
	public GameObject playerHandRight;

	public TextMeshPro statusText;




	void Start()
	{

	}

	void Update()
	{
		
	}

	void LogStatus(string message)
	{
		Debug.Log(message);
		if (statusText != null)
		{
			statusText.text = message + "\n" + statusText.text;

			// Split the text into lines and check the number of lines
			string[] lines = statusText.text.Split('\n');
			if (lines.Length > 10) // Assuming maxLines is 10
			{
				// Keep only the most recent 10 lines
				statusText.text = string.Join("\n", lines, 0, 10);
			}
		}
	}

}