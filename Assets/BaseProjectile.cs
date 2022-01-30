using System;
using Game.Common.Projectiles;
using UnityEngine;
using UnityEngine.Events;

public class BaseProjectile : MonoBehaviour
{
    [SerializeField]
    private UnityEvent projectileCollision;

    public event Action<BaseProjectile, Collider> OnProjectileHit;

    public Renderer rendererComponent;

    public ProjectileState State { get; set; }

    private void Awake() {
        rendererComponent = GetComponent<ParticleSystemRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        IHittable[] hittables = other.GetComponents<IHittable>();
        foreach (IHittable hittable in hittables)
            hittable.OnHit(State);

        projectileCollision.Invoke();
        OnProjectileHit?.Invoke(this, other);
    }
}
