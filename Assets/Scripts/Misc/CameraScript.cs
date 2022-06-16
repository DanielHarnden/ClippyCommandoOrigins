using UnityEngine;

// Some code stolen from an old project.
public class CameraScript : MonoBehaviour
{
    // The offset from the player and the locked bounds of the camera. Currently used for single rooms.
    public Vector3 offset;
    public Vector2 boundsHigh;
    public Vector2 boundsLow;
    // The smoothing of the camera.
    public float smoothing = 0.0f;
    // The player object, a 0 vector, and a temporary vector respectively.
    private GameObject player;
    private Vector2 velocity;
    private Vector3 temp;

    // Sets the player rigidbody (which the camera follows).
    void Start() { player = GameObject.FindGameObjectWithTag("Player"); }

    // Smoothly moves the camera to follow the player, applys an offset.
    void FixedUpdate() 
    {
        Cursor.lockState = CursorLockMode.Confined;
        temp.x = Mathf.Clamp(Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothing), boundsLow.x, boundsHigh.x);
        temp.y = Mathf.Clamp(Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smoothing), boundsLow.y, boundsHigh.y);
        temp.z = player.transform.position.z;
        temp += offset;
        transform.position = temp;
    }
}
