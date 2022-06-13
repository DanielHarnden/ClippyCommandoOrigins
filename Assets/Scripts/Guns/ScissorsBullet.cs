using UnityEngine;

public class ScissorsBullet : MonoBehaviour
{
    private float deathTimer = 60f;
    private int damage = 10000;

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
            case "Ruler":
                colObj.gameObject.GetComponent<Ruler>().health -= damage;
                break;

            case "Gluer":
                colObj.gameObject.GetComponent<Gluer>().health -= damage;
                break;

            case "Gunner":
                colObj.gameObject.GetComponent<Gunner>().health -= damage;
                break;

            case "StoryRuler":
                colObj.gameObject.GetComponent<StoryRuler>().health -= damage;
                break;

            case "StoryGluer":
                colObj.gameObject.GetComponent<StoryGluer>().health -= damage;
                break;

            case "StoryGunner":
                colObj.gameObject.GetComponent<StoryGunner>().health -= damage;
                break;

            default:
                break;
        }
    }
}