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
    public static Action OnStart;
    public static Action OnRestart;

    public static Action OnMiss;
    
    public static Action OnClick;
    public static Action OnScoreIncreased;
    
    private InputManager _inputManager;
    private InputController.PlayerInteractionsActions _playerInteractions;
    
    private SoundManager _soundManager;

    private const float CubesDeletingDuration = 0.5f;

    [Header("Start Cube")]
    [SerializeField] private MeshRenderer startCubeMaterial;
    [SerializeField] private MeshRenderer startTowerMaterial;
    [SerializeField] private Transform placedBlocks;
    [SerializeField] private Transform fallingBlocks;

    [Header("Fog")] 
    [SerializeField] private MeshRenderer fogMaterial;

    [SerializeField] private Camera mainCamera;

    [SerializeField] private float throwingDistance;

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
        TryToClearAllCubes();
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

    #region WorkWithCube

    private async Task ClearAllCubes()
    {
        var cubesCount = placedBlocks.transform.childCount;
        float latency = CubesDeletingDuration / cubesCount;
        
        ThrowUpFallingBlocks(latency);
        for (int i = cubesCount - 1; i >= 0; i--)
        {
            DestroyCube(i);
            await Task.Delay(TimeSpan.FromSeconds(latency));
        }
        
    }

    private async void TryToClearAllCubes()
    {
        if (placedBlocks.transform.childCount > 0)
        {
            await ClearAllCubes();
        }
    }

    private void DestroyCube(int i)
    {
        Transform cube = placedBlocks.transform.GetChild(i);
        Destroy(cube.gameObject);
    }
    
    private void ThrowUpFallingBlocks(float latency)
    {
        fallingBlocks.DOMoveY(fallingBlocks.transform.position.y + throwingDistance, latency * 10);
        
    }

    #endregion

    private void SetResolutionAndFrameRate()
    {
        Resolution[] resolutions = Screen.resolutions;
        Application.targetFrameRate = (int)resolutions.Last().refreshRateRatio.value;
        QualitySettings.vSyncCount = 0;
    }
}

