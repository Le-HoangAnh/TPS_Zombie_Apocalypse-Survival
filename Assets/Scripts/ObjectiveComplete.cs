using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveComplete : MonoBehaviour
{
    [Header("Objectives to Complete")]
    public Text objective1;
    public Text objective2;
    public Text objective3;

    [Header("Mission Completed")]
    public bool obj1 = false;
    public bool obj2 = false;
    public bool obj3 = false;

    public void GetObjectivesDone(bool obj1, bool obj2, bool obj3)
    {
        if (obj1 == true)
        {
            objective1.text = "1. Completed";
            objective1.color = Color.green;
        }
        else
        {
            objective1.text = "01. Find the rifle";
            objective1.color = Color.white;
        }

        if (obj2 == true)
        {
            objective2.text = "2. Completed";
            objective2.color = Color.green;
        }
        else
        {
            objective2.text = "2. Find the vehicle";
            objective2.color = Color.white;
        }

        if (obj3 == true)
        {
            objective3.text = "3. Completed";
            objective3.color = Color.green;
        }
        else
        {
            objective3.text = "3. Locate the villagers";
            objective3.color = Color.white;
        }
    }
}
