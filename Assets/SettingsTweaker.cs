using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;

public class SettingsTweaker : NetworkBehaviour
{
	public TextMeshPro tweakerText;
	
	public GameObject LancePrefab;
	public float lanceHeight;
	
	public DemoManager demoManager;
	
	
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		tweakerText.text = "Lance Height: " + lanceHeight + "\n";
	}
	
	
}
