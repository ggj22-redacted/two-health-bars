using System;
using Game.Common.Projectiles;
using StarterAssets;
using UnityEngine;

public class EntityState : MonoBehaviour
{
    public event Action<EntityState> OnDied;

    public event Action<float, float> OnHealthChanged;

    public event Action<float, float> OnSpeedChanged;

    public event Action<float, float> OnJumpHeightChanged;

    public event Action<float, float> OnGravityChanged;

    public Transform HPBar;

    public bool allowfire = true;

    [SerializeField]
    private int maxHealth = 100;

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

    public float MaxHealth => maxHealth;

    public float Health
    {
        get => _currentHealth;
        set
        {
            value = Mathf.Clamp(value, 0, maxHealth);
            float previous = _currentHealth;
            _currentHealth = value;
            if(HPBar) { HPBar.localScale = new Vector3(0.3f, _currentHealth / maxHealth, 0.3f); }

            OnHealthChanged?.Invoke(previous, _currentHealth);

            if (_currentHealth <= 0)
                OnDied?.Invoke(this);
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

    public ProjectileState ProjectileState => projectileState;

    public Transform ProjectileGunBarrel => projectileGunBarrel;

    private void Start ()
    {
        Health = maxHealth;
        Speed = speed;
        JumpHeight = jumpHeight;
        Gravity = gravity;
    }
}
