using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeSpawnerThingy : MonoBehaviour
{
	
	public GameObject bikePrefab;
	public Transform spawnPoint;
	
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		// if shift s b is pressed, spawn a bike
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.B))
		{
			SpawnBike();
		}
	}
	
	public void SpawnBike()
	{
		Instantiate(bikePrefab, transform.position, transform.rotation);
	}
}
