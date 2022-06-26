using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Pathfinding;

public class Ruler : MonoBehaviour
{
    // Movement and Player Locating
    public AIPath aiPath;               // A* Pathfinding for Unity https://arongranberg.com/astar/
    private WaveManager WaveManager;
    private PlayerStats playerStats;
    private Rigidbody2D rulerRigidbody;

    // Used in AimRuler() to determine the player's position
    private Transform playerTransform;  
    private Vector2 playerPos;          
    private Vector2 rulerPos;       

    // Bools
    private bool playerInRange;
    private bool charging;
    private bool rulerEnraged;
    private bool dead;

    // Stats
    public float health;                  
    public float damage;
    private float moveSpeed;

    private float chargePrepTime;       // Used to determine how long the ruler preps to attack
    private float chargePrepTimer;

    private float chargeTime;           // Used to determine how long the ruler attacks
    private float chargeTimer;

    // UI and Audiovisual Flair
    private Text healthText;
    private AudioSource rulerAudio;
    private ParticleSystem rulerParticles;
    public GameObject[] bits;
    


    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        WaveManager = GameObject.FindGameObjectWithTag("WaveManager").GetComponent<WaveManager>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        rulerRigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        healthText = this.gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
        rulerAudio = this.gameObject.GetComponent<AudioSource>();
        rulerParticles = this.transform.GetChild(1).GetComponent<ParticleSystem>();

        InstantiateStats();
    }



    // Sets all the ruler's stats (taken from WaveManager.cs)
    void InstantiateStats()
    {
        health = WaveManager.rulerStats[0];
        damage = WaveManager.rulerStats[1];
        moveSpeed = WaveManager.rulerStats[2];
        chargePrepTime = WaveManager.rulerStats[3];
        chargeTime = WaveManager.rulerStats[4];

        chargeTimer = chargeTime;
        chargePrepTimer = chargePrepTime;

        aiPath.maxSpeed = moveSpeed;
    }



    void FixedUpdate() 
    {
        // Uses a raycast to determine if the player is in range or not
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up), 8f);

        if (hit.collider != null && hit.transform.tag == "Player")
        {
            playerInRange = true;
        } else {
            playerInRange = false;
        }

        // Can be in three possible states: Charging, Player Not in Range, Player in Range
        // Charging:            Rushes the player for a predetermined amount of time. Can not exit 
        //                          charge once inside until timer runs out.                
        //
        // Player Not in Range: Uses A* Pathfinding to find the most efficient way to the player.
        //
        // Player in Range:     Uses my old "AI" to move slowly towards the player (if not, A* 
        //                          flickers on and off). Also reduces the charge prep timer, and 
        //                          when it reaches 0, the ruler charges.
                                  
        if (!charging)
        {
            rulerRigidbody.freezeRotation = false;
            AimRuler();

            if (playerInRange)
            {
                aiPath.enabled = false;

                Vector2 newPos = new Vector2 (rulerRigidbody.position.x - this.transform.right.x / 50f, rulerRigidbody.position.y - this.transform.right.y / 50f);

                rulerRigidbody.MovePosition(newPos + playerPos * (0.1f * moveSpeed) * Time.fixedDeltaTime);

                chargePrepTimer -= Time.deltaTime;

                if (chargePrepTimer <= 0f)
                {
                    charging = true;
                    chargePrepTimer = chargePrepTime;
                }

            } else {
                aiPath.enabled = true;
            }
        } else {

            rulerRigidbody.freezeRotation = true;
            
            chargeTimer -= Time.deltaTime;

            if (chargeTimer <= 0f)
            {
                charging = false;
                chargeTimer = chargeTime;
            }

            rulerRigidbody.MovePosition(rulerRigidbody.position + new Vector2(this.transform.up.x, this.transform.up.y) * (moveSpeed * 5f) * Time.fixedDeltaTime); 
        }

        // Kills if health is depleted
        if (health <= 0)
        {
            KillRuler();
        } else {
            healthText.text = health.ToString();
        }
    }



    // Ruler "looks" towards player
    void AimRuler()
    {
        playerPos = playerTransform.position;
        rulerPos = this.transform.position;

        // Compares player and enemy positions, stores in playerPos
        playerPos.x = playerPos.x - rulerPos.x;
        playerPos.y = playerPos.y - rulerPos.y;

        // Determines the angle needed to rotate
        float angle = Mathf.Atan2(playerPos.y, playerPos.x) * Mathf.Rad2Deg;

        // Angle - 90 in Vector3 due to sprite being vertical and rotate towards pointing right, not up
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle - 90)), Time.deltaTime * 750f);
    }



    void KillRuler()
    {
        if (!dead)
        {
            dead = true;
            gameObject.tag = "Untagged";
            this.GetComponent<SpriteRenderer>().enabled = false;
            
            foreach(Transform child in transform)
            {
                if (child.GetComponent<ParticleSystem>() == null)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
            
            aiPath.enabled = false;
            rulerRigidbody.isKinematic = false;
            this.GetComponent<BoxCollider2D>().enabled = false;

            playerStats.enemiesKilled += 1;
            playerStats.enemiesLeft -= 1;
            playerStats.money += Random.Range(1, 10);

            for (int i = 0; i < bits.Length; i++)
            {
                if (!rulerEnraged)
                {
                    Instantiate(bits[i], this.transform.position + new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0f), this.transform.rotation);
                } else {
                    GameObject tempBit = Instantiate(bits[i], this.transform.position + new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0f), this.transform.rotation);
                    tempBit.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
        } else {
            if (!rulerAudio.isPlaying && !rulerParticles.isPlaying)
            {
                Destroy(this.gameObject);
            }
        }
    }
    
    public void Damaged()
    {
        rulerAudio.Play();
        if (!rulerEnraged)
        {
            rulerParticles.Play();
        } else {
            rulerParticles.startColor = Color.red;
            rulerParticles.Play();
        }
    }

    // Used to make a boss version of the ruler
    public void Enrage()
    {
        health *= 2;
        damage *= 2;
        moveSpeed *= 1.5f;
        aiPath.maxSpeed *= moveSpeed;
        chargeTime *= 2;
        chargePrepTime /= 2;
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        rulerEnraged = true;
    }
}