using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edrink : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject target = collision.collider.gameObject;
        if (target.GetComponents<BossAI>().Length > 0)
        {
            int newHealth = target.GetComponent<BossAI>().health += 10;
            if(newHealth < target.GetComponent<BossAI>().healthBar.slider.maxValue)
            {
                target.GetComponent<BossAI>().health += 100;
            }
            else
            {
                target.GetComponent<BossAI>().health = (int)target.GetComponent<BossAI>().healthBar.slider.maxValue;
            }
            target.GetComponent<BossAI>().chasesEnergyDrink = false;
            Destroy(gameObject);
        }
    }
}
