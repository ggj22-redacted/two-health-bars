using System;
using Game.Common.Entities;
using Game.Common.Projectiles;
using UnityEngine;

public class EntityState : MonoBehaviour
{
    public event Action<EntityState> OnDied;

    public event Action<EntityState> OnRespawned;

    public event Action<float, float> OnHealthChanged;

    public event Action<float, float> OnShieldChanged;

    public event Action<float, float> OnSpeedChanged;

    public event Action<float, float> OnJumpHeightChanged;

    public event Action<float, float> OnGravityChanged;

    public bool allowfire = true;

    [SerializeField]
    private int maxHealth = 100;

    [SerializeField]
    private int maxShield = 100;

    [Header("Movement")]
    [SerializeField]
    private float speed = 1;

    [SerializeField]
    private float accelerationFactor = 1;

    [Header("Jumping")]
    [SerializeField]
    private float jumpHeight = 1;

    [SerializeField]
    private float gravity = -9.81f;

    [Header("Projectiles")]
    [SerializeField]
    private ProjectileState projectileState;

    [SerializeField]
    private Transform projectileGunBarrel;

    private float _currentHealth;

    private float _currentShield;

    public bool IsDead => Health <= 0;

    public float MaxHealth => maxHealth;

    public float MaxShield => maxShield;

    public float Health
    {
        get => _currentHealth;
        set
        {
            value = Mathf.Clamp(value, 0, maxHealth);
            float previous = _currentHealth;
            _currentHealth = value;

            OnHealthChanged?.Invoke(previous, _currentHealth);

            if (_currentHealth <= 0)
                OnDied?.Invoke(this);

            if (previous <= 0 && _currentHealth > 0)
                OnRespawned?.Invoke(this);
        }
    }

    public float Shield
    {
        get => _currentShield;
        set
        {
            value = Mathf.Clamp(value, 0, maxShield);
            float previous = _currentShield;
            _currentShield = value;

            OnShieldChanged?.Invoke(previous, _currentShield);
        }
    }

    public float Speed
    {
        get => speed;
        set
        {
            float previous = speed; 
            speed = value;

            OnSpeedChanged?.Invoke(previous, speed);
        }
    }

    public float Acceleration => accelerationFactor * speed;

    public float JumpHeight
    {
        get => jumpHeight;
        set
        {
            float previous = jumpHeight;
            jumpHeight = value;

            OnJumpHeightChanged?.Invoke(previous, jumpHeight);
        }
    }

    public float Gravity
    {
        get => gravity;
        set
        {
            float previous = gravity;
            gravity = value;

            OnGravityChanged?.Invoke(previous, gravity);
        }
    }

    public ProjectileState ProjectileState
    {
        get => projectileState;
        set => projectileState = value;
    }

    public Transform ProjectileGunBarrel => projectileGunBarrel;

    public EntityStats OriginalStats { get; private set; } 

    private void Awake ()
    {
        Health = maxHealth;
        Shield = maxShield;
        Speed = speed;
        JumpHeight = jumpHeight;
        Gravity = gravity;
    }

    private void Start ()
    {
        OriginalStats = new EntityStats(maxHealth, maxShield, speed, jumpHeight, gravity, projectileState);
    }
}
