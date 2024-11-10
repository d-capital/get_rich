using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Bullet prefabToUse;
    public Bullet bulletPrefab;
    PlayerBullet playerPrefabToUse;
    public Transform firePoint;
    public Transform firePoint2;
    public Transform firePoint3;
    public float fireForce = 20f;

    public AudioSource shotSound;
    public AudioSource reloadSound;
    public Animator animator;

    Queue<Transform> firePointsQueue = new Queue<Transform>();

    void Start()
    {
        firePointsQueue.Enqueue(firePoint);
        firePointsQueue.Enqueue(firePoint2);
        firePointsQueue.Enqueue(firePoint3);
    }
    public void Fire()
    {
        prefabToUse = bulletPrefab;
        Bullet bullet = Instantiate(prefabToUse, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
    }

    public void MultiFire()
    {
        prefabToUse = bulletPrefab;
        if(firePointsQueue.Count == 0)
        {
            firePointsQueue.Enqueue(firePoint);
            firePointsQueue.Enqueue(firePoint2);
            firePointsQueue.Enqueue(firePoint3); 
        }
        Transform firePointToUse = firePointsQueue.Dequeue();
        Bullet bullet = Instantiate(prefabToUse, firePointToUse.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
        shotSound.Play();
    }

    public void PlayerFire()
    {
        int currentWeaponIndex = GameObject.FindObjectOfType<PlayerController>().currentWeaponIndex;
        playerPrefabToUse = GameObject.FindObjectOfType<PlayerController>().weaponInfoSources[currentWeaponIndex].waeponPrefab;
        PlayerBullet bullet = Instantiate(playerPrefabToUse, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
        bool isRifle = GameObject.FindObjectOfType<PlayerController>().weaponInfoSources[currentWeaponIndex].isRifle;
        if(isRifle)
        {
            animator.SetTrigger("bang_rifle");
        }
        else
        {
            animator.SetTrigger("bang");
        }
    }

    public void Player3Shoot()
    {
        int currentWeaponIndex = GameObject.FindObjectOfType<PlayerController>().currentWeaponIndex;
        playerPrefabToUse = GameObject.FindObjectOfType<PlayerController>().weaponInfoSources[currentWeaponIndex].waeponPrefab;
        PlayerBullet bullet1 = Instantiate(playerPrefabToUse, firePoint.position, firePoint.rotation);
        bullet1.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
        PlayerBullet bullet2 = Instantiate(playerPrefabToUse, firePoint2.position, firePoint2.rotation);
        bullet2.GetComponent<Rigidbody2D>().AddForce(firePoint2.up * fireForce, ForceMode2D.Impulse);
        PlayerBullet bullet3 = Instantiate(playerPrefabToUse, firePoint3.position, firePoint3.rotation);
        bullet3.GetComponent<Rigidbody2D>().AddForce(firePoint3.up * fireForce, ForceMode2D.Impulse);
    }
}
