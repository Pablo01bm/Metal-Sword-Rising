using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributesControler : MonoBehaviour
{
    public float playerDamage;
    public float playerDamageMultiplier = 1;
    public float playerHealth;
    public float playerSpeed = 1;
    public float meleeEnemyDamage = 1f;
    public float meleeEnemyDamageMultiplier = 1f;
    public GameObject GameOverUI;
    public GameOverScreen GameOverScreen;
    public GameObject FinishGameUI;
    public FinishGameMenu FinishScreen;

    public int score = 0;
    public int highscore;
    private bool gameFinished = false;


    private void Start()
    {
        Invoke("DelayedStart", 0.3f);

        highscore = PlayerPrefs.GetInt("highscore", 0);
    }

    private void Update()
    {
        if (gameFinished)
        {
            Time.timeScale = 0;
        }
    }

    private void DelayedStart()
    {
        GameOverUI = GameObject.Find("Background");
        GameOverScreen = GameOverUI.GetComponent<GameOverScreen>();
        GameOverUI.SetActive(false);

        FinishGameUI = GameObject.Find("Background2");
        FinishScreen = FinishGameUI.GetComponent<FinishGameMenu>();
        FinishGameUI.SetActive(false);

    }

    public void damagePowerUp()
    {
        playerDamageMultiplier = playerDamageMultiplier + 0.1f;
        playerDamage = 10f * playerDamageMultiplier;
        Debug.Log("Current Player Damage: " + playerDamage);
    }

    public void healPowerUp()
    {
        playerHealth = playerHealth + 20;
        if (playerHealth > 100)
        {
            playerHealth = 100;
        }
        Debug.Log("Current Player Health: " + playerHealth);
    }

    public void AddPoint()
    {
        print("CURRENT SCORE: " + score);
        score += 100;
        if (highscore < score)
        {
            PlayerPrefs.SetInt("highscore", score);
        }
    }

    public void GameOver () 
    {
        GameOverScreen.Setup();
    }

    public void EndGame()
    {
        gameFinished = true;
        FinishScreen.Setup();

    }



}
