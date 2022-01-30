using System.Collections;
using System;
using System.Collections.Generic;
using Game.Common.Projectiles;
using UnityEngine;

public class ProjectileSystem : MonoBehaviour
{
    public BaseProjectile projectilePrefab;

    [Header("Settings")]
    public int maxGameProjectiles;

    [SerializeField]
    private int maxInstantiationsPerFrame = 10;

    private BaseProjectile[] _projectilePool;

    private void Start ()
    {
        _projectilePool = new BaseProjectile[maxGameProjectiles];

        StartCoroutine(
            InstantiatePool(_projectilePool, projectilePrefab));
    }

    private IEnumerator InstantiatePool (IList<BaseProjectile> pool, BaseProjectile projectilePrefab)
    {
        int currentInstantiations = 0;
        for (int i = 0; i < pool.Count; i++) {
            var inst = Instantiate(projectilePrefab);
            inst.OnProjectileHit += HandleProjectileCollision;

            GameObject o = inst.gameObject;
            o.SetActive(false);

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
        var projectileToUse = Array.Find(_projectilePool, (projectile) => !projectile.gameObject.activeSelf);
        ProjectileState projectileState = shooter.ProjectileState;

        if (projectileToUse) {

            projectileToUse.gameObject.layer = shooter.gameObject.name == "Player"
                ? LayerMask.NameToLayer("PlayerProjectiles")
                : LayerMask.NameToLayer("EnemyProjectiles");

            projectileToUse.rendererComponent.material = projectileState.material;
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