using System;
using System.Linq;
using System.Threading.Tasks;
using Controllers;
using DG.Tweening;
using UnityEngine;
using Zenject;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public static event Action OnStart;
    public static event Action OnRestart;
    public static event Action OnClick;
    public static event Action OnScoreIncreased;
    
    private InputManager _inputManager;
    private InputController.PlayerInteractionsActions _playerInteractions;
    
    private SoundManager _soundManager;

    [Header("Start Cube")]
    [SerializeField] private MeshRenderer startCubeMaterial;
    [SerializeField] private MeshRenderer startTowerMaterial;

    [Header("Fog")] 
    [SerializeField] private MeshRenderer fogMaterial;

    [SerializeField] private Camera mainCamera;

    private Material _stackMaterial;
    private Material _environmentMaterial;
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
    }

    private void Start()
    {
        SetResolutionAndFrameRate();
        
        _playerInteractions = _inputManager.PlayerInteractions;
        _playerInteractions.Click.started += context => OnClick.Invoke();
    }

    private void OnEnable()
    {
        MovingCubeController.OnMiss += 
        MovingCubeController.OnStack += IncreaseScore;
        MovingCubeController.OnPerfectStack += IncreasePerfectStackCount;
        MovingCubeController.OnSlice += ResetPerfectStackCount;

        CameraManager.OnCameraReset += SetSceneStyle;
    }
    
    private void OnDisable()
    {
        MovingCubeController.OnStack -= IncreaseScore;
        MovingCubeController.OnPerfectStack -= IncreasePerfectStackCount;
        MovingCubeController.OnSlice -= ResetPerfectStackCount;
        
        CameraManager.OnCameraReset -= SetSceneStyle;
    }

    public void StartGame()
    {
        OnStart.Invoke();
    }

    public void RestartGame()
    {
        Scores = 0;
        PerfectStacksCount = 0;
        OnRestart.Invoke();
    }

    private void SetSceneStyle()
    {
        _stackMaterial = ColorController.GetBaseMaterialWithRandomColor(startCubeMaterial.sharedMaterial);
        _environmentMaterial = ColorController.GetBaseMaterialWithRandomColor(startCubeMaterial.sharedMaterial);
        
        startCubeMaterial.material = _stackMaterial;
        startTowerMaterial.material = _stackMaterial;
        
        fogMaterial.material.SetColor("_FogColor", _environmentMaterial.color);
        mainCamera.backgroundColor = _environmentMaterial.color;
    }

    #region Score

    private void IncreaseScore()
    {
        Scores += 1;
        OnScoreIncreased.Invoke();
    }

    private void IncreasePerfectStackCount()
    {
        PerfectStacksCount += 1;
    }

    private void ResetPerfectStackCount()
    {
        PerfectStacksCount = 0;
    }

    #endregion

    private void SetResolutionAndFrameRate()
    {
        Resolution[] resolutions = Screen.resolutions;
        Application.targetFrameRate = (int)resolutions.Last().refreshRateRatio.value;
        QualitySettings.vSyncCount = 0;
    }
}

