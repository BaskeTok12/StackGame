using System;
using Controllers;
using DG.Tweening;
using Game_Manager;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraManager : MonoBehaviour
{
    public static Action OnCameraReset;
    
    private Camera _camera;
    private Vector3 startPosition;
    
    [SerializeField] private float MissCameraSise = 10f; //const
    [SerializeField] private float InGameCameraSise = 6f; //const
    
    [SerializeField] private float changesDuration;
    [FormerlySerializedAs("movingCubeController")] [SerializeField] private CubeController cubeController;
    [SerializeField] private MeshRenderer fogMaterial;
    private void Awake()
    {
        _camera = Camera.main;
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        GameManager.OnStart += () => SetCameraSize(InGameCameraSise);
        CubeController.OnMiss += () => SetCameraSize(MissCameraSise); 
        GameManager.OnRestart += ResetCamera;
        
        CubeController.OnBlockPlaced += RaiseCameraPosition;
    }

    private void OnDisable()
    {
        GameManager.OnStart -= () => SetCameraSize(InGameCameraSise);
        CubeController.OnMiss -= () => SetCameraSize(MissCameraSise); 
        GameManager.OnRestart -= ResetCamera;
        
        CubeController.OnBlockPlaced -= RaiseCameraPosition;
    }

    private void RaiseCameraPosition()
    {
        transform.DOMoveY(transform.position.y + cubeController.transform.localScale.y, changesDuration);
    }

    private void ResetCamera()
    {
        transform.DOMoveY(startPosition.y, changesDuration);
        SetCameraSize(InGameCameraSise);
        
        OnCameraReset.Invoke();
    }
    private void SetCameraSize(float size)
    {
        _camera.DOOrthoSize(size, changesDuration);
    }
}
