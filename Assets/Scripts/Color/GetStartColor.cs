using UnityEngine;

public class GetStartColor : MonoBehaviour
{
    public GameObject StartCube;
    private void Start()
    {
        GetComponent<Renderer>().material.color = StartCube.GetComponent<Renderer>().material.color;
    }
}
