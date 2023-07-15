using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class GameManagerInstaller : MonoInstaller
{
    [SerializeField] private GameManager gameManager;
    public override void InstallBindings()
    {
        var gameManagerInstance = Container.InstantiatePrefabForComponent<GameManager>(gameManager);
        Container.Bind<GameManager>().FromInstance(gameManagerInstance).AsSingle().NonLazy();
        Container.QueueForInject(gameManager);
    }
}