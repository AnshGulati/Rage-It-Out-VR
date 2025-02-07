﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class SimpleShoot : MonoBehaviour
{
    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location Refrences")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;

    public AudioSource source;
    public AudioClip fireSound;
    public AudioClip reload;
    public AudioClip noAmmo;
    public Magazine magazine;
    public XRBaseInteractor socketInteractor;
    private bool hasSlide = true;

    public void AddMagazine(XRBaseInteractable interactable)
    {
        magazine = interactable.GetComponent<Magazine>();
        source.PlayOneShot(reload);
        hasSlide = false;
    }

    public void RemoveMagazine(XRBaseInteractable interactable)
    {
        magazine = null;
        source.PlayOneShot(reload);
    }

    private void OnMagazineInserted(SelectEnterEventArgs args)
    {
        AddMagazine(args.interactableObject.transform.GetComponent<XRBaseInteractable>());
    }

    private void EjectMagazine()
    {
        if (magazine == null) return;

        // Detach the magazine
        XRGrabInteractable magazineInteractable = magazine.GetComponent<XRGrabInteractable>();
        if (magazineInteractable != null)
        {
            magazineInteractable.interactionLayers = InteractionLayerMask.GetMask("Magazine"); // Ensure this layer is set in the project
        }

        Rigidbody magazineRigidbody = magazine.GetComponent<Rigidbody>();
        if (magazineRigidbody != null)
        {
            magazineRigidbody.isKinematic = false; // Enable physics
            magazineRigidbody.AddForce(Vector3.down * 2f, ForceMode.Impulse); // Add a downward force
        }

        // Clear the magazine reference
        magazine = null;

        // Reset the socket interactor
        if (socketInteractor is XRSocketInteractor socket)
        {
            socket.interactablesSelected.Clear();
            socket.allowSelect = true; // Allow selecting a new magazine
        }

        // Play the reload sound
        source.PlayOneShot(reload);
    }


    private void OnDestroy()
    {
        // Remove the listener to avoid memory leaks
        socketInteractor.selectEntered.RemoveListener(OnMagazineInserted);
    }

    public void Slide()
    {
        hasSlide = true;
        source.PlayOneShot(reload);
    }
    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();

        socketInteractor.selectEntered.AddListener(OnMagazineInserted);
    }

    public void PullTheTrigger()
    {
        if (magazine && magazine.numberOfBullet > 0 && hasSlide)
        {
            gunAnimator.SetTrigger("Fire");
        }
        else
        {
            source.PlayOneShot(noAmmo);
        }
    }


    //This function creates the bullet behavior
    void Shoot()
    {
        magazine.numberOfBullet--;
        source.PlayOneShot(fireSound);

        if (magazine.numberOfBullet == 0)
        {
            // Eject the magazine
            EjectMagazine();
        }

        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }

        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }

        // Create a bullet and add force on it in direction of the barrel
        Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation).GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);

    }

    //This function creates a casing at the ejection slot
    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }
}
