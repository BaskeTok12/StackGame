using UnityEngine;

namespace UI.Scripts
{
    public class TwoPointTransition : MonoBehaviour
    {
        [SerializeField] private RectTransform transformUI;
        [SerializeField] private Transform firstPoint;
        [SerializeField] private Transform secondPoint;
        [SerializeField] private bool onStartTransition;
        [SerializeField] private float duration = 1f;
        
        private void Start()
        {
            if (onStartTransition)
                ToTransition();
        }

        public void ToTransition()
        {
            Transition.MakeTransitionFromTo(transformUI, firstPoint.position, secondPoint.position, duration);
        }

        public void FromTransition()
        {
            Transition.MakeTransitionFromTo(transformUI, secondPoint.position, firstPoint.position, duration);
        }
    }
}
