using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : Element2DController
{
    protected Rigidbody rigidBody;
    [SerializeField] string collectibleName;

    protected override void Start()
    {
        base.Start();
        name = collectibleName;
    }

    public virtual void Collect()
    {
        Destroy(gameObject);
    }

}
