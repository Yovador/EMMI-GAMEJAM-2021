using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : Projectile
{
    [HideInInspector] public float hitboxDuration;
    private List<GameObject> ennemiAlreadyHit = new List<GameObject>();
    private UIManager uiManager;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(hitboxLife());
        uiManager = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
    }

    protected override void OnContact(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ennemi"))
        {
            if (collision.gameObject.GetComponent<EnemyController>().IsAlive())
            {
                collision.gameObject.GetComponent<EnemyController>().LooseLife(damageDealt);
                ennemiAlreadyHit.Add(collision.gameObject);
            }
        }
    }

    IEnumerator hitboxLife()
    {
        yield return new WaitForSecondsRealtime(hitboxDuration);
        uiManager.ResetLeftHand();
        DestroySelf();
    }

}
