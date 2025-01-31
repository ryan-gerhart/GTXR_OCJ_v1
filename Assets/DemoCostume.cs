using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class DemoCostume : NetworkBehaviour
{
	public enum CostumeType
	{
		Knight,
		Horse
	}
	public CostumeType costumeType;

	public enum Team
	{
		Red,
		Blue
	}
	public Team team;

	private int redTeamLayer = 21;
	private int blueTeamLayer = 22;

	CostumeManager costumeManager;
	PlayerAnchors playerAnchors;

	public GameObject costumeHeadPrefab;
	public GameObject costumeHandLeftPrefab;
	public GameObject costumeHandRightPrefab;

	Quaternion costumeHandLeftRotationOffset;
	Quaternion costumeHandRightRotationOffset;

	NetworkObject currentHead;
	NetworkObject currentHandLeft;
	NetworkObject currentHandRight;

	Rigidbody currentHeadRb;
	Rigidbody currentHandLeftRb;
	Rigidbody currentHandRightRb;

	GameObject headIndicator;

	// Costume status
	[HideInInspector] public bool shieldIsLost = false;
	[HideInInspector] public bool lanceIsLost = false;
	[HideInInspector] public bool helmetIsLost = false;

	public float shieldRotationSmooth = 1f;
	public float lanceRotationSmooth = 10f;

	private bool isEquipped = false;

	// Debug
	public bool debugEquip = false;
	public bool debugReturn = false;

	TextMeshPro statusText;

	// Start is called before the first frame update
	void Start()
	{
		statusText = GameObject.Find("StatusText").GetComponent<TextMeshPro>();
		if (statusText == null)
			LogStatus("StatusText not found");

		costumeManager = GameObject.Find("CostumeManager").GetComponent<CostumeManager>();
		if (costumeManager == null)
			LogStatus("CostumeManager not found");

		playerAnchors = GameObject.Find("PlayerAnchors").GetComponent<PlayerAnchors>();
		if (playerAnchors == null)
			LogStatus("PlayerAnchors not found");

		headIndicator = gameObject.transform.Find("HeadIndicator").gameObject;
	}

	void Update()
	{
		if (debugEquip)
		{
			debugEquip = false;
			CostumeEquip();
		}

		if (debugReturn)
		{
			debugReturn = false;
			CostumeReturn();
		}
	}

	void FixedUpdate()
	{
		if (isEquipped)
		{
			//HandleHandPhysics(currentHandLeftRb, playerAnchors.leftHandAnchor.transform, shieldRotationSmooth, costumeHandLeftRotationOffset,   );
			//HandleHandPhysics(currentHandRightRb, playerAnchors.rightHandAnchor.transform, lanceRotationSmooth, costumeHandRightRotationOffset, lanceIsLost);
		}
	}

	void HandleHandPhysics(Rigidbody handRb, Transform handAnchor, float rotationSmooth, Quaternion rotationOffset, bool isLost)
	{
		if (handRb == null || isLost)
			return;

		// Smoothly update the position of the costume piece
		Vector3 targetPosition = handAnchor.position;
		Vector3 currentPosition = handRb.position;
		handRb.velocity = (targetPosition - currentPosition) / Time.fixedDeltaTime;

		// Smoothly update the rotation of the costume piece
		Quaternion targetRotation = handAnchor.rotation * rotationOffset;
		Quaternion currentRotation = handRb.rotation;
		handRb.MoveRotation(Quaternion.Slerp(currentRotation, targetRotation, Time.fixedDeltaTime * rotationSmooth));
	}

	public void CostumeEquip()
	{
		if (isEquipped)
		{
			LogStatus("This costume has already been equipped");
			return;
		}

		if (playerAnchors.playerCostume != null)
		{
			LogStatus("You already have a costume equipped");
			return;
		}

		try
		{
			// Equip the costume
			currentHead = Runner.Spawn(costumeHeadPrefab, playerAnchors.headAnchor.transform.position, playerAnchors.headAnchor.transform.rotation, Object.InputAuthority);
			currentHead.transform.SetParent(playerAnchors.headAnchor.transform);

			// Set layer to team layer
			if (team == Team.Red)
				SetLayerRecursively(currentHead.gameObject, redTeamLayer);
			else if (team == Team.Blue)
				SetLayerRecursively(currentHead.gameObject, blueTeamLayer);

			// Request state authority for the costume
			currentHead.RequestStateAuthority();


			// Equip the left hand
			if (costumeHandLeftPrefab != null)
			{
				currentHandLeft = Runner.Spawn(costumeHandLeftPrefab, playerAnchors.leftHandAnchor.transform.position, playerAnchors.leftHandAnchor.transform.rotation, Object.InputAuthority);
				currentHandLeft.transform.SetParent(playerAnchors.leftHandAnchor.transform);

				// Set layer to team layer
				if (team == Team.Red)
					SetLayerRecursively(currentHandLeft.gameObject, redTeamLayer);
				else if (team == Team.Blue)
					SetLayerRecursively(currentHandLeft.gameObject, blueTeamLayer);

				// Request state authority for the left hand
				currentHandLeft.RequestStateAuthority();
			}

			// Equip the right hand
			if (costumeHandRightPrefab != null)
			{
				currentHandRight = Runner.Spawn(costumeHandRightPrefab, playerAnchors.rightHandAnchor.transform.position, playerAnchors.rightHandAnchor.transform.rotation, Object.InputAuthority);
				currentHandRight.transform.SetParent(playerAnchors.rightHandAnchor.transform);

				// Set layer to team layer
				if (team == Team.Red)
					SetLayerRecursively(currentHandRight.gameObject, redTeamLayer);
				else if (team == Team.Blue)
					SetLayerRecursively(currentHandRight.gameObject, blueTeamLayer);

				// Request state authority for the right hand
				currentHandRight.RequestStateAuthority();
			}

			LogStatus("Costume equipped");
		}
		catch (Exception ex)
		{
			LogStatus($"Error equipping costume: {ex.Message}");
		}

		playerAnchors.playerCostume = this;
		headIndicator.SetActive(false);
		isEquipped = true;
	}

	private NetworkObject SpawnCostumePart(GameObject prefab, Transform anchor, out Rigidbody rb)
	{
		NetworkObject part = Runner.Spawn(prefab, anchor.position, anchor.rotation, Object.InputAuthority);
		rb = part.GetComponent<Rigidbody>();

		int layer = team == Team.Red ? redTeamLayer : blueTeamLayer;
		SetLayerRecursively(part.gameObject, layer);

		part.RequestStateAuthority();
		return part;
	}

	private NetworkObject SpawnCostumePart(GameObject prefab, Transform anchor, Rigidbody rb = null)
	{
		return SpawnCostumePart(prefab, anchor, out _);
	}

	public void CostumeReturn()
	{
		if (!isEquipped)
		{
			LogStatus("Costume already removed");
			return;
		}

		if (playerAnchors.playerCostume != this)
		{
			LogStatus("Another costume already equipped");
			return;
		}
		
		if (currentHead != null)
		{
			DespawnCostumePart(currentHead);
		}

		if (currentHandLeft != null)
		{
			DespawnCostumePart(currentHandLeft);
		}

		if (currentHandRight != null)
		{
			DespawnCostumePart(currentHandRight);
		}

		isEquipped = false;
		playerAnchors.playerCostume = null;
		headIndicator.SetActive(true);

		LogStatus("Costume removal confirmed");
	}

	private void DespawnCostumePart(NetworkObject part)
	{
		if (part != null)
			Runner.Despawn(part);
	}
	
	public void DespawnHelmet()
	{
		if (currentHead != null)
		{
			Runner.Despawn(currentHead);
			helmetIsLost = true;
		}
		
		
	}
	
	public void DespawnShield()
	{
		if (currentHandLeft != null)
		{
			Runner.Despawn(currentHandLeft);
			shieldIsLost = true;
		}
	}
	
	public void DespawnLance()
	{
		if (currentHandRight != null)
		{
			Runner.Despawn(currentHandRight);
			lanceIsLost = true;
		}
	}
	
	
	

	public bool GetEquipStatus()
	{
		return isEquipped;
	}

	private void SetLayerRecursively(GameObject obj, int newLayer)
	{
		if (obj == null) return;

		obj.layer = newLayer;

		foreach (Transform child in obj.transform)
		{
			if (child == null) continue;
			SetLayerRecursively(child.gameObject, newLayer);
		}
	}

	void LogStatus(string message)
	{
		Debug.Log(message);
		if (statusText != null)
		{
			statusText.text = message + "\n" + statusText.text;

			// Limit lines to avoid overflow
			string[] lines = statusText.text.Split('\n');
			if (lines.Length > 50)
			{
				statusText.text = string.Join("\n", lines, 0, 50);
			}
		}
	}
}