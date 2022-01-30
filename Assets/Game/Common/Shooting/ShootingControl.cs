using System;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Game.Common.Shooting
{
    public class ShootingControl : MonoBehaviour
    {
        [SerializeField]
        private EntityState entityState;

        [SerializeField]
        private UnityEvent onShoot;

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

        public void Update()
        {
            if (!shooting)
                return;

            _projectileSystem.OnShoot(entityState);

            onShoot.Invoke();
        }

        private void InitializeProjectileSystem ()
        {
            if (!_projectileSystem)
                _projectileSystem = FindObjectOfType<ProjectileSystem>();
        }
    }
}