using UnityEngine;
using Zenject;

namespace Game.Common.Entities
{
    public class EntityRespawnerInstaller : MonoInstaller<EntityRespawnerInstaller>
    {
        [SerializeField]
        private EntityRespawner entityRespawner;

        public override void InstallBindings()
        {
            Container.Bind<EntityRespawner>().FromInstance(entityRespawner).AsSingle();
        }
    }
}