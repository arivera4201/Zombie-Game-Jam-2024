using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointText : MonoBehaviour
{
    private TextMeshProUGUI text;
    [SerializeField] Player player;

    //Get component of text
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    //Display player's points
    void Update()
    {
        text.SetText("Points: " + player.score);
    }
}
