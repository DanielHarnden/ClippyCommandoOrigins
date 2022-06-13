using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Pathfinding;

public class Gluer : MonoBehaviour
{
    // Movement and Player Locating
    public AIPath aiPath;               // A* Pathfinding for Unity https://arongranberg.com/astar/
    private WaveManager WaveManager;
    private PlayerStats playerStats;

    // Used in AimGluer() to determine the player's position
    private Transform playerTransform;  
    private Vector2 playerPos;          
    private Vector2 gluerPos;       

    // Used to calculate gluer's speed
    private float distanceBetween;
    
    // Stats
    public float health;                  
    public float damage;
    private float moveSpeed;

    public GameObject gluePuddle;       // Glue puddle prefab 

    // UI and Audiovisual Flair
    private Text healthText;
    private AudioSource gluerAudio;
    private ParticleSystem gluerParticles;
    


    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        WaveManager = GameObject.FindGameObjectWithTag("WaveManager").GetComponent<WaveManager>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        healthText = this.gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
        gluerAudio = this.gameObject.GetComponent<AudioSource>();
        gluerParticles = this.transform.GetChild(1).GetComponent<ParticleSystem>();

        InstantiateStats();
    }



    // Sets all the ruler's stats (taken from WaveManager.cs)
    void InstantiateStats()
    {
        health = WaveManager.gluerStats[0];
        damage = WaveManager.gluerStats[1];
        moveSpeed = WaveManager.gluerStats[2];
    }



    void FixedUpdate() 
    {
        playerPos = playerTransform.position;
        gluerPos = this.transform.position;

        // Detemines the distance between the gluer and the player
        distanceBetween = Vector2.Distance(gluerPos, playerPos);
        // Uses a predetermined function to define move speed. More info in enemies/README
        aiPath.maxSpeed = -((2 * (Mathf.Pow(distanceBetween - 6, 2) / 10)) - 10 - moveSpeed);
        // Makes sure the speed never goes below 2 (they'd be motionless :( )
        if (aiPath.maxSpeed < 2)
        {
            aiPath.maxSpeed = 2;
        }

        AimGluer();
            
        // Kills if health is depleted
        if (health <= 0)
        {
            KillGluer(false);
        } else {
            healthText.text = health.ToString();
        }
    }



    // Gluer "looks" towards player
    void AimGluer()
    {
        // Compares player and enemy positions, stores in playerPos
        playerPos.x = playerPos.x - gluerPos.x;
        playerPos.y = playerPos.y - gluerPos.y;

        // Determines the angle needed to rotate
        float angle = Mathf.Atan2(playerPos.y, playerPos.x) * Mathf.Rad2Deg;

        // Angle - 90 in Vector3 due to sprite being vertical and rotate towards pointing right, not up
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle - 90)), Time.deltaTime * 750f);
    }



    // The suicided bool is important here, since the gluer doesn't generate a puddle if the player kills it.
    public void KillGluer(bool suicided)
    {
        playerStats.enemiesKilled += 1;
        playerStats.enemiesLeft -= 1;
        playerStats.money += Random.Range(0, 5);

        if (suicided)
        {
            Instantiate(gluePuddle, this.transform.position,   this.transform.rotation);
        }
        
        Destroy(this.gameObject);
    }
    
    public void Damaged()
    {
        gluerAudio.Play();
        gluerParticles.Play();
    }
}