using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI.Scripts
{
    public class FadingManager : MonoBehaviour
    {
        [Header("Main UI Element")]
        [SerializeField] private CanvasGroup canvasGroup;

        private Tween fadingTextTween;
       
        public void HidePanel(GameObject panel, float tweenDuration)
        {
            canvasGroup.DOFade(0f, tweenDuration).OnComplete(() => DeactivatePanel(panel));
        }
        
        public void ShowPanel(GameObject panel, float tweenDuration)
        {
            canvasGroup.DOFade(1f, tweenDuration).OnComplete(() => ActivatePanel(panel));
        }
        
        public void SetFadingText(TextMeshProUGUI text, float tweenDuration)
        {
            fadingTextTween = text.DOFade(0.0f, tweenDuration).SetEase(Ease.InSine).SetLoops(-1, LoopType.Yoyo);
        }

        private void DeactivatePanel(GameObject panel)
        {
            panel.SetActive(false);
            fadingTextTween.Pause();
        }
        
        private void ActivatePanel(GameObject panel)
        {
            panel.SetActive(true);
            fadingTextTween.Play();
        }
    }
}
