using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Controllers;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    public static Action OnStart;

    public static Action OnClick;
    
    public static Action OnRestart;

    public static Action OnScoreIncreased;
    
    private SoundManager _soundManager;

    private bool _isAfterLoading;
    private int _perfectStackCounter;

    private const float CubesDeletingDuration = 0.5f;

    [Header("Start Cube")]
    [SerializeField] private MeshRenderer startCubeMaterial;
    [SerializeField] private Transform placedBlocks;
    
    private InputManager _inputManager;
    private InputController.PlayerInteractionsActions _playerInteractions;

    
    //
    public int Scores { private set; get; }
    public int BestScore { private set; get; }
    public int PerfectStacksCount { private set; get; }
    //
    [Inject]
    private void Construct(InputManager inputManager)
    {
        _inputManager = inputManager ? inputManager : throw new ArgumentNullException(nameof(inputManager));
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
        MovingCubeController.OnPerfectStack += () => PerfectStacksCount += 1;
        MovingCubeController.OnSlice += () => PerfectStacksCount = 0;
            
        MovingCubeController.OnMiss += OnRestart;
    }

    private void OnDisable()
    {
        MovingCubeController.OnStack -= IncreaseScore;
        MovingCubeController.OnPerfectStack -= () => PerfectStacksCount += 1;
        MovingCubeController.OnSlice -= () => PerfectStacksCount = 0;
        
        MovingCubeController.OnMiss -= OnRestart;
    }

    public void StartGame()
    {
        OnStart.Invoke();
    }

    public void RestartGame()
    {
        TryToClearAllCubes();
        OnRestart.Invoke();
    }

    private void IncreaseScore()
    {
        Scores += 1;
        OnScoreIncreased.Invoke();
    }

    private void SetResolutionAndFrameRate()
    {
        Resolution[] resolutions = Screen.resolutions;
        Application.targetFrameRate = (int)resolutions.Last().refreshRateRatio.value;
        QualitySettings.vSyncCount = 0;
    }

    private async Task ClearAllCubesCoroutine()
    {
        var cubesCount = placedBlocks.transform.childCount;
        float latency = CubesDeletingDuration / cubesCount;
        
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
            await ClearAllCubesCoroutine();
        }
    }

    private void DestroyCube(int i)
    {
        Transform cube = placedBlocks.transform.GetChild(i);
        Destroy(cube.gameObject);
    }
}

