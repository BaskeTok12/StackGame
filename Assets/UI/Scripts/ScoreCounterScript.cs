using System;
using Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ScoreCounterScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    private void Awake()
    {
        UpdateBestScore();
    }

    private void OnEnable()
    {
        GameManager.OnScoreIncreased += UpdateScore;
        
        GameManager.OnRestart += UpdateScore;
        GameManager.OnRestart += UpdateBestScore;
    }

    private void OnDisable()
    {
        GameManager.OnScoreIncreased -= UpdateScore;
        
        GameManager.OnRestart -= UpdateScore;
        GameManager.OnRestart -= UpdateBestScore;
    }

    private void UpdateScore()
    {
        scoreText.text = gameManager.Scores.ToString();
    }

    private void UpdateBestScore()
    {
        bestScoreText.text = gameManager.BestScore.ToString();
    }
}

