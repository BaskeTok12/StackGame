using System;
using System.Linq;
using Block_Controller.Scripts;
using Camera;
using Input_Manager;
using SFX.Scripts;
using UnityEngine;
using Zenject;

namespace Game_Manager
{
    public class GameManager : MonoBehaviour
    {
        [Header("Start Cube")]
        [SerializeField] private MeshRenderer startCubeMaterial;
        [SerializeField] private MeshRenderer startTowerMaterial;

        [Header("Fog")] 
        [SerializeField] private MeshRenderer fogMaterial;

        [SerializeField] private UnityEngine.Camera mainCamera;
        
        public static event Action OnStart;
        public static event Action OnRestart;
        public static event Action OnClick;
        public static event Action OnScoreIncreased;
        public static event Action OnBestScoreIncreased;
    
        private InputManager _inputManager;
        private InputController.PlayerInteractionsActions _playerInteractions;
    
        private SoundManager _soundManager;

        private Material _stackMaterial;
        private Material _environmentMaterial;
        
        private static readonly int FogColor = Shader.PropertyToID("_FogColor");
        private const string BestScoreProp = "BestScore";
        
        public int Scores { private set; get; }
        public int BestScore { private set; get; }
        public int PerfectStacksCount { private set; get; }

        [Inject]
        private void Construct(InputManager inputManager)
        {
            _inputManager = inputManager ? inputManager : throw new ArgumentNullException(nameof(inputManager));
        }

        private void Awake()
        {
            SetSceneStyle();
            GetBestScore();
        }

        private void Start()
        {
            SetResolutionAndFrameRate();

            _playerInteractions = _inputManager.PlayerInteractions;
            _playerInteractions.Click.started += context => OnClick?.Invoke();
        }

        private void OnEnable()
        {
            CubeController.OnStack += IncreaseScore;
            CubeController.OnPerfectStack += IncreasePerfectStackCount;
            CubeController.OnSlice += ResetPerfectStackCount;
            CubeController.OnMiss += SetBestScore;
            CameraManager.OnCameraReset += SetSceneStyle;
        }
    
        private void OnDisable()
        {
            CubeController.OnStack -= IncreaseScore;
            CubeController.OnPerfectStack -= IncreasePerfectStackCount;
            CubeController.OnSlice -= ResetPerfectStackCount;
            CubeController.OnMiss -= SetBestScore;
            CameraManager.OnCameraReset -= SetSceneStyle;
        }

        public void StartGame()
        {
            OnStart?.Invoke();
        }

        public void RestartGame()
        {
            Scores = 0;
            PerfectStacksCount = 0;
            OnRestart?.Invoke();
        }

        private void SetSceneStyle()
        {
            _stackMaterial = ColorController.GetBaseMaterialWithRandomColor(startCubeMaterial.sharedMaterial);
            _environmentMaterial = ColorController.GetBaseMaterialWithRandomColor(startCubeMaterial.sharedMaterial);
        
            startCubeMaterial.material = _stackMaterial;
            startTowerMaterial.material = _stackMaterial;
        
            fogMaterial.material.SetColor(FogColor, _environmentMaterial.color);
            mainCamera.backgroundColor = _environmentMaterial.color;
        }

        #region Score

        private void IncreaseScore()
        {
            Scores += 1;
            OnScoreIncreased?.Invoke();
        }

        private void IncreasePerfectStackCount()
        {
            IncreaseScore();
            PerfectStacksCount += 1;
        }

        private void ResetPerfectStackCount()
        {
            PerfectStacksCount = 0;
        }

        private void SetBestScore()
        {
            if (Scores <= BestScore) return;
            BestScore = Scores;
            PlayerPrefs.SetInt(BestScoreProp, BestScore);
            OnBestScoreIncreased?.Invoke();
            Debug.Log("GM: " + BestScore);
        }

        private void GetBestScore()
        {
            BestScore = PlayerPrefs.GetInt(BestScoreProp);
            OnBestScoreIncreased?.Invoke();
        }

        #endregion

        private void SetResolutionAndFrameRate()
        {
            Resolution[] resolutions = Screen.resolutions;
            Application.targetFrameRate = (int)resolutions.Last().refreshRateRatio.value;
            QualitySettings.vSyncCount = 0;
        }
    }
}

