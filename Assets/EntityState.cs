using System;
using Game.Common.Projectiles;
using UnityEngine;

public class EntityState : MonoBehaviour
{
    public event Action<EntityState> OnDied;
    public Transform HPBar;

    public bool allowfire = true;

    [SerializeField]
    private int maxHealth = 100;

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

    public ProjectileState ProjectileState => projectileState;

    public Transform ProjectileGunBarrel => projectileGunBarrel;

    private void Start ()
    {
        _currentHealth = maxHealth;
    }
}
