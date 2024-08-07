using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    //THIS IS A MESS THESE WILL NOT ALL BE PUBLIC LATER
    public AudioSource gunSound;
    public Transform cameraTransform;
    public int currentAmmo = 8;
    public int reservedAmmo = 32;
    public int maxCurrentAmmo = 8;
    public int maxReservedAmmo = 32;
    public float rateOfFire;
    public float damage = 75.0f;
    public float additionalDamage = 75.0f;
    public float reloadTime = 0.5f;
    public ParticleSystem muzzleFlash;
    public bool isReloading;
    public bool canShoot = true;
    public float recoilAngle = 10f; // The angle to rotate the gun upwards
    public float recoilDuration = 0.1f; // The time it takes for the gun to recoil upwards
    public float returnDuration = 0.1f; // The time it takes for the gun to return to the original position
    public Vector3 initialPosition = new Vector3();
    public TrailRenderer trailRenderer;

    //Save transform of camera, used for firing
    private void Start()
    {
        cameraTransform = transform.parent.parent.transform;
        gunSound = GetComponent<AudioSource>();
        initialPosition = transform.localPosition;
        trailRenderer = FindObjectOfType<TrailRenderer>();
    }

    //Perform a raycast that checks for zombies and removes one bullet, if zombie is hit, apply damage
    public virtual void Fire()
    {
        if (Time.timeScale > 0)
        {
            if (currentAmmo > 0)
            {
                PlayGunSound();
                currentAmmo--;
                muzzleFlash.Play();
                RaycastHit hit;
                if (Physics.Raycast(cameraTransform.position, cameraTransform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
                {
                    StartCoroutine(RenderTrail(hit.point));
                    if (hit.collider.gameObject.tag == "Zombie")
                    {
                        Zombie zombie = hit.collider.gameObject.GetComponent<Zombie>();
                        zombie.TakeDamage(damage);
                    }
                } else StartCoroutine(RenderTrail(transform.position + (muzzleFlash.transform.TransformDirection(Vector3.forward)) * 50));
                if (currentAmmo <= 0) StartReload();
            }
        }
    }

    //Initiates when player presses left click
    public abstract void StartFiring();

    //Initiates when player releases left click
    public abstract void StopFiring();

    //Starts reloading, after a delay, reload gun
    public void StartReload()
    {
        if (currentAmmo != maxCurrentAmmo && reservedAmmo > 0 && !isReloading)
        {
            isReloading = true;
            StartCoroutine(ReloadAnimation());
        }
    }

    //Take from reserve ammo and fill up current ammo
    void Reload()
    {
        reservedAmmo = reservedAmmo - (maxCurrentAmmo - currentAmmo);
        currentAmmo = maxCurrentAmmo;
        if (reservedAmmo < 0)
        {
            currentAmmo += reservedAmmo;
            reservedAmmo = 0;
        }
        isReloading = false;
    }

    public void PlayGunSound()
    {
        gunSound.pitch = Random.Range(0.8f, 1.2f);
        gunSound.Play();
    }

    private IEnumerator ReloadAnimation()
    {
        Vector3 newReloadPosition = initialPosition + new Vector3(0f, -0.5f, 0f);
        float reloadDown = reloadTime * 0.33f;
        float reloadUp = reloadTime * 0.66f;

        // Rotate upwards
        float elapsedTime = 0f;
        while (elapsedTime < reloadDown)
        {
            transform.localPosition = Vector3.Slerp(initialPosition, newReloadPosition, elapsedTime / reloadDown);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = newReloadPosition;

        // Rotate back to initial position
        elapsedTime = 0f;
        while (elapsedTime < reloadUp)
        {
            transform.localPosition = Vector3.Slerp(newReloadPosition, initialPosition, elapsedTime / reloadUp);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = initialPosition;

        Reload();
    }

    public IEnumerator RenderTrail(Vector3 hitPoint)
    {
        float time = 0;
        TrailRenderer trail = Instantiate(trailRenderer, muzzleFlash.transform.position, Quaternion.identity);
        Vector3 startPosition = muzzleFlash.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hitPoint, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }
        trail.transform.position = hitPoint;
        Destroy(trail.gameObject, trail.time);
    }
}
