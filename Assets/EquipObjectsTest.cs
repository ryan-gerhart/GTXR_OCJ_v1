using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class EquipObjectsTest : MonoBehaviour
{
	public GameObject playerHeadAnchor;
	
	public GameObject KnightHead;

	public GameObject HorseHead;
	
	private GameObject currentHead;
	
	
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		if(currentHead == null)
			return;
		
		currentHead.transform.position = playerHeadAnchor.transform.position;
	}
	
	public void EquipKnight()
	{
		currentHead = KnightHead;
	}
	
	public void EquipHorse()
	{
		currentHead = HorseHead;
	}
	
	/*
	private void TransferOwnershipToLocalPlayer()
	{
		if (!HasStateAuthority)
		{
			Object.RequestStateAuthority();
		}
	}
	*/
	
}
