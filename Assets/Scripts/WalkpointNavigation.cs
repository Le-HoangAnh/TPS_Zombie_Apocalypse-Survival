using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkpointNavigation : MonoBehaviour
{
    [Header("NPC Zombie")]  //NPC: Non-Player Character
    public Zombie1 zombie1;
    public WalkPoint currentWalkpoint;

    private void Awake()
    {
        zombie1 = GetComponent<Zombie1>();
    }

    private void Start()
    {
        zombie1.locateDestination(currentWalkpoint.GetPosition());
    }

    private void Update()
    {
        if (zombie1.destinationReached)
        {
            currentWalkpoint = currentWalkpoint.nextWalkPoint;
            zombie1.locateDestination(currentWalkpoint.GetPosition());
        }
    }
}
