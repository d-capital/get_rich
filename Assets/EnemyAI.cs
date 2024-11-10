using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public Weapon weapon;

    public float distance;

    public float distanceToChase;
    public float distanceToStop;

    public int stamina = 30;
    public int health = 60;
    public float fireRate;
    public float nextFire = 0;
    public Animator npcAnimator;

    public NavMeshAgent agent;

    public HealthBar healthBar;

    public bool isOnlyMeleeFighter;

    public SpriteRenderer damageAffectedSprite;

    //public AudioSource footStepsSound;
    public AudioSource shotSound;
    public AudioSource reloadSound;

    public Transform grenadePoint;
    public int grenadeCount;
    public float grenadeForce;
    public Grenade grenade;
    public int throwStamina = 100;
    public int throwStaminaNeeded = 100;
    public bool allowedToShootAndMove = true;

    private void Start()
    {
        agent.updatePosition = true;
        agent.updateUpAxis = false;
        agent.updateRotation = false;
        healthBar.SetMaxHealth(health);
    }
    private void Update()
    {
        GameObject player = GameObject.FindObjectOfType<PlayerController>().gameObject;
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        if(allowedToShootAndMove)
        {
            if (distance < distanceToChase && distance >= distanceToStop)
            {
                agent.speed = speed;
                agent.destination = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
                if (!isOnlyMeleeFighter && stamina>=5)
                {
                    Vector3 rayDirection = player.transform.position - weapon.firePoint.position;
                    RaycastHit2D playerHit = Physics2D.Raycast(weapon.firePoint.position, rayDirection);
                    Debug.DrawRay(weapon.firePoint.position, rayDirection);
                    if(playerHit.transform.gameObject.name == "Player")
                    {
                        npcAnimator.SetBool("hasToRun", true);
                        npcAnimator.SetBool("hasToAttack", true);
                    }
                }
                else
                {
                    npcAnimator.SetBool("hasToRun", true);
                    npcAnimator.SetBool("hasToAttack", false);
                }
                //footStepsSound.Play();
            }
            else if (distance <= distanceToStop)
            {
                agent.destination = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y,
                    gameObject.transform.position.z);
                agent.speed = 0;
                if(stamina>=5)
                {
                    Vector3 rayDirection = player.transform.position - weapon.firePoint.position;
                    RaycastHit2D playerHit = Physics2D.Raycast(weapon.firePoint.position, rayDirection);
                    Debug.DrawRay(weapon.firePoint.position, rayDirection);
                    if(playerHit.transform.gameObject.name == "Player")
                    {
                        npcAnimator.SetBool("hasToRun", false);                
                        npcAnimator.SetBool("hasToAttack", true);
                    }
                }
                else if(throwStamina == throwStaminaNeeded && grenadeCount > 0)
                {
                    throwStamina -= 20;
                    grenadeCount -= 1;
                    npcAnimator.SetTrigger("throw");
                }
                else
                {
                    npcAnimator.SetBool("hasToRun", false);                
                    npcAnimator.SetBool("hasToAttack", false);
                }

            }
            else if (distance >= distanceToChase)
            {
                agent.destination = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y,
                    gameObject.transform.position.z);
                agent.speed = 0;
                npcAnimator.SetBool("hasToRun", false);
                npcAnimator.SetBool("hasToAttack", false);
                //footStepsSound.Stop();
            }
            updateFireRate();
        }
        else
        {
            agent.destination = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y,
                    gameObject.transform.position.z);
            agent.speed = 0;
            npcAnimator.SetBool("hasToRun", false);
            npcAnimator.SetBool("hasToAttack", false);
        }
    }

    public void updateFireRate()
    {
        if (stamina <5)
        {
            npcAnimator.SetBool("hasToAttack", false);
            StartCoroutine(RestoreStaminAsync());
            reloadSound.Play();
        }
    }

    IEnumerator RestoreThrowStaminaAsync()
    {
        yield return new WaitForSeconds(7.0f);
        throwStamina = 100;
    }

    IEnumerator RestoreStaminAsync()
    {
        yield return new WaitForSeconds(2.0f);
        npcAnimator.SetBool("hasToAttack", true);
        stamina = 30;

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

    public void setFireModeWhileWalking()
    {
        npcAnimator.Play("goesandshoots");
    }
    public void setFireModeWhileStaying()
    {
        npcAnimator.Play("fire");
    }

    private void FixedUpdate()
    {
        Vector2 playerPosition = GameObject.FindObjectOfType<PlayerController>().gameObject.transform.position;
        Vector2 aimDirection = playerPosition - rb.position;
        float aimAngel = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngel;

    }
    public void fireNextBottle()
    {
        stamina = stamina - 5;
        if (stamina > 5)
        {
            weapon.Fire();
            shotSound.Play();
        }
    }

    public void ThrowGrenade()
    {
        Grenade bullet = Instantiate(grenade, grenadePoint.position, grenadePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(grenadePoint.up * grenadeForce, ForceMode2D.Impulse);
        StartCoroutine(RestoreThrowStaminaAsync());
    }
}
