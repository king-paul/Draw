using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeleeWeapon : MonoBehaviour
{
    //public AudioClip collisionSound;

    AudioSource audio;

    GameManager gameManager;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();        
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
                ///Debug.Log(enemyObject.name + "Was destroyed by tomahawk"); 
                
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
