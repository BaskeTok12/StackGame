using UnityEngine;

public class GetStartColor : MonoBehaviour
{
    public GameObject StartCube;
    void Start()
    {
        GetComponent<Renderer>().material.color = StartCube.GetComponent<Renderer>().material.color;
    }
}
