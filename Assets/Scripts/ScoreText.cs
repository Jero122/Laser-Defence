using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    Text scoreText;
    int score;
    void Start()
    {
        scoreText = gameObject.GetComponent<Text>();
    }

    private void Update()
    {
        score = FindObjectOfType<GameSession>().GetScore();
        scoreText.text = score.ToString();
    }
}
