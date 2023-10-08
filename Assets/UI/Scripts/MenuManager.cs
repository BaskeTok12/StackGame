using System.Collections;
using Controllers;
using TMPro;
using UnityEngine;

namespace UI.Scripts
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;

        [Header("Panels")]
        [SerializeField] private GameObject startGamePanel;
        [SerializeField] private GameObject finalGamePanel;

        [Header("UI Text")]
        [SerializeField] private TextMeshProUGUI tapToPlayText;
        
        [Header("Fading")]
        [SerializeField] private FadingManager fadingManager;
        [SerializeField] private float tweenDuration;
        [SerializeField] private float restartLatency;

        private void Awake()
        {
            fadingManager.SetFadingText(tapToPlayText, tweenDuration);
        }
        private void OnEnable()
        {
            //CameraManager.OnCameraReset += () => { startGamePanel.SetActive(true); fadingManager.SetFadingText(tapToPlayText, tweenDuration); };
            GameManager.OnRestart += () => fadingManager.ShowPanel(startGamePanel, tweenDuration);
            MovingCubeController.OnMiss += () => fadingManager.ShowPanel(finalGamePanel, tweenDuration);
        }
        private void OnDisable()
        {
            //CameraManager.OnCameraReset -= () => { startGamePanel.SetActive(true); fadingManager.SetFadingText(tapToPlayText, tweenDuration); };
            GameManager.OnRestart -= () => fadingManager.ShowPanel(startGamePanel, tweenDuration);
            MovingCubeController.OnMiss -= () => fadingManager.ShowPanel(finalGamePanel, tweenDuration);
        }

        public void StartGame()
        {
            StartCoroutine(FadeToPlaymode());
        }
        
        public void RestartGame()
        {
            StartCoroutine(FadeToRestart());
        }

        private IEnumerator FadeToPlaymode()
        {
            fadingManager.HidePanel(startGamePanel, tweenDuration);

            yield return new WaitForSeconds(tweenDuration);
            
            gameManager.StartGame();
        }
        
        private IEnumerator FadeToRestart()
        {
            fadingManager.HidePanel(finalGamePanel, restartLatency);

            yield return new WaitForSeconds(restartLatency);
            
            gameManager.RestartGame();
            
            yield return new WaitForSeconds(restartLatency);
            
            
            
        }
    }
}
