using System;
using Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ScoreCounterScript : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void OnEnable()
    {
       
    }

    private void OnDisable()
    {
        
        
    }

    private void UpdateScore()
    {
        scoreText.text = gameManager.Scores.ToString();
    }
    
}

