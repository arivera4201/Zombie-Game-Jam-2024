using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup: Interactable
{
    public GameObject newWeapon;

    //If the player has enough points, purchase new weapon
    public override void Interact()
    {
        if (player.score >= price)
        {
            var weaponInstance = Instantiate(newWeapon);
            weaponInstance.transform.parent = player.transform.GetChild(0).GetChild(1).transform;
            weaponInstance.transform.localPosition = player.transform.GetChild(0).GetChild(1).GetChild(0).transform.localPosition;
            weaponInstance.transform.localRotation = Quaternion.Euler(0, 0, 0);
            Destroy(player.weapon.gameObject);
            player.weapon = weaponInstance.GetComponent<BaseWeapon>();
            player.score -= price;
        }
    }
}
