using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreIncrementer : MonoBehaviour
{
    public int score;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) {
            Increment();
        }
    }

    void Increment() {
        score++;
        gameObject.GetComponent<TextMeshPro>().text = score.ToString();
    }
}
