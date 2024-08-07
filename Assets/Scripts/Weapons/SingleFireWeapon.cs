using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleFireWeapon : BaseWeapon
{
    //Fire a single shot when left click is pressed
    public override void StartFiring()
    {
        if (canShoot && currentAmmo > 0 && !isReloading && Time.timeScale > 0)
        {
            Fire();
            StartCoroutine(Recoil());
        }
    }

    public override void StopFiring()
    {

    }

    private IEnumerator Recoil()
    {
        canShoot = false;

        // Initial rotation
        Quaternion initialRotation = transform.localRotation;

        // Recoil rotation
        Quaternion recoilRotation = initialRotation * Quaternion.Euler(-recoilAngle, 0f, 0f);

        // Rotate upwards
        float elapsedTime = 0f;
        while (elapsedTime < recoilDuration)
        {
            transform.localRotation = Quaternion.Slerp(initialRotation, recoilRotation, elapsedTime / recoilDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localRotation = recoilRotation;

        // Rotate back to initial position
        elapsedTime = 0f;
        while (elapsedTime < returnDuration)
        {
            transform.localRotation = Quaternion.Slerp(recoilRotation, initialRotation, elapsedTime / returnDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localRotation = initialRotation;

        canShoot = true;
    }
}
