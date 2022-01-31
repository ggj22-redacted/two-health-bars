using System;
using Game.Common.Projectiles;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = System.Random;

public class BaseProjectile : MonoBehaviour
{
    public event Action<BaseProjectile, Collider> OnProjectileHit;

    [SerializeField]
    private Rigidbody referenceRigidbody;

    [SerializeField]
    private Renderer referenceRenderer;

    [FormerlySerializedAs("projectileCollision")] [SerializeField]
    private UnityEvent onHit;

    [SerializeField]
    private UnityEvent onShoot;

    [SerializeField]
    private AudioSource shootAudioSource;

    [SerializeField]
    private AudioSource hitAudioSource;

    private float _startMoment;

    private Vector3 _startPosition;

    private Random _random;

    public ProjectileState State { get; private set; }

    private bool IsLifetimeReached => Time.time - _startMoment >= State.Lifetime;

    private bool IsRangeReached => (transform.position - _startPosition).sqrMagnitude >= State.Range * State.Range;

    private void Awake()
    {
        _random = new Random(Guid.NewGuid().GetHashCode());

        referenceRenderer = GetComponent<ParticleSystemRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        IHittable[] hittables = other.GetComponents<IHittable>();
        foreach (IHittable hittable in hittables)
            hittable.OnHit(State);

        if (hittables.Length > 0)
            onHit.Invoke();

        OnProjectileHit?.Invoke(this, other);
    }

    public void Shoot (ProjectileState state, Vector3 position, Vector3 direction, string layer)
    {
        _startPosition = position;
        _startMoment = Time.time;

        gameObject.layer = LayerMask.NameToLayer(layer);
        gameObject.SetActive(true);

        State = state;

        shootAudioSource.clip = state.ShootClip;
        hitAudioSource.clip = state.HitClip;

        transform.position = position;
        transform.localScale = new Vector3(State.Size,State.Size,State.Size) * .5f;

        referenceRigidbody.velocity = Vector3.zero;
        direction.x += ((float)_random.NextDouble() * 2 - 1) * State.Spread;
        direction.y += ((float)_random.NextDouble() * 2 - 1) * State.Spread;
        referenceRigidbody.AddForce(direction.normalized * State.Speed, ForceMode.VelocityChange);

        referenceRenderer.sharedMaterial = state.Material;

        onShoot.Invoke();
    }

    private void Update ()
    {
        if (IsRangeReached || IsLifetimeReached)
            gameObject.SetActive(false);
    }
}
