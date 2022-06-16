using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShootScissors : MonoBehaviour
{
    public Transform bulletPrefab;
    private GameObject gunBarrel;

    // Scissor stats. All public since they need to be edited via WaveManager.cs
    public float bulletSpeed = 1000f;
    public int totalAmmo = 1;

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
        gunBarrel = GameObject.FindGameObjectWithTag("GunTip");
    }

    void Update()
    {
        ammoUI.text = totalAmmo.ToString();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ShootCheck();
        }
    }

    public void ShootCheck()
    {
        if (totalAmmo > 0)
        {
            thisGun.Play();
            Shoot();
        } else {
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
}