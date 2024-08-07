using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Interactable : MonoBehaviour
{
    public Player player;
    public string interactText = "Press F to";
    public int price;
    private TextMeshProUGUI text;

    //Find the player along with the text that displays interact text
    void Start()
    {
        text = GameObject.FindWithTag("Interactable").gameObject.GetComponent<TextMeshProUGUI>();
        player = GameObject.FindWithTag("Player").gameObject.GetComponent<Player>();
    }

    //Abstract method to determine what child scripts will do
    public abstract void Interact();

    //When a player overlaps the object, display the text and price of the object and set player interactable object to this

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            text.SetText(interactText + " for " + price);
            player.interactableObject = this;
        }
    }

    //When overlap ends, remove text and set player interactable object to null
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            text.SetText("");
            player.interactableObject = null;
        }
    }
}
