using UnityEngine;
using UnityEngine.Audio;
using Toggle = UnityEngine.UI.Toggle;

namespace UI.Scripts
{
    public class ToggleVolumeButton : MonoBehaviour
    {
        [Header("Mixer")] 
        [SerializeField] private AudioMixer mainMixer;

        private Toggle _toggle;

        private const string MasterVolume = "MasterVolume";

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
        }

        private void OnEnable()
        {
            _toggle.onValueChanged.AddListener(ToggleValueChanged);
        }

        private void OnDisable()
        {
            _toggle.onValueChanged.RemoveListener(ToggleValueChanged);
        }

        private void ToggleValueChanged(bool isOn)
        {
            if (isOn)
                mainMixer.SetFloat(MasterVolume, 0);
            else
                mainMixer.SetFloat(MasterVolume, -80);
        }
    }
}