using CodeBase.Game_Manager;
using UnityEngine;
using Zenject;

namespace Resources.Project_Context.Installers
{
    public class GameManagerInstaller : MonoInstaller
    {
        [SerializeField] private GameManager gameManager;
        public override void InstallBindings()
        {
            Container.Bind<GameManager>().FromInstance(gameManager).AsSingle().NonLazy();
            Container.QueueForInject(gameManager);
        }
    }
}