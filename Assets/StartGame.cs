using Fusion;
using UnityEngine;
using TMPro;
using System;

public class StartGame : NetworkBehaviour
{
    public NetworkRunner networkRunner;
    public NetworkProjectConfig networkProjectConfig;

    private TextMeshPro statusText;
    [SerializeField] private NetworkObject titleScreen; // Assign the prefab in the Inspector
    [SerializeField] private NetworkObject scoreboard; // Assign the prefab in the Inspector
    
    void Start()
    {
        statusText = GameObject.Find("StatusText")?.GetComponent<TextMeshPro>();
        if (statusText == null)
        {
            Debug.LogError("StatusText not found");
        }

        networkRunner = FindObjectOfType<NetworkRunner>();
        if (networkRunner == null)
        {
            LogStatus("NetworkRunner not found! Make sure one exists in the scene.");
            return;
        }

        if (networkProjectConfig == null)
        {
            LogStatus("NetworkProjectConfig not found! Make sure one exists in the scene.");
            return;
        }

        if (titleScreen == null || scoreboard == null)
        {
            LogStatus("Object prefab not assigned! Make sure it is assigned in the Inspector.");
            return;
        }

        // Register the prefab in the NetworkProjectConfig
        try
        {
            RegisterPrefab(networkProjectConfig, titleScreen);
            RegisterPrefab(networkProjectConfig, scoreboard);
            LogStatus("Prefabs registered successfully.");
        }
        catch (Exception e)
        {
            LogStatus($"Failed to register prefab: {e.Message}");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStart() {
        if (networkRunner.IsServer && titleScreen != null && scoreboard != null) {
            networkRunner.Despawn(titleScreen);
            networkRunner.Spawn(scoreboard);
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

    // This method registers the prefab in the NetworkProjectConfig
    NetworkPrefabId RegisterPrefab(NetworkProjectConfig config, NetworkObject prefab)
    {
        var source = new NetworkPrefabSourceStatic
        {
            PrefabReference = prefab
        };

        if (config.PrefabTable.TryAdd(prefab.NetworkGuid, source, out var id))
        {
            return id;
        }

        throw new ArgumentException($"Failed to register prefab with guid: {prefab.NetworkGuid}");
    }
}
