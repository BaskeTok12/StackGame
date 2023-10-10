using System;
using System.Threading.Tasks;
using Controllers;
using DG.Tweening;
using Enums;
using Game_Manager;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Transform zSpawnerPosition;
    [SerializeField] private Transform xSpawnerPosition;
    
    [Header("For Cube")]
    [SerializeField] private Transform placedBlocks;
    [SerializeField] private Transform fallingBlocks;
    [SerializeField] private float throwingDistance;
    
    private const float CubesDeletingDuration = 0.5f;
    
    private void OnEnable()
    {
        GameManager.OnRestart += TryToClearAllCubes;
    }

    private void OnDisable()
    {
        GameManager.OnRestart -= TryToClearAllCubes;
    }

    public Vector3 GetSpawnPosition(MoveDirection direction , Vector3 blockPosition , Vector3 lastCubePosition)
    {
        float xPosition = direction == MoveDirection.X ? xSpawnerPosition.transform.position.x : lastCubePosition.x;
        float zPosition = direction == MoveDirection.Z ? xSpawnerPosition.transform.position.z : lastCubePosition.z;
            
        if (direction == MoveDirection.Z)
        {
            xPosition = lastCubePosition.x;
            zPosition = zSpawnerPosition.transform.position.z;
        }
        return new Vector3(xPosition, zSpawnerPosition.position.y , zPosition);
    }

    public Vector3 GetDefaultPosition()
    {
        return zSpawnerPosition.position;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(zSpawnerPosition.position , CubeController.DefaultScale);
        Gizmos.DrawWireCube(xSpawnerPosition.position , CubeController.DefaultScale);
    }
    
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
}
