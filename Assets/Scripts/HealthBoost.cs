using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoost : MonoBehaviour
{
    [Header("HealthBoost")]
    public PlayerScript player;
    private float healthToGive = 100f;
    private float radius = 2.5f;

    [Header("Sounds")]
    public AudioClip healthBoostSound;
    public AudioSource audioSource;

    [Header("HealthBox Animator")]
    public Animator animator;

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < radius)
        {
            if (Input.GetKeyUp("f"))
            {
                animator.SetBool("Open", true);
                player.presentHealth = healthToGive;

                //sound effect
                audioSource.PlayOneShot(healthBoostSound);

                Object.Destroy(gameObject, 0.5f);
            }
        }
    }
}
