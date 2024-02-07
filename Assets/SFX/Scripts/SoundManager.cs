using CodeBase.Block_Controller;
using CodeBase.Game_Manager;
using UI.Scripts;
using UnityEngine;
using UnityEngine.Audio;

namespace SFX.Scripts
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        
        [Header("Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource vfxSource;
        
        [Header("Sounds")] 
        [SerializeField] private AudioClip uiButtonSound;
        [SerializeField] private AudioClip startSound; 
        [SerializeField] private AudioClip stackSound;
        [SerializeField] private AudioClip beginningSound;
        [SerializeField] private AudioClip endingSound;
        [SerializeField] private AudioClip[] perfectStackSounds;
        
        private void Start()
        {
            musicSource.PlayOneShot(startSound);
        }

        private void OnEnable()
        {
            GameManager.OnStart += PlayBeginningSound;
            GameManager.OnRestart += PlayEndingSound;
            CubeController.OnStack += PlayStackSound;
            CubeController.OnPerfectStack += PlayPerfectStackSound;
            MenuManager.OnButtonClick += PlayButtonSound;
        }

        private void OnDisable()
        {
            GameManager.OnStart -= PlayBeginningSound;
            GameManager.OnRestart -= PlayEndingSound;
            CubeController.OnStack -= PlayStackSound;
            CubeController.OnPerfectStack -= PlayPerfectStackSound;
            MenuManager.OnButtonClick -= PlayButtonSound;
        }

        private void PlayStackSound()
        {
            vfxSource.PlayOneShot(stackSound);
        }
        
        private void PlayBeginningSound()
        {
            musicSource.PlayOneShot(beginningSound);
        }
        
        private void PlayEndingSound()
        {
            musicSource.PlayOneShot(endingSound);
        }
        
        private void PlayPerfectStackSound()
        {
            if (gameManager.PerfectStacksCount >= perfectStackSounds.Length)
            {
                vfxSource.PlayOneShot(perfectStackSounds[^1]);
                return;
            }
            vfxSource.PlayOneShot(perfectStackSounds[gameManager.PerfectStacksCount]);
        }

        private void PlayButtonSound()
        {
            vfxSource.PlayOneShot(uiButtonSound);
        }
    }
}
