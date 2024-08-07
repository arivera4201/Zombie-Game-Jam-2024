using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetWeaponAmmo : MonoBehaviour
{
    private TextMeshProUGUI text;
    [SerializeField] Player player;

    //Get component of text
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    //Display player's current ammo
    void Update()
    {
        if(player.weapon.isReloading) text.SetText("Reloading...");
        else text.SetText(player.weapon.currentAmmo.ToString() + "|" + player.weapon.reservedAmmo.ToString());
    }
}
