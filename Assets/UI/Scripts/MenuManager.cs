using System;
using System.Collections;
using Controllers;
using Game_Manager;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Scripts
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;

        [Header("Panels")]
        [SerializeField] private GameObject startGamePanel;
        [SerializeField] private GameObject finalGamePanel;
        [SerializeField] private GameObject scoreObject;
        [SerializeField] private GameObject bestScoreObject;

        [SerializeField] private GameObject settingsPanel;

        [Header("UI Text")]
        [SerializeField] private TextMeshProUGUI tapToPlayText;
        
        [Header("Fading")]
        [SerializeField] private FadingManager fadingManager;
        [SerializeField] private float tweenDuration;
        [SerializeField] private float restartLatency;
        [SerializeField] private float uiTransitionsLatency;

        public static event Action OnButtonClick;

        private void Awake()
        {
            fadingManager.SetFadingText(tapToPlayText, tweenDuration);
        }
        private void OnEnable()
        {
            GameManager.OnRestart += ShowStartPanel;
            CubeController.OnMiss += ShowFinalPanel;
        }
        
        private void OnDisable()
        {
            GameManager.OnRestart -= ShowStartPanel;
            CubeController.OnMiss -= ShowFinalPanel;
        }

        public void StartGame()
        {
            StartCoroutine(FadeToPlaymode());
        }
        
        public void RestartGame()
        {
            StartCoroutine(FadeToRestart());
        }

        private void ShowStartPanel()
        {
            fadingManager.ShowPanel(startGamePanel, tweenDuration);
        }

        private void ShowFinalPanel()
        {
            fadingManager.ShowPanel(finalGamePanel, tweenDuration);
        }

        public void ShowSettings()
        {
            fadingManager.ShowPanel(settingsPanel, uiTransitionsLatency);
            OnButtonClick?.Invoke();
        }
        
        public void CLoseSettings()
        {
            fadingManager.HidePanel(settingsPanel, uiTransitionsLatency);
            OnButtonClick?.Invoke();
        }
        
        private IEnumerator FadeToPlaymode()
        {
            fadingManager.HidePanel(startGamePanel, tweenDuration);

            yield return new WaitForSeconds(tweenDuration);
            
            gameManager.StartGame();
            fadingManager.ShowPanel(scoreObject, tweenDuration);
        }
        
        private IEnumerator FadeToRestart()
        {
            fadingManager.HidePanel(finalGamePanel, restartLatency);

            yield return new WaitForSeconds(restartLatency);
            
            gameManager.RestartGame();
            fadingManager.HidePanel(scoreObject, restartLatency);
            yield return new WaitForSeconds(restartLatency);
            
        }
    }
}
