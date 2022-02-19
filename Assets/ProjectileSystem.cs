using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectileSystem : MonoBehaviour
{
    public BaseProjectile projectilePrefab;

    [Header("Settings")]
    public int maxGameProjectiles;

    [SerializeField]
    private int maxInstantiationsPerFrame = 10;

    [Inject]
    private EntityState _playerState;

    private BaseProjectile[] _projectilePool;

    private static async void PoolProjectile (BaseProjectile projectile, Collider targetHit)
    {
        projectile.Hide();

        await projectile.PostHitTask;

        projectile.gameObject.SetActive(false);
    }

    private void Start ()
    {
        _projectilePool = new BaseProjectile[maxGameProjectiles];

        StartCoroutine(InstantiatePool(_projectilePool, projectilePrefab));
    }

    private void OnEnable ()
    {
        _playerState.OnRespawned += DisableAllProjectiles;
    }

    private void OnDisable ()
    {
        _playerState.OnRespawned -= DisableAllProjectiles;
    }

    private IEnumerator InstantiatePool (IList<BaseProjectile> pool, BaseProjectile projectilePrefab)
    {
        int currentInstantiations = 0;
        for (int i = 0; i < pool.Count; i++) {
            BaseProjectile projectile = Instantiate(projectilePrefab);
            projectile.OnProjectileHit += PoolProjectile;

            projectile.gameObject.SetActive(false);

            pool[i] = projectile;

            if (++currentInstantiations < maxInstantiationsPerFrame)
                continue;

            currentInstantiations = 0;
            yield return null;
        }
    }

    private void DisableAllProjectiles (EntityState entityState)
    {
        foreach (BaseProjectile projectile in _projectilePool) {
            projectile.gameObject.SetActive(false);
        }
    }

    public bool OnShoot (EntityState shooter)
    {
        if (!shooter.allowfire)
            return false;
    
        StartCoroutine(Shoot(shooter));

        return true;
    }

    private IEnumerator Shoot (EntityState shooter)
    {
        shooter.allowfire = false;

        BaseProjectile projectile = Array.Find(_projectilePool, projectile => !projectile.gameObject.activeSelf);

        if (projectile)
            projectile.Shoot(
                shooter.ProjectileState,
                shooter.ProjectileGunBarrel.position,
                shooter.ProjectileGunBarrel.forward,
                shooter.ProjectileState.Layer);
        else
            Debug.LogError("Exceeding available projectiles, increase pool size or lower fire rate/projectile time to live");

        yield return new WaitForSeconds(1f / shooter.ProjectileState.FireRate);

        shooter.allowfire = true;
    }
}