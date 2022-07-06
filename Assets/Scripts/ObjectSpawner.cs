using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    #region variable delcaration
    public GameManager gameManager;
    public GameObject objectToSpawn;

    [SerializeField][Range(0.1f, 30f)]
    float initialTimeDelay = 1.0f;
    [SerializeField][Range(0.1f, 30f)]
    float timeBetweenSpawns = 3.0f;

    [Header("Time Scales")]
    [SerializeField][Range(0.1f, 10)]
    float travelSpeed = 1;
    [SerializeField] [Range(0.1f, 2)]
    float delayScale = 1;

    [Header("Curved Motion")]
    [Tooltip("Changes the arc of objects thrown at a curved motion")]
    [SerializeField][Range(0, 25f)]
    float throwHeight = 10;
    [SerializeField]
    bool showPath = true;

    // private variables
    private Transform player;
    private LaunchData data;
    private Projectile projectile;
    private float h;

    private float initialTravelSpeed;
    private float initialDelayScale;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        initialTravelSpeed = travelSpeed;
        initialDelayScale = delayScale;

        Invoke("SpawnObject", initialTimeDelay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region public functions
    // Setters
    public void IncreaseTravelSpeed(float percent)
    {
        travelSpeed += initialTravelSpeed * (percent / 100);
    }

    public void ReduceTimeDelay(float percent)
    {
        delayScale -= initialDelayScale * (percent / 100);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public LaunchData CalculateLaunchData()
    {
        float gravity = Physics.gravity.y;
        h = throwHeight;

        float displacementY = player.position.y - transform.position.y;
        Vector3 displacementXZ = new Vector3(player.position.x - transform.position.x, 0, player.position.z - transform.position.z);
        float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);

        // Uup = sqrt(-2gh)
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);

        // Uright = Px / ( sqrt(-2h/g) + sqrt(2(Py-h)/g) )
        Vector3 velocityXZ = displacementXZ / time;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
    }
    #endregion

    #region private functions
    void SpawnObject()
    {      
        var spawnedObject = Instantiate(objectToSpawn, transform.position, transform.rotation);
        projectile = spawnedObject.GetComponent<Projectile>();

        projectile.TimeScale = travelSpeed;

        if (projectile.GetType().Equals(typeof(CurvedProjectile)))
            ((CurvedProjectile)projectile).ThrowHeight = throwHeight;

        gameManager.PlayRandomSpeech();

        Invoke("SpawnObject", timeBetweenSpawns * delayScale);
    }

    private void OnDrawGizmos()
    {
        player = GameObject.FindWithTag("Player").transform;

        if (!showPath || player == null)
            return;

        // check if the object is to be thrown in a curve
        if (!objectToSpawn.GetComponent<CurvedProjectile>())
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, player.position);
            return;
        }

        LaunchData launchData = CalculateLaunchData();
        Vector3 previousDrawPoint = transform.position;

        int resolution = 30;

        for (int i = 1; i <= resolution; i++)
        {
            float simulationTime = i / (float)resolution * launchData.timeToTarget;

            // s = ut + (at^2)/2
            Vector3 displacement = launchData.initialVelocity * simulationTime + Physics.gravity * simulationTime * simulationTime / 2f;
            Vector3 drawPoint = transform.position + displacement;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(previousDrawPoint, drawPoint);
            previousDrawPoint = drawPoint;
        }
    }
    #endregion
}

public struct LaunchData
{
    public readonly Vector3 initialVelocity;
    public readonly float timeToTarget;

    public LaunchData(Vector3 initialVelocity, float timeToTarget)
    {
        this.initialVelocity = initialVelocity;
        this.timeToTarget = timeToTarget;
    }

}