using Game_Manager;
using TMPro;
using UnityEngine;

namespace UI.Scripts
{
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
        private void OnEnable()
        {
            GameManager.OnScoreIncreased += UpdateScore;

            GameManager.OnRestart += ResetScore;
            GameManager.OnBestScoreIncreased += UpdateBestScore;
        }

        private void OnDisable()
        {
            GameManager.OnScoreIncreased -= UpdateScore;

            GameManager.OnRestart -= ResetScore;
            GameManager.OnBestScoreIncreased -= UpdateBestScore;
        }

        private void UpdateScore()
        {
            scoreText.text = gameManager.Scores.ToString();
        }

        private void UpdateBestScore()
        {
            bestScoreText.text = gameManager.BestScore.ToString();
        }

        private void ResetScore()
        {
            scoreText.text = string.Empty;
        }
    }
}