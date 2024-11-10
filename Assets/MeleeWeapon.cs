using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public int damage = 10;
    public AudioSource weaponSound;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject target = collision.collider.gameObject;
        if (target.name == "Player")
        {
            if (target.GetComponent<PlayerController>().health > 0)
            {
                target.GetComponent<PlayerController>().health -= damage;
                int playerHealth = target.GetComponent<PlayerController>().health;
                target.GetComponent<PlayerController>().HealthBar.SetHealth(playerHealth);
                SaveSystem.Instance.playerData.Health = playerHealth;
                SaveSystem.Instance.SavePlayer();
                target.GetComponent<PlayerController>().rb.angularVelocity = 0;
                weaponSound.Play();
            }
            else
            {
                target.GetComponent<PlayerController>().rb.angularVelocity = 0;
                GameObject.FindObjectOfType<PlayerController>().ShowGameOver();
            }
        }
    }
}
