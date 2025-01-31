using Fusion;
using UnityEngine;
using TMPro;

public class CostumeManager : NetworkBehaviour
{
	[SerializeField] private DemoCostume Knight1; 
	//[SerializeField] private DemoCostume Knight2; 
	[SerializeField] private DemoCostume Horse1; 
	//[SerializeField] private DemoCostume Horse2;
	
	
	private PlayerAnchors playerAnchors;
	
	private TextMeshPro statusText;
	
	
	
	void Start()
	{
		statusText = GameObject.Find("StatusText")?.GetComponent<TextMeshPro>();
		if (statusText == null)
		{
			Debug.LogError("StatusText not found");
		}
		
		

	}

	void Update()
	{
		
	}
	
	public bool CheckIfPlayersReady()
	{
		if(Knight1.GetEquipStatus() && Horse1.GetEquipStatus())
		{
			return true;
		}
		else
		{
			return false;
		}
		
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