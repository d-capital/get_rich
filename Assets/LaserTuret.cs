using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTuret : MonoBehaviour
{
    public int health = 100;
    public HealthBar healthBar;
    private void Start()
    {
        healthBar.SetMaxHealth(health);
    }
    void Update()
    {
        transform.Rotate(0, 0, 50 * Time.deltaTime);
    }
}
