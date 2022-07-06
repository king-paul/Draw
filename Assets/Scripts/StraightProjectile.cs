using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightProjectile : Projectile
{
    [Header("Straight projectile properties")]
    [SerializeField]
    private float force = 100;

    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        direction = (player.position - transform.position).normalized;

        rigidbody.AddForce(direction * force);
    }

}
