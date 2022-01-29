using System.Collections;
using System;
using System.Collections.Generic;
using Game.Common.Projectiles;
using UnityEngine;

public class ProjectileSystem : MonoBehaviour
{
    public BaseProjectile baseProjectilePrefab;

    [Header("Settings")]
    public int maxPlayerProjectiles;

    public int maxEnemyProjectiles;

    [SerializeField]
    private int maxInstantiationsPerFrame = 10;

    private BaseProjectile[] _enemyProjectiles;

    private BaseProjectile[] _playerProjectiles;

    private void Start ()
    {
        _playerProjectiles = new BaseProjectile[maxPlayerProjectiles];
        _enemyProjectiles = new BaseProjectile[maxEnemyProjectiles];

        StartCoroutine(
            InstantiatePool(_playerProjectiles, baseProjectilePrefab, LayerMask.NameToLayer("PlayerProjectiles")));
        StartCoroutine(
            InstantiatePool(_enemyProjectiles, baseProjectilePrefab, LayerMask.NameToLayer("EnemyProjectiles")));
    }

    private IEnumerator InstantiatePool (IList<BaseProjectile> pool, BaseProjectile projectilePrefab, int layerID)
    {
        int currentInstantiations = 0;
        for (int i = 0; i < pool.Count; i++) {
            var inst = Instantiate(projectilePrefab);
            inst.OnProjectileHit += HandleProjectileCollision;

            GameObject o = inst.gameObject;
            o.SetActive(false);
            o.layer = layerID;

            pool[i] = inst;

            currentInstantiations++;
            if (currentInstantiations < maxInstantiationsPerFrame)
                continue;

            currentInstantiations = 0;
            yield return null;
        }
    }

    private void HandleProjectileCollision (BaseProjectile projectile, Collider targetHit)
    {
        projectile.gameObject.SetActive(false);

        if (targetHit.gameObject.name == "player"
            || targetHit.gameObject.name == "horn"
            || targetHit.gameObject.name == "halo") {
            //targetHit.gameObject.GetComponent<HealthSystem>()
        }
    }

    public void OnShoot (EntityState shooter)
    {
        if (shooter.allowfire) {
            StartCoroutine(Shoot(shooter));
        }
    }

    private IEnumerator Shoot (EntityState shooter)
    {
        shooter.allowfire = false;
        var usePool = shooter.gameObject.name == "Player" ? _playerProjectiles : _enemyProjectiles;
        var projectileToUse = Array.Find(usePool, (projectile) => !projectile.gameObject.activeSelf);
        ProjectileState projectileState = shooter.ProjectileState;

        if (projectileToUse) {
            projectileToUse.transform.position = shooter.ProjectileGunBarrel.position;
            projectileToUse.gameObject.SetActive(true);
            projectileToUse.State = projectileState;

            var rigidbody = projectileToUse.GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(shooter.ProjectileGunBarrel.forward * projectileState.Speed, ForceMode.VelocityChange);

            StartCoroutine(DeactivateProjectile(projectileToUse, projectileState.Lifetime));
        }
        else
        {
            Debug.Log("Exceeding available projectiles, increase pool size or lower fire rate/projectile time to live");
        }

        yield return new WaitForSeconds(1f / projectileState.FireRate);

        shooter.allowfire = true;
    }

    private IEnumerator DeactivateProjectile (BaseProjectile projectile, float secondsToLive)
    {
        yield return new WaitForSeconds(secondsToLive);
        projectile.gameObject.SetActive(false);
    }
}