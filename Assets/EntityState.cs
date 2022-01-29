using System;
using UnityEngine;

public class EntityState : MonoBehaviour
{
    public ProjectileSystem projectileSystem;
    public bool allowfire = true;
    int health = 100;
    float projectileSpeedMod = 0f;
    float projectileSizeMod = 0f;
    float projectileRangeMod = 0f;
    float projectileSpreadMod = 0f;

    public void OnShoot() {
        Debug.Log("onShoot");
        projectileSystem.OnPlayerShoot();
    }

}
