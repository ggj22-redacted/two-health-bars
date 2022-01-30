using System;
using Game.Common.Projectiles;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class BaseProjectile : MonoBehaviour
{
    [SerializeField]
    private UnityEvent projectileCollision;

    [SerializeField]
    private UnityEvent onShoot;

    [SerializeField]
    private AudioSource shootAudioSource;

    [SerializeField]
    private AudioSource hitAudioSource;

    public event Action<BaseProjectile, Collider> OnProjectileHit;

    public Renderer rendererComponent;

    public ProjectileState State { get; private set; }

    private void Awake()
    {
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

    public void Shoot (ProjectileState state)
    {
        State = state;

        shootAudioSource.clip = state.ShootClip;
        hitAudioSource.clip = state.HitClip;

        onShoot.Invoke();
    }
}
