using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : EnemyController
{
    [SerializeField] private GameObject projectile;
    public Enemy1()
    {
        lifePoint = 10;
        damage = 2;
        distanceToShoot = 10f;
        distanceToChase = 100f;
        shootSpeed = 0.4f;
        startShootTime = 1.0f;
        speed = 1.0f;
        maxTimeStunned = 0.0f;
    }

    protected override void Shoot()
    {
        Vector3 distance = Vector3.Normalize(cible.position - transform.position);
        Vector3 posToShootFrom = transform.position + distance;
        Vector3 shootDirection = cible.position - posToShootFrom;

        projectile.GetComponent<Projectile>().projectileDirection = shootDirection;
        projectile.GetComponent<Projectile>().damageDealt = damage;
        Instantiate(projectile, posToShootFrom, Quaternion.identity);

        PlaySound(attackClip);
        LaunchAnimation(shootAnimation);
    }

}
