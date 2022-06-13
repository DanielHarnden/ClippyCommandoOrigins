using UnityEngine;

// Same as ArrowToEnemy.cs
public class StoryArrowToEnemy : MonoBehaviour
{
    private Transform enemy;
    private PlayerStatsStory player;
    private SpriteRenderer thisSprite;

    void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsStory>();
        thisSprite = this.gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player.enemiesLeft > 0)
        {
            thisSprite.enabled = true;
            if (enemy != null)
            {
                LocateEnemy();
            } 
            else
            {
                if (GameObject.FindGameObjectsWithTag("StoryRuler").Length != 0)
                {
                    enemy = GameObject.FindGameObjectWithTag("StoryRuler").GetComponent<Transform>();
                } 
                else if (GameObject.FindGameObjectsWithTag("StoryGluer").Length != 0)
                {
                    enemy = GameObject.FindGameObjectWithTag("StoryGluer").GetComponent<Transform>();
                } 
                else if (GameObject.FindGameObjectsWithTag("StoryGunner").Length != 0)
                {
                    enemy = GameObject.FindGameObjectWithTag("StoryGunner").GetComponent<Transform>();
                }
            }
        } else {
            thisSprite.enabled = false;
        }
    }

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
