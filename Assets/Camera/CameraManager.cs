using System;
using Controllers;
using DG.Tweening;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static Action OnCameraReset;
    
    private Camera _camera;
    private Vector3 startPosition;
    
    [SerializeField] private float MissCameraSise = 10f; //const
    [SerializeField] private float InGameCameraSise = 6f; //const
    
    [SerializeField] private float changesDuration;
    [SerializeField] private MovingCubeController movingCubeController;
    [SerializeField] private MeshRenderer fogMaterial;
    private void Awake()
    {
        _camera = Camera.main;
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        GameManager.OnStart += () => SetCameraSize(InGameCameraSise);
        GameManager.OnMiss += () => SetCameraSize(MissCameraSise); 
        GameManager.OnRestart += ResetCamera;
        
        MovingCubeController.OnStack += RaiseCameraPosition;
    }

    private void OnDisable()
    {
        GameManager.OnStart -= () => SetCameraSize(InGameCameraSise);
        GameManager.OnMiss -= () => SetCameraSize(MissCameraSise); 
        GameManager.OnRestart -= ResetCamera;
        
        MovingCubeController.OnStack -= RaiseCameraPosition;
    }

    private void RaiseCameraPosition()
    {
        transform.DOMoveY(transform.position.y + movingCubeController.transform.localScale.y, changesDuration);
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
