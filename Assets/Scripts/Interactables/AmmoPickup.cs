using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : Interactable
{

    //If the player has enough points, purchase new weapon
    public override void Interact()
    {
        if (player.score >= price)
        {
            player.weapon.currentAmmo = player.weapon.maxCurrentAmmo;
            player.weapon.reservedAmmo = player.weapon.maxReservedAmmo;
            player.score -= price;
        }
    }
}
