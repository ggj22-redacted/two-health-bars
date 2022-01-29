using System;
using Game.Common.Projectiles;
using UnityEngine;
using Zenject;

public class EntityState : MonoBehaviour
{
    public event Action<EntityState> OnDied;

    public bool allowfire = true;

    [SerializeField]
    private int maxHealth = 100;

    [Header("Projectiles")]
    [SerializeField]
    private ProjectileState projectileState;

    [SerializeField]
    private Transform projectileGunBarrel;

    private float _currentHealth;

    [Inject]
    private ProjectileSystem _projectileSystem;

    public float Health
    {
        get => _currentHealth;
        set
        {
            value = Mathf.Clamp(value, 0, maxHealth);
            _currentHealth = value;

            if (_currentHealth <= 0)
                OnDied?.Invoke(this);
        }
    }

    public ProjectileState ProjectileState => projectileState;

    public Transform ProjectileGunBarrel => projectileGunBarrel;

    private void Start ()
    {
        _currentHealth = maxHealth;
    }

    public void OnShoot() {
        Debug.Log("onShoot");
        _projectileSystem.OnShoot(this);
    }
}
