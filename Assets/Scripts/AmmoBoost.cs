using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBoost : MonoBehaviour
{
    [Header("AmmoBoost")]
    public Rifle rifle;
    private int magToGive = 10;
    private float radius = 2.5f;

    [Header("Sounds")]
    public AudioClip ammoBoostSound;
    public AudioSource audioSource;

    [Header("HealthBox Animator")]
    public Animator animator;

    private void Update()
    {
        if (Vector3.Distance(transform.position, rifle.transform.position) < radius)
        {
            if (Input.GetKeyUp("f"))
            {
                animator.SetBool("Open", true);
                rifle.mag = magToGive;

                //sound effect
                audioSource.PlayOneShot(ammoBoostSound);

                Object.Destroy(gameObject, 0.5f);
            }
        }
    }
}
