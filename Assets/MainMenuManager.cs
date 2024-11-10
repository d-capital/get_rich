using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class MainMenuManager : MonoBehaviour
{
    
    public Button newGameBtn;
    public TMP_Text newGameBtnTxt;
    public string enNewGameBtnTxt;
    public string ruNewGameBtnTxt;
    public Button creditsBtn;
    public TMP_Text creditsBtnTxt;
    public string enCreditsBtnTxt;
    public string ruCreditsBtnTxt;
    public Button exitBtn;
    public TMP_Text exitBtnTxt;
    public string enExitBtnTxt;
    public string ruExitBtnTxt;

    public Button continueBtn;
    public TMP_Text continueBtnTxt;
    public string enContinueBtnBtnTxt;
    public string ruContinueBtnTxt;

    public Button bullethellBtn;
    public TMP_Text bullethellBtnTxt;
    public string enBullethellBtnTxt;
    public string ruBullethellBtnTxt;

    public GameObject Credits;
    // Start is called before the first frame update
    void Start()
    {
        if (Language.Instance.CurrentLanguage == "ru")
        {
            newGameBtnTxt.text = ruNewGameBtnTxt;
            creditsBtnTxt.text = ruCreditsBtnTxt;
            exitBtnTxt.text = ruExitBtnTxt;
            continueBtnTxt.text = ruContinueBtnTxt;
        }
        else
        {
            newGameBtnTxt.text = enNewGameBtnTxt;
            creditsBtnTxt.text = enCreditsBtnTxt;
            exitBtnTxt.text = enExitBtnTxt;
            continueBtnTxt.text = enContinueBtnBtnTxt;
        } 
        SaveSystem.Instance.LoadPlayerData();
        if(SaveSystem.Instance.playerData.CurrentLevelName != "SampleScene" 
        && SaveSystem.Instance.playerData.CurrentLevelName != "")
        {
            continueBtn.gameObject.SetActive(true);
        }
    }

    public void StartNewGame()
    {
        PlayerData pd = new PlayerData();
        SaveSystem.Instance.playerData = pd;
        SaveSystem.Instance.playerData.Health = 500;
        SaveSystem.Instance.playerData.Stamina = 100;
        SaveSystem.Instance.playerData.MaxHealth = 500;
        SaveSystem.Instance.playerData.MaxStamina = 100;
        SaveSystem.Instance.playerData.CurrentLevelName = "SampleScene";
        SaveSystem.Instance.playerData.HasWeapon = true;
        SaveSystem.Instance.playerData.HasKeyLevel = false;
        SaveSystem.Instance.playerData.WeaponType = "gun";
        SaveSystem.Instance.playerData.Language = Language.Instance.CurrentLanguage;
        SaveSystem.Instance.playerData.Tracks = new List<TrackToSave>{
            new TrackToSave{
                artistName = "Skilsel",
                trackName = "energetic-hip-hop",
                abilities = new List<AbilityToSave>{
                    new AbilityToSave
                    {
                        pathToAbilitySprite = "Ability/speed_up",
                        abilityName = "Speed Up",
                        isActive = false,
                        duration = 2,
                        stamina = 15,
                        cameraEffectName = ""
                    }
                }
            }
        };
        SaveSystem.Instance.playerData.weaponInfoSources = new List<WeaponInfoSourceToSave>{
            new WeaponInfoSourceToSave{
                weaponType = "gun",
                pathToWeaponInGameSprite = "Weapons/gun-in-scene-weapon",
                pathToWeaponCanvaseSprite = "Weapons/gun-weapon-info",
                bulletPrefabName = "Bullets/PlayerBullet Gun.prefab",
                weaponDamage = 10,
                weaponTimeTillNextShot = 2,
                maxLoadQuantity = 30,
                ammoQuantityInCurrentLoad = 30,
                currentLoadsQuantity = 3,
                isRifle = false,
                shotSoundName = "gunShotSound",
                reloadSoundName = "gunRealoadSound",
                nothingToReloadSoundName = "nothingToReloadSound",
                nothingToShootSoundName = "nothingToShootSound"
            },
            new WeaponInfoSourceToSave{
                weaponType = "shot-gun",
                pathToWeaponInGameSprite = "Weapons/shot-gun-in-scene-weapon",
                pathToWeaponCanvaseSprite = "Weapons/shot-gun-weapon-info",
                bulletPrefabName = "Bullets/PlayerBullet ShotGun.prefab",
                weaponDamage = 20,
                weaponTimeTillNextShot = 5,
                maxLoadQuantity = 1,
                ammoQuantityInCurrentLoad = 1,
                currentLoadsQuantity = 20,
                isRifle = true,
                shotSoundName = "shotGunShotSound",
                reloadSoundName = "shotGunRealoadSound",
                nothingToReloadSoundName = "nothingToReloadSound",
                nothingToShootSoundName = "nothingToShootSound"
            },
            new WeaponInfoSourceToSave{
                weaponType = "rifle",
                pathToWeaponInGameSprite = "Weapons/rifle-in-scene-weapon",
                pathToWeaponCanvaseSprite = "Weapons/rifle-weapon-info",
                bulletPrefabName = "Bullets/PlayerBullet Rifle.prefab",
                weaponDamage = 15,
                weaponTimeTillNextShot = 1,
                maxLoadQuantity = 50,
                ammoQuantityInCurrentLoad = 50,
                currentLoadsQuantity = 5,
                isRifle = true,
                shotSoundName = "rifleShotSound",
                reloadSoundName = "gunRealoadSound",
                nothingToReloadSoundName = "nothingToRealoadSound",
                nothingToShootSoundName = "nothingToShootSound"
            }
        };
        SaveSystem.Instance.SaveToFile();
        SceneManager.LoadScene("CutScene0");
    }

    public void Continue()
    {
        SaveSystem.Instance.LoadPlayerData();
        SceneManager.LoadScene(SaveSystem.Instance.playerData.CurrentLevelName);
    }

    public void ShowCredits()
    {
        Credits.SetActive(true);
    }

    public void BulletHell()
    {
        PlayerData pd = new PlayerData();
        SaveSystem.Instance.playerData = pd;
        SaveSystem.Instance.playerData.Health = 500;
        SaveSystem.Instance.playerData.Stamina = 100;
        SaveSystem.Instance.playerData.MaxHealth = 500;
        SaveSystem.Instance.playerData.MaxStamina = 100;
        SaveSystem.Instance.playerData.CurrentLevelName = "SampleScene";
        SaveSystem.Instance.playerData.HasWeapon = true;
        SaveSystem.Instance.playerData.HasKeyLevel = false;
        SaveSystem.Instance.playerData.WeaponType = "gun";
        SaveSystem.Instance.playerData.Language = Language.Instance.CurrentLanguage;
        SaveSystem.Instance.playerData.Tracks = new List<TrackToSave>{
            new TrackToSave{
                artistName = "Skilsel",
                trackName = "energetic-hip-hop",
                abilities = new List<AbilityToSave>{
                    new AbilityToSave
                    {
                        pathToAbilitySprite = "Ability/speed_up",
                        abilityName = "Speed Up",
                        isActive = false,
                        duration = 2,
                        stamina = 15,
                        cameraEffectName = ""
                    }
                }
            }
        };
        SaveSystem.Instance.playerData.weaponInfoSources = new List<WeaponInfoSourceToSave>{
            new WeaponInfoSourceToSave{
                weaponType = "gun",
                pathToWeaponInGameSprite = "Weapons/gun-in-scene-weapon",
                pathToWeaponCanvaseSprite = "Weapons/gun-weapon-info",
                bulletPrefabName = "Bullets/PlayerBullet Gun.prefab",
                weaponDamage = 10,
                weaponTimeTillNextShot = 2,
                maxLoadQuantity = 30,
                ammoQuantityInCurrentLoad = 30,
                currentLoadsQuantity = 1000,
                isRifle = false,
                shotSoundName = "gunShotSound",
                reloadSoundName = "gunRealoadSound",
                nothingToReloadSoundName = "nothingToReloadSound",
                nothingToShootSoundName = "nothingToShootSound"
            },
            new WeaponInfoSourceToSave{
                weaponType = "shot-gun",
                pathToWeaponInGameSprite = "Weapons/shot-gun-in-scene-weapon",
                pathToWeaponCanvaseSprite = "Weapons/shot-gun-weapon-info",
                bulletPrefabName = "Bullets/PlayerBullet ShotGun.prefab",
                weaponDamage = 20,
                weaponTimeTillNextShot = 5,
                maxLoadQuantity = 1,
                ammoQuantityInCurrentLoad = 1,
                currentLoadsQuantity = 20,
                isRifle = true,
                shotSoundName = "shotGunShotSound",
                reloadSoundName = "shotGunRealoadSound",
                nothingToReloadSoundName = "nothingToReloadSound",
                nothingToShootSoundName = "nothingToShootSound"
            },
            new WeaponInfoSourceToSave{
                weaponType = "rifle",
                pathToWeaponInGameSprite = "Weapons/rifle-in-scene-weapon",
                pathToWeaponCanvaseSprite = "Weapons/rifle-weapon-info",
                bulletPrefabName = "Bullets/PlayerBullet Rifle.prefab",
                weaponDamage = 15,
                weaponTimeTillNextShot = 1,
                maxLoadQuantity = 50,
                ammoQuantityInCurrentLoad = 50,
                currentLoadsQuantity = 5,
                isRifle = true,
                shotSoundName = "rifleShotSound",
                reloadSoundName = "gunRealoadSound",
                nothingToReloadSoundName = "nothingToRealoadSound",
                nothingToShootSoundName = "nothingToShootSound"
            }
        };
        SceneManager.LoadScene("Bullethell");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
