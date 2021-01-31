using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandProjectile : Projectile
{
    [SerializeField] private GameObject droppedItem;
    private GameObject dropPos;
    private bool onEnnemi = false;

    protected override void Start()
    {
        base.Start();
        dropPos = GameObject.Find("DropPos");
    }

    protected override void DestroySelf()
    {
        if (!onEnnemi)
        {
            Instantiate(droppedItem, dropPos.transform.position, Quaternion.identity);
        }
        base.DestroySelf();
    }

    protected override void OnContact(Collision collision)
    {
        base.OnContact(collision);
        if (collision.gameObject.CompareTag("Ennemi"))
        {
            EnemyController ennemi = collision.gameObject.GetComponent<EnemyController>();
            ennemi.LooseLife(damageDealt);
            ennemi.Stunne();
            onEnnemi = true;
            if (ennemi.GetLife() <= 0 )
            {
                Instantiate(droppedItem, dropPos.transform.position, Quaternion.identity);
            }
            DestroySelf();
        }

    }
}
