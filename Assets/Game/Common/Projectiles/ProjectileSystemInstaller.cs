using UnityEngine;
using Zenject;

namespace Game.Common.Projectiles
{
    public class ProjectileSystemInstaller : MonoInstaller<ProjectileSystemInstaller>
    {
        [SerializeField]
        private ProjectileSystem projectileSystem;

        public override void InstallBindings()
        {
            Container.BindInstance(projectileSystem).AsSingle();
        }
    }
}