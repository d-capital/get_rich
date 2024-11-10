using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class BossAI : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public Weapon weapon;

    public Ulta Ulta;

    public float distance;

    public float distanceToChase;
    public float distanceToStop;

    public int stamina = 30;
    public int health = 60;
    public float coolDownRate;

    public float ultaRate;
    public float nextUlta = 0;

    public Animator npcAnimator;

    public NavMeshAgent agent;

    public AudioSource footStepsSound;

    public HealthBar healthBar;

    bool hasToDash;
    int dashStamina = 30;

    public SpriteRenderer damageAffectedSprite;
    public Animator betAnimator;

    GameObject closestCorner;

    ScriptedSceneManager scriptedSceneManager;

    public float staminaRestoreTime;

    public bool chasesEnergyDrink;

    private void Start()
    {
        agent.updatePosition = true;
        agent.updateUpAxis = false;
        agent.updateRotation = false;
        if(FindObjectsOfType<ScriptedSceneManager>().Length > 0)
        {
            FindObjectOfType<ScriptedSceneManager>().isBossSpawned = true;
        }
        healthBar.SetMaxHealth(health);
        scriptedSceneManager = FindObjectOfType<ScriptedSceneManager>();
        StartCoroutine(RestoreStaminaAsync());
    }
    private void Update()
    {
        GameObject player = GameObject.FindObjectOfType<PlayerController>().gameObject;
        distance = Vector2.Distance(transform.position, player.transform.position);
        //chase player
        if (distance < distanceToChase && distance >= distanceToStop 
            && SceneManager.GetActiveScene().name == "SampleScene"
            && !scriptedSceneManager.isInGameSceneShowing
            && !chasesEnergyDrink)
        {
            agent.speed = speed;
            agent.destination = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
            npcAnimator.SetBool("hasToAttack", false);
            npcAnimator.SetBool("hasToRun", true);
            //footStepsSound.Play();
        }
        else if(distance < distanceToChase && distance >= distanceToStop 
                && SceneManager.GetActiveScene().name != "SampleScene"
                && SceneManager.GetActiveScene().name != "Level3"
                && !scriptedSceneManager.isInGameSceneShowing
                && !chasesEnergyDrink)
        {
            agent.speed = speed;
            agent.destination = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
            Vector3 rayDirection = player.transform.position - weapon.firePoint.position;
            RaycastHit2D playerHit = Physics2D.Raycast(weapon.firePoint.position, rayDirection);
            Debug.DrawRay(weapon.firePoint.position, rayDirection);
            if(playerHit.transform.gameObject.name == "Player")
            {
                npcAnimator.SetBool("hasToAttack", true);
                npcAnimator.SetBool("hasToRun", true);
            }else
            {
                npcAnimator.SetBool("hasToAttack", false);
                npcAnimator.SetBool("hasToRun", true);  
            }
        }
        //attack player
        else if (distance <= distanceToStop 
                && !scriptedSceneManager.isInGameSceneShowing  && SceneManager.GetActiveScene().name != "Level3"
                && !chasesEnergyDrink)
        {
            //check if player is reachable
            Vector3 rayDirection = player.transform.position - weapon.firePoint.position;
            RaycastHit2D playerHit = Physics2D.Raycast(weapon.firePoint.position, rayDirection);
            Debug.DrawRay(weapon.firePoint.position, rayDirection);
            if(playerHit.transform.gameObject.name == "Player")
            {
                //following is only for level 2 boss
                //find corners
                //go to one of the corners
                //fire
                agent.destination = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y,
                    gameObject.transform.position.z);
                agent.speed = 0;
                npcAnimator.SetBool("hasToAttack", true);
                npcAnimator.SetBool("hasToRun", false);
                //footStepsSound.Stop();
            }
        }
        /*else if(distance <= distanceToStop 
            && closestCorner && transform.position != closestCorner.transform.position
            && SceneManager.GetActiveScene().name != "SampleScene")
        {
            closestCorner = GetClosestCorner();
            agent.destination = new Vector3(closestCorner.transform.position.x, closestCorner.transform.position.y,
                    closestCorner.transform.position.z);
            agent.speed = speed;
            npcAnimator.SetBool("hasToAttack", true);
            npcAnimator.SetBool("hasToRun", true);
        }
        else if(distance <= distanceToStop 
            && closestCorner && transform.position == closestCorner.transform.position 
            && SceneManager.GetActiveScene().name != "SampleScene")
        {
             agent.destination = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y,
                    gameObject.transform.position.z);
            agent.speed = 0;
            npcAnimator.SetBool("hasToAttack", true);
            npcAnimator.SetBool("hasToRun", false);
        }*/
        else if(SceneManager.GetActiveScene().name == "Level7" 
            && FindObjectsOfType<Edrink>().Length > 0
            && health < healthBar.slider.maxValue/2)
        {
            Edrink closestEnergyDrink = FindObjectOfType<Edrink>();
            agent.destination = new Vector3(closestEnergyDrink.transform.position.x, closestEnergyDrink.transform.position.y,
                    closestEnergyDrink.transform.position.z);
            agent.speed = speed*2;
            npcAnimator.SetBool("hasToRun", true);
            npcAnimator.SetBool("hasToAttack", false);
            chasesEnergyDrink = true;
        }
        else
        {
            agent.destination = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y,
                gameObject.transform.position.z);
            agent.speed = 0;
            if(SceneManager.GetActiveScene().name != "Level3")
            {
                npcAnimator.SetBool("hasToAttack", false);
                npcAnimator.SetBool("hasToRun", false);
            }

            //footStepsSound.Stop();
        }
        ShoulPerformUlta();
        if(SceneManager.GetActiveScene().name != "SampleScene" 
            && SceneManager.GetActiveScene().name != "Level3")
        {
            Reload();
        }
    }

    public void ShoulPerformUlta()
    {
        if (distance < distanceToChase && stamina == dashStamina && SceneManager.GetActiveScene().name == "SampleScene")
        {
            hasToDash = true;
        }
    }

    private void FixedUpdate()
    {
        Vector2 playerPosition = GameObject.FindObjectOfType<PlayerController>().gameObject.transform.position;
        Vector2 aimDirection = playerPosition - rb.position;
        float aimAngel = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngel;

        if(hasToDash)
        {
            transform.position = new Vector2(playerPosition.x+3f, playerPosition.y+3f);
            stamina=0;
            npcAnimator.ResetTrigger("Ulta");
            npcAnimator.SetTrigger("Ulta");
            hasToDash = false;      
            betAnimator.SetBool("prepForUlta", false);
            StartCoroutine(RestoreStaminaAsync());
            StartCoroutine(ShowPreparationForUlta());
        }
    }

    public void ShowDamage()
    {
        damageAffectedSprite.color = Color.red;
        StartCoroutine(HideDamageAsync());
    }

    IEnumerator HideDamageAsync()
    {
        yield return new WaitForSeconds(1.0f);
        damageAffectedSprite.color = new Color(255,255,255,255);
    }
    public void fireNextBottle()
    {
        Ulta.Fire(stamina);
    }

    public void Fire()
    {
        weapon.MultiFire();
        stamina -=7;
    }

    IEnumerator RestoreStaminaAsync(){
        yield return new WaitForSeconds(staminaRestoreTime);
        stamina = dashStamina;
    }

    IEnumerator ShowPreparationForUlta()
    {
       yield return new WaitForSeconds(6.0f);
       betAnimator.SetBool("prepForUlta", true);
    }

    public void BackToTheGame()
    {
        FindObjectOfType<ScriptedSceneManager>().BackToTheGame();
    }

    void Reload()
    {
        if(stamina <=5)
        {
            npcAnimator.SetBool("hasToAttack",false);
            weapon.reloadSound.Play();
            StartCoroutine(RestoreStaminaAsync());
            StartCoroutine(SwitchAttackAnimationOnAsync());
        }
    }

    GameObject GetClosestCorner()
    {
        List<GameObject> corners = GameObject.FindGameObjectsWithTag("Corner").ToList();
        Dictionary<GameObject, float> bossCornerDistances = new Dictionary<GameObject, float>();
        foreach(GameObject corner in corners)
        {
            Vector3 cornerPosition = corner.transform.position;
            float distance = Vector3.Distance(cornerPosition, transform.position);
            bossCornerDistances.Add(corner, distance);
        }
        GameObject closestCorner = bossCornerDistances.FirstOrDefault(x => x.Value.Equals(bossCornerDistances.Values.Min())).Key;
        return closestCorner;
    }

    IEnumerator SwitchAttackAnimationOnAsync(){
        yield return new WaitForSeconds(1.0f);
        npcAnimator.SetBool("hasToAttack",true);
    }
}
