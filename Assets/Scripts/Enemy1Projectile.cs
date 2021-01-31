using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Projectile : Projectile
{

    protected override void OnContact(Collision collision)
    {
        base.OnContact(collision);
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().LooseLife(damageDealt);
            DestroySelf();
            Debug.Log("oui");
        }
    }
}
