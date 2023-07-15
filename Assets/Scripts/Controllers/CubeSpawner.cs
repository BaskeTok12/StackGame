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
    // public void SpawnCube()
    // {
    //     var newCube = Instantiate(cubePrefab);
    //     if (MovingCubeController.LastCubeController != null && MovingCubeController.LastCubeController.gameObject != GameObject.Find("StartCube"))
    //     {
    //         float xPosition = moveDirection == MoveDirection.X
    //             ? transform.position.x
    //             : MovingCubeController.LastCubeController.transform.position.x;
    //         
    //         float zPosition = moveDirection == MoveDirection.Z
    //             ? transform.position.z
    //             : MovingCubeController.LastCubeController.transform.position.z;
    //         
    //         newCube.transform.position = new Vector3(xPosition,
    //             MovingCubeController.LastCubeController.transform.position.y + cubePrefab.transform.localScale.y, zPosition);
    //     }
    //     else
    //     {
    //         newCube.transform.position = transform.position;
    //     }
    //
    //     newCube.MoveDirection = moveDirection;
    // }

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
