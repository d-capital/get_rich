using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class WeaponInfoSource
{
    public string weaponType;
    public Sprite weaponInGameSprite;
    public Sprite weaponCanvaseSprite;
    public PlayerBullet waeponPrefab;
    public int weaponDamage;
    public int weaponTimeTillNextShot;
    public int maxLoadQuantity; //how much bullets can there be in one load 90/[100] 1
    public int ammoQuantityInCurrentLoad; //how much bullets are there in current load [90]/100 1
    public int currentLoadsQuantity; //how much load are there now for that weapon 90/100 [1]
    public bool isRifle;
    public string shotSoundName;
    public AudioSource shotSound;
    public string reloadSoundName;
    public AudioSource reloadSound;
    public string nothingToReloadSoundName;
    public AudioSource nothingToReloadSound;
    public string nothingToShootSoundName;
    public AudioSource nothingToShootSound;
}

[System.Serializable]
public class WeaponInfoSourceToSave
{
    public string weaponType;
    public string pathToWeaponInGameSprite;
    public string pathToWeaponCanvaseSprite;
    public string bulletPrefabName;
    public int weaponDamage;
    public int weaponTimeTillNextShot;
    public int maxLoadQuantity; //how much bullets can there be in one load 90/[100] 1
    public int ammoQuantityInCurrentLoad; //how much bullets are there in current load [90]/100 1
    public int currentLoadsQuantity; //how much load are there now for that weapon 90/100 [1]
    public bool isRifle;
    public string shotSoundName;
    public string reloadSoundName;
    public string nothingToReloadSoundName;
    public string nothingToShootSoundName;
}

public class PlayerController : MonoBehaviour
{
    private Vector3 aimDirection;
    private float LastHorizontal;
    private float LastVertical;

    public float moveSpeed = 7f;
    public Rigidbody2D rb;
    public int health = 100;
    public int stamina = 50;

    [SerializeField] public List<WeaponInfoSource> weaponInfoSources;

    public Weapon weapon;
    public float fireRate = 5;
    public float nextFire;
    public float coolDownRate;
    public bool hasTarget;

    private Vector2 moveDirection;
    private bool isDashButtonDown;

    public Vector3 mousePos;
    public Camera mainCamera;
    public Vector3 mousePosWorld;
    public Vector2 mousePosWorld2D;
    public Vector3 slideDirection;

    public HealthBar HealthBar;
    public StaminaBar StaminaBar;

    public bool isQteActive = false;

    public Animator animator;

    public Texture2D cursorTextureNoWeapon;
    public Texture2D cursorTextureWithWeapon;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    public Vector2 hotSpotCenteredWithWeapon;
    public Vector2 hotSpotCenteredNoWeapon;

    public string jsonString;

    public AudioSource footStepsSound;

    public bool isUiHidden = false;

    public bool isMobileBrowser;

    public int currentWeaponIndex;

    [SerializeField]
    private Transform dashEffect;

    public Ability activeAbility;

    public float currentAbilityDuration;

    public AbilityDuration abilityDuration;

    public float nextStaminaUpdateTime;
    public float staminaRestorationInterval;
    public int ammoQuantityInCurrentLoad; // how much bullet player has in current load. [90]/100 1
    public int currentWeaponMaxLoadQuantity; // how much bullets can there be in one load for this weapon. 90/[100] 1
    public int currentWeaponLoadsQuantity; // how much load of bullets current weapon has. 90/100 [1]
    public TMP_Text ammoInLoad;
    public TMP_Text maxAmmoInLoad;
    public TMP_Text numberOfLoads;
    public SpriteRenderer damageAffectedSprite;
    public bool isUiOpen;

    public string nextLevelName;

    public ScriptedSceneManager scriptedSceneManager;

    public Grenade grenade;
    public Grenade shuriken;
    public float throwForce;
    public string throwType;
    // Start is called before the first frame update
    void Start()
    {
        InitializePlayer();
    }

    public void InitializePlayer()
    {
        bool hasPlayerWeapon = SaveSystem.Instance.playerData.HasWeapon;
        health = SaveSystem.Instance.playerData.Health;
        stamina = SaveSystem.Instance.playerData.Stamina;
        hasPlayerWeapon = SaveSystem.Instance.playerData.HasWeapon;
        hasPlayerWeapon = true;
        HealthBar.SetMaxHealth(SaveSystem.Instance.playerData.MaxHealth);
        HealthBar.SetHealth(health);
        StaminaBar.SetMaxStamina(SaveSystem.Instance.playerData.MaxStamina);
        StaminaBar.SetStamina(SaveSystem.Instance.playerData.Stamina);
        if (hasPlayerWeapon)
        {
            hotSpotCenteredWithWeapon = new Vector2(cursorTextureWithWeapon.width / 2, cursorTextureWithWeapon.height / 2);
            Cursor.SetCursor(cursorTextureWithWeapon, hotSpotCenteredWithWeapon, cursorMode);
            Weapon[] weapons = Resources.FindObjectsOfTypeAll<Weapon>();
            foreach (Weapon i in weapons)
            {
                if (i.gameObject.name == "playerWeapon")
                {
                    i.gameObject.SetActive(true);
                    animator.Play("idle with weapon");
                    animator.SetBool("hasWeapon", true);
                    animator.SetBool("isMoving", false);
                }
            }
            WeaponInfo[] weaponInfos = Resources.FindObjectsOfTypeAll<WeaponInfo>();
            foreach (WeaponInfo wi in weaponInfos)
            {
                wi.gameObject.SetActive(true);
            }
            //weapon
            string weaponType = SaveSystem.Instance.playerData.WeaponType;
            weaponType = "gun";
            SetWeapon(weaponType);

        }
        else
        {
            hotSpotCenteredNoWeapon = new Vector2(cursorTextureNoWeapon.width / 2, cursorTextureNoWeapon.height / 2);
            Cursor.SetCursor(cursorTextureNoWeapon, hotSpotCenteredNoWeapon, cursorMode);
            ShowGoalText();
        }
    }

    // Update is called once per frame
    void Update()
    {
        HideUI();
        if (GameObject.Find("PauseMenuManager").GetComponent<PauseMenu>().isPauseMenuOpen == false 
            && !scriptedSceneManager.isInGameSceneShowing)
        {
            HandleMovement();
            HandleFire();
            HandleAbilityStart();
            HandleAbilityStop();
            HandleWeaponChange();
            HandleStaminaRestoration();
            HandleReload();
            rb.angularVelocity = 0;
        }
        else
        {
            float moveX = 0;
            float moveY = 0;
            moveDirection = new Vector2(moveX, moveY).normalized;
            animator.SetBool("isMoving", false);
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

    private void FixedUpdate()
    {
        if (GameObject.Find("PauseMenuManager").GetComponent<PauseMenu>().isPauseMenuOpen == false 
            && !scriptedSceneManager.isInGameSceneShowing)
        {
            mousePos = Input.mousePosition;
            mousePosWorld = mainCamera.ScreenToWorldPoint(mousePos);
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
            aimDirection = mousePosWorld - transform.position;
            float aimAngel = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = aimAngel;
            if (!isMoving())
            {
                animator.SetBool("isMoving", false);
                //footStepsSound.Stop();
            }
            if (isDashButtonDown && isMoving())
            {
                float dashAmount = 4f;
                Vector3 beforeDashPosition = transform.position;
                Transform dashEffectTransform = Instantiate(dashEffect, beforeDashPosition, Quaternion.identity);
                float dashAngel = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg + 180f;
                dashEffectTransform.eulerAngles = new Vector3(0,0, dashAngel);
                rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + moveDirection * dashAmount);
                isDashButtonDown = false;
            }
        }

    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;
        if (isMoving())
        {
            animator.SetBool("isMoving", true);
            //footStepsSound.Play();
        }

    }

    public void HandleMobileUiDash()
    {
        isDashButtonDown = true;
    }
    public void HandleMobileUiFire()
    {
        if (gameObject.GetComponentInChildren<Weapon>() != null)
        {
            Fire();
        }
    }

    void HandleStaminaRestoration()
    {
        if(stamina != SaveSystem.Instance.playerData.MaxStamina && Time.time >= nextStaminaUpdateTime)
        {
            stamina += 5;
            StaminaBar.SetStamina(stamina);
            SaveSystem.Instance.playerData.Stamina = stamina;
            nextStaminaUpdateTime = Time.time + staminaRestorationInterval;
        }

    }

    void ReduceStamina()
    {
        stamina -= activeAbility.stamina;
        StaminaBar.SetStamina(stamina);
        SaveSystem.Instance.playerData.Stamina = stamina;
    }

    private void HandleAbilityStart()
    {
        if(SceneManager.GetActiveScene().name != "Level3")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(activeAbility.abilityName == "Dash")
                {
                    if (stamina > activeAbility.stamina) 
                    {
                        isDashButtonDown = true;
                        ReduceStamina();
                    }
                }
                if(activeAbility.abilityName == "Grenade")
                {
                    if (stamina > activeAbility.stamina) 
                    {
                        throwType = "Grenade";
                        animator.SetTrigger("throw");
                        ReduceStamina();
                    }
                }
                if(activeAbility.abilityName == "Shuriken")
                {
                    if (stamina > activeAbility.stamina) 
                    {
                        throwType = "Shuriken";
                        animator.SetTrigger("throw");
                        ReduceStamina();
                    }
                }
                if(activeAbility.abilityName == "Bullet Time")
                {
                    if (!activeAbility.isActive && stamina > activeAbility.stamina)
                    {
                        currentAbilityDuration = activeAbility.duration;
                        abilityDuration.SetDuration(activeAbility.duration);
                        activeAbility.isActive = true;
                        Time.timeScale = .5f;
                        activeAbility.cameraEffect.weight = 1f;
                        ReduceStamina();
                    }
                    else if (activeAbility.isActive)
                    {
                        activeAbility.isActive = false;
                        Time.timeScale = 1f;
                        activeAbility.cameraEffect.weight = 0f;
                    }
                }
                if(activeAbility.abilityName == "3-Shot")
                {
                    if (!activeAbility.isActive && stamina>activeAbility.stamina)
                    {
                        currentAbilityDuration = activeAbility.duration;
                        abilityDuration.SetDuration(activeAbility.duration);
                        activeAbility.isActive = true;
                        ReduceStamina();
                    }
                    else if (activeAbility.isActive)
                    {
                        activeAbility.isActive = false;
                    }
                }
                if(activeAbility.abilityName == "Healing")
                {
                    if (!activeAbility.isActive && stamina > activeAbility.stamina)
                    {
                        currentAbilityDuration = activeAbility.duration;
                        abilityDuration.SetDuration(activeAbility.duration);
                        activeAbility.isActive = true;
                        ReduceStamina();
                        StartCoroutine(HealAsync());
                    }
                    else if (activeAbility.isActive)
                    {
                        activeAbility.isActive = false;
                    }
                }
                if(activeAbility.abilityName == "Speed Up")
                {
                    if (!activeAbility.isActive && stamina > activeAbility.stamina)
                    {
                        currentAbilityDuration = activeAbility.duration;
                        abilityDuration.SetDuration(activeAbility.duration);
                        activeAbility.isActive = true;
                        moveSpeed = 15;
                        ReduceStamina();
                    }
                    else if (activeAbility.isActive)
                    {
                        activeAbility.isActive = false;
                        moveSpeed = 7;
                    }
                }
            }
            if(activeAbility.isActive)
            {
                float newDuration = currentAbilityDuration - 0.01f;
                if(newDuration > 0)
                {
                    currentAbilityDuration -= 0.01f;
                    abilityDuration.UpdateDuration(currentAbilityDuration);
                }else
                {
                    currentAbilityDuration = 0;
                    abilityDuration.UpdateDuration(currentAbilityDuration);
                    StopAnyAbility();
                }
                
            }
        }
    }

    private void HideUI()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (!isUiHidden)
            {
                isUiHidden = true;
                GameObject.Find("Canvas").gameObject.SetActive(false);
            }
            else
            {
                MainCanvasManager[] Canvas = Resources.FindObjectsOfTypeAll<MainCanvasManager>();
                foreach (MainCanvasManager mcm in Canvas)
                {
                    mcm.gameObject.SetActive(true);
                }
                isUiHidden = false;
            }
        }
    }

    private void HandleFire()
    {
        if (gameObject.GetComponentInChildren<Weapon>() != null && !isUiOpen)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Fire();
            }
            else if (hasTarget)
            {
                updateFireRate();
                hasTarget = false;
            }
        }
    }

    private void HandleReload()
    {
        if (gameObject.GetComponentInChildren<Weapon>() != null)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
        }
    }

    private void HandleAbilityStop()
    {
        if(activeAbility.isActive && currentAbilityDuration <= 0)
        {
            StopAnyAbility();
        }
    }
    void UpdateAbilityDuration()
    {
        if (currentAbilityDuration > 0)
        {
            currentAbilityDuration -= Time.deltaTime;
            abilityDuration.UpdateDuration(currentAbilityDuration);
        }
    }

    private void HandleWeaponChange()
    {
        int newIndex = currentWeaponIndex;
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            weaponInfoSources[currentWeaponIndex].ammoQuantityInCurrentLoad = ammoQuantityInCurrentLoad;
            weaponInfoSources[currentWeaponIndex].currentLoadsQuantity = currentWeaponLoadsQuantity;
            //index+1
            newIndex += 1;
            try
            {
                string weaponType = weaponInfoSources[newIndex].weaponType;
                SaveSystem.Instance.playerData.WeaponType = weaponType;
                bool isRifle = weaponInfoSources[newIndex].isRifle;
                if (isRifle)
                {
                    animator.SetBool("hasRifle", true);
                }
                else
                {
                    animator.SetBool("hasRifle", false);
                }
                SetWeapon(weaponType);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            weaponInfoSources[currentWeaponIndex].ammoQuantityInCurrentLoad = ammoQuantityInCurrentLoad;
            weaponInfoSources[currentWeaponIndex].currentLoadsQuantity = currentWeaponLoadsQuantity;
            //index-1
            newIndex -= 1;
            try
            {
                string weaponType = weaponInfoSources[newIndex].weaponType;
                SaveSystem.Instance.playerData.WeaponType = weaponType;
                SetWeapon(weaponType);
                bool isRifle = weaponInfoSources[newIndex].isRifle;
                if (isRifle)
                {
                    animator.SetBool("hasRifle", true);
                }
                else
                {
                    animator.SetBool("hasRifle", false);
                }
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }

        }

    }


    Transform GetClosestEnemy(Transform[] enemies)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Transform t in enemies)
        {
            float dist = Vector3.Distance(t.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadDistinctLevel(PlayerData playerData, int lvlIndex)
    {
        SceneManager.LoadScene(lvlIndex);
    }

    public void Fire()
    {
        if (activeAbility.abilityName == "3-Shot" && activeAbility.isActive && ammoQuantityInCurrentLoad>=3)
        {
            ammoQuantityInCurrentLoad -= 3;
            ammoInLoad.text = ammoQuantityInCurrentLoad.ToString();
            weapon.Player3Shoot();
            weaponInfoSources[currentWeaponIndex].shotSound.Play();
        }
        else if(ammoQuantityInCurrentLoad > 0)
        {
            ammoQuantityInCurrentLoad -= 1;
            ammoInLoad.text = ammoQuantityInCurrentLoad.ToString();
            weapon.PlayerFire();
            weaponInfoSources[currentWeaponIndex].shotSound.Play();
        }
        else
        {
            weaponInfoSources[currentWeaponIndex].nothingToShootSound.Play();
        }
    }

    public void updateFireRate()
    {
        if (Time.time >= nextFire)
        {
            Fire();
            nextFire = Time.time + fireRate;
        }
    }

    bool isMoving()
    {
        return Input.GetKey(KeyCode.W)
            || Input.GetKey(KeyCode.A)
            || Input.GetKey(KeyCode.S)
            || Input.GetKey(KeyCode.D)
            || Input.GetKey(KeyCode.UpArrow)
            || Input.GetKey(KeyCode.DownArrow)
            || Input.GetKey(KeyCode.LeftArrow)
            || Input.GetKey(KeyCode.RightArrow);
    }

    public void ChangeToWeaponAim()
    {
        hotSpotCenteredWithWeapon = new Vector2(cursorTextureWithWeapon.width / 2, cursorTextureWithWeapon.height / 2);
        Cursor.SetCursor(cursorTextureWithWeapon, hotSpotCenteredWithWeapon, cursorMode);
    }

    public void ShowGameOver()
    {

        WastedOverlay[] gameOverScreens = Resources.FindObjectsOfTypeAll<WastedOverlay>();
        foreach (WastedOverlay screen in gameOverScreens)
        {
            screen.GetComponent<WastedOverlay>().ShowWastedScreen();
        }

    }

    public void ShowGoalText()
    {
        InfoText[] bfInfoTexts = Resources.FindObjectsOfTypeAll<InfoText>();
        foreach (InfoText i in bfInfoTexts)
        {
            if (i.gameObject.name == "bfInfoText")
            {
                i.gameObject.SetActive(true);
            }
        }
    }

    public void SetWeapon(string weaponType)
    {
        WeaponInfo weaponInfo = GameObject.FindObjectOfType<WeaponInfo>();
        WeaponInfoSource weaponInfoSource = weaponInfoSources.Where(wifs => wifs.weaponType == weaponType).First();
        weapon.GetComponent<SpriteRenderer>().sprite = weaponInfoSource.weaponInGameSprite;
        weaponInfo.GetComponent<Image>().sprite = weaponInfoSource.weaponCanvaseSprite;
        currentWeaponIndex = weaponInfoSources.FindIndex(wifs => wifs.weaponType == weaponType);
        currentWeaponLoadsQuantity = weaponInfoSource.currentLoadsQuantity;
        currentWeaponMaxLoadQuantity = weaponInfoSource.maxLoadQuantity;
        ammoQuantityInCurrentLoad = weaponInfoSource.ammoQuantityInCurrentLoad;
        ammoInLoad.text = ammoQuantityInCurrentLoad.ToString();
        maxAmmoInLoad.text = currentWeaponMaxLoadQuantity.ToString();
        numberOfLoads.text = currentWeaponLoadsQuantity.ToString();
    }

    public void Reload()
    {
        if(currentWeaponLoadsQuantity>0)
        {
            currentWeaponLoadsQuantity -= 1;
            ammoQuantityInCurrentLoad = currentWeaponMaxLoadQuantity;
            ammoInLoad.text = ammoQuantityInCurrentLoad.ToString();
            numberOfLoads.text = currentWeaponLoadsQuantity.ToString();
            weaponInfoSources[currentWeaponIndex].reloadSound.Play();
        }   
        else
        {
            weaponInfoSources[currentWeaponIndex].nothingToReloadSound.Play();
        }
    }

    public void StopAnyAbility()
    {
        if (activeAbility.cameraEffect)
        {
            activeAbility.cameraEffect.weight = 0;
        }
        moveSpeed = 7;
        activeAbility.isActive = false;
        Time.timeScale = 1f;
        StopCoroutine(HealAsync());
    }
    IEnumerator HealAsync()
    {
        yield return new WaitForSeconds(2.0f);
        int newHealth = health + 10;
        if(newHealth < SaveSystem.Instance.playerData.MaxHealth)
        {
            health +=10;
            HealthBar.SetHealth(health);
            FloatingNumber floatingNumber = Resources.LoadAll<FloatingNumber>("FloatingNumber").FirstOrDefault();
            FloatingNumber fn = Instantiate(floatingNumber, transform.position, Quaternion.identity, transform);
            fn.ShowNumber(10);
        }
        else
        {
            health = SaveSystem.Instance.playerData.MaxHealth;
            HealthBar.SetHealth(health);
            FloatingNumber floatingNumber = Resources.LoadAll<FloatingNumber>("FloatingNumber").FirstOrDefault();
            FloatingNumber fn = Instantiate(floatingNumber, transform.position, Quaternion.identity, transform);
            fn.ShowNumber(10);
        }
    }

    public void ThrowGrenadeOrShuriken()
    {
        if(throwType == "Grenade")
        {
            Grenade bullet = Instantiate(grenade, weapon.firePoint3.position, weapon.firePoint3.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(weapon.firePoint3.up * throwForce, ForceMode2D.Impulse);
        }
        else if(throwType == "Shuriken")
        {
            Grenade bullet = Instantiate(shuriken, weapon.firePoint3.position, weapon.firePoint3.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(weapon.firePoint3.up * throwForce, ForceMode2D.Impulse);
        }
    }

}
