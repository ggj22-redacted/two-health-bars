using UnityEngine;
using Zenject;

namespace Game.Common.Entities
{
    public class EntityStateInstaller : MonoInstaller<EntityStateInstaller>
    {
        [SerializeField]
        private EntityState entityState;

        public override void InstallBindings ()
        {
            Container.Bind<EntityState>().FromInstance(entityState).AsSingle();
        }
    }
}