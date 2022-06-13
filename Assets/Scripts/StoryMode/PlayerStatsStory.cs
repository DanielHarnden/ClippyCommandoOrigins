using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsStory : MonoBehaviour
{ 
    public int health = 100;

    private bool[] skillTree;
    public int enemiesLeft;

    public bool dodging;        
    private float dodgeTimeMax = 1f;
    private float dodgeTime;    

    private AudioSource playerAudio;
    public Text healthUI;

    

    private void Start() 
    {
        playerAudio = this.GetComponent<AudioSource>();
    }  

    private void FixedUpdate() 
    {
        healthUI.text = "HEALTH:" + health.ToString();

        if (dodging)
        {
            this.gameObject.layer = LayerMask.NameToLayer("Water");
            if (dodgeTime > 0)
            {
                dodgeTime -= Time.deltaTime;
            } else {
                dodging = false;
            }
        } else {
            dodgeTime = dodgeTimeMax;
            this.gameObject.layer = LayerMask.NameToLayer("EnemyInteractable");
        }
    }



    private void OnCollisionEnter2D(Collision2D colObj) 
    {
        switch (colObj.gameObject.tag)
        {
            case "StoryRuler":
                health -= (int)colObj.gameObject.GetComponent<StoryRuler>().damage;
                break;

            case "StoryGluer":
                colObj.gameObject.GetComponent<StoryGluer>().KillGluer(true);
                break;

            default:
                break;
        }
    }



    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "PistolStory")
        {
            skillTree[0] = true;
            Destroy(other.gameObject);
            this.transform.GetChild(0).GetComponent<PlayerMovementStory>().SwitchGun(0);
        }

        if (other.gameObject.tag == "ShotgunStory")
        {
            skillTree[1] = true;
            Destroy(other.gameObject);
            this.transform.GetChild(0).GetComponent<PlayerMovementStory>().SwitchGun(1);
        }

        if (other.gameObject.tag == "MinigunStory")
        {
            skillTree[2] = true;
            Destroy(other.gameObject);
            this.transform.GetChild(0).GetComponent<PlayerMovementStory>().SwitchGun(2);
        }

        if (other.gameObject.tag == "ScissorsStory")
        {
            skillTree[3] = true;
            Destroy(other.gameObject);
            this.transform.GetChild(0).GetComponent<PlayerMovementStory>().SwitchGun(3);
        }
    }




    public void Initialize(bool[] tempSkillTree, int startingGun, int tempTotalAmmo)
    {
        skillTree = tempSkillTree;
        this.transform.GetChild(0).GetComponent<PlayerMovementStory>().skillTree = skillTree;

        switch (startingGun)
        {
            case 1:
                this.transform.GetChild(0).GetChild(0).GetComponent<ShootPistol>().totalAmmo = tempTotalAmmo - this.transform.GetChild(0).GetChild(0).GetComponent<ShootPistol>().clipAmmo;
                this.transform.GetChild(0).GetComponent<PlayerMovementStory>().SwitchGun(0);
                break;

            case 2:
                this.transform.GetChild(0).GetChild(1).GetComponent<ShootShotgun>().totalAmmo = tempTotalAmmo - this.transform.GetChild(0).GetChild(1).GetComponent<ShootShotgun>().clipAmmo;
                this.transform.GetChild(0).GetComponent<PlayerMovementStory>().SwitchGun(1);
                break;

            case 3:
                this.transform.GetChild(0).GetChild(2).GetComponent<ShootMinigun>().totalAmmo = tempTotalAmmo;
                this.transform.GetChild(0).GetComponent<PlayerMovementStory>().SwitchGun(2);
                break;

            case 4:
                this.transform.GetChild(0).GetChild(3).GetComponent<ShootScissors>().totalAmmo = tempTotalAmmo;
                this.transform.GetChild(0).GetComponent<PlayerMovementStory>().SwitchGun(3);
                break;

            default:
                break;
        }
    }
}