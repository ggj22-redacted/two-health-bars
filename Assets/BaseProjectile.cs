using System;
using UnityEngine;
using UnityEngine.Events;

public class BaseProjectile : MonoBehaviour
{
    [SerializeField] private UnityEvent projectileCollision;
    public event Action<BaseProjectile, Collider> onProjectileHit;
    private void OnTriggerEnter(Collider other) {
        Debug.Log("onTriggerEnter");
        Debug.Log(other.gameObject.name);
        projectileCollision.Invoke();
        onProjectileHit?.Invoke(this, other);
    }
}
