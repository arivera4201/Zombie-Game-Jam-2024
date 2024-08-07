using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthText : MonoBehaviour
{
    private TextMeshProUGUI text;
    [SerializeField] Player player;
    
    //Get component of text
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    //Display player's health
    void Update()
    {
        text.SetText("BrainRot: " + player.brainrot);
    }
}
