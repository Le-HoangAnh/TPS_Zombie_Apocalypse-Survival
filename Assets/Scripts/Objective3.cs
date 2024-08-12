using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Objective3 : MonoBehaviour
{
    public ObjectiveComplete mission;
    public Menu menu;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            mission.obj3 = true;
            //mission.GetObjectivesDone(true, true, true);
            if (mission.obj1 == true && mission.obj2 == true && mission.obj3 == true)
            {
                mission.GetObjectivesDone(true, true, true);

                menu.winMenuUI.SetActive(true);
                Time.timeScale = 5.0f;
                SceneManager.LoadScene("MainMenu");
            }
            //complete objective
            //mission.obj3 = true;
            //mission.GetObjectivesDone(true, true, true);

            ////Destroy(gameObject, 2f);
            //menu.winMenuUI.SetActive(true);
            //Time.timeScale = 5.0f;
            //SceneManager.LoadScene("MainMenu");
        }
    }
}
