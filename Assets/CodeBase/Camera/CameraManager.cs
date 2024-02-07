using System;
using Block_Controller.Scripts;
using DG.Tweening;
using Game_Manager;
using UnityEngine;
using UnityEngine.Serialization;

namespace Camera
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private float missCameraSize = 10f;
        [SerializeField] private float inGameCameraSize = 6f;
    
        [SerializeField] private float changesDuration;
        [SerializeField] private CubeController cubeController;
        
        public static Action OnCameraReset;
    
        private UnityEngine.Camera _camera;
        private Vector3 _startPosition;
        
        private void Awake()
        {
            _camera = UnityEngine.Camera.main;
            _startPosition = transform.position;
        }

        private void OnEnable()
        {
            GameManager.OnStart += GameManager_OnStart;
            CubeController.OnMiss += CubeController_OnMiss; 
            GameManager.OnRestart += ResetCamera;
            CubeController.OnBlockPlaced += RaiseCameraPosition;
        }

        private void OnDisable()
        {
            GameManager.OnStart -= GameManager_OnStart;
            CubeController.OnMiss -= CubeController_OnMiss; 
            GameManager.OnRestart -= ResetCamera;
            CubeController.OnBlockPlaced -= RaiseCameraPosition;
        }

        private void CubeController_OnMiss()
        {
            SetCameraSize(missCameraSize);
        }

        private void GameManager_OnStart()
        {
            SetCameraSize(inGameCameraSize);
        }
        
        private void RaiseCameraPosition()
        {
            transform.DOMoveY(transform.position.y + cubeController.transform.localScale.y, changesDuration);
        }

        private void ResetCamera()
        {
            transform.DOMoveY(_startPosition.y, changesDuration);
            SetCameraSize(inGameCameraSize);
        
            OnCameraReset.Invoke();
        }
        private void SetCameraSize(float size)
        {
            _camera.DOOrthoSize(size, changesDuration);
        }
    }
}
