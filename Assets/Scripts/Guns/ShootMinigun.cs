using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShootMinigun : MonoBehaviour
{
    public Transform bulletPrefab;
    private GameObject gunBarrel;

    // Minigun stats. All public since they need to be edited via WaveManager.cs
    public float bulletSpeed = 250f;
    public int totalAmmo = 500;

    private bool canShoot = true;
    private bool shooting;

    private Text ammoUI;
    private AudioSource thisGun;
    private AudioSource emptyClip;

    void Start()
    {
        ammoUI = GameObject.FindGameObjectWithTag("AmmoUI").GetComponent<Text>();
        thisGun = this.gameObject.GetComponent<AudioSource>();
        emptyClip = this.transform.parent.GetComponent<AudioSource>();
    }

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
        ammoUI.text = totalAmmo.ToString();

        // Shoot
        if (Input.GetKey(KeyCode.Mouse0))
        {
            ShootCheck();
        } else {
            shooting = false;
        }

        // Used for the audio, since clips can't overlap :(
        if (shooting)
        {
            if (!thisGun.isPlaying)
            {
                thisGun.Play();
            }
        } else {
            thisGun.Stop();
        }
    }

    public void ShootCheck()
    {
        if (totalAmmo > 0)
        {
            if (canShoot)
            {
                shooting = true;
                canShoot = false;
                StartCoroutine(ShootTimer());
                Shoot();
            }
        } else {
            shooting = false;
            if (!emptyClip.isPlaying)
            {
                emptyClip.Play();
            }
        }
    }

    void Shoot()
    {
        Rigidbody2D newBul = Instantiate(bulletPrefab, gunBarrel.transform.position,   this.transform.rotation).GetComponent<Rigidbody2D>();
        newBul.AddForce(gunBarrel.transform.right * bulletSpeed);
        totalAmmo -= 1;
    }

    IEnumerator ShootTimer()
    {
        yield return new WaitForSeconds(0.05f);
        canShoot = true;
    }
}