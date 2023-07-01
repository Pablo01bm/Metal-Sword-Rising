using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FinishGameMenu : MonoBehaviour
{
    public void Setup()
    {
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None; //We make cursor disappears
        Cursor.visible = true;
    }

    public void ExitButtom()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
