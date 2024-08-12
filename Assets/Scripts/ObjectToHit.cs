using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToHit : MonoBehaviour
{
    public float objectHealth = 100f;

    public void ObjectHitDamame(float amount)
    {
        objectHealth -= amount;
        if (objectHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
