using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class SoundManagerInstaller : MonoInstaller
{
    [FormerlySerializedAs("_soundManager")] [SerializeField] private SoundManager soundManager;
    public override void InstallBindings()
    {
        var soundManagerInstance = Container.InstantiatePrefabForComponent<SoundManager>(soundManager);
        Container.Bind<SoundManager>().FromInstance(soundManagerInstance).AsSingle().NonLazy();
        Container.QueueForInject(soundManager);
    }
}