using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSystem : MonoBehaviour
{
    public BaseProjectile projectilePrefab;

    [Header("Settings")]
    public int maxGameProjectiles;

    [SerializeField]
    private int maxInstantiationsPerFrame = 10;

    private BaseProjectile[] _projectilePool;

    private static void DisableProjectile (BaseProjectile projectile, Collider targetHit)
    {
        projectile.gameObject.SetActive(false);
    }

    private void Start ()
    {
        _projectilePool = new BaseProjectile[maxGameProjectiles];

        StartCoroutine(InstantiatePool(_projectilePool, projectilePrefab));
    }

    private IEnumerator InstantiatePool (IList<BaseProjectile> pool, BaseProjectile projectilePrefab)
    {
        int currentInstantiations = 0;
        for (int i = 0; i < pool.Count; i++) {
            BaseProjectile projectile = Instantiate(projectilePrefab);
            projectile.OnProjectileHit += DisableProjectile;

            projectile.gameObject.SetActive(false);

            pool[i] = projectile;

            if (++currentInstantiations < maxInstantiationsPerFrame)
                continue;

            currentInstantiations = 0;
            yield return null;
        }
    }

    public void OnShoot (EntityState shooter)
    {
        if (shooter.allowfire)
            StartCoroutine(Shoot(shooter));
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