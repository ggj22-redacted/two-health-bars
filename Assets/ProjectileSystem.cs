using System.Collections;
using System;
using UnityEngine;

public class ProjectileSystem : MonoBehaviour
{
    public EntityState player;
    public Transform gunBarrel;
    public BaseProjectile baseProjectilePrefab;
    public int maxPlayerProjectiles;
    public int maxEnemyProjectiles;
    private GameObject[] enemyProjectiles;
    private GameObject[] playerProjectiles;

    // Start is called before the first frame update
    void Start()
    {
        InstantiatePool(ref playerProjectiles, maxPlayerProjectiles, baseProjectilePrefab, 9);
        InstantiatePool(ref enemyProjectiles, maxEnemyProjectiles, baseProjectilePrefab, 8);
    }

    void InstantiatePool(ref GameObject[] pool, int amount, BaseProjectile projectilePrefab, int layerID)
    {
        pool = new GameObject[amount];
        for (int i = 0; i < amount; i++) {
            var inst = Instantiate(projectilePrefab);
            inst.gameObject.SetActive(false);
            inst.gameObject.layer = layerID;
            pool[i] = inst.gameObject;
            inst.onProjectileHit += HandleProjectileCollision;
        }
    }

    public void HandleProjectileCollision(BaseProjectile projectile, Collider targetHit) {
        Debug.Log("Projectile HIT");
        Debug.Log(targetHit.name);
        projectile.gameObject.SetActive(false);

        if (targetHit.gameObject.name == "player"
        || targetHit.gameObject.name == "horn"
        || targetHit.gameObject.name == "halo")
        {
            //targetHit.gameObject.GetComponent<HealthSystem>()
        }
    }

    public void OnPlayerShoot() {
        StartCoroutine(Shoot(player, gunBarrel));
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator Shoot(EntityState shooter, Transform gunBarrel) {
        shooter.allowfire = false;
        var usePool = shooter.gameObject.name == "Player" ? playerProjectiles : enemyProjectiles;
        var projectileToUse = Array.Find(usePool, (projectile) => !projectile.activeSelf);

        if(projectileToUse) {
            projectileToUse.transform.position = gunBarrel.position;
            projectileToUse.SetActive(true);
            var rigidbody = projectileToUse.GetComponent<Rigidbody>();
            rigidbody.velocity = new Vector3(0,0,0);
            rigidbody.AddForce(gunBarrel.forward.normalized * 10f);
            StartCoroutine(DeactivateProjectile(projectileToUse, 1.0f));
        }
        else
        {
            Debug.Log("Exceeding available projectiles, increase pool size or lower fire rate/projectile time to live");
        }

        yield return new WaitForSeconds(1);
        shooter.allowfire = true;
    }

    IEnumerator DeactivateProjectile(GameObject projectile, float secondsToLive) {
        yield return new WaitForSeconds(secondsToLive);
        projectile.SetActive(false);
    }

}
