using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Projectile : MonoBehaviour
{
    [SerializeField] int pointsWhenDestroyed = 100;

    [Header("Destruction Properties")]
    [SerializeField] bool canBeShot = true;
    [SerializeField] bool destroyedByMelee = true;

    protected Transform player;
    protected Rigidbody rigidbody;

    private AudioSource audio;

    [Header("Sound Effects")]
    public AudioClip spawnSound;
    public AudioClip destroySound;

    // properties
    public float TimeScale { get; set; } = 1f;
    public int Points { get => pointsWhenDestroyed; }

    public bool CanBeShot { get => canBeShot; }
    public bool DestroyedByMelee { get => destroyedByMelee; }

    // Start is called before the first frame update
    protected void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rigidbody = GetComponent<Rigidbody>();

        audio = GetComponent<AudioSource>();

        if (audio != null && spawnSound != null)
            audio.PlayOneShot(spawnSound);
    }

    private void OnCollisionEnter(Collision collision)
    {       
        if (audio != null && destroySound != null)
            audio.PlayOneShot(destroySound);

        GameObject.Destroy(this.gameObject);
    }
   
}