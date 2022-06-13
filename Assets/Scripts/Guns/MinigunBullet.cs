using UnityEngine;

public class MinigunBullet : MonoBehaviour
{
    private float deathTimer = 5f;
    private int damage = 1;
    private bool die = false;

    private PlayerStats playerStats;

    // Determines where weapon damage is stored.
    private void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            damage = playerStats.weaponBulletDamage[0];
        } else {
            damage = 1;
        }
    }

    // Timer to delete old bullets
    private void Update() 
    {
        if (deathTimer > 0f)
        {
            deathTimer -= Time.deltaTime;
        } else {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D colObj) 
    {
        // Switch statement since each enemy has a different script.
        switch (colObj.gameObject.tag)
        {
            case "Wall":
                die = true;
                break;
                
            case "Ruler":
                colObj.gameObject.GetComponent<Ruler>().health -= damage;
                colObj.gameObject.GetComponent<Ruler>().Damaged();
                die = true;
                break;

            case "StoryRuler":
                colObj.gameObject.GetComponent<StoryRuler>().health -= damage;
                colObj.gameObject.GetComponent<StoryRuler>().Damaged();
                die = true;
                break;

            case "Gluer":
                colObj.gameObject.GetComponent<Gluer>().health -= damage;
                colObj.gameObject.GetComponent<Gluer>().Damaged();
                die = true;
                break;

            case "StoryGluer":
                colObj.gameObject.GetComponent<StoryGluer>().health -= damage;
                colObj.gameObject.GetComponent<StoryGluer>().Damaged();
                die = true;
                break;

            case "Gunner":
                colObj.gameObject.GetComponent<Gunner>().health -= damage;
                colObj.gameObject.GetComponent<Gunner>().Damaged();
                die = true;
                break;

            case "StoryGunner":
                colObj.gameObject.GetComponent<StoryGunner>().health -= damage;
                colObj.gameObject.GetComponent<StoryGunner>().Damaged();
                die = true;
                break;

            default:
                break;
        }

        if (die)
        {
            Destroy(this.gameObject);
        }
    }
}