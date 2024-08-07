using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullAutoWeapon : BaseWeapon
{
    private bool isFiring;
    
    //Begin the firing loop for fully automatic firing
    public override void StartFiring()
    {
        isFiring = true;
        FiringLoop();
    }

    //Continuously fire as long as player is holding left click
    void FiringLoop()
    {
        if (isFiring && canShoot && currentAmmo > 0 && !isReloading)
        {
            Fire();
            StartCoroutine(Recoil());
        }
    }

    //Stop firing once left click is released
    public override void StopFiring()
    {
        isFiring = false;
    }

    private IEnumerator Recoil()
    {
        canShoot = false;

        Vector3 initialPosition = transform.localPosition;
        Vector3 recoilPosition = initialPosition + new Vector3(0f, 0f, -0.1f);

        // Rotate upwards
        float elapsedTime = 0f;
        while (elapsedTime < recoilDuration)
        {
            transform.localPosition = Vector3.Slerp(initialPosition, recoilPosition, elapsedTime / recoilDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = recoilPosition;

        // Rotate back to initial position
        elapsedTime = 0f;
        while (elapsedTime < returnDuration)
        {
            transform.localPosition = Vector3.Slerp(recoilPosition, initialPosition, elapsedTime / recoilDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = initialPosition;

        canShoot = true;
        FiringLoop();
    }
}
