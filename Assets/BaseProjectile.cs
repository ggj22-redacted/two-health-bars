using System;
using Cysharp.Threading.Tasks;
using Game.Common.Projectiles;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = System.Random;

public class BaseProjectile : MonoBehaviour
{
    private static Quaternion _upRotation = Quaternion.Euler(0, 90, 0);

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

    public UniTask PostHitTask { get; private set; } = UniTask.CompletedTask;

    private bool IsLifetimeReached => Time.time - _startMoment >= State.Lifetime;

    private bool IsRangeReached => (transform.position - _startPosition).sqrMagnitude >= State.Range * State.Range;

    private void Awake ()
    {
        _random = new Random(Guid.NewGuid().GetHashCode());
    }

    private void OnTriggerEnter (Collider other)
    {
        IHittable[] hittables = other.GetComponentsInChildren<IHittable>();
        foreach (IHittable hittable in hittables)
            hittable.OnHit(State);

        if (hittables.Length > 0)
            onHit.Invoke();

        if (State.HitClip)
            PostHitTask = UniTask.Delay(TimeSpan.FromSeconds(State.HitClip.length), true);

        OnProjectileHit?.Invoke(this, other);
    }

    public void Hide ()
    {
        referenceRenderer.enabled = false;
    }

    public void Shoot (ProjectileState state, Vector3 position, Vector3 direction, string layer)
    {
        _startPosition = position;
        _startMoment = Time.time;

        gameObject.layer = LayerMask.NameToLayer(layer);
        gameObject.SetActive(true);

        State = state;

        shootAudioSource.clip = state.ShootClip;
        shootAudioSource.Stop();
        hitAudioSource.clip = state.HitClip;
        hitAudioSource.Stop();

        transform.position = position;
        transform.localScale = new Vector3(State.Size, State.Size, State.Size);

        referenceRigidbody.velocity = Vector3.zero;
        Vector3 up = _upRotation * direction;
        Vector3 right = Vector3.Cross(direction, up);
        up *= ((float)_random.NextDouble() * 2 - 1) * State.Spread;
        right *= ((float)_random.NextDouble() * 2 - 1) * State.Spread;
        referenceRigidbody.AddForce((direction + up + right).normalized * State.Speed, ForceMode.VelocityChange);

        referenceRenderer.enabled = true;
        referenceRenderer.sharedMaterial = state.Material;

        PostHitTask = UniTask.CompletedTask;

        onShoot.Invoke();
    }

    private void Update ()
    {
        if (IsRangeReached || IsLifetimeReached)
            gameObject.SetActive(false);
    }
}