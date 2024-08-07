using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgrade : Interactable
{
    //If player has enough points, upgrade current weapon
    public override void Interact()
    {
        if (player.score >= price)
        {
            player.score -= price;
            player.weapon.currentAmmo = player.weapon.maxCurrentAmmo;
            player.weapon.reservedAmmo = player.weapon.maxReservedAmmo;
            player.weapon.damage = player.weapon.damage + player.weapon.additionalDamage;
        }
    }
}
