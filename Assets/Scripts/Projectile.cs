using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Element2DController
{
    
    [HideInInspector] public int damageDealt;
    [SerializeField] protected float speed;
    protected Rigidbody rigidBody;
    [HideInInspector] public Vector3 projectileDirection = Vector3.zero;



    protected override void Start()
    {
        base.Start();
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(projectileDirection.normalized * speed, ForceMode.Impulse);
    }

    protected override void Update()
    {

        base.Update();

        Move();
    }

    protected virtual void Move()
    {
        rigidBody.AddForce(projectileDirection.normalized * speed);
    }

    protected virtual void DestroySelf()
    {
        Destroy(gameObject);
    }

    protected virtual void OnContact(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            DestroySelf();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {


        OnContact(collision);

    }

}
