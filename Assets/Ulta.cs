using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ulta : MonoBehaviour
{
    Bullet prefabToUse;
    public Bullet bulletPrefab;
    public Transform ultaPoint1;
    public Transform ultaPoint2;
    public float fireForce = 20f;

    public void Fire(int stamina)
    {
        List<Transform> ultaPoints = new List<Transform>();
        ultaPoints.Add(ultaPoint1);
        ultaPoints.Add(ultaPoint2);
        if (stamina > 5)
        {
            prefabToUse = bulletPrefab;
            foreach (Transform ultaPoint in ultaPoints)
            {
                Bullet bullet = Instantiate(prefabToUse, ultaPoint.position, ultaPoint.rotation);
                bullet.GetComponent<Rigidbody2D>().AddForce(ultaPoint.up * fireForce, ForceMode2D.Impulse);
            }
        }


    }
}
