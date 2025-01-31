using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShieldController : MonoBehaviour
{
	DemoManager demoManager;
	public DemoCostume demoCostume;
	private LanceController lanceController;

	private Vector3 previousPosition;
	private Vector3 velocity;

	[Header("Impact Velocities")]
	public float lightHit = 5f;
	public float mediumHit = 8f;
	public float heavyHit = 10f;

	TextMeshPro statusText;

	// Start is called before the first frame update
	void Start()
	{
		statusText = GameObject.Find("StatusText").GetComponent<TextMeshPro>();
		if (statusText == null)
		{
			LogStatus("StatusText not found");
		}

		demoManager = GameObject.Find("DemoManager").GetComponent<DemoManager>();
		if (demoManager == null)
		{
			LogStatus("DemoManager not found");
		}

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
		Debug.Log($"Shield Velocity: {velocity}");
	}

	// on collision with lance
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Lance")
		{
			LogStatus("Shield hit by lance");

			// Get the LanceController component from the colliding object
			lanceController = collision.gameObject.GetComponent<LanceController>();
			if (lanceController != null)
			{
				// Calculate the relative velocity using the velocity trackers
				Vector3 relativeVelocity = velocity - lanceController.GetVelocity();
				float impactVelocity = relativeVelocity.magnitude;
				LogStatus("IMPACT!! Impact Velocity: " + impactVelocity);

				// send impact velocity to demo manager

				if (impactVelocity >= lightHit)
				{
					demoCostume.DespawnShield();
				}
				// if it's a heavy hit, lose lance
				else if (impactVelocity >= heavyHit)
				{
					demoCostume.DespawnLance();
				}
			}
		}
	}

	public Vector3 GetVelocity()
	{
		return velocity;
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
				// Remove the last line
				statusText.text = statusText.text.Substring(0, statusText.text.LastIndexOf('\n'));
			}
		}
	}

	public void SetDemoCostume(DemoCostume demoCostume)
	{
		this.demoCostume = demoCostume;
	}
}