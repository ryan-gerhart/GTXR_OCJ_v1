using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KnightHorseSwapper : MonoBehaviour
{
    [Header("Player Anchors")]
    public GameObject playerHead;
    public GameObject playerHandLeft;
    public GameObject playerHandRight;

    [Header("Knight Prefabs")]
    public GameObject knightHeadPrefab;
    public GameObject knightHandLeftPrefab;
    public GameObject knightHandRightPrefab;

    [Header("Horse Prefabs")]
    public GameObject horseHeadPrefab;
    public GameObject horseHandLeftPrefab;
    public GameObject horseHandRightPrefab;

    public TextMeshPro statusText;

    private GameObject currentHead;
    private GameObject currentHandLeft;
    private GameObject currentHandRight;

    // Start is called before the first frame update
    void Start()
    {
        SwapToPlayer();
    }

    // Update is called once per frame
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

    public void SwapToKnight()
    {
        ClearCurrentParts();

        currentHead = Instantiate(knightHeadPrefab, playerHead.transform.position, playerHead.transform.rotation, playerHead.transform);
        currentHandLeft = Instantiate(knightHandLeftPrefab, playerHandLeft.transform.position, playerHandLeft.transform.rotation, playerHandLeft.transform);
        currentHandRight = Instantiate(knightHandRightPrefab, playerHandRight.transform.position, playerHandRight.transform.rotation, playerHandRight.transform);

        LogStatus("Swapped to Knight");
    }

    public void SwapToHorse()
    {
        ClearCurrentParts();

        currentHead = Instantiate(horseHeadPrefab, playerHead.transform.position, playerHead.transform.rotation, playerHead.transform);
        currentHandLeft = Instantiate(horseHandLeftPrefab, playerHandLeft.transform.position, playerHandLeft.transform.rotation, playerHandLeft.transform);
        currentHandRight = Instantiate(horseHandRightPrefab, playerHandRight.transform.position, playerHandRight.transform.rotation, playerHandRight.transform);

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
}