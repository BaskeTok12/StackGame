using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject startGamePanel;
    [SerializeField] private GameObject finalGamePanel;

    [SerializeField] private TextMeshProUGUI tapToPlayText;
    
    [SerializeField] private float tweenDuration;

    private Action _onFadeInEnded;
    private Action _onFadeOutEnded;

    private Sequence _tweenSequence;

    private void Awake()
    {
        AppendTweensToSequence();
    }

    private void OnEnable()
    {
        CameraManager.OnCameraReset += () => { startGamePanel.SetActive(true); StartTweensSequence(); };
        GameManager.OnMiss += () => finalGamePanel.SetActive(true);
    }
    private void OnDisable()
    {
        CameraManager.OnCameraReset -= () => { startGamePanel.SetActive(true); StartTweensSequence(); };
        GameManager.OnMiss -= () => finalGamePanel.SetActive(true);
    }

    private void AppendTweensToSequence()
    {
        _tweenSequence = DOTween.Sequence();
        
        _tweenSequence.Append(tapToPlayText.DOFade(0, tweenDuration));
        _tweenSequence.Append(tapToPlayText.DOFade(1, tweenDuration));

        _tweenSequence.SetLoops(-1);
    }

    private void StartTweensSequence()
    {
        _tweenSequence?.Play();
    }

    private void StopTweensSequence()
    {
        _tweenSequence?.Kill();
    }
}
