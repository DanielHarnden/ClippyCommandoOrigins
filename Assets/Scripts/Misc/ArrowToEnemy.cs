using UnityEngine;

// Used to point a sprite towards an enemy. Not the nearest enemy, but instead the first (living) enemy from a list generated at run time.
public class ArrowToEnemy : MonoBehaviour
{
    // The temporary transfrom of the current target.
    private Transform targetEnemy;
    private PlayerStats playerStats;
    private SpriteRenderer thisSprite;
    float smallestDistance;


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
            if (playerStats.enemiesLeft < 5)
            {
                // Finds the closest enemy.
                smallestDistance = 10000f;

                GameObject[] rulers = GameObject.FindGameObjectsWithTag("Ruler");
                GameObject[] gluers = GameObject.FindGameObjectsWithTag("Gluer");
                GameObject[] gunners = GameObject.FindGameObjectsWithTag("Gunner");

                foreach(GameObject enemy in rulers)
                {
                    if (Vector2.Distance(this.transform.position, enemy.transform.position) < smallestDistance)
                    {
                        targetEnemy = enemy.transform;
                        smallestDistance = Vector2.Distance(this.transform.position, enemy.transform.position);
                    }
                }

                foreach(GameObject enemy in gluers)
                {
                    if (Vector2.Distance(this.transform.position, enemy.transform.position) < smallestDistance)
                    {
                        targetEnemy = enemy.transform;
                        smallestDistance = Vector2.Distance(this.transform.position, enemy.transform.position);
                    }
                }

                foreach(GameObject enemy in gunners)
                {
                    if (Vector2.Distance(this.transform.position, enemy.transform.position) < smallestDistance)
                    {
                        targetEnemy = enemy.transform;
                        smallestDistance = Vector2.Distance(this.transform.position, enemy.transform.position);
                    }
                }

                thisSprite.enabled = true;
                if (targetEnemy != null)
                {
                    LocateEnemy();
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
        Vector2 enemyPos = targetEnemy.position;
        Vector2 arrowPos = this.transform.position;

        enemyPos.x = enemyPos.x - arrowPos.x;
        enemyPos.y = enemyPos.y - arrowPos.y;

        float angle = Mathf.Atan2(enemyPos.y, enemyPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
