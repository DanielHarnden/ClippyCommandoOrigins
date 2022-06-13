using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Pathfinding;

public class StoryGunner : MonoBehaviour
{
    // Movement and Player Locating
    public AIPath aiPath;               // A* Pathfinding for Unity https://arongranberg.com/astar/

    private Transform playerTransform;  // Used in AimGunner() to determine the player's position
    private Vector2 playerPos;          
    private Vector2 gunnerPos;
    private Vector2 idlePos;  
    private Vector2 tempPos;        
    
    private Rigidbody2D gunnerRigidbody; // Rigidbody used for movement

    // Bools
    private bool playerInRange;         // Determines if the player is in range
    private bool canShoot;
    public bool playerSeen;

    public float idleTime;           // Used to determine how long the gunner attacks
    private float idleTimer;

    // Stats
    public float health;                  
    public float damage;
    private float moveSpeed = 2f;
    private float bulletSpeed = 500f;

    public Transform staplerBarrel;
    public Transform staplePrefab;
    
    private float shootTime = 2.5f;
    private float shootTimer;

    // UI and Audiovisual Flair
    private Text healthText;
    private AudioSource gunnerAudio;
    private ParticleSystem gunnerParticles;
    


    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        gunnerRigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        healthText = this.gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
        gunnerAudio = this.gameObject.GetComponent<AudioSource>();
        gunnerParticles = this.transform.GetChild(1).GetComponent<ParticleSystem>();

        InstantiateStats();
    }



    void InstantiateStats()
    {
        aiPath.maxSpeed = moveSpeed;
    }



    void FixedUpdate() 
    {
        if (playerSeen)
        {
            playerPos = playerTransform.position;
            gunnerPos = this.transform.position;

            // Uses a raycast to determine if the player is in range or not
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.down), 25f);

            if (hit.collider != null && hit.transform.tag == "Player")
            {
                playerInRange = true;
            } else {
                playerInRange = false;
            }

            if (Vector2.Distance(gunnerPos, playerPos) <= 15 && playerInRange)
            {
                aiPath.enabled = false;
            } else {
                aiPath.enabled = true;
            }


            AimGunner();

            if (!canShoot)
            {
                shootTimer -= Time.deltaTime;

                if (shootTimer <= 0f)
                {
                    canShoot = true;
                    shootTimer = shootTime;
                }
            }
                
            if (playerInRange && canShoot)
            {
                playerPos = playerTransform.position;
                gunnerPos = this.transform.position;

                Rigidbody2D newBul = Instantiate(staplePrefab, staplerBarrel.transform.position,   this.transform.rotation).GetComponent<Rigidbody2D>();
                newBul.AddForce((playerPos - gunnerPos).normalized * bulletSpeed);

                canShoot = false;
            } 

        } else {
            aiPath.enabled = false;

            if (idleTimer > 0)
            {
                idleTimer -= Time.deltaTime;
            } else {
                idlePos = this.transform.position + new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), 0f);
                idleTimer = idleTime;

                Rigidbody2D newBul = Instantiate(staplePrefab, staplerBarrel.transform.position,   this.transform.rotation).GetComponent<Rigidbody2D>();
                newBul.AddForce(-staplerBarrel.transform.up * bulletSpeed);
            }

            IdleGunner();
        }

        // Kills if health is depleted
        if (health <= 0)
        {
            KillGunner();
        } else {
            healthText.text = health.ToString();
        }
    }



    // Gunner "looks" towards player
    void AimGunner()
    {
        // Compares player and enemy positions, stores in playerPos
        playerPos.x = playerPos.x - gunnerPos.x;
        playerPos.y = playerPos.y - gunnerPos.y;

        // Determines the angle needed to rotate
        float angle = Mathf.Atan2(playerPos.y, playerPos.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle + 90)), Time.deltaTime * 750f);
    }

    void IdleGunner()
    {
        gunnerPos = this.transform.position;

        // Compares player and enemy positions, stores in playerPos
        tempPos.x = idlePos.x - gunnerPos.x;
        tempPos.y = idlePos.y - gunnerPos.y;

        // Determines the angle needed to rotate
        float angle = Mathf.Atan2(tempPos.y, tempPos.x) * Mathf.Rad2Deg;

        // Angle - 90 in Vector3 due to sprite being vertical and rotate towards pointing right, not up
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle - 90)), Time.deltaTime * 750f);

        gunnerRigidbody.MovePosition(gunnerRigidbody.position + tempPos * (moveSpeed * 0.5f) * Time.fixedDeltaTime);
    }



    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            playerSeen = true;
        }
    }

    void KillGunner()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsStory>().enemiesLeft -= 1;
        Destroy(this.gameObject);
    }
    
    public void Damaged()
    {
        gunnerAudio.Play();
        gunnerParticles.Play();
    }
}
