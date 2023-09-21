using System;
using Controllers;
using DG.Tweening;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Camera _camera;
    private Vector3 startPosition;

    private const float MissCameraSise = 10f;
    private const float InGameCameraSise = 6f;
    
    [SerializeField] private float changesDuration;
    [SerializeField] private MovingCubeController movingCubeController;
    private void Awake()
    {
        _camera = Camera.main;
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        GameManager.OnStart += () => SetCameraSize(InGameCameraSise);
        MovingCubeController.OnStack += () => RaiseCameraPosition();
        GameManager.OnMiss += () => SetCameraSize(MissCameraSise); //was OnRestart
        GameManager.OnRestart += () => ResetCamera(); 
    }

    private void OnDisable()
    {
        GameManager.OnStart -= () => SetCameraSize(InGameCameraSise);
        MovingCubeController.OnStack -= () => RaiseCameraPosition();
        GameManager.OnMiss -= () => SetCameraSize(MissCameraSise); //was OnRestart
        GameManager.OnRestart -= () => ResetCamera();
    }

    private void Start()
    {
        SetRandomBackground();
    }

    private void SetRandomBackground()
    {
        _camera.backgroundColor = ColorController.GetRandomColor();
    }

    private void RaiseCameraPosition()
    {
        transform.DOMoveY(transform.position.y + movingCubeController.transform.localScale.y, changesDuration);
    }

    private void ResetCamera()
    {
        _camera.transform.position = startPosition;
        SetCameraSize(InGameCameraSise);
    }
    private void SetCameraSize(float size)
    {
        _camera.DOOrthoSize(size, changesDuration);
    }
}
