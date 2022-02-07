using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Common.Projectiles
{
    public class HittableEventHandler : MonoBehaviour, IHittable
    {
        [SerializeField]
        private UnityProjectileEvent onHit;

        public void OnHit (ProjectileState state) => onHit.Invoke(state);
    }

    [Serializable]
    public class UnityProjectileEvent : UnityEvent<ProjectileState> { }
}