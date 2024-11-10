using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ammo : MonoBehaviour
{
    public string weaponType;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //TODO: add sound effect
        if (collision.gameObject.name == "Player")
        {
            int currentWeaponIndex = collision.gameObject.GetComponent<PlayerController>().currentWeaponIndex;
            List<WeaponInfoSource> coincidingWeaponInfoSource = collision.gameObject.
                GetComponent<PlayerController>().
                weaponInfoSources.Where(wifs => wifs.weaponType == weaponType).ToList();
            if (coincidingWeaponInfoSource.Count>0)
            {
                collision.gameObject.
                    GetComponent<PlayerController>().
                    weaponInfoSources.
                    FirstOrDefault(wifs => 
                        wifs.weaponType == weaponType).
                        currentLoadsQuantity += 1;
                if(collision.gameObject.GetComponent<PlayerController>()
                    .weaponInfoSources[currentWeaponIndex].weaponType == weaponType)
                {
                    collision.gameObject.GetComponent<PlayerController>().currentWeaponLoadsQuantity += 1;
                    collision.gameObject.GetComponent<PlayerController>().numberOfLoads.text = collision.gameObject.GetComponent<PlayerController>().currentWeaponLoadsQuantity.ToString();
                }
                Destroy(gameObject);
            }
        }
    }
}
