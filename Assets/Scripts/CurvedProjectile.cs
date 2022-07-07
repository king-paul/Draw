using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedProjectile : Projectile
{
    [Header("Curved Motion Properties")]
    [SerializeField][Range(0.01f, 1)]
    protected float speed = 1;

    public float ThrowHeight { get; set; }

    LaunchData launchData;
    float simulationTime = 0;
    Vector3 initialPosition;
    float weight;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        //rigidbody.velocity = CalculateLaunchData().initialVelocity;

        launchData = CalculateLaunchData();
        initialPosition = transform.position;

        weight = rigidbody.mass * Physics.gravity.y + rigidbody.drag;
    }

    // Update is called once per frame
    void Update()
    {
        // increment the total time by the frame time multiplied by the scale
        simulationTime += Time.deltaTime * TimeScale * speed;

        // calculate the displacement from current from starting position
        // = v * t + (gravity * t^2 / 2)
        Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * weight * simulationTime * simulationTime / 2.0f;

        // update the position
        transform.position = initialPosition + displacement;
    }

    LaunchData CalculateLaunchData()
    {
        float gravity = Physics.gravity.y;
        float h = ThrowHeight;

        float displacementY = player.position.y - transform.position.y;
        Vector3 displacementXZ = new Vector3(player.position.x - transform.position.x, 0, player.position.z - transform.position.z);
        float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);

        // Uup = sqrt(-2gh)
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);

        // Uright = Px / ( sqrt(-2h/g) + sqrt(2(Py-h)/g) )
        Vector3 velocityXZ = displacementXZ / time;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
    }

}
