using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using OVR.Input;

public class KnightHorseManager : MonoBehaviour
{
    TextMeshPro statusText;
    PlayerAnchorsManager playerAnchorsManager;

    [Header("Knight Prefabs")]
    public GameObject knightHeadPrefab;
    public GameObject knightHandLeftPrefab;
    public GameObject knightHandRightPrefab;

    [Header("Horse Prefabs")]
    public GameObject horseHeadPrefab;
    public GameObject horseHandLeftPrefab;
    public GameObject horseHandRightPrefab;

    // Current parts
    private GameObject currentHead;
    private GameObject currentHandLeft;
    private GameObject currentHandRight;

    void Start()
    {
        statusText = GameObject.Find("StatusText").GetComponent<TextMeshPro>();
        if (statusText == null)
            LogStatus("StatusText not found");

        playerAnchorsManager = GameObject.Find("PlayerAnchorsManager").GetComponent<PlayerAnchorsManager>();
        if (playerAnchorsManager == null)
            LogStatus("PlayerAnchorsManager not found");
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            SwapToKnight();
        }
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            SwapToHorse();
        }
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            SwapToPlayer();
        }
    }

    public void SwapToKnight()
    {
        ClearCurrentParts();

        currentHead = Instantiate(knightHeadPrefab, playerAnchorsManager.playerHead.transform.position, playerAnchorsManager.playerHead.transform.rotation, playerAnchorsManager.playerHead.transform);
        currentHandLeft = Instantiate(knightHandLeftPrefab, playerAnchorsManager.playerHandLeft.transform.position, playerAnchorsManager.playerHandLeft.transform.rotation, playerAnchorsManager.playerHandLeft.transform);
        currentHandRight = Instantiate(knightHandRightPrefab, playerAnchorsManager.playerHandRight.transform.position, playerAnchorsManager.playerHandRight.transform.rotation, playerAnchorsManager.playerHandRight.transform);

        LogStatus("Swapped to Knight");
    }

    public void SwapToHorse()
    {
        ClearCurrentParts();

        currentHead = Instantiate(horseHeadPrefab, playerAnchorsManager.playerHead.transform.position, playerAnchorsManager.playerHead.transform.rotation, playerAnchorsManager.playerHead.transform);
        currentHandLeft = Instantiate(horseHandLeftPrefab, playerAnchorsManager.playerHandLeft.transform.position, playerAnchorsManager.playerHandLeft.transform.rotation, playerAnchorsManager.playerHandLeft.transform);
        currentHandRight = Instantiate(horseHandRightPrefab, playerAnchorsManager.playerHandRight.transform.position, playerAnchorsManager.playerHandRight.transform.rotation, playerAnchorsManager.playerHandRight.transform);

        LogStatus("Swapped to Horse");
    }

    public void SwapToPlayer()
    {
        ClearCurrentParts();

        LogStatus("Swapped to Player");
    }

    private void ClearCurrentParts()
    {
        if (currentHead != null) Destroy(currentHead);
        if (currentHandLeft != null) Destroy(currentHandLeft);
        if (currentHandRight != null) Destroy(currentHandRight);
    }

    void LogStatus(string message)
    {
        Debug.Log(message);
        if (statusText != null)
        {
            statusText.text = message + "\n" + statusText.text;

            // Split the text into lines and check the number of lines
            string[] lines = statusText.text.Split('\n');
            if (lines.Length > 50) // Assuming maxLines is 10
            {
                // Keep only the most recent 10 lines
                statusText.text = string.Join("\n", lines, 0, 50);
            }
        }
    }
}