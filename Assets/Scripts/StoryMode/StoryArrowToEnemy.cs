using UnityEngine;

// Same as ArrowToEnemy.cs
public class StoryArrowToEnemy : MonoBehaviour
{
    private Transform targetEnemy;
    private PlayerStatsStory player;
    private SpriteRenderer thisSprite;
    float smallestDistance;

    void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsStory>();
        thisSprite = this.gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player.enemiesLeft > 0)
        {
            smallestDistance = 10000f;

            GameObject[] rulers = GameObject.FindGameObjectsWithTag("StoryRuler");
            GameObject[] gluers = GameObject.FindGameObjectsWithTag("StoryGluer");
            GameObject[] gunners = GameObject.FindGameObjectsWithTag("StoryGunner");

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
    }

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
