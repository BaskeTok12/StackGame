using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class VolumeSlider : MonoBehaviour
{
    private SoundManager _soundManager;

    [Inject]
    private void Construct(SoundManager soundManager)
    {
        _soundManager = soundManager ? soundManager : throw new ArgumentNullException(nameof(soundManager));
    }
    [SerializeField] private Slider slider;
    void Start()
    {
        slider.onValueChanged.AddListener(value => _soundManager.ChangeMasterVolume(value));
    }

}
