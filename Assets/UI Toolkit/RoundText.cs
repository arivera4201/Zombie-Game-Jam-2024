using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundText : MonoBehaviour
{
    private TextMeshProUGUI text;
    private RoundHandler roundHandler;

    //Get component of text
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        roundHandler = FindObjectOfType<RoundHandler>();
    }

    //Display current round
    void Update()
    {
        text.SetText("Round " + roundHandler.round.ToString());
    }
}
