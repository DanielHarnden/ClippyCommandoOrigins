using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Pathfinding;

public class StoryRuler : MonoBehaviour
{
    // Movement and Player Locating
    public AIPath aiPath;               // A* Pathfinding for Unity https://arongranberg.com/astar/

    private Transform playerTransform;  // Used in AimRuler() to determine the player's position
    private Vector2 playerPos;          
    private Vector2 rulerPos;    
    private Vector2 idlePos;  
    private Vector2 tempPos; 
    
    private Rigidbody2D rulerRigidbody; // Rigidbody used for movement

    // Bools
    private bool playerInRange;         // Determines if the player is in range
    private bool charging;              // Determines if the ruler is currently charging
    public bool playerSeen;

    // Stats
    public float health;                  
    public float damage;
    public float moveSpeed;

    public float chargePrepTime;       // Used to determine how long the ruler preps to attack
    private float chargePrepTimer;

    public float chargeTime;           // Used to determine how long the ruler attacks
    private float chargeTimer;

    public float idleTime;           // Used to determine how long the ruler attacks
    private float idleTimer;

    // UI and Audiovisual Flair
    private Text healthText;
    private AudioSource rulerAudio;
    private ParticleSystem rulerParticles;
    


    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        rulerRigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        healthText = this.gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
        rulerAudio = this.gameObject.GetComponent<AudioSource>();
        rulerParticles = this.transform.GetChild(1).GetComponent<ParticleSystem>();

        InstantiateStats();
    }



    // Sets all the ruler's stats (taken from PlayerStats.cs)
    void InstantiateStats()
    {
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

        if (playerSeen)
        {   
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
        } else {
            aiPath.enabled = false;

            if (idleTimer > 0)
            {
                idleTimer -= Time.deltaTime;
            } else {
                idlePos = this.transform.position + new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), 0f);
                idleTimer = idleTime;
                
            }

            IdleRuler();
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

    void IdleRuler()
    {
        rulerPos = this.transform.position;

        // Compares player and enemy positions, stores in playerPos
        tempPos.x = idlePos.x - rulerPos.x;
        tempPos.y = idlePos.y - rulerPos.y;

        // Determines the angle needed to rotate
        float angle = Mathf.Atan2(tempPos.y, tempPos.x) * Mathf.Rad2Deg;

        // Angle - 90 in Vector3 due to sprite being vertical and rotate towards pointing right, not up
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle - 90)), Time.deltaTime * 750f);

        rulerRigidbody.MovePosition(rulerRigidbody.position + tempPos * (moveSpeed * 0.5f) * Time.fixedDeltaTime);
    }



    void KillRuler()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsStory>().enemiesLeft -= 1;
        Destroy(this.gameObject);
    }
    
    public void Damaged()
    {
        playerSeen = true;
        rulerAudio.Play();
        rulerParticles.Play();
    }

    public void Enrage()
    {
        health *= 2;
        damage *= 2;
        moveSpeed *= 1.5f;
        aiPath.maxSpeed *= moveSpeed;
        chargeTime *= 2;
        chargePrepTime /= 2;
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            if (!playerSeen)
            {
                AlertFriends();
            }
        }
    }

    public void AlertFriends()
    {
        if (!playerSeen)
        {
            playerSeen = true;

            // Could add sound effect here

            Collider2D[] allOverlappingColliders = Physics2D.OverlapCircleAll(this.transform.position, 5);

            foreach(Collider2D friend in allOverlappingColliders)
            {
                StoryRuler friendAI = friend.GetComponentInParent<StoryRuler>();

                if (friendAI != null)
                {
                    friendAI.AlertFriends();
                    friendAI.playerSeen = true;
                }
            }
        }
    }
}
