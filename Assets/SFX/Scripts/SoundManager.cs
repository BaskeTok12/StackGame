using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SoundManager : MonoBehaviour
{
    [FormerlySerializedAs("_musicSource")] [SerializeField] private AudioSource musicSource;
    [FormerlySerializedAs("_vfxSource")] [SerializeField] private AudioSource vfxSource;
    [FormerlySerializedAs("_stackSound")] public AudioClip stackSound;
    public List<AudioClip> PerfectStackSounds { get; private set; }

    public void PlaySound(AudioClip audioClip)
    {
        vfxSource.PlayOneShot(audioClip);
    }

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }
}
