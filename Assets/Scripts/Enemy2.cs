using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : EnemyController
{

    [Header("GrabAnimation")]
    [SerializeField] protected float grabTimeBetweenFrame = 0.1f;
    [SerializeField] protected List<Material> grabFrames;
    protected AnimationWithMaterial grabAnimation;

    protected override void Start()
    {
        base.Start();

        grabAnimation = new AnimationWithMaterial();
        grabAnimation.frames = grabFrames;
        grabAnimation.timeBetweenFrame = grabTimeBetweenFrame;
    }

    public Enemy2()
    {
        lifePoint = 5;
        damage = 1;
        distanceToShoot = 5f;
        distanceToChase = 100f;
        shootSpeed = 3f;
        startShootTime = 2f;
        speed = 1.5f;
        maxTimeStunned = 5.0f;
    }

    public override void Stunne()
    {
        base.Stunne();
        LaunchAnimation(grabAnimation);
    }

    protected override void Shoot()
    {
        Vector3 distance = Vector3.Normalize(cible.position - transform.position);
        Vector3 posToShootFrom = transform.position + distance;
        Vector3 shootDirection = cible.position - posToShootFrom;

        RaycastHit hit;
        if (Physics.Raycast(posToShootFrom, shootDirection, out hit,distanceToShoot))
        {
            if (hit.collider.tag == "Player")
            {
                hit.collider.gameObject.GetComponent<PlayerController>().LooseLife(damage);
            }
        }

        PlaySound(attackClip);
        LaunchAnimation(shootAnimation);
    }
}
