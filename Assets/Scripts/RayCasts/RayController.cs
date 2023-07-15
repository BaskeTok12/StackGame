using UnityEngine;
using UnityEngine.Serialization;

public class RayController : MonoBehaviour
{
    [FormerlySerializedAs("_maxRaycastDistance")] [SerializeField] private float maxRaycastDistance;
    [FormerlySerializedAs("_raycastPosition")] [SerializeField] private Transform raycastPosition;
    [FormerlySerializedAs("_extremePointLayer")] [SerializeField] private LayerMask extremePointLayer;
    [FormerlySerializedAs("_spawnPointLayer")] [SerializeField] private LayerMask spawnPointLayer;
    
    private Ray _ray;
    private RaycastHit _raycastHit;
    
    private void FixedUpdate()
    {
        _ray = new Ray(raycastPosition.position, raycastPosition.forward * maxRaycastDistance);
        Debug.DrawRay(raycastPosition.position, raycastPosition.forward * maxRaycastDistance, Color.red);
        IsRaycastHittedExtremePoint();
    }

    public bool IsRaycastHittedExtremePoint()
    {
        return Physics.Raycast(_ray, out _raycastHit, maxRaycastDistance, extremePointLayer);
    }
    
    public bool IsRaycastHittedSpawnPoint()
    {
        return Physics.Raycast(_ray, out _raycastHit, maxRaycastDistance, spawnPointLayer);
    }
}
