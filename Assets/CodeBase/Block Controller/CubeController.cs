using System;
using CodeBase.Block_Controller.Enums;
using CodeBase.Game_Manager;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace CodeBase.Block_Controller
{
    public class CubeController : MonoBehaviour
    {
        [SerializeField] private Transform lastCubeTransform;
        [SerializeField] private CubeSpawner spawner;
        [SerializeField] private Transform placedBlocks;
        [SerializeField] private Transform fallingBlocks;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float perfectHangoverDeviation = 0.002f;
        [SerializeField] private bool isCanMove = true;
        [SerializeField] private Gradient colorGradient;
        
        public static readonly Vector3 DefaultScale = new Vector3(5, 0.5f, 5);
        
        public static Action OnStack;
        public static Action OnPerfectStack;
        public static event Action OnBlockPlaced;
        
        public static Action OnSlice;
        
        private float _currentMoveSpeed;
        private MoveDirection _moveDirection = MoveDirection.Z;
        private MoveDestination _moveDestination = MoveDestination.Forward;

        private Vector3 _startPosition;
        private const float MoveBackDistance = 10f;
        
        private Transform _startBlock;
        private Rigidbody _rigidbody;
        private MeshRenderer _renderer;
        private GameManager _gameManager;

        public static event Action OnMiss;

        [Inject]
        private void Constructor(GameManager gameManager)
        {
            _gameManager = gameManager;
        }
        
        private void Awake()
        {
            GetComponents();
            _startBlock = lastCubeTransform;
            _currentMoveSpeed = moveSpeed;
        }
        
        private void OnEnable()
        {
            GameManager.OnStart += () => isCanMove = true;
            GameManager.OnClick += Stop;
            GameManager.OnRestart += ResetGame;

            _renderer = GetComponent<MeshRenderer>();
        }
        private void OnDisable()
        {
            GameManager.OnStart -= () => isCanMove = true;
            GameManager.OnClick -= Stop;
            GameManager.OnRestart -= ResetGame;
        }
        private void Update()
        {
            if (isCanMove)
            {
                Move();
            }
        }
        
        private void Move()
        {
            var distanceToSpawner = CalculateDistanceToSpawner();
            CalculateMoveDestination(distanceToSpawner);
        
            if (_moveDestination == MoveDestination.Forward)
                MoveForward();
            else
                MoveBackward();
        }
        private void GetComponents()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
    
        private float CalculateDistanceToSpawner()
        {
            if (_moveDirection == MoveDirection.Z)
            {
                return lastCubeTransform.transform.position.z - transform.position.z;
            }
            return lastCubeTransform.transform.position.x - transform.position.x;
        }

        private void CalculateMoveDestination(float distanceToSpawner)
        {
            if (distanceToSpawner < -MoveBackDistance)
            {
                _moveDestination = MoveDestination.Backward;
            }
            if (distanceToSpawner > MoveBackDistance)
            {
                _moveDestination = MoveDestination.Forward;
            }
        }
        private void MoveForward()
        {
            if (_moveDirection == MoveDirection.Z)
                transform.position += transform.forward * Time.deltaTime * _currentMoveSpeed;
            else
                transform.position += transform.right * Time.deltaTime * _currentMoveSpeed;
        }
    
        private void MoveBackward()
        {
            if (_moveDirection == MoveDirection.Z)
                transform.position += -transform.forward * Time.deltaTime * _currentMoveSpeed;
            else
                transform.position += -transform.right * Time.deltaTime * _currentMoveSpeed;
        }

        private void Stop()
        {
            if (!isCanMove) return;
            
            CubeState currentCubeState = CubeState.Stack;
            float hangover = CalculateHangover();

            float max = _moveDirection == MoveDirection.Z
                ? lastCubeTransform.transform.localScale.z
                : lastCubeTransform.transform.localScale.x;
                
            if (Mathf.Abs(hangover) <= perfectHangoverDeviation)
            {
                hangover = 0;
                if (_currentMoveSpeed < maxSpeed)
                {
                    _currentMoveSpeed += 0.5f;
                }
                currentCubeState = CubeState.PerfectStack;
            }
            else
            {
                if (_currentMoveSpeed > 10f)
                {
                    _currentMoveSpeed -= 0.5f;
                }
            }

            if (Mathf.Abs(hangover) >= max)
            {
                Miss();
                return;
            }
            PlaceBlock(hangover, currentCubeState);
        }

        private void PlaceBlock(float hangover, CubeState state)
        {
            if (state == CubeState.Stack)
            {
                float direction = hangover > 0 ? 1f : -1f;
                SplitCube(hangover, direction);
                OnStack.Invoke();
            }
            else if (state == CubeState.PerfectStack)
            {
                transform.position = new Vector3(lastCubeTransform.transform.position.x, transform.position.y, lastCubeTransform.transform.position.z);
                OnPerfectStack.Invoke();
            }
            SpawnLastCube();
            DeployStackBlock();
            OnBlockPlaced.Invoke();
        }

        private float CalculateHangover()
        {
            if (_moveDirection == MoveDirection.Z)
            {
                return transform.position.z - lastCubeTransform.transform.position.z;
            }
            return transform.position.x - lastCubeTransform.transform.position.x;
        }

        private void SplitCube(float hangover, float direction)
        {
            float currentLastCubeControllerLocalScale;
            float currentLastCubeTransformPosition;
            float currentLocalScale;
   
            if (_moveDirection == MoveDirection.Z)
            {
                currentLastCubeControllerLocalScale = lastCubeTransform.transform.localScale.z;
                currentLastCubeTransformPosition = lastCubeTransform.transform.position.z;
                currentLocalScale = transform.localScale.z;
            }
            else
            {
                currentLastCubeControllerLocalScale = lastCubeTransform.transform.localScale.x;
                currentLastCubeTransformPosition = lastCubeTransform.transform.position.x;
                currentLocalScale = transform.localScale.x;
            }
            
            float newSize = currentLastCubeControllerLocalScale - Mathf.Abs(hangover);
            float fallingBlockSize = currentLocalScale - newSize;

            float newZPosition = currentLastCubeTransformPosition + (hangover / 2);
            float edgeOfCube;
            if (_moveDirection == MoveDirection.Z)
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newSize);
                transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);
                edgeOfCube = transform.position.z + (newSize / 2 * direction);
            }
            else
            {
                transform.localScale = new Vector3(newSize, transform.localScale.y, transform.localScale.z);
                transform.position = new Vector3(newZPosition, transform.position.y, transform.position.z);
                edgeOfCube = transform.position.x + (newSize / 2 * direction);
            }

            float fallingBlockZPosition = edgeOfCube + fallingBlockSize / 2f * direction;

            CreateFallingCube(fallingBlockZPosition, fallingBlockSize);
        }

        private void CreateFallingCube(float position, float fallingBlockSize)
        {
            OnSlice.Invoke();
            var newCube = CreateNewCube();
            newCube.transform.SetParent(fallingBlocks.transform);
            if (_moveDirection == MoveDirection.Z)
            {
                newCube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
                newCube.transform.position = new Vector3(transform.position.x, transform.position.y, position);
            }
            else
            {
                newCube.transform.localScale = new Vector3(fallingBlockSize, transform.localScale.y, transform.localScale.z);
                newCube.transform.position = new Vector3(position, transform.position.y, transform.position.z);
            }

            newCube.AddComponent<Rigidbody>();
            newCube.GetComponent<MeshRenderer>().material.color = _renderer.material.color;
            Destroy(newCube.GameObject(), 2f);
        }
        private void DeployStackBlock()
        {
            ModifyMoveDirection();

            Vector3 deployPosition = spawner.GetSpawnPosition(_moveDirection, transform.position, lastCubeTransform.position);
            transform.position = new Vector3(deployPosition.x, transform.position.y + lastCubeTransform.localScale.y,
                deployPosition.z);
            isCanMove = true;
        }
        
        private GameObject CreateNewCube()
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.SetParent(placedBlocks.transform);
            cube.GetComponent<MeshRenderer>().material = _renderer.material;
            return cube;
        }

        private void Miss()
        {
            OnMiss?.Invoke();
            _rigidbody.isKinematic = false;
            isCanMove = false;
        }

        private void ModifyMoveDirection()
        {
            _moveDirection = _moveDirection == MoveDirection.Z ? MoveDirection.X : MoveDirection.Z;
        }
        private void SpawnLastCube()
        {
            var newCube = CreateNewCube();

            newCube.transform.position = transform.position;
            newCube.transform.localScale = transform.localScale; 

            lastCubeTransform = newCube.transform;
        }
        private void ResetGame()
        {
            _moveDirection = MoveDirection.Z;
            transform.localScale = DefaultScale;
            transform.rotation = new Quaternion();
            _rigidbody.isKinematic = true;

            _currentMoveSpeed = moveSpeed;
            transform.position = spawner.GetDefaultPosition();
            lastCubeTransform = _startBlock;
        }
    }
}
