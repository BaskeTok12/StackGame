using UnityEngine;

public class GetInvertedPosition : MonoBehaviour
{
    public Transform StartPosition;

    private void Start()
    {
        transform.position = new Vector3(-StartPosition.position.x, -5f, -StartPosition.position.z);
    }
}
