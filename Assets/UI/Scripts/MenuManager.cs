using System;
using System.Collections;
using Block_Controller.Scripts;
using Game_Manager;
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
        [SerializeField] private GameObject scoreObject;
        [SerializeField] private GameObject bestScoreObject;

        [Header("Settings")]
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private TwoPointTransition settingPointTransitions;

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
            GameManager.OnRestart += GameManager_OnRestart;
            CubeController.OnMiss += CubeController_OnMiss;
        }
        
        private void OnDisable()
        {
            GameManager.OnRestart -= GameManager_OnRestart;
            CubeController.OnMiss -= CubeController_OnMiss;
        }
        
        private void GameManager_OnRestart()
        {
            ShowStartPanel();
        }
        
        private void CubeController_OnMiss()
        {
            ShowFinalPanel();
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
            settingPointTransitions.ToTransition();
            OnButtonClick?.Invoke();
        }
        
        public void CLoseSettings()
        {
            settingPointTransitions.FromTransition();
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
