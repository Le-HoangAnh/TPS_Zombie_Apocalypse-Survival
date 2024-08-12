using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieZone : MonoBehaviour
{
    [Header("ZombieSpawn Var")]
    public GameObject dangerZone;
    private float repeatCycle = 1f;


    [Header("Sound")]
    public AudioClip dangerZoneSound;
    public AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            InvokeRepeating("EnemySpawner", 1f, repeatCycle);
            audioSource.PlayOneShot(dangerZoneSound);
            StartCoroutine(dangerZoneTimer());
            Destroy(gameObject, 10f);
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    void EnemySpawner()
    {
        Instantiate(dangerZone, dangerZone.transform);
    }

    IEnumerator dangerZoneTimer()
    {
        dangerZone.SetActive(true);
        yield return new WaitForSeconds(5f);
        dangerZone.SetActive(false);
    }
}
