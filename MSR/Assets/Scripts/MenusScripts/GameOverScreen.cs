using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public void Setup()
    { 
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None; //We make cursor disappears
        Cursor.visible = true;
    }

    public void RestartButtom() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitButtom()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); 
    }
}
