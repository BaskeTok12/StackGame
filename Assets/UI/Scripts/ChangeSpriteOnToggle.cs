using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class ChangeSpriteOnToggle : MonoBehaviour
    {
        [SerializeField] private Sprite firstSprite;
        [SerializeField] private Sprite secondSprite;
        [SerializeField] private Image image;
        
        private Toggle _toggle;

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
            _toggle.onValueChanged.AddListener(ChangeSprite);
        }
        
        private void ChangeSprite(bool isEnabled)
        {
            if (isEnabled)
            {
                image.sprite = firstSprite;
                return;
            }
            image.sprite = secondSprite;
        }
    }
}