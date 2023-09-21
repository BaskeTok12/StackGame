using UnityEngine;

    public class ColorController : MonoBehaviour
    {
        public static Color GetRandomColor()
        {
            return new Color(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f),UnityEngine.Random.Range(0, 1f));
        }
    }
