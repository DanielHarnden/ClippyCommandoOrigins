using UnityEngine;

// Used to point a sprite towards an enemy. Not the nearest enemy, but instead the first (living) enemy from a list generated at run time.
public class ArrowToEnemy : MonoBehaviour
{
    // The temporary transfrom of the current target.
    private Transform enemy;
    private PlayerStats playerStats;
    private SpriteRenderer thisSprite;

    void Start() 
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        thisSprite = this.gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (playerStats.enemiesLeft > 0)
        {
            // The maximum number of enemies allowed before the sprite renders.
            if (playerStats.enemiesLeft < 3)
            {
                thisSprite.enabled = true;
                if (enemy != null)
                {
                    LocateEnemy();
                } 
                else
                {
                    // TODO: It would probably be smart to change this to the closest enemy. Performance is something you should keep in mind when finding out how to calculate that.

                    // Checks for rulers, then gluers, then gunners.
                    if (GameObject.FindGameObjectsWithTag("Ruler").Length != 0)
                    {
                        enemy = GameObject.FindGameObjectWithTag("Ruler").GetComponent<Transform>();
                    } 
                    else if (GameObject.FindGameObjectsWithTag("Gluer").Length != 0)
                    {
                        enemy = GameObject.FindGameObjectWithTag("Gluer").GetComponent<Transform>();
                    } 
                    else if (GameObject.FindGameObjectsWithTag("Gunner").Length != 0)
                    {
                        enemy = GameObject.FindGameObjectWithTag("Gunner").GetComponent<Transform>();
                    }
                }
            } else {
                thisSprite.enabled = false;
            }
        } else {
            thisSprite.enabled = false;
        }
    }

    // Same as the gun and enemy pointing. These 6 lines of code are probably the most used in this project.
    void LocateEnemy()
    {
        Vector2 enemyPos = enemy.position;
        Vector2 arrowPos = this.transform.position;

        enemyPos.x = enemyPos.x - arrowPos.x;
        enemyPos.y = enemyPos.y - arrowPos.y;

        float angle = Mathf.Atan2(enemyPos.y, enemyPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
