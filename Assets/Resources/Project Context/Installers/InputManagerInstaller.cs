using CodeBase.Input_Manager;
using UnityEngine;
using Zenject;

namespace Resources.Project_Context.Installers
{
    public class InputManagerInstaller : MonoInstaller
    {
        [SerializeField] private InputManager inputManager;
        public override void InstallBindings()
        {
            var inputManagerInstance = Container.InstantiatePrefabForComponent<InputManager>(inputManager);
            Container.Bind<InputManager>().FromInstance(inputManagerInstance).AsSingle().NonLazy();
            Container.QueueForInject(inputManager);
        }
    }
}