using System;
using Cysharp.Threading.Tasks;
using Game.Common.Projectiles;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = System.Random;

public class BaseProjectile : MonoBehaviour
{
    private static readonly Quaternion UpRotation = Quaternion.Euler(0, 90, 0);

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

    [SerializeField, Range(0, 5)]
    private float poolDelay;

    private bool _hasHit;

    private float _startMoment;

    private Vector3 _startPosition;

    private Random _random;

    private Animator _projectileAnim;

    public ProjectileState State { get; private set; }

    public UniTask PostHitTask { get; private set; } = UniTask.CompletedTask;

    private bool IsLifetimeReached => Time.time - _startMoment >= State.Lifetime;

    private bool IsRangeReached => (transform.position - _startPosition).sqrMagnitude >= State.Range * State.Range;

    private void Awake ()
    {
        _random = new Random(Guid.NewGuid().GetHashCode());
        _projectileAnim = gameObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter (Collider other)
    {
        if (_hasHit)
            return;

        _hasHit = true;

        IHittable[] hittables = other.GetComponentsInChildren<IHittable>();
        foreach (IHittable hittable in hittables)
            hittable.OnHit(State);

        if (hittables.Length > 0)
            onHit.Invoke();

        float delay = Mathf.Max(0, poolDelay);
        if (State.HitClip)
            delay = Mathf.Max(delay, State.HitClip.length);
        PostHitTask = UniTask.Delay(TimeSpan.FromSeconds(delay), true);

        OnProjectileHit?.Invoke(this, other);
    }

    public void Hide ()
    {
        referenceRenderer.enabled = false;
    }

    public void Shoot (ProjectileState state, Vector3 position, Vector3 direction, string layer)
    {
        // set position and shoot moment
        _startPosition = position;
        _startMoment = Time.time;

        // set collision layer
        gameObject.layer = LayerMask.NameToLayer(layer);
        gameObject.SetActive(true);

        // play spawn animation
        _projectileAnim.Play("ProjectileAnimation");

        // set shooter EntityState
        State = state;

        // set sound clips
        shootAudioSource.clip = state.ShootClip;
        shootAudioSource.Stop();
        hitAudioSource.clip = state.HitClip;
        hitAudioSource.Stop();

        // set size
        transform.position = position;
        transform.localScale = new Vector3(State.Size, State.Size, State.Size);

        // set velocity, considering shoot direction and spread
        referenceRigidbody.velocity = Vector3.zero;
        Vector3 up = UpRotation * direction;
        Vector3 right = Vector3.Cross(direction, up);
        up *= ((float)_random.NextDouble() * 2 - 1) * State.Spread;
        right *= ((float)_random.NextDouble() * 2 - 1) * State.Spread;
        referenceRigidbody.AddForce((direction + up + right).normalized * State.Speed, ForceMode.VelocityChange);

        // enable the renderer
        referenceRenderer.enabled = true;
        referenceRenderer.sharedMaterial = state.Material;

        // reset the post hit task
        PostHitTask = UniTask.CompletedTask;

        // reset the hit status
        _hasHit = false;

        // notify listeners
        onShoot.Invoke();
    }

    private void Update ()
    {
        if (IsRangeReached || IsLifetimeReached)
            gameObject.SetActive(false);
    }
}