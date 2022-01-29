using UnityEngine;
using Zenject;

namespace Game.Common.Shooting
{
    public class ShootingControl : MonoBehaviour
    {
        [SerializeField]
        private EntityState entityState;

        [Inject]
        private ProjectileSystem _projectileSystem;

        public void OnShoot()
        {
            _projectileSystem.OnShoot(entityState);
        }
    }
}