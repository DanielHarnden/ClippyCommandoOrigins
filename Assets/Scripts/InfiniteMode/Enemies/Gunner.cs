using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Pathfinding;

public class Gunner : MonoBehaviour
{
    // Movement and Player Locating
    public AIPath aiPath;               // A* Pathfinding for Unity https://arongranberg.com/astar/
    private WaveManager WaveManager;
    private PlayerStats playerStats;

    // Used in AimGunner() to determine the player's position
    private Transform playerTransform;  
    private Vector2 playerPos;          
    private Vector2 gunnerPos;       

    // Bools
    private bool playerInRange;
    private bool canShoot;

    // Stats
    public float health;                  
    public float damage;
    private float moveSpeed;
    private float bulletSpeed;
    private Transform staplerBarrel;

    private float shootTime;
    private float shootTimer;

    public Transform staplePrefab;
    
    // UI and Audiovisual Flair
    private Text healthText;
    private AudioSource gunnerAudio;
    private ParticleSystem gunnerParticles;
    


    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        WaveManager = GameObject.FindGameObjectWithTag("WaveManager").GetComponent<WaveManager>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        staplerBarrel = this.gameObject.transform.GetChild(2).GetComponent<Transform>();
        healthText = this.gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
        gunnerAudio = this.gameObject.GetComponent<AudioSource>();
        gunnerParticles = this.transform.GetChild(1).GetComponent<ParticleSystem>();

        InstantiateStats();
    }



    // Sets all the ruler's stats (taken from WaveManager.cs)
    void InstantiateStats()
    {
        health = WaveManager.gunnerStats[0];
        damage = WaveManager.gunnerStats[1];
        moveSpeed = WaveManager.gunnerStats[2];
        bulletSpeed = WaveManager.gunnerStats[3];
        shootTime = WaveManager.gunnerStats[4];
        shootTimer = shootTime;

        aiPath.maxSpeed = moveSpeed;
    }



    void FixedUpdate() 
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

        // Doesn't get too close to the player
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
        } else {
            ShootGunner();
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



    // Gunner shoots at the player
    void ShootGunner()
    {
        if (playerInRange)
        {
            playerPos = playerTransform.position;
            gunnerPos = this.transform.position;

            Rigidbody2D newBul = Instantiate(staplePrefab, staplerBarrel.transform.position,   this.transform.rotation).GetComponent<Rigidbody2D>();
            // Addforce has to be normalized so it shoots correctly
            newBul.AddForce((playerPos - gunnerPos).normalized * bulletSpeed);

            canShoot = false;
        } 
    }



    void KillGunner()
    {
        playerStats.enemiesKilled += 1;
        playerStats.enemiesLeft -= 1;
        playerStats.money += Random.Range(5, 10);

        Destroy(this.gameObject);
    }
    
    public void Damaged()
    {
        gunnerAudio.Play();
        gunnerParticles.Play();
    }
}