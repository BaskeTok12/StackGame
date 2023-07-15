using System;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class PlayStartSound : MonoBehaviour
{
    private SoundManager _soundManager;
    [SerializeField] private AudioClip clip;
    [Inject]
    private void Construct(SoundManager soundManager)
    {
        _soundManager = soundManager ? soundManager : throw new ArgumentNullException(nameof(soundManager));
    }
    private void Start()
    {
        _soundManager.PlaySound(clip);
    }
}
