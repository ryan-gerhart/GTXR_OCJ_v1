using Fusion;
using UnityEngine;
using TMPro;
using System;

public class SpawnerTest1 : MonoBehaviour
{
    [SerializeField] private NetworkObject objectPrefab; // Assign the prefab in the Inspector
    public NetworkRunner networkRunner;
    public NetworkProjectConfig networkProjectConfig;

    private TextMeshPro statusText;
    public Transform spawnPoint;

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

        if (objectPrefab == null)
        {
            LogStatus("Object prefab not assigned! Make sure it is assigned in the Inspector.");
            return;
        }

        // Register the prefab in the NetworkProjectConfig
        try
        {
            RegisterPrefab(networkProjectConfig, objectPrefab);
            LogStatus("Prefab registered successfully.");
        }
        catch (Exception e)
        {
            LogStatus($"Failed to register prefab: {e.Message}");
            return;
        }

        // Example: Spawn an object when the game starts
        if (networkRunner.IsServer)
        {
            SpawnObject(Vector3.zero, Quaternion.identity);
        }
    }

    void Update()
    {
        // Example: Spawn an object when the user presses the space key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnObjectAtSpawnPoint();
        }
    }

    public void SpawnObject(Vector3 position, Quaternion rotation)
    {
        if (networkRunner != null)
        {
            networkRunner.Spawn(objectPrefab, position, rotation, networkRunner.LocalPlayer);
            LogStatus("Spawned object");
        }
        else
        {
            Debug.LogError("NetworkRunner not found! Make sure one exists in the scene.");
        }
    }

    public void SpawnObjectAtSpawnPoint()
    {
        LogStatus("Spawn object at spawn point");
        SpawnObject(spawnPoint.position, spawnPoint.rotation);
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