using TMPro;
using UI.Scripts;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private FadingManager fadingManager;
    
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    
    [Header("Tween duration")] 
    [SerializeField] private float tweenDuration;

    private void Awake()
    {
        UpdateBestScore();
    }

    private void OnEnable()
    {
        GameManager.OnScoreIncreased += UpdateScore;

        GameManager.OnStart -= ActivateScore;
        GameManager.OnRestart += DeactivateScore;
        GameManager.OnRestart += UpdateScore;
        GameManager.OnRestart += UpdateBestScore;
    }

    private void OnDisable()
    {
        GameManager.OnScoreIncreased -= UpdateScore;

        GameManager.OnStart -= ActivateScore;
        GameManager.OnRestart -= DeactivateScore;
        GameManager.OnRestart -= UpdateScore;
        GameManager.OnRestart -= UpdateBestScore;
    }
    
    private void ActivateScore()
    {
        scoreText.gameObject.SetActive(true);
    }
    
    private void DeactivateScore()
    {
        scoreText.gameObject.SetActive(false);
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