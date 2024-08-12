using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour //rifle: sung truong
{
    [Header("Rifle Things")]
    public Camera cam;
    private float giveDamageOf = 10f;
    public float shootingRange = 100f; //pham vi ngam ban
    public float fireCharge = 15f;
    private float nextTimeToShoot = 0f;
    public Animator animator;
    public PlayerScript player;
    public Transform hand;
    public GameObject rifleUI;

    [Header("Rifle Ammunition and shooting")] 
    private int maximumAmmunition = 30;   //Ammunition: đạn dược
    public int mag = 10;                  //Magazine(mag): kho đạn dược
    private int presentAmmunition;
    public float reloadingTime = 0.5f;
    private bool setReloading = false;

    [Header("Rifle effects")]
    public ParticleSystem muzzleSpark;
    public GameObject WoodedEffect;
    public GameObject goreEffect;

    [Header("Sounds and UI")]
    public GameObject AmmoOutUI;
    public AudioClip shootingSound;
    public AudioClip reloadingSound;
    public AudioSource audioSource;

    private void Awake()
    {
        transform.SetParent(hand);
        rifleUI.SetActive(true);
        presentAmmunition = maximumAmmunition;
    }

    private void Update()
    {
        if (setReloading) return;

        if (presentAmmunition <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToShoot)
        {
            animator.SetBool("Fire", true);
            animator.SetBool("Idle", false);
            nextTimeToShoot = Time.time + 1f / fireCharge;
            Shoot();
        }
        else if (Input.GetButton("Fire1") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetBool("Idle", false);
            animator.SetBool("FireWalk", true);
        }
        else if (Input.GetButton("Fire2") && Input.GetButton("Fire1"))
        {
            animator.SetBool("Idle", false);
            animator.SetBool("IdleAim", true);
            animator.SetBool("FireWalk", true);
            animator.SetBool("Walk", true);
            animator.SetBool("Reloading", false);
        }
        else
        {
            animator.SetBool("Fire", false);
            animator.SetBool("Idle", true);
            animator.SetBool("FireWalk", false);
        }
    }

    private void Shoot()
    {
        //check for mag
        if (mag == 0)
        {
            //show ammo out text
            StartCoroutine(ShowAmmoOut());
            return;
        }
        presentAmmunition--;

        if (presentAmmunition == 0)
        {
            mag--;
        }
        //updating the UI
        AmmoCourt.occurrence.UpdateAmmoText(presentAmmunition);
        AmmoCourt.occurrence.UpdateMagText(mag);

        muzzleSpark.Play();
        audioSource.PlayOneShot(shootingSound);
        RaycastHit hitInfo;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, shootingRange))
        {
            Debug.Log(hitInfo.transform.name);

            ObjectToHit objectToHit = hitInfo.transform.GetComponent<ObjectToHit>();
            Zombie1 zombie1 = hitInfo.transform.GetComponent<Zombie1>();
            Zombie2 zombie2 = hitInfo.transform.GetComponent<Zombie2>();


            if (objectToHit != null)
            {
                objectToHit.ObjectHitDamame(giveDamageOf);
                GameObject WoodGo = Instantiate(WoodedEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(WoodGo, 1f);
            }
            else if (zombie1 != null)
            {
                zombie1.zombieHitDamage(giveDamageOf);
                GameObject goreEffectGo = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(goreEffectGo, 1f);
            }
            else if (zombie2 != null)
            {
                zombie2.zombieHitDamage(giveDamageOf);
                GameObject goreEffectGo = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(goreEffectGo, 1f);
            }
        }
    }

    IEnumerator Reload()
    {
        player.playerSpeed = 0f;
        player.playerSprint = 0f;
        setReloading = true;
        Debug.Log("Reloading...");
        animator.SetBool("Reloading", true);
        audioSource.PlayOneShot(reloadingSound);
        yield return new WaitForSeconds(reloadingTime);
        animator.SetBool("Reloading", false);
        presentAmmunition = maximumAmmunition;
        player.playerSpeed = 1.9f;
        player.playerSprint = 3f;
        setReloading = false;
    }

    IEnumerator ShowAmmoOut()
    {
        AmmoOutUI.SetActive(true);
        yield return new WaitForSeconds(5f);
        AmmoOutUI.SetActive(false);
    }
}
