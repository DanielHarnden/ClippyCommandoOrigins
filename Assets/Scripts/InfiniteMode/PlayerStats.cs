using UnityEngine;
using UnityEngine.UI;

// Holds a bunch of information regarding the current Infinite Mode run and updates UI and other scripts respectively. The glue that holds a bunch of random things together.
public class PlayerStats : MonoBehaviour
{
    [Header ("Wave Manager and Related")]
    private WaveManager WaveManager;             

    private float timeRemaining = 45f;
    private bool waveOn = false;                

    [Header ("Player Stats")]
    public int health = 100;                    
    public int money = 0;                       
    private int totalScore = 0;                 
    public int wave = 0;                        

    public int enemiesLeft = 0;                 
    public int enemiesKilled = 0;  

    public bool dodging;        
    private float dodgeTimeMax = 1f;
    private float dodgeTime;     

    private AudioSource playerAudio;

    [Header ("Weapon Information")]
    public int[] weaponBulletDamage;

    private bool rulerEnraged = false;

    [Header ("UI Elements")]
    public Text moneyUI;
    public Text waveUI;
    public Text enemiesLeftUI;
    public Text enemiesKilledUI;
    public Text timerUI;
    public Text scoreUI;
    public Text healthUI;



    private void Start() 
    {
        WaveManager = GameObject.FindGameObjectWithTag("WaveManager").GetComponent<WaveManager>();
        WaveManager.WaveStart(0);
        playerAudio = this.GetComponent<AudioSource>();
    }  

    private void FixedUpdate() 
    {
        TimerAndUI();
        WinCondition();
        LoseConditions();
        RandomEvents();

        if (dodging)
        {
            this.gameObject.layer = LayerMask.NameToLayer("Water");
            if (dodgeTime > 0)
            {
                dodgeTime -= Time.deltaTime;
            } else {
                dodging = false;
            }
        } else {
            dodgeTime = dodgeTimeMax;
            this.gameObject.layer = LayerMask.NameToLayer("EnemyInteractable");
        }
    } 


    
    private void TimerAndUI()
    {
        if (waveOn)
        {
            timeRemaining -= Time.deltaTime;

            timerUI.text = timeRemaining.ToString("F2") + "/ $" + (Mathf.Pow(timeRemaining, 1f / 3f) * 50f).ToString("F0");
        }

        moneyUI.text = "$" + money.ToString();
        waveUI.text = "WAVE:" + wave.ToString();
        enemiesLeftUI.text = "ENEMIES REMAINING:" + enemiesLeft.ToString();
        enemiesKilledUI.text = "TOTAL ENEMIES KILLED:" + enemiesKilled.ToString();
        healthUI.text = "HEALTH:" + health.ToString();
        scoreUI.text = totalScore.ToString() + " PNTS";
    }



    // Checks to see if the win condition has been completed
    private void WinCondition()
    {
        if (enemiesLeft <= 0)
        {
            if (waveOn)
            {
                // Updates stats accordingly
                waveOn = false;

                money += (int)(money * 0.25f);
                money += (int)(Mathf.Round((Mathf.Pow(timeRemaining, 1f / 3f) * 50) * 50f) / 100f);

                totalScore += 100;
                totalScore += enemiesKilled;
                totalScore += money;
                totalScore += wave * 10;

                GameObject.FindGameObjectWithTag("Music").GetComponent<MusicController>().track = 0;

                WaveManager.OpenMarket();
            }
        }
    }



    // Checks to see if any of the lose conditions have been met
    private void LoseConditions()
    {
        if (timeRemaining <= 0)
        {
            WaveManager.Lose("You ran out of time :(");
            GameObject.FindGameObjectWithTag("Music").GetComponent<MusicController>().track = 0;

        }

        if (health <= 0)
        {
            WaveManager.Lose("You ran out of health :(");
            GameObject.FindGameObjectWithTag("Music").GetComponent<MusicController>().track = 0;

        }
    }

    private void RandomEvents()
    {
        if (enemiesLeft == 1 && !rulerEnraged)
        {
            if (GameObject.FindGameObjectWithTag("Ruler") != null)
            {
                GameObject.FindGameObjectWithTag("Ruler").GetComponent<Ruler>().Enrage();
                rulerEnraged = true;
            }
        }

        if (enemiesLeft == 2)
        {
            GameObject.FindGameObjectWithTag("Music").GetComponent<MusicController>().track = 2;
        }
    }



    // Starts the wave (called from WaveManager.cs)
    public void StartWave(float tempTimeRemaining, int tempEnemiesLeft)
    {
        GameObject.FindGameObjectWithTag("Music").GetComponent<MusicController>().track = 1;

        wave += 1;
        waveOn = true;
        timeRemaining = tempTimeRemaining;
        enemiesLeft = tempEnemiesLeft;
        rulerEnraged = false;

        this.transform.GetChild(0).GetComponent<PlayerMovement>().StartWave();

        this.transform.position = new Vector2(0, 0);
    }

    private void OnCollisionEnter2D(Collision2D colObj) 
    {
        if (!dodging)
        {
            switch (colObj.gameObject.tag)
            {
                case "Ruler":
                    health -= (int)colObj.gameObject.GetComponent<Ruler>().damage;
                    playerAudio.Play();
                    break;

                case "Gluer":
                    health -= (int)colObj.gameObject.GetComponent<Gluer>().damage;
                    colObj.gameObject.GetComponent<Gluer>().KillGluer(true);
                    playerAudio.Play();
                    break;

                case "Gunner":
                    health -= (int)colObj.gameObject.GetComponent<Gunner>().damage;
                    playerAudio.Play();
                    break;

                case "Staple":
                    playerAudio.Play();
                    break;

                default:
                    break;
            }
        }
    }
}