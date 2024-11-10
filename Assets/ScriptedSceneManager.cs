using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptedSceneManager : MonoBehaviour
{
    public PlayerController player;
    public float distance;
    public float distanceToShowScene;
    public bool isSceneShown = false;
    BossAI boss;
    public bool isBossSpawned = false;
    public bool isBossDead = false;
    public Camera mainCamera;
    Camera bossCamera;
    public bool isInGameSceneShowing;
    public DialogueBox dialogueBox;

    public HealthBar playerHealthBar;
    public StaminaBar playerStaminaBar;
    public HitCounter hitCounter;
    public WeaponInfo weaponInfo;
    public GameObject musicPlayer;
    public float backToTheGame  = 40.0f;
    public bool nextLevelCoroutineStarted = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(isBossSpawned 
            && !isBossDead 
            && SceneManager.GetActiveScene().name != "Level4" 
            && SceneManager.GetActiveScene().name != "Level6")
        {
            if(boss == null)
            {
                boss = FindObjectOfType<BossAI>();
                bossCamera = boss.GetComponentInChildren<Camera>();
            }
            distance = Vector3.Distance(player.transform.position, boss.transform.position);
            if(distance<=distanceToShowScene && !isSceneShown)
            {
                isInGameSceneShowing = true;
                //block shooting and moving of enemies exept boss
                EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();
                foreach(EnemyAI e in enemies)
                {
                    e.allowedToShootAndMove = false;
                }
                if(SceneManager.GetActiveScene().name == "SampleScene")
                {
                    mainCamera.enabled = false;
                    bossCamera.enabled = true;
                    FindObjectOfType<MusicPlayerManager>().StopAnyAbility();
                    boss.npcAnimator.SetBool("drinkEnergyDrink", true);
                }
                else if(SceneManager.GetActiveScene().name == "Level2" || SceneManager.GetActiveScene().name == "Level5"
                    || SceneManager.GetActiveScene().name == "Level7")
                {
                    mainCamera.orthographicSize = 20;
                    HideUiObjects();
                    dialogueBox.gameObject.SetActive(true);
                    FindObjectOfType<MusicPlayerManager>().StopAnyAbility();
                    player.rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                    dialogueBox.transform.position = new Vector3(dialogueBox.transform.position.x,
                                dialogueBox.transform.position.y, -1);
                    StartCoroutine(BackToTheGameAync());
                }
                else if(SceneManager.GetActiveScene().name == "Level3")
                {
                    boss.npcAnimator.SetBool("handsUp",true);
                    HideUiObjects();
                    dialogueBox.gameObject.SetActive(true);
                    FindObjectOfType<MusicPlayerManager>().StopAnyAbility();
                    player.rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                    dialogueBox.transform.position = new Vector3(dialogueBox.transform.position.x,
                                dialogueBox.transform.position.y, -1);
                    if(!nextLevelCoroutineStarted)
                    {
                        StartCoroutine(ToTheNextLevelAsync());
                    }
                }
            }
        }else if(SceneManager.GetActiveScene().name == "Level4" && GameObject.FindObjectOfType<HitCounter>().hits >=40)
        {
            isInGameSceneShowing = true;
            //block shooting and moving of enemies exept boss
            EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();
            foreach(EnemyAI e in enemies)
            {
                e.allowedToShootAndMove = false;
                dialogueBox.gameObject.SetActive(true);
                player.rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                dialogueBox.transform.position = new Vector3(dialogueBox.transform.position.x,
                            dialogueBox.transform.position.y, -1);
            }
            if(!nextLevelCoroutineStarted)
            {
                StartCoroutine(ToTheNextLevelAsync());
            }
        }
    }

    public void BackToTheGame()
    {
        if(SceneManager.GetActiveScene().name == "SampleScene")
        {
            boss.npcAnimator.SetBool("drinkEnergyDrink", false);
        }
        StartCoroutine(ReturnToPlayer());
        StartCoroutine(UnfreezeEnemies());   
    }

    public void HideUiObjects()
    {
        playerHealthBar.gameObject.SetActive(false);
        playerStaminaBar.gameObject.SetActive(false);
        musicPlayer.SetActive(false);
        weaponInfo.gameObject.SetActive(false);
        hitCounter.gameObject.SetActive(false);
    }

    public void ShowUiObjects()
    {
        playerHealthBar.gameObject.SetActive(true);
        playerStaminaBar.gameObject.SetActive(true);
        musicPlayer.SetActive(true);
        weaponInfo.gameObject.SetActive(true);
        hitCounter.gameObject.SetActive(true);
    }

    IEnumerator BackToTheGameAync()
    {
        yield return new WaitForSeconds(backToTheGame);
        isSceneShown = true;
        isInGameSceneShowing = false;
        mainCamera.orthographicSize = 7;
        player.rb.constraints = RigidbodyConstraints2D.None;
        dialogueBox.gameObject.SetActive(false);
        ShowUiObjects();
        StartCoroutine(UnfreezeEnemies());
    }

    IEnumerator ReturnToPlayer(){
        yield return new WaitForSeconds(4.0f);
        //return to player
        mainCamera.enabled = true;
        bossCamera.enabled = false;
        isSceneShown = true;
        isInGameSceneShowing = false;
    }

    IEnumerator UnfreezeEnemies()
    {
        yield return new WaitForSeconds(7.0f);
        EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();
        foreach(EnemyAI e in enemies)
        {
            e.allowedToShootAndMove = true;
        }
    }

    IEnumerator ToTheNextLevelAsync()
    {
        nextLevelCoroutineStarted = true;
        yield return new WaitForSeconds(20.0f);
        string nextLevelName = SaveSystem.Instance.GetNextLevelName(SaveSystem.Instance.playerData.CurrentLevelName);
        SaveSystem.Instance.playerData.CurrentLevelName = nextLevelName;
        SaveSystem.Instance.SavePlayer();
        SaveSystem.Instance.SaveToFile();
        isSceneShown = true;
        isInGameSceneShowing = false;
        player.rb.constraints = RigidbodyConstraints2D.None;
        dialogueBox.gameObject.SetActive(false);
        //ShowUiObjects();
        StartCoroutine(UnfreezeEnemies());
        SceneManager.LoadScene(player.nextLevelName);
    }
}
