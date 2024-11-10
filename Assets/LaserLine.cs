using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLine : MonoBehaviour
{
    public int damage = 1;
    public Transform rayStartPoint;
    public Transform rayEndPoint;
    public float hitDistance = 3.9f;


    private void Update()
    {
        RaycastHit2D hit;
        var rayDirection = rayEndPoint.position - rayStartPoint.position;
        hit = Physics2D.Raycast(rayStartPoint.position, rayDirection);
        //Debug.DrawRay(rayStartPoint.position, rayEndPoint.position);
        Vector3.Distance(rayStartPoint.position, rayEndPoint.position);
        var target = hit.collider;
        if (target != null && target.gameObject.name == "Player" 
            && Vector3.Distance(rayStartPoint.position, target.gameObject.transform.position) <= hitDistance)
        {
            if (target.GetComponent<PlayerController>().health > 0)
            {
                target.GetComponent<PlayerController>().health -= damage;
                int playerHealth = target.GetComponent<PlayerController>().health;
                target.GetComponent<PlayerController>().HealthBar.SetHealth(playerHealth);
                SaveSystem.Instance.playerData.Health = playerHealth;
                SaveSystem.Instance.SavePlayer();
                target.GetComponent<PlayerController>().rb.angularVelocity = 0;
            }
            else
            {
                target.GetComponent<PlayerController>().rb.angularVelocity = 0;
                GameObject.FindObjectOfType<PlayerController>().ShowGameOver();
            }
        }
    }
}
