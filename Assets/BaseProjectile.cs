using System;
using UnityEngine;
using UnityEngine.Events;

public class BaseProjectile : MonoBehaviour
{
    [SerializeField] private UnityEvent projectileCollision;
    public event Action<BaseProjectile, Collider> onProjectileHit;
    private void OnTriggerEnter(Collider other) {
        projectileCollision.Invoke();
        onProjectileHit?.Invoke(this, other);
    }
}
