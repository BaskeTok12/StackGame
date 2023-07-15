using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class InputManagerInstaller : MonoInstaller
{
    [FormerlySerializedAs("_inputManager")] [SerializeField] private InputManager inputManager;
    public override void InstallBindings()
    {
        var inputManagerInstance = Container.InstantiatePrefabForComponent<InputManager>(inputManager);
        Container.Bind<InputManager>().FromInstance(inputManagerInstance).AsSingle().NonLazy();
        Container.QueueForInject(inputManager);
    }
}