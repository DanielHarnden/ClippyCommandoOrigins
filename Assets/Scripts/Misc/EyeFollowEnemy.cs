using UnityEngine;

// Originally was going to be used by the enemies like how Clippy's eyes point to the mouse, but the sprites always automatically point to the player anyways. I'll keep this, but I don't think it'll be used at all
public class EyeFollowEnemy : MonoBehaviour
{
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        Vector2 playerPos = player.position;
        Vector2 rulerPos = this.transform.position;

        playerPos.x = playerPos.x - rulerPos.x;
        playerPos.y = playerPos.y - rulerPos.y;

        float angle = Mathf.Atan2(playerPos.y, playerPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
