using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(AudioSource))]
public class MeleeWeapon : MonoBehaviour
{
    AudioSource audio;
    GameObject manager;
    GameManager gameManager;

    [SerializeField] Vector3 offsetFromCamera = new Vector3(0, 0, -1);

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        manager = GameObject.Find("GameManager");

        if(manager != null)
            gameManager = manager.GetComponent<GameManager>();        
    }

    private void OnCollisionEnter(Collision collision)
    {
        var enemyObject = collision.gameObject;

        if (enemyObject.layer == 6) // projectile layer
        {
            var projectile = enemyObject.GetComponent<Projectile>();

            if (projectile.DestroyedByMelee)
            {
                gameManager.AddPoints(projectile.Points);
                GameObject.Destroy(enemyObject);
                //Debug.Log(enemyObject.name + "Was destroyed by tomahawk"); 
                
                if(audio != null)
                    audio.Play();
            }
            else
            {
                Debug.Log("Object cannot be destroyed by tomahawk");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

}
