using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunWeapon : BaseWeapon
{
    public int pellets = 20;
    private List<Zombie> zombiesHit;

    //New Start function required to declare list of zombies hit by one shot
    private void Start()
    {
        cameraTransform = transform.parent.parent.transform;
        zombiesHit = new List<Zombie>();
        gunSound = GetComponent<AudioSource>();
        initialPosition = transform.localPosition;
        trailRenderer = FindObjectOfType<TrailRenderer>();
    }

    //Single fire once left click is pressed
    public override void StartFiring()
    {
        if (canShoot && currentAmmo > 0 && !isReloading)
        {
            Fire();
            StartCoroutine(Recoil());
            PlayGunSound();
        }
    }

    public override void StopFiring()
    {

    }

    //Perform several raycasts based on number of pellets, save each zombie hit, apply damage to each
    public override void Fire()
    {
        if (currentAmmo > 0)
        {
            for (int i = 0; i < pellets; ++i)
            {
                Vector3 offset = new Vector3(Random.Range(-.1f, .1f), Random.Range(-.1f, .1f), Random.Range(-.1f, .1f));
                muzzleFlash.Play();
                RaycastHit hit;
                if (Physics.Raycast(cameraTransform.position, (cameraTransform.TransformDirection(Vector3.forward) + offset), out hit, 10))
                {
                    StartCoroutine(RenderTrail(hit.point));
                    if (hit.collider.gameObject.tag == "Zombie")
                    {
                        Zombie zombie = hit.collider.gameObject.GetComponent<Zombie>();
                        if (!zombiesHit.Contains(zombie)) zombiesHit.Add(zombie);
                        Debug.Log(zombie);
                    }
                }
                else
                {
                    Debug.Log((muzzleFlash.transform.TransformDirection(Vector3.forward)) * 10);
                    StartCoroutine(RenderTrail(transform.position + (muzzleFlash.transform.TransformDirection(Vector3.forward) + offset) * 10));
                }
            }
            foreach (Zombie zombie in zombiesHit) zombie.TakeDamage(damage);
            zombiesHit.Clear();
            currentAmmo--;
            if (currentAmmo <= 0) StartReload();
        }
    }

    private IEnumerator Recoil()
    {
        canShoot = false;

        Quaternion initialRotation = transform.localRotation;
        Quaternion recoilRotation = initialRotation * Quaternion.Euler(-recoilAngle, 0f, 0f);

        Vector3 initialPosition = transform.localPosition;
        Vector3 recoilPosition = initialPosition + new Vector3(0f, 0f, -0.3f);

        // Rotate upwards
        float elapsedTime = 0f;
        while (elapsedTime < recoilDuration)
        {
            transform.localRotation = Quaternion.Slerp(initialRotation, recoilRotation, elapsedTime / recoilDuration);
            transform.localPosition = Vector3.Slerp(initialPosition, recoilPosition, elapsedTime / recoilDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localRotation = recoilRotation;
        transform.localPosition = recoilPosition;

        // Rotate back to initial position
        elapsedTime = 0f;
        while (elapsedTime < returnDuration)
        {
            transform.localRotation = Quaternion.Slerp(recoilRotation, initialRotation, elapsedTime / returnDuration);
            transform.localPosition = Vector3.Slerp(recoilPosition, initialPosition, elapsedTime / recoilDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localRotation = initialRotation;
        transform.localPosition = initialPosition;

        canShoot = true;
    }
}
