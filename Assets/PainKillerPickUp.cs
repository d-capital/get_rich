using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PainkillerPickUp : MonoBehaviour
{
    public int healthPointsToAdd = 30;

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //TODO: animated effect of painkillers
        //GameObject.FindObjectOfType<PlayerController>().GetComponent<PlayerController>().animator.SetBool("hasWeapon", true);
        //TODO: add sound effect
        if (collision.gameObject.name == "Player")
        {
            if (collision.gameObject.GetComponent<PlayerController>().health + healthPointsToAdd > SaveSystem.Instance.playerData.MaxHealth)
            {
                collision.gameObject.GetComponent<PlayerController>().health = SaveSystem.Instance.playerData.MaxHealth;
                SaveSystem.Instance.playerData.Health = collision.gameObject.GetComponent<PlayerController>().health;
                collision.gameObject.GetComponent<PlayerController>().HealthBar.SetHealth(collision.gameObject.GetComponent<PlayerController>().health);
                Destroy(gameObject);
                FloatingNumber floatingNumber = Resources.LoadAll<FloatingNumber>("FloatingNumber").FirstOrDefault();
                GameObject player = FindObjectOfType<PlayerController>().gameObject;
                FloatingNumber fn = Instantiate(floatingNumber, player.transform.position, Quaternion.identity, player.transform);
                fn.ShowNumber(healthPointsToAdd);

            }
            else
            {
                collision.gameObject.GetComponent<PlayerController>().health += healthPointsToAdd;
                SaveSystem.Instance.playerData.Health = collision.gameObject.GetComponent<PlayerController>().health;
                collision.gameObject.GetComponent<PlayerController>().HealthBar.SetHealth(collision.gameObject.GetComponent<PlayerController>().health);
                Destroy(gameObject);
                FloatingNumber floatingNumber = Resources.LoadAll<FloatingNumber>("FloatingNumber").FirstOrDefault();
                GameObject player = FindObjectOfType<PlayerController>().gameObject;
                FloatingNumber fn = Instantiate(floatingNumber, player.transform.position, Quaternion.identity, player.transform);
                fn.ShowNumber(healthPointsToAdd);
            }
        }
    }
}
