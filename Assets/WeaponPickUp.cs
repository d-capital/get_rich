using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class WeaponPickUp : MonoBehaviour
{
    public WeaponInfoSource weaponInfoSource;

    private void Start()
    {
        weaponInfoSource.shotSound = FindObjectsOfType<AudioSource>().
            Where(x => x.name == weaponInfoSource.shotSoundName).FirstOrDefault();
        weaponInfoSource.reloadSound = FindObjectsOfType<AudioSource>().
            Where(x => x.name == weaponInfoSource.reloadSoundName).FirstOrDefault();
        weaponInfoSource.nothingToReloadSound = FindObjectsOfType<AudioSource>().
            Where(x => x.name == weaponInfoSource.nothingToReloadSoundName).FirstOrDefault();
        weaponInfoSource.nothingToShootSound = FindObjectsOfType<AudioSource>().
            Where(x => x.name == weaponInfoSource.nothingToShootSoundName).FirstOrDefault();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            List<WeaponInfoSource> playerWeaponInfoSource = collision.gameObject.GetComponent<PlayerController>().weaponInfoSources;
            bool hasPlayerSuchWeaponAlready = playerWeaponInfoSource.Where(x=>x.weaponType == weaponInfoSource.weaponType).ToList().Count()>0;
            if(!hasPlayerSuchWeaponAlready){
                playerWeaponInfoSource.Add(weaponInfoSource);
            }
            else
            {
                playerWeaponInfoSource.FirstOrDefault(x=>x.weaponType == weaponInfoSource.weaponType).currentLoadsQuantity += 5;
            }
            Destroy(gameObject);
        }
    }
}
