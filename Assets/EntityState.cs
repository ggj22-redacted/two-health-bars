using System;
using Game.Common.Projectiles;
using StarterAssets;
using UnityEngine;

public class EntityState : MonoBehaviour
{
    [SerializeField]
    private ThirdPersonController controller;

    public event Action<EntityState> OnDied;
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

    public float Health
    {
        get => _currentHealth;
        set
        {
            value = Mathf.Clamp(value, 0, maxHealth);
            _currentHealth = value;
            if(HPBar) { HPBar.localScale = new Vector3(0.3f, _currentHealth / maxHealth, 0.3f); }

            if (_currentHealth <= 0)
                OnDied?.Invoke(this);
        }
    }

    public float Speed
    {
        get => speed;
        set
        {
            speed = value;

            controller.MoveSpeed = speed;
            controller.SpeedChangeRate = accelerationFactor * speed;
        }
    }

    public float JumpHeight
    {
        get => jumpHeight;
        set
        {
            jumpHeight = value;

            controller.JumpHeight = jumpHeight;
        }
    }

    public float Gravity
    {
        get => gravity;
        set
        {
            gravity = value;

            controller.Gravity = gravity;
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
