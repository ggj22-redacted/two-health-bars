using System;
using UnityEngine;

public class ProjectileSystem : MonoBehaviour
{
    public BaseProjectile baseProjectilePrefab;
    public int maxPlayerProjectiles;
    public int maxEnemyProjectiles;
    private GameObject[] enemyProjectiles;
    private GameObject[] playerProjectiles;

    // Start is called before the first frame update
    void Start()
    {
        InstantiatePool(playerProjectiles, maxPlayerProjectiles, baseProjectilePrefab);
        InstantiatePool(enemyProjectiles, maxEnemyProjectiles, baseProjectilePrefab);
    }

    void InstantiatePool(GameObject[] pool, int amount, BaseProjectile projectilePrefab)
    {
        pool = new GameObject[amount];
        for (int i = 0; i < amount; i++) {
            var inst = Instantiate(projectilePrefab);
            inst.gameObject.SetActive(false);
            pool[i] = inst.gameObject;
            inst.onProjectileHit += handleProjectileCollision;
        }
    }

    public void handleProjectileCollision(BaseProjectile projectile, Collider targetHit) {
        projectile.gameObject.SetActive(false);

        if (targetHit.gameObject.name == "player"
        || targetHit.gameObject.name == "horn"
        || targetHit.gameObject.name == "halo")
        {
            //targetHit.gameObject.GetComponent<HealthSystem>()
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(GameObject shooter) {
        var projectileToUse = Array.Find( 
            shooter.name == "Player" ? playerProjectiles : enemyProjectiles,
            (projectile) => !projectile.activeSelf);

        projectileToUse.transform.position = shooter.transform.position;
        projectileToUse.GetComponent<Rigidbody>().AddForce(shooter.transform.forward * 10f);
        projectileToUse.SetActive(true);
    }

}
