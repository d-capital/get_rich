using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    public int damage = 10;
    public string soundCode;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject target = collision.collider.gameObject;
        /*if (soundCode == "glass")
        {
            GameObject.Find("shatteredGlassSound").GetComponent<AudioSource>().Play();
        }*/

        Destroy(gameObject);
        //check if enemy was hit and register damage
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
                target.GetComponent<PlayerController>().ShowDamage();
            }
            else
            {
                target.GetComponent<PlayerController>().rb.angularVelocity = 0;
                GameObject.FindObjectOfType<PlayerController>().ShowGameOver();
            }
        }

    }
}
