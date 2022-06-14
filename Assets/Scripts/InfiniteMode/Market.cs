using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// This script is huge but not complicated at all. The setup of the market is done automatically through FindGameObject in Awake(), while further initialization at the start of each market phase is done in OnEnable(). Everything after that is just the individual button scripts corresponding to the different purchases that can be made in the market.

//TODO: It would probably be smart to figure out a way to keep text and prices consistent using one singular point. However, for what Infinite Mode is right now, it works fine.
public class Market : MonoBehaviour
{
    private bool initialized = false;

    private GameObject player;
    private GameObject WaveManager;
    private GameObject movementHolder;

    public GameObject pistol;
    public GameObject shotgun;
    public GameObject minigun;
    public GameObject scissors;

    private Text startNextWaveText;
    private Text playerAddHealthText;
    private Text playerAddBaseMoveText;
    private Text playerAddSprintSpeedText;
    private Text playerAddDodgeTimeText;

    private Text pistolDamageText;
    private Text shotgunDamageText;
    private Text minigunDamageText;

    private Text pistolAddAmmoText;
    private Text pistolAddClipSizeText;
    private Text pistolAddBulletSpeedText;
    private Text pistolReduceShootCooldownText;

    private Text shotgunAddAmmoText;
    private Text shotgunAddClipSizeText;
    private Text shotgunAddBulletSpeedText;
    private Text shotgunReduceShootCooldownText;
    private Text shotgunReduceSpreadFactorText;
    private Text shotgunIncreasePelletCountText;

    private Text minigunAddAmmoText;
    private Text minigunAddBulletSpeedText;

    private Text scissorsAddAmmoText;
    private Text scissorsAddBulletSpeedText;

    private Text quarterMasterDialogue;

    public string[] dialogue;



    void Awake()
    {
        // Initialization
        if (!initialized)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            WaveManager = GameObject.FindGameObjectWithTag("WaveManager");
            movementHolder = GameObject.Find("MovementHolder");
            
            startNextWaveText = GameObject.Find("StartNextWaveText").GetComponent<Text>();
            playerAddHealthText = GameObject.Find("PlayerAddHealthText").GetComponent<Text>();
            playerAddBaseMoveText = GameObject.Find("PlayerAddBaseMoveText").GetComponent<Text>();
            playerAddSprintSpeedText = GameObject.Find("PlayerAddSprintSpeedText").GetComponent<Text>();
            playerAddDodgeTimeText = GameObject.Find("PlayerAddDodgeTimeText").GetComponent<Text>();

            pistolDamageText = GameObject.Find("PistolDamageText").GetComponent<Text>();
            shotgunDamageText = GameObject.Find("ShotgunDamageText").GetComponent<Text>();
            minigunDamageText = GameObject.Find("MinigunDamageText").GetComponent<Text>(); 

            pistolAddAmmoText = GameObject.Find("PistolAddAmmoText").GetComponent<Text>();
            pistolAddClipSizeText = GameObject.Find("PistolAddClipSizeText").GetComponent<Text>();
            pistolAddBulletSpeedText = GameObject.Find("PistolAddBulletSpeedText").GetComponent<Text>();
            pistolReduceShootCooldownText = GameObject.Find("PistolReduceShootCooldownText").GetComponent<Text>();

            shotgunAddAmmoText = GameObject.Find("ShotgunAddAmmoText").GetComponent<Text>();
            shotgunAddClipSizeText = GameObject.Find("ShotgunAddClipSizeText").GetComponent<Text>();
            shotgunAddBulletSpeedText = GameObject.Find("ShotgunAddBulletSpeedText").GetComponent<Text>();
            shotgunReduceShootCooldownText = GameObject.Find("ShotgunReduceCooldownText").GetComponent<Text>();
            shotgunReduceSpreadFactorText = GameObject.Find("ShotgunReduceSpreadFactorText").GetComponent<Text>();
            shotgunIncreasePelletCountText = GameObject.Find("ShotgunIncreasePelletCountText").GetComponent<Text>();

            minigunAddAmmoText = GameObject.Find("MinigunAddAmmoText").GetComponent<Text>();
            minigunAddBulletSpeedText = GameObject.Find("MinigunAddBulletSpeedText").GetComponent<Text>();

            scissorsAddAmmoText = GameObject.Find("ScissorsAddAmmoText").GetComponent<Text>();
            scissorsAddBulletSpeedText = GameObject.Find("ScissorsAddBulletSpeedText").GetComponent<Text>();

            quarterMasterDialogue = GameObject.Find("QuaterMasterText").GetComponent<Text>();

            initialized = true;
        }
    }

    void OnEnable() 
    {
        pistol.SetActive(true);
        shotgun.SetActive(true);
        minigun.SetActive(true);
        scissors.SetActive(true);

        quarterMasterDialogue.text = dialogue[Random.Range(0, dialogue.Length)];
        
        startNextWaveText.text = "START NEXT WAVE";
        playerAddHealthText.text = "$50 - Add 10 Health (Current): " + player.GetComponent<PlayerStats>().health.ToString();
        playerAddBaseMoveText.text = "$500 - Add 1 to Base Speed (Current): " + player.transform.GetChild(0).GetComponent<PlayerMovement>().baseMoveSpeed.ToString();
        playerAddSprintSpeedText.text = "$500 - Add 1 to Sprint Speed (Current): " + player.transform.GetChild(0).GetComponent<PlayerMovement>().sprintMoveSpeed.ToString();
        playerAddDodgeTimeText.text = "$1500 - Add 0.05 Seconds to Dodge Time (Current): " + player.transform.GetChild(0).GetComponent<PlayerMovement>().dodgeTimeMax.ToString();

        pistolDamageText.text = "$250 - Increase Pistol Damage By 1 (Current): " + player.GetComponent<PlayerStats>().weaponBulletDamage[0].ToString();
        shotgunDamageText.text = "$500 - Increase Shotgun Damage By 1 (Current): " + player.GetComponent<PlayerStats>().weaponBulletDamage[1].ToString();
        minigunDamageText.text = "$1000 - Increase Minigun Damage By 1 (Current): " + player.GetComponent<PlayerStats>().weaponBulletDamage[2].ToString();

        pistolAddAmmoText.text = "$25 - Add 8 Ammo (Current): " + (pistol.GetComponent<ShootPistol>().totalAmmo + pistol.GetComponent<ShootPistol>().clipAmmo).ToString();
        pistolAddClipSizeText.text = "$50 - Add 4 to Clip Size (Current): " + pistol.GetComponent<ShootPistol>().clipSize.ToString();
        pistolAddBulletSpeedText.text = "$100 - Increase Bullet Speed (Current): " + pistol.GetComponent<ShootPistol>().bulletSpeed.ToString();
        pistolReduceShootCooldownText.text = "$100 - Reduce Shooting Cooldown (Current): " + pistol.GetComponent<ShootPistol>().shootCooldown.ToString();

        shotgunAddAmmoText.text = "$50 - Add 4 Ammo (Current): " + (shotgun.GetComponent<ShootShotgun>().totalAmmo + shotgun.GetComponent<ShootShotgun>().clipAmmo).ToString();
        shotgunAddClipSizeText.text = "$100 - Add 2 to Clip Size (Current): " + shotgun.GetComponent<ShootShotgun>().clipSize.ToString();
        shotgunAddBulletSpeedText.text = "$250 - Increase Bullet Speed (Current): " + shotgun.GetComponent<ShootShotgun>().bulletSpeed.ToString();
        shotgunReduceShootCooldownText.text = "$500 - Reduce Shooting Cooldown (Current): " + shotgun.GetComponent<ShootShotgun>().shootCooldown.ToString();
        shotgunReduceSpreadFactorText.text = "$100 - Decrease Spread Factor (Current): " + shotgun.GetComponent<ShootShotgun>().spreadFactor.ToString();
        shotgunIncreasePelletCountText.text = "$500 - Increase Pellet Count (Current): " + shotgun.GetComponent<ShootShotgun>().pelletCount.ToString();

        minigunAddAmmoText.text = "$100 - Add 25 Ammo (Current): " + minigun.GetComponent<ShootMinigun>().totalAmmo.ToString();
        minigunAddBulletSpeedText.text = "$250 - Increase Bullet Speed (Current): " + minigun.GetComponent<ShootMinigun>().bulletSpeed.ToString();

        scissorsAddAmmoText.text = "$100 - Add 1 Scissor (Current): " + scissors.GetComponent<ShootScissors>().totalAmmo.ToString();
        scissorsAddBulletSpeedText.text = "$100 - Increase Scissor Speed (Current): " + scissors.GetComponent<ShootScissors>().bulletSpeed.ToString();

        pistol.SetActive(false);
        shotgun.SetActive(false);
        minigun.SetActive(false);
        scissors.SetActive(false);
    }



/*********************************************************************************************************General *********************************************************************************************************/
    public void StartNextWave()
    {
        WaveManager.GetComponent<WaveManager>().WaveStart(player.GetComponent<PlayerStats>().wave);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayerAddHealth()
    {
        if (player.GetComponent<PlayerStats>().money >= 50 && player.GetComponent<PlayerStats>().health < 100)
        {
            player.GetComponent<PlayerStats>().money -= 50;
            player.GetComponent<PlayerStats>().health += 10;

            if (player.GetComponent<PlayerStats>().health > 100)
            {
                player.GetComponent<PlayerStats>().health = 100;
            }

            playerAddHealthText.text = "$50 - Add 10 Health (Current): " + player.GetComponent<PlayerStats>().health.ToString();
        }
    }

    public void PlayerAddBaseMove()
    {
        if (player.GetComponent<PlayerStats>().money >= 500 && player.transform.GetChild(0).GetComponent<PlayerMovement>().baseMoveSpeed < 100)
        {
            player.GetComponent<PlayerStats>().money -= 500;
            player.GetComponent<PlayerMovement>().baseMoveSpeed += 1;

            playerAddBaseMoveText.text = "$500 - Add 1 to Base Speed (Current): " + player.GetComponent<PlayerMovement>().baseMoveSpeed.ToString();
        }
    }

    public void PlayerAddSprint()
    {
        if (player.GetComponent<PlayerStats>().money >= 500 && player.transform.GetChild(0).GetComponent<PlayerMovement>().sprintMoveSpeed < 20)
        {
            player.GetComponent<PlayerStats>().money -= 500;
            player.GetComponent<PlayerMovement>().sprintMoveSpeed += 1;

            playerAddSprintSpeedText.text = "$500 - Add 1 to Sprint Speed (Current): " + player.GetComponent<PlayerMovement>().sprintMoveSpeed.ToString();
        }
    }

    public void PlayerAddDodge()
    {
        if (player.GetComponent<PlayerStats>().money >= 1500 && player.transform.GetChild(0).GetComponent<PlayerMovement>().dodgeTimeMax < 0.5f)
        {
            player.GetComponent<PlayerStats>().money -= 1500;
            player.GetComponent<PlayerMovement>().dodgeTimeMax += 0.05f;

            playerAddDodgeTimeText.text = "$1500 - Add 0.05 Seconds to Dodge Time (Current): " + player.GetComponent<PlayerMovement>().dodgeTimeMax.ToString();
        }
    }

    public void PistolIncreaseDamage()
    {
        if (player.GetComponent<PlayerStats>().money >= 250 && player.GetComponent<PlayerStats>().weaponBulletDamage[0] < 10)
        {
            player.GetComponent<PlayerStats>().money -= 250;
            player.GetComponent<PlayerStats>().weaponBulletDamage[0] += 1;
            pistolDamageText.text = "$250 - Increase Pistol Damage By 1 (Current): " + player.GetComponent<PlayerStats>().weaponBulletDamage[0].ToString();
        }
    }      

    public void ShotgunIncreaseDamage()
    {
        if (player.GetComponent<PlayerStats>().money >= 500 && player.GetComponent<PlayerStats>().weaponBulletDamage[1] < 5)
        {
            player.GetComponent<PlayerStats>().money -= 500;
            player.GetComponent<PlayerStats>().weaponBulletDamage[1] += 1;
            shotgunDamageText.text = "$500 - Increase Shotgun Damage By 1 (Current): " + player.GetComponent<PlayerStats>().weaponBulletDamage[1].ToString();
        }
    }

    public void MinigunIncreaseDamage()
    {
        if (player.GetComponent<PlayerStats>().money >= 1000 && player.GetComponent<PlayerStats>().weaponBulletDamage[2] < 5)
        {
            player.GetComponent<PlayerStats>().money -= 1000;
            player.GetComponent<PlayerStats>().weaponBulletDamage[2] += 1;
            minigunDamageText.text = "$1000 - Increase Minigun Damage By 1 (Current): " + player.GetComponent<PlayerStats>().weaponBulletDamage[2].ToString();
        }
    }



/*********************************************************************************************************Pistol *********************************************************************************************************/
    public void PistolAddAmmo()
    {
        pistol.SetActive(true);
        if (player.GetComponent<PlayerStats>().money >= 25)
        {
            player.GetComponent<PlayerStats>().money -= 25;
            pistol.GetComponent<ShootPistol>().totalAmmo += 8;
            pistolAddAmmoText.text = "$25 - Add 8 Ammo (Current): " + (pistol.GetComponent<ShootPistol>().totalAmmo + pistol.GetComponent<ShootPistol>().clipAmmo).ToString();
        }
        pistol.SetActive(false);
    }
    
    public void PistolAddClipSize()
    {
        pistol.SetActive(true);
        if (player.GetComponent<PlayerStats>().money >= 50 && pistol.GetComponent<ShootPistol>().clipSize <= 400)
        {
            player.GetComponent<PlayerStats>().money -= 50;
            pistol.GetComponent<ShootPistol>().clipSize += 4;
            pistolAddClipSizeText.text = "$50 - Add 4 to Clip Size (Current): " + pistol.GetComponent<ShootPistol>().clipSize.ToString();
        }
        pistol.SetActive(false);
    }

    public void PistolAddBulletSpeed()
    {
        pistol.SetActive(true);
        if (player.GetComponent<PlayerStats>().money >= 100 && pistol.GetComponent<ShootPistol>().bulletSpeed <= 2000)
        {
            player.GetComponent<PlayerStats>().money -= 100;
            pistol.GetComponent<ShootPistol>().bulletSpeed += 50;
            pistolAddBulletSpeedText.text = "$100 - Increase Bullet Speed (Current): " + pistol.GetComponent<ShootPistol>().bulletSpeed.ToString();
        }
        pistol.SetActive(false);
    }

    public void PistolReduceShootCooldown()
    {
        pistol.SetActive(true);
        if (player.GetComponent<PlayerStats>().money >= 100 && pistol.GetComponent<ShootPistol>().shootCooldown >= 0)
        {
            player.GetComponent<PlayerStats>().money -= 100;
            pistol.GetComponent<ShootPistol>().shootCooldown -= 0.1f;
            pistolReduceShootCooldownText.text = "$100 - Reduce Shooting Cooldown (Current): " + pistol.GetComponent<ShootPistol>().shootCooldown.ToString();
        }
        pistol.SetActive(false);
    }



/*********************************************************************************************************Shotgun *********************************************************************************************************/

    

    public void ShotgunAddAmmo()
    {
        shotgun.SetActive(true);
        if (player.GetComponent<PlayerStats>().money >= 50)
        {
            player.GetComponent<PlayerStats>().money -= 50;
            shotgun.GetComponent<ShootShotgun>().totalAmmo += 4;
            shotgunAddAmmoText.text = "$50 - Add 4 Ammo (Current): " + (shotgun.GetComponent<ShootShotgun>().totalAmmo + shotgun.GetComponent<ShootShotgun>().clipAmmo).ToString();
        }
        shotgun.SetActive(false);
    }

    public void ShotgunAddClipSize()
    {
        shotgun.SetActive(true);
        if (player.GetComponent<PlayerStats>().money >= 100 && shotgun.GetComponent<ShootShotgun>().clipSize < 40)
        {
            player.GetComponent<PlayerStats>().money -= 100;
            shotgun.GetComponent<ShootShotgun>().clipSize += 2;
            shotgunAddClipSizeText.text = "$100 - Add 2 to Clip Size (Current): " + shotgun.GetComponent<ShootShotgun>().clipSize.ToString();
        }
        shotgun.SetActive(false);
    }

    public void ShotgunAddBulletSpeed()
    {
        shotgun.SetActive(true);
        if (player.GetComponent<PlayerStats>().money >= 250 && shotgun.GetComponent<ShootShotgun>().bulletSpeed < 2000)
        {
            player.GetComponent<PlayerStats>().money -= 250;
            shotgun.GetComponent<ShootShotgun>().bulletSpeed += 100;
            shotgunAddBulletSpeedText.text = "$250 - Increase Bullet Speed (Current): " + shotgun.GetComponent<ShootShotgun>().bulletSpeed.ToString();
        }
        shotgun.SetActive(false);
    }

    public void ShotgunReduceShootCooldown()
    {
        shotgun.SetActive(true);
        if (player.GetComponent<PlayerStats>().money >= 500 && shotgun.GetComponent<ShootShotgun>().shootCooldown > 0)
        {
            player.GetComponent<PlayerStats>().money -= 500;
            shotgun.GetComponent<ShootShotgun>().shootCooldown -= 0.05f;
            shotgunReduceShootCooldownText.text = "$500 - Reduce Shooting Cooldown (Current): " + shotgun.GetComponent<ShootShotgun>().shootCooldown.ToString();
        }
        shotgun.SetActive(false);
    }

    public void ShotgunReduceSpreadFactor()
    {
        shotgun.SetActive(true);
        if (player.GetComponent<PlayerStats>().money >= 100 && shotgun.GetComponent<ShootShotgun>().spreadFactor >= 0.05f)
        {
            player.GetComponent<PlayerStats>().money -= 100;
            shotgun.GetComponent<ShootShotgun>().spreadFactor -= 0.05f;
            shotgunReduceSpreadFactorText.text = "$100 - Decrease Spread Factor (Current): " + shotgun.GetComponent<ShootShotgun>().spreadFactor.ToString();
        }
        shotgun.SetActive(false);
    }

    public void ShotgunIncreasePelletCount()
    {
        shotgun.SetActive(true);
        if (player.GetComponent<PlayerStats>().money >= 500 && shotgun.GetComponent<ShootShotgun>().pelletCount < 10)
        {
            player.GetComponent<PlayerStats>().money -= 500;
            shotgun.GetComponent<ShootShotgun>().pelletCount += 1;
            shotgunIncreasePelletCountText.text = "$500 - Increase Pellet Count (Current): " + shotgun.GetComponent<ShootShotgun>().pelletCount.ToString();
        }
        shotgun.SetActive(false);
    }


/*********************************************************************************************************Minigun *********************************************************************************************************/
    public void MinigunAddAmmo()
    {
        minigun.SetActive(true);
        if (player.GetComponent<PlayerStats>().money >= 100)
        {
            player.GetComponent<PlayerStats>().money -= 100;
            minigun.GetComponent<ShootMinigun>().totalAmmo += 25;
            minigunAddAmmoText.text = "$100 - Add 25 Ammo (Current): " + minigun.GetComponent<ShootMinigun>().totalAmmo.ToString();
        }
        minigun.SetActive(false);
    }

    public void MinigunAddBulletSpeed()
    {
        minigun.SetActive(true);
        if (player.GetComponent<PlayerStats>().money >= 250 && minigun.GetComponent<ShootMinigun>().bulletSpeed <= 5000)
        {
            player.GetComponent<PlayerStats>().money -= 250;
            minigun.GetComponent<ShootMinigun>().bulletSpeed += 50;
            minigunAddBulletSpeedText.text = "$250 - Increase Bullet Speed (Current): " + minigun.GetComponent<ShootMinigun>().bulletSpeed.ToString();
        }
        minigun.SetActive(false);
    }



/*********************************************************************************************************Scissors *********************************************************************************************************/
    public void ScissorsAddAmmo()
    {
        scissors.SetActive(true);
        if (player.GetComponent<PlayerStats>().money >= 100)
        {
            player.GetComponent<PlayerStats>().money -= 100;
            scissors.GetComponent<ShootScissors>().totalAmmo += 1;
            scissorsAddAmmoText.text = "$100 - Add 1 Scissor (Current): " + scissors.GetComponent<ShootScissors>().totalAmmo.ToString();
        }
        scissors.SetActive(false);
    }

    public void ScissorsAddBulletSpeed()
    {
        scissors.SetActive(true);
        if (player.GetComponent<PlayerStats>().money >= 100 && scissors.GetComponent<ShootScissors>().bulletSpeed <= 10000)
        {
            player.GetComponent<PlayerStats>().money -= 100;
            scissors.GetComponent<ShootScissors>().bulletSpeed += 500;
            scissorsAddBulletSpeedText.text = "$100 - Increase Scissor Speed (Current): " + scissors.GetComponent<ShootScissors>().bulletSpeed.ToString();
        }
        scissors.SetActive(false);
    }
}