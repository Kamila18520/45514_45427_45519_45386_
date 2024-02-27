using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawnerTransform;
    [SerializeField] float weaponCooldown;

    private float lastShootTime;

    public bool Shoot()
    {
            if(IsCooldown()) return false;

            Instantiate(bulletPrefab, bulletSpawnerTransform.position, bulletSpawnerTransform.rotation);
            lastShootTime = Time.time;
            return true;

    }

    private bool IsCooldown()
    {
        return lastShootTime + weaponCooldown > Time.time;
    }
}
