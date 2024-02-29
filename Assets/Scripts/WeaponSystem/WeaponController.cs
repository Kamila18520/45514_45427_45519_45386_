using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawnerTransform;
    [SerializeField] float weaponCooldown;
    [SerializeField] AudioSource weaponShoot;

    private float lastShootTime;

    public bool Shoot()
    {
            if(IsCooldown()) return false;

            Instantiate(bulletPrefab, bulletSpawnerTransform.position, bulletSpawnerTransform.rotation);
             weaponShoot.Play();
            lastShootTime = Time.time;
            return true;

    }

    private bool IsCooldown()
    {
        return lastShootTime + weaponCooldown > Time.time;
    }
}
