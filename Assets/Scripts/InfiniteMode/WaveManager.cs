using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

// Used to manage the waves. Changes stats at the beginnings of waves, spawns enemies, spawns random obstacles, and handles the transitions between fighting and the market phase.

// Probably some optimizations that could be made here, but I don't see anything too glaring.
public class WaveManager : MonoBehaviour
{
    // Player and Related
    private int tempEnemiesLeft;        // How many new enemies are spawned (for PlayerStats.StartWave())

    // Enemy Prefabs
    public Transform[] enemyPrefabs;

    // Arrays of enemy stats
    [Header ("Health / Damage / Move Speed / Charge Prep Time / Charge Time")]
    public float[] rulerStats;   
    [Header ("Health / Damage / Move Speed")]      
    public float[] gluerStats;
    [Header ("Health / Damage / Move Speed / Bullet Speed / Shoot Time")]
    public float[] gunnerStats;

    // Spawing variables used in SpawnEnemies()
    private Vector3 enemySpawnOffset; 
    private int enemySpawnInteger;      

    [Header ("Environmental Objects")]
    public Transform randomWalls;       // The walls that randomly spawn / despawn
    public Transform spawners;          // Where each enemy can spawn
    public Transform[] randomSpawners;  // 4 spawners randomly chosen from the above set

    [Header ("UI Elements")]
    public GameObject InGameUI;
    public GameObject MarketUI;
    public GameObject LoseUI;
    public Text LoseText;

    // Wave Stats
    private int enemySpawnsMin = 0;
    private int enemySpawnsMax = 0;
    private float totalWaveTime = 45f;

    private bool spawnGunners = false;
    private bool spawnGluers = false;
    private bool spawnHardRulers = false;
    private bool spawnHardGunners = false;
    private bool spawnHardGluers = false;



    // Used to set everything up to start a new wave
    public void WaveStart(int wave)
    {
        // Randomly sets the walls every 5 rounds
        if (wave % 5 == 0)
        {
            foreach (Transform wall in randomWalls)
            {
                if (Random.Range(-100, 100) >= 0)
                {
                    wall.gameObject.SetActive(true);
                } else {
                    wall.gameObject.SetActive(false);
                }
            }
            // Ensures pathfinding is updated after obsticles are changed
            AstarPath.active.Scan();
        }

        WaveStatInitialization(wave);
        SpawnEnemies();

        InGameUI.SetActive(true);
        MarketUI.SetActive(false);

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().StartWave(totalWaveTime, tempEnemiesLeft);
    }



    // Changes various wave elements
    private void WaveStatInitialization(int wave)
    {
        // The wave inputted is always 1 less. First wave is wave 1.
        wave = wave + 1;

        switch (wave)
        {
            case 1:
                enemySpawnsMin = 1;
                enemySpawnsMax = 1;
                rulerStats[0] = 2;
                rulerStats[1] = 10;
                rulerStats[2] = 3f;
                rulerStats[3] = 2;
                rulerStats[4] = 1f;
                break;

            case 2:
                enemySpawnsMax = 2;
                rulerStats[0] = 3;
                break;

            case 3:
                enemySpawnsMax = 3;
                break;

            case 4:
                enemySpawnsMin = 2;
                enemySpawnsMax = 3;
                rulerStats[0] = 4;
                break;

            case 5:
                rulerStats[0] = 5;
                rulerStats[2] = 4f;
                spawnGluers = true;
                gluerStats[0] = 1;
                gluerStats[1] = 5;
                gluerStats[2] = 5f;
                break;

            case 7:
                enemySpawnsMax = 4;
                gluerStats[0] = 2;
                break;

            case 10:
                totalWaveTime = 40f;
                enemySpawnsMin = 3;
                enemySpawnsMax = 5;
                rulerStats[0] = 10;
                rulerStats[1] = 15;
                rulerStats[2] = 5f;
                rulerStats[3] = 1.5f;
                rulerStats[4] = 1.5f;
                gluerStats[0] = 3;
                gluerStats[2] = 7.5f;
                spawnGunners = true;
                gunnerStats[0] = 2;
                gunnerStats[1] = 5;
                gunnerStats[2] = 3f;
                gunnerStats[3] = 500f;
                gunnerStats[4] = 2f;
                break;

            case 13:
                enemySpawnsMin = 4;
                enemySpawnsMax = 6;
                break;

            case 15:
                totalWaveTime = 35f;
                gluerStats[1] = 10;
                gunnerStats[0] = 3;
                gunnerStats[1] = 15;
                break;

            case 20:
                totalWaveTime = 30f;
                enemySpawnsMin = 5;
                enemySpawnsMax = 6;
                rulerStats[0] = 15;
                rulerStats[1] = 20;
                rulerStats[2] = 7.5f;
                rulerStats[3] = 1f;
                rulerStats[4] = 2f;
                gluerStats[0] = 5;
                gluerStats[1] = 15;
                gluerStats[2] = 10f;
                gunnerStats[0] = 5;
                gunnerStats[1] = 25;
                gunnerStats[2] = 5f;
                gunnerStats[3] = 1000f;
                gunnerStats[4] = 1.5f;
                break;

            case 25:
                totalWaveTime = 25f;
                enemySpawnsMin = 5;
                enemySpawnsMax = 7;
                rulerStats[2] = 10f;
                gluerStats[2] = 15f;
                gunnerStats[2] = 7.5f;
                break;

            case 30:
                totalWaveTime = 20f;
                enemySpawnsMin = 6;
                rulerStats[0] = 20;
                rulerStats[1] = 25;
                rulerStats[2] = 12.5f;
                rulerStats[3] = 0.5f;
                gluerStats[0] = 10;
                gluerStats[1] = 20;
                gluerStats[2] = 20f;
                gunnerStats[1] = 35;
                gunnerStats[2] = 10f;
                gunnerStats[3] = 1500f;
                gunnerStats[4] = 1f;
                break;

            default:
                Debug.Log("Wave does not exist");
                break;
        }
    }



    // Randomly spawns enemies at each of the spawners
    void SpawnEnemies()
    {
        // Randomly chooses 4 spawners from the available spawners
        randomSpawners = new Transform[4];
        int j = 0;

        foreach (Transform spawner in spawners)
        {
            switch(j)
            {
                case 0:
                    if (Random.Range(-100, 100) >= 0)
                    {
                        randomSpawners[0] = spawner;
                        j++;
                    }
                    break;

                case 1:
                    if (Random.Range(-100, 100) >= 0)
                    {
                        randomSpawners[1] = spawner;
                        j++;
                    }
                    break;

                case 2:
                    if (Random.Range(-100, 100) >= 0)
                    {
                        randomSpawners[2] = spawner;
                        j++;
                    }
                    break;

                case 3:
                    if (Random.Range(-100, 100) >= 0)
                    {
                        randomSpawners[3] = spawner;
                        j++;
                    }
                    break;

                default:
                    break;
            }
        }

        // There's a chance no spawners will be assigned. This is to ensure that does not happen
        for (int i = 0; i < 4; i++)
        {
            if (randomSpawners[i] == null)
            {
                randomSpawners[i] = spawners.GetChild(i);
            }
        }

        tempEnemiesLeft = 0;

        // Used for instantiating enemies. Adds 1 per enemy able to be instantiated.
        int enemySpawnIndex = 1;
        if (spawnGunners) { enemySpawnIndex += 1; }
        if (spawnGluers) { enemySpawnIndex += 1; }
        if (spawnHardRulers) { enemySpawnIndex += 1; }
        if (spawnHardGunners) { enemySpawnIndex += 1; }
        if (spawnHardGluers) { enemySpawnIndex += 1; }

        foreach (Transform spawner in randomSpawners)
        {
            // Spawns a random amount of enemies between the min and max
            for (int i = Random.Range(enemySpawnsMin, enemySpawnsMax + 1); i <= enemySpawnsMax; i++)
            {
                // Randomly offsets them from each other
                enemySpawnOffset.x = Random.Range(-2, 3);
                enemySpawnOffset.y = Random.Range(-2, 3);
                enemySpawnOffset.z = 0;

                // Selects a random enemy from the array of prefabs and spawns them plus the offset
                Instantiate(enemyPrefabs[Random.Range(0, enemySpawnIndex)], spawner.transform.position + enemySpawnOffset, spawner.transform.rotation);

                tempEnemiesLeft += 1;
            }
        }
    }

    public void OpenMarket()
    {
        InGameUI.SetActive(false);
        MarketUI.SetActive(true);
    }

    public void Lose(string loseMessage)
    {
        Time.timeScale = 0f;
        InGameUI.SetActive(false);
        LoseUI.SetActive(true);
        LoseText.text = loseMessage;    
    }
}