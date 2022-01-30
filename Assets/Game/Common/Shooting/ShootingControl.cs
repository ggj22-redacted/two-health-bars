using System;
using UnityEngine;
using Zenject;

namespace Game.Common.Shooting
{
    public class ShootingControl : MonoBehaviour
    {
        [SerializeField]
        private EntityState entityState;

        private bool shooting = false;

        [Inject]
        private ProjectileSystem _projectileSystem;

        private void Awake () => InitializeProjectileSystem();

        public void OnShoot()
        {
            shooting = !shooting;
        }

        public void SetShooting(bool value)
        {
            shooting = value;
        }

        public void Update() {
            if (shooting)
                _projectileSystem.OnShoot(entityState);
        }

        private void InitializeProjectileSystem ()
        {
            if (!_projectileSystem)
                _projectileSystem = FindObjectOfType<ProjectileSystem>();
        }
    }
}