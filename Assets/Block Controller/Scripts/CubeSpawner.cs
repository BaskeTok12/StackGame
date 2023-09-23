using Controllers;
using Enums;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Transform zSpawnerPosition;
    [SerializeField] private Transform xSpawnerPosition;
    
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
        Gizmos.DrawWireCube(zSpawnerPosition.position , MovingCubeController.DefaultScale);
        Gizmos.DrawWireCube(xSpawnerPosition.position , MovingCubeController.DefaultScale);
    }
}
