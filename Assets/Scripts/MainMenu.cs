using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject selectCharacter;
    public GameObject mainMenu;

    public void OnPlayButton()
    {
        selectCharacter.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void OnQuitButton()
    {
        Debug.Log("Quitting Game....");
        Application.Quit();
    }
}
