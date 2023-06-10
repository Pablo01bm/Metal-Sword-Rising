using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText;
    private AttributesControler atributesScript;

    int score;
    int highscore ;

    // Start is called before the first frame update
    void Start()
    {
        GameObject aux = GameObject.Find("GameManager");
        atributesScript = aux.GetComponent<AttributesControler>();
        score = atributesScript.score;
        highscore = atributesScript.highscore; 
        scoreText.text = score.ToString() + " POINTS";
        highscoreText.text = "HIGHSCORE: " + highscore.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        score = atributesScript.score;
        scoreText.text = score.ToString() + " POINTS";

        highscore = atributesScript.highscore;
        highscoreText.text = "HIGHSCORE: " + highscore.ToString();
    }
}
