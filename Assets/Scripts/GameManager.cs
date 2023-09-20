using System;
using System.Collections;
using System.Linq;
using Controllers;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    public static Action OnStart;

    public static Action OnClick;
    
    public static Action OnRestart;
    

    private SoundManager _soundManager;

    private bool _isAfterLoading;
    private int _perfectStackCounter;

    private const float CubesDeletingDuration = 1f;

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
        MovingCubeController.OnStack += () => Scores += 1;
        MovingCubeController.OnPerfectStack += () => PerfectStacksCount += 1;
        MovingCubeController.OnSlice += () => PerfectStacksCount = 0;
    }

    private void OnDisable()
    {
        MovingCubeController.OnStack -= () => Scores += 1;
        MovingCubeController.OnPerfectStack -= () => PerfectStacksCount += 1;
        MovingCubeController.OnSlice -= () => PerfectStacksCount = 0;
    }

    public void StartGame()
    {
        OnStart.Invoke();
    }

    public void RestartGame()
    {
        StartCoroutine(
            ClearAllCubesCoroutine());
        OnRestart.Invoke();
    }

    private void IncreaseScore()
    {
        Scores += 1;
        
    }

    private void SetResolutionAndFrameRate()
    {
        Resolution[] resolutions = Screen.resolutions;
        Application.targetFrameRate = resolutions.Last().refreshRate;
        QualitySettings.vSyncCount = 0;
    }

    private IEnumerator ClearAllCubesCoroutine()
    {
        if (placedBlocks.transform.childCount > 0)
        {
            var cubesCount = placedBlocks.transform.childCount;
            float latency = CubesDeletingDuration / cubesCount;
            for (int i = cubesCount - 1; i >= 0; i--)
            {
                Transform cube = placedBlocks.transform.GetChild(i);
                Destroy(cube.gameObject);
                yield return new WaitForSeconds(latency);
            }
        }
    }
}

