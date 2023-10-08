using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class GameManagerInstaller : MonoInstaller
{
    [SerializeField] private GameManager gameManager;
    public override void InstallBindings()
    {
        Container.Bind<GameManager>().FromInstance(gameManager).AsSingle().NonLazy();
        Container.QueueForInject(gameManager);
    }
}