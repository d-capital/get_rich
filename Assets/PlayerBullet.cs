using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBullet : MonoBehaviour
{
    public int damage = 10;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject target = collision.collider.gameObject;
        Destroy(gameObject);
        //check if enemy was hit and register damage
        if (target.GetComponents<EnemyAI>().Length > 0)
        {
            HealthBar enemyHealthBar = target.GetComponentInChildren<HealthBar>();
            if (target.GetComponent<EnemyAI>().health > 0)
            {
                target.GetComponent<EnemyAI>().health -= damage;
                enemyHealthBar.SetHealth(target.GetComponent<EnemyAI>().health);
                target.GetComponent<EnemyAI>().ShowDamage();
            }
            else
            {
                //destroy:
                GameObject.FindObjectOfType<RoomManager>().enemiesCount -= 1;
                Vector3 spawnPointToUse = target.transform.position;
                Destroy(target);
                HideEnemyHealthBar();
                FindObjectOfType<HitCounter>().UpdateHitCounter();
                Explosion explosion = Resources.Load<Explosion>("explosion");
                Instantiate(explosion, spawnPointToUse, Quaternion.identity);
                int ammoOrPainkillerProbability = Random.Range(0, 100);
                if (ammoOrPainkillerProbability > 20) 
                {
                    PainkillerPickUp PainkillerPrefab = Resources.Load<PainkillerPickUp>("Painkiller_pack");
                    int painkillerSpawnProbability = Random.Range(0, 100);
                    Instantiate(explosion, spawnPointToUse, Quaternion.identity);
                    if (painkillerSpawnProbability > 10)
                    {
                        Instantiate(PainkillerPrefab, spawnPointToUse, Quaternion.identity);
                    }
                }
                else
                {
                    int ammoOrWeaponProbability = Random.Range(0, 100);
                    if (ammoOrWeaponProbability > 35)
                    {
                        Ammo[] ammo = Resources.LoadAll<Ammo>("Ammo");
                        
                        int ammoIndexToSpawn = Random.Range(0, ammo.Length-1);
                        int ammoSpwanProbability = Random.Range(0, 100);
                        
                        if (ammoSpwanProbability > 50)
                        {
                            Instantiate(ammo[ammoIndexToSpawn], spawnPointToUse, Quaternion.identity);
                        }
                    }
                    else
                    {
                        WeaponPickUp[] weaponPickUps = Resources.LoadAll<WeaponPickUp>("WeaponPickUps");
                        int weaponIndexToSpawn = Random.Range(0, weaponPickUps.Length-1);
                        int weaponSpwanProbability = Random.Range(0, 100);
                        if (weaponSpwanProbability > 10)
                        {
                            Instantiate(weaponPickUps[weaponIndexToSpawn], spawnPointToUse, Quaternion.identity);
                        }
                    }
                }
            }
        }
        else if (target.GetComponents<BossAI>().Length > 0 && SceneManager.GetActiveScene().name != "Level3")
        {
            HealthBar enemyHealthBar = target.GetComponentInChildren<HealthBar>();
            if (target.GetComponent<BossAI>().health > 0)
            {
                target.GetComponent<BossAI>().health -= damage;
                target.GetComponent<BossAI>().healthBar.gameObject.SetActive(true);
                enemyHealthBar.SetHealth(target.GetComponent<BossAI>().health);
                target.GetComponent<BossAI>().ShowDamage();
            }
            else
            {
                //destroy:
                Vector3 spawnPointToUse = target.transform.position;
                Destroy(target);
                FindObjectOfType<HitCounter>().UpdateHitCounter();
                CollectableItem keyPrefab = Resources.Load<CollectableItem>("key");
                Explosion explosion = Resources.Load<Explosion>("explosion");
                Instantiate(explosion, spawnPointToUse, Quaternion.identity);
                Instantiate(keyPrefab, spawnPointToUse, Quaternion.identity);
                if(SceneManager.GetActiveScene().name == "Level2")
                {
                    PickUpItem evidenceCase = Resources.Load<PickUpItem>("PickUps/Evidence");
                    Instantiate(evidenceCase, spawnPointToUse, Quaternion.identity);
                }
                FindObjectOfType<ScriptedSceneManager>().isBossDead = true;
                if(SceneManager.GetActiveScene().name == "Level7")
                {
                    SceneManager.LoadScene("CutScene7");
                }
            }
        }
        else if (target.GetComponents<DestractableObject>().Length > 0)
        {
            ShowEnemyHealthBar();
            HealthBar enemyHealthBar = GameObject.Find("EnemyHealthBar").GetComponent<HealthBar>();
            enemyHealthBar.ResetNameAndHealth(
                target.GetComponent<DestractableObject>().health,
                " ");
            if (target.GetComponent<DestractableObject>().health > 0)
            {
                target.GetComponent<DestractableObject>().health -= damage;
                enemyHealthBar.SetHealth(target.GetComponent<DestractableObject>().health);
            }
            else
            {
                //destroy:
                Vector3 spawnPointToUse = target.transform.position;
                Destroy(target);
                HideEnemyHealthBar();
            }
        }
        else if (target.GetComponents<LaserTuret>().Length > 0)
        {
            HealthBar enemyHealthBar = target.GetComponentInChildren<HealthBar>();
            if (target.GetComponent<LaserTuret>().health > 0)
            {
                target.GetComponent<LaserTuret>().health -= damage;
                enemyHealthBar.SetHealth(target.GetComponent<LaserTuret>().health);
            }
            else
            {
                //destroy:
                Vector3 spawnPointToUse = target.transform.position;
                Destroy(target);
                HideEnemyHealthBar();
                FindObjectOfType<HitCounter>().UpdateHitCounter();
                Explosion explosion = Resources.Load<Explosion>("explosion");
                int ammoOrWeaponProbability = Random.Range(0, 100);
                if (ammoOrWeaponProbability > 20)
                {
                    Ammo[] ammo = Resources.LoadAll<Ammo>("Ammo");
                    
                    int ammoIndexToSpawn = Random.Range(0, ammo.Length-1);
                    int ammoSpwanProbability = Random.Range(0, 100);
                    
                    if (ammoSpwanProbability > 50)
                    {
                        Instantiate(ammo[ammoIndexToSpawn], spawnPointToUse, Quaternion.identity);
                    }
                }
                else
                {
                    WeaponPickUp[] weaponPickUps = Resources.LoadAll<WeaponPickUp>("WeaponPickUps");
                    int weaponIndexToSpawn = Random.Range(0, weaponPickUps.Length-1);
                    int weaponSpwanProbability = Random.Range(0, 100);
                    if (weaponSpwanProbability > 50)
                    {
                        Instantiate(weaponPickUps[weaponIndexToSpawn], spawnPointToUse, Quaternion.identity);
                    }
                }
                Instantiate(explosion, spawnPointToUse, Quaternion.identity);
            }
        }


    }

    private void ShowEnemyHealthBar()
    {
        HealthBar[] healthBars = Resources.FindObjectsOfTypeAll<HealthBar>();
        foreach (HealthBar i in healthBars)
        {
            if (i.gameObject.name == "EnemyHealthBar")
            {
                i.gameObject.SetActive(true);
            }
        }
    }

    private void HideEnemyHealthBar()
    {
        HealthBar[] healthBarControlls = GameObject.FindObjectsOfType<HealthBar>();
        foreach (HealthBar i in healthBarControlls)
        {
            if (i.gameObject.name == "EnemyHealthBar")
            {
                i.gameObject.SetActive(false);
            }
        }
    }
}
