using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShootShotgun : MonoBehaviour
{
    public Transform bulletPrefab;
    private GameObject gunBarrel;

    // Shotgun stats. All public since they need to be edited via WaveManager.cs
    public float bulletSpeed = 500f;
    public int totalAmmo = 32;
    public int clipAmmo = 4;
    public int clipSize = 4;
    public float shootCooldown = 1f;
    private float shootTimer;
    public float spreadFactor = 0.5f;
    public int pelletCount = 6;
    
    private bool canShoot = true;

    private Text ammoUI;
    private AudioSource thisGun;
    private AudioSource emptyClip;

    void Start()
    {
        ammoUI = GameObject.FindGameObjectWithTag("AmmoUI").GetComponent<Text>();
        thisGun = this.gameObject.GetComponent<AudioSource>();
        emptyClip = this.transform.parent.GetComponent<AudioSource>();
    }

    // Not in start because if it was switching while false would softlock the gun.
    void OnEnable() 
    {
        canShoot = true;
        gunBarrel = GameObject.FindGameObjectWithTag("GunTip");
    }

    void Update()
    {
        // Updates ammo UI
        ammoUI.text = clipAmmo.ToString() + "/" + totalAmmo.ToString();

        // Shoot
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ShootCheck();
        }

        // Shoot cooldown timer
        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        } else {
            canShoot = true;
        }

        // Reload
        if (Input.GetKeyDown(KeyCode.Mouse1) && clipAmmo < (clipSize / 2))
        {
            // Doesn't reload if remaining ammo is less than 0.
            if (totalAmmo - (clipSize - clipAmmo) >= 0)
            {
                totalAmmo -= (clipSize - clipAmmo);
                clipAmmo = clipSize;
            } else {
                clipAmmo += totalAmmo;
                totalAmmo = 0;
            }
        }
    }

    public void ShootCheck()
    {
        if (clipAmmo > 0)
        {
            if (canShoot)
            {
                thisGun.Play();
                canShoot = false;
                shootTimer = shootCooldown;
                Shoot();
            }
        } else {
            emptyClip.Play();
        }
    }

    void Shoot()
    {
        for (int i = 0; i < pelletCount; i++)
        {
            // Adds a random spread to the gun barrel location
            Vector3 spreadPos = gunBarrel.transform.right;
            spreadPos.x += Random.Range(-spreadFactor, spreadFactor);
            spreadPos.y += Random.Range(-spreadFactor, spreadFactor);
            spreadPos.z = 0;

            Rigidbody2D newBul = Instantiate(bulletPrefab, gunBarrel.transform.position, this.transform.rotation).GetComponent<Rigidbody2D>();

            newBul.AddForce(spreadPos * bulletSpeed);
        }
        clipAmmo -= 1;
    }
}