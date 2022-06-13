using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementStory : MonoBehaviour
{
    // Weapon Prefabs
    public Transform pistol;
    public Transform shotgun;
    public Transform minigun;
    public Transform scissors;

    private Transform currentWeapon;
    private int currentWeaponInteger;

    // Player Information
    public float moveSpeed = 10f;
    private Rigidbody2D playerRigidbody;
    private Vector2 moveVector;
    private PlayerStatsStory playerStats;

    public bool[] skillTree;
    private int tempGunSwitchingInt;



    private void Start() 
    {
        sprintMeter = sprintMeterMax;
        playerRigidbody = this.transform.parent.GetComponent<Rigidbody2D>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsStory>();
    }

    void Update() 
    {
        // Weapon switching (mouse scroll)
        if (Input.mouseScrollDelta.y != 0)
        {
            tempGunSwitchingInt = -100;
            if (skillTree[0]) { tempGunSwitchingInt = 1; }
            if (skillTree[1]) { tempGunSwitchingInt += 1; }
            if (skillTree[2]) { tempGunSwitchingInt += 1; }
            if (skillTree[3]) { tempGunSwitchingInt += 1; }

            currentWeaponInteger = (currentWeaponInteger += (int)Input.mouseScrollDelta.y) % tempGunSwitchingInt;
            SwitchGun(currentWeaponInteger);
        }
        // Weapon switching (alphanumeric)
        SwitchGunAlphaNumeric();


        // Movement and aiming
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.y = Input.GetAxis("Vertical");
        AimGun();   
        Sprint();
        DodgeRoll();
    }

    void FixedUpdate() 
    {
        if (!dodging)
        {
            for (int i = 0; i < gluePuddle; i++)
            {
                moveSpeed /= 2f;
            }
        } else {
            moveSpeed = 100f;
        }

        playerRigidbody.MovePosition(playerRigidbody.position + moveVector * moveSpeed * Time.fixedDeltaTime);
    }




    // Aims the player's gun towards the mouse position
    void AimGun()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f; // Distance between camera and object in world space
 
        Vector3 objectPos = Camera.main.WorldToScreenPoint (transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;
 
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }



    private float sprintMeterMax = 2f;
    private float sprintMeter;
    private float sprintCooldownMax = 2f;
    private float sprintCooldown;
    private bool sprinting;
    public Text sprintUI;
    public int gluePuddle;

    void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && sprintMeter > 0)
        {
            sprinting = true;
            sprintMeter -= Time.deltaTime;
        } else {
            sprinting = false;
        }

        if (sprinting)
        {
            moveSpeed = 14;
            sprintCooldown = sprintCooldownMax;
        } else {
            moveSpeed = 7;
            if (sprintCooldown > 0)
            {
                sprintCooldown -= Time.deltaTime;
            } else {
                if (sprintMeter < sprintMeterMax)
                {
                    sprintMeter += Time.deltaTime;
                }
            }
        }

        sprintUI.text = sprintMeter.ToString("F1");
    }

    private float dodgeCooldownMax = 2f;
    private float dodgeCooldown;
    private float dodgeTimeMax = 0.1f;
    private float dodgeTime;
    private bool dodging;

    void DodgeRoll()
    {
        if (dodgeTime > 0)
        {
            dodging = true;
            playerStats.dodging = true;
            dodgeTime -= Time.deltaTime;
        } else {

            dodging = false;

            if (dodgeCooldown > 0)
            {
                dodgeCooldown -= Time.deltaTime;
            } else {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    
                    dodgeCooldown = dodgeCooldownMax;
                    dodgeTime = dodgeTimeMax;
                }
            }
        }   
    }



    void SwitchGunAlphaNumeric()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && skillTree[0])
        {
            SwitchGun(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && skillTree[1])
        {
            SwitchGun(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && skillTree[2])
        {
            SwitchGun(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && skillTree[3])
        {
            SwitchGun(3);
        }
    }

    // Switches the ammo type of the player's gun
    public void SwitchGun(int newInt)
    {
        pistol.gameObject.SetActive(false);
        shotgun.gameObject.SetActive(false);
        minigun.gameObject.SetActive(false);
        scissors.gameObject.SetActive(false);

        switch (newInt)
        {
            case 0:
                currentWeapon = pistol;
                pistol.gameObject.SetActive(true);
                break;

            case -3:
            case 1:
                currentWeapon = shotgun;
                shotgun.gameObject.SetActive(true);
                break;

            case -2:
            case 2:
                currentWeapon = minigun;
                minigun.gameObject.SetActive(true);
                break;

            case -1:
            case 3:
                if (skillTree[3])
                {
                    currentWeapon = scissors;
                    scissors.gameObject.SetActive(true);
                } else {
                    currentWeapon = shotgun;
                    shotgun.gameObject.SetActive(true);
                }
                
                break;

            default:
                Debug.Log("Error in SwitchGun()");
                break;
        }
    }
}
