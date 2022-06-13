using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShootPistol : MonoBehaviour
{
    public Transform bulletPrefab;
    private GameObject gunBarrel;

    // Pistol stats. All public since they need to be edited via WaveManager.cs
    public float bulletSpeed = 250f;
    public int totalAmmo = 56;
    public int clipAmmo = 8;
    public int clipSize = 8;
    public float shootCooldown = 0.5f;
    
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
        if (gunBarrel == null)
        {
            Debug.Log("No barrel.");
        }
    }

    void Update()
    {
        ammoUI.text = clipAmmo.ToString() + "/" + totalAmmo.ToString();

        // Shoot
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ShootCheck();
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
                StartCoroutine(ShootTimer());
                Shoot();
            }
        } else {
            emptyClip.Play();
        }
    }

    void Shoot()
    {
        Rigidbody2D newBul = Instantiate(bulletPrefab, gunBarrel.transform.position,   this.transform.rotation).GetComponent<Rigidbody2D>();
        newBul.AddForce(gunBarrel.transform.right * bulletSpeed);
        clipAmmo -= 1;
    }

    IEnumerator ShootTimer()
    {
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}
