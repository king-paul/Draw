using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    public Camera camera;
    public Transform revolver;
    public Transform tommahawk;
    public Transform crosshair;
    public Transform firingPoint;
    public TextMeshProUGUI ammoValue;

    //public GameObject fireIndicator;
    public GameManager gameManager;

    [Header("Gun Variables")]
    [Range(1, 50)]
    [SerializeField] int maxShots = 6;
    [Range(0, 10)]
    [SerializeField] float reloadTime = 3;

    [Space]
    [Header("Options")]
    [SerializeField]
    bool fireFromCamera = false;
    [SerializeField]
    bool showLineRenderer = false;

    [Header("Sound effects")]
    public AudioClip gunShotSound;
    public AudioClip reloadSound;
    public AudioClip[] ricochetSounds;
    public AudioClip dieSound;

    //float meleeRadius = 0.001f;
    private int shotsLeft;

    private LineRenderer line;
    private AudioSource audio;

    private Vector3 offset = new Vector3(0, 0, 5);

    // Start is called before the first frame update
    void Start()
    {
        if(showLineRenderer)
            line = revolver.GetComponent<LineRenderer>();
        
        audio = GetComponent<AudioSource>();

        shotsLeft = maxShots;
        ammoValue.text = shotsLeft.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawLine(camera.position, camera.position + camera.forward * 50, Color.blue);

        //UseMeleeWeapon();          
    }

    public void FireGun()
    {
        if (!gameManager.GameRunning || shotsLeft <= 0)
            return;

        //Debug.Log("Firing gun from position:" + revolver.position);        

        RaycastHit hit;
        Transform fireOrigin;

        if (fireFromCamera)
            fireOrigin = camera.transform;
        else
            fireOrigin = firingPoint;

        if (showLineRenderer)
        {
            line.positionCount = 2;
            line.SetPosition(0, fireOrigin.position);
        }

        // play gun shot sound
        if (audio != null && gunShotSound != null)
            audio.PlayOneShot(gunShotSound);

        //Debug.DrawRay(fireOrigin.position, fireOrigin.forward, Color.blue, 1);

        //Vector3 rayOrigin = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        Vector3 fireDirection = (crosshair.position - fireOrigin.position).normalized;

        if (Physics.Raycast(fireOrigin.position, fireDirection, out hit))
        {
            //Debug.Log("Raycast hit " + hit.transform.name);
            if (showLineRenderer)
                line.SetPosition(1, hit.point);

            var enemyObject = hit.transform.gameObject;

            if (enemyObject.layer == 6) // projectile layer
            {            
                var projectile = enemyObject.GetComponent<Projectile>();

                if (projectile.CanBeShot)
                {
                    if (audio != null && projectile.destroySound != null)
                    {
                        audio.PlayOneShot(projectile.destroySound);
                    }

                    gameManager.AddPoints(projectile.Points);
                    GameObject.Destroy(enemyObject);

                    //Debug.Log(enemyObject.name + "Was destroyed by revolver");
                    
                }
                else
                {
                    Debug.Log("Object cannot be shot");

                    if (ricochetSounds != null)
                    {
                        int random = Random.Range(0, ricochetSounds.Length);
                        audio.PlayOneShot(ricochetSounds[random]);
                    }
                        
                }
            }
            else if(ricochetSounds != null)
            {
                int random = Random.Range(0,ricochetSounds.Length);
                audio.PlayOneShot(ricochetSounds[random]);
            }
        }
        else
        {
            if (showLineRenderer)
                line.SetPosition(1, fireOrigin.position + (fireDirection * 50));
        }

        shotsLeft--;
        //Debug.Log("Shots Left: " + shotsLeft);
        ammoValue.text = shotsLeft.ToString();

        if (shotsLeft == 0)
            StartCoroutine(ReloadGun());

        //StartCoroutine(ShowFireText());

    }    

    public void ReleaseGun()
    {
        line.positionCount = 0;
    }

    IEnumerator ReloadGun()
    {
        if (reloadSound != null)
            audio.PlayOneShot(reloadSound);

        yield return new WaitForSeconds(reloadTime);
        shotsLeft = maxShots;
        audio.Stop();

        //Debug.Log("Gun Reloaded");
        //Debug.Log("Shots Left: " + shotsLeft);
        ammoValue.text = shotsLeft.ToString();
    }

    // Not currently used
    /*
    private void UseMeleeWeapon()
    {
        Ray ray = new Ray(tommahawk.position, transform.forward);
        RaycastHit hit;

        if (Physics.SphereCast(ray, meleeRadius, out hit))
        {
            GameObject enemyObject = hit.transform.gameObject;

            if (enemyObject.layer == 6)
            {
                var projectile = enemyObject.GetComponent<Projectile>();

                if (projectile.DestroyedByMelee)
                {
                    gameManager.AddPoints(projectile.Points);
                    GameObject.Destroy(enemyObject);
                    Debug.Log(enemyObject.name + "Was destroyed by tomahawk");
                }
                else
                {
                    Debug.Log("Object cannot be destroyed by tomahawk");
                }

            }
        }
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Debug.Log("Player was hit by a projectile");

            if (audio != null && dieSound != null)
                audio.PlayOneShot(dieSound);

            gameManager.LoseLife();
        }

    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(tommahawk.position + transform.forward, meleeRadius);
    }

    /*
    IEnumerator ShowFireText()
    {
        fireIndicator.SetActive(true);
        yield return new WaitForSeconds(2);
        fireIndicator.SetActive(false);
    }*/        
        

}
