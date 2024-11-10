using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using Newtonsoft.Json;
using UnityEditor;
using Unity.VisualScripting;


public class SaveSystem : MonoBehaviour
{
    public PlayerData playerData;

    public static SaveSystem Instance;

    PostProcessVolume spawnedPpv;

    public Dictionary<int, string> levelDictinonary = new Dictionary<int, string>(){
        {0,"SampleScene"},
        {1,"Level2"},
        {2,"Level3"},
        {3,"Level4"},
        {4,"Level5"},
        {5,"Level6"},
        {6,"Level7"}
    };

    private void Awake()
    {
        if (Instance == null)
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayer()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        playerData.Health = player.health;
        playerData.Stamina = player.stamina;
        int nextSceneIndex = levelDictinonary.FirstOrDefault(x=>x.Value == SceneManager.GetActiveScene().name).Key+1;
        if(levelDictinonary.Count() < nextSceneIndex)
        {
            string nextSceneName = levelDictinonary[nextSceneIndex];       
            playerData.CurrentLevelName = nextSceneName;
        }
        HitCounter hitCounter = Resources.FindObjectsOfTypeAll<HitCounter>()[0];
        playerData.Kills = hitCounter.hits;
        playerData.Tracks = ConvertTracksToTracksToSave(FindObjectOfType<MusicPlayerManager>().Tracks);
        playerData.weaponInfoSources = ConvertWisToWisToSave(player.weaponInfoSources);
        playerData.WeaponType = player.weaponInfoSources[player.currentWeaponIndex].weaponType;
    }

    public List<TrackToSave> ConvertTracksToTracksToSave(List<Track> tracks)
    {
        List<TrackToSave> tracksToSave = new List<TrackToSave>();

        foreach(Track t in tracks)
        {
            if(t.artistName != "Skilsel")
            {
                TrackToSave newTrackToSave = new TrackToSave{
                    artistName = t.artistName,
                    trackName = t.trackName,
                    abilities = new List<AbilityToSave>
                    {
                        new AbilityToSave
                        {
                            pathToAbilitySprite = "Ability/" + t.abilities[0].abilitySprite.name,
                            abilityName = t.abilities[0].abilityName,
                            isActive = t.abilities[0].isActive,
                            duration = t.abilities[0].duration,
                            stamina = t.abilities[0].stamina,
                            cameraEffectName =  "PostProcessVolumes/" + t.abilities[0].cameraEffectName,
                        }
                    }
                };
                tracksToSave.Add(newTrackToSave);
            }
        }
        return tracksToSave;
    }

    public List<WeaponInfoSourceToSave> ConvertWisToWisToSave(List<WeaponInfoSource> wisl)
    {
        List<WeaponInfoSourceToSave> wisToSave= new List<WeaponInfoSourceToSave>();
        foreach(WeaponInfoSource wis in wisl)
        {
            string sourceString = wis.waeponPrefab.name;
            string removeString = " (PlayerBullet)";
            int index = sourceString.IndexOf(removeString);
            string prefabName = (index < 0) ? sourceString : sourceString.Remove(index, removeString.Length);
            WeaponInfoSourceToSave newWisToSave = new WeaponInfoSourceToSave
            {
                weaponType = wis.weaponType,
                pathToWeaponInGameSprite = "Weapons/" + wis.weaponInGameSprite.name,
                pathToWeaponCanvaseSprite = "Weapons/" + wis.weaponCanvaseSprite.name,
                bulletPrefabName = "Bullets/" + prefabName,
                weaponDamage = wis.weaponDamage,
                weaponTimeTillNextShot = wis.weaponTimeTillNextShot,
                maxLoadQuantity = wis.maxLoadQuantity,
                ammoQuantityInCurrentLoad = wis.ammoQuantityInCurrentLoad,
                currentLoadsQuantity = wis.currentLoadsQuantity,
                isRifle = wis.isRifle,
                shotSoundName = wis.shotSoundName,
                reloadSoundName = wis.reloadSoundName,
                nothingToReloadSoundName = wis.nothingToReloadSoundName,
                nothingToShootSoundName = wis.nothingToShootSoundName
            };
            wisToSave.Add(newWisToSave);
        }
        return wisToSave;
    }

    public void SaveToFile()
    {
        string path = Application.persistentDataPath + "/player.fun";
        PlayerData data = playerData;
        string json = JsonConvert.SerializeObject(data);
        File.WriteAllText(path, json);
    }

    public void LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/player.fun";
        Debug.Log(path);
        if(File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.OpenOrCreate, 
                                                        FileAccess.ReadWrite, 
                                                        FileShare.ReadWrite);
            stream.Dispose();
            var fileContents = File.ReadAllText(path);
            PlayerData pd = JsonConvert.DeserializeObject<PlayerData>(fileContents);
            playerData = pd;
            stream.Dispose();
            if(SceneManager.GetActiveScene().name != "MainMenu") 
            {
                PlayerController player = FindObjectOfType<PlayerController>();
                player.stamina = playerData.Stamina;
                player.health = playerData.Health;
                if(playerData.Tracks.Count > 0)
                {
                    foreach (TrackToSave track in playerData.Tracks)
                    {
                        RestoreTrack(track);
                    }
                }
                if(playerData.weaponInfoSources.Count>0)
                {
                    foreach (WeaponInfoSourceToSave wis in playerData.weaponInfoSources)
                    {
                        RestorWeapon(wis);
                    }
                }
            }
        }
        else
        {
            Debug.Log("Save file not found in " + path);
        }
    }

    public void RestorWeapon(WeaponInfoSourceToSave wis)
    {
        List<WeaponInfoSource> playerWeaponInfoSource = FindObjectOfType<PlayerController>().weaponInfoSources;
        bool hasPlayerSuchWeaponAlready = playerWeaponInfoSource.Where(x=>x.weaponType == wis.weaponType).ToList().Count()>0;
        List<AudioSource> soundEffects = GameObject.Find("AudioSources").GetComponentsInChildren<AudioSource>().ToList();
        if(!hasPlayerSuchWeaponAlready){
            WeaponInfoSource newWis = new WeaponInfoSource
            {
                weaponType = wis.weaponType,
                weaponInGameSprite = Resources.Load<Sprite>(wis.pathToWeaponInGameSprite),
                weaponCanvaseSprite = Resources.Load<Sprite>(wis.pathToWeaponCanvaseSprite),
                waeponPrefab = Resources.Load<PlayerBullet>(wis.bulletPrefabName),
                weaponDamage = wis.weaponDamage,
                weaponTimeTillNextShot = wis.weaponTimeTillNextShot,
                maxLoadQuantity = wis.maxLoadQuantity,
                ammoQuantityInCurrentLoad = wis.ammoQuantityInCurrentLoad,
                currentLoadsQuantity = wis.currentLoadsQuantity,
                isRifle = wis.isRifle,
                shotSoundName = wis.shotSoundName,
                shotSound = soundEffects.Where(x => x.name == wis.shotSoundName).First(),
                reloadSoundName = wis.reloadSoundName,
                reloadSound =  soundEffects.Where(x => x.name == wis.reloadSoundName).First(),
                nothingToReloadSoundName = wis.nothingToReloadSoundName,
                nothingToReloadSound = soundEffects.Where(x => x.name == wis.nothingToReloadSoundName).First(),
                nothingToShootSoundName = wis.nothingToShootSoundName,
                nothingToShootSound = soundEffects.Where(x => x.name == wis.nothingToShootSoundName).First()
            };
            FindObjectOfType<PlayerController>().weaponInfoSources.Add(newWis);
        }
    }
    public void RestoreTrack(TrackToSave track)
    {
        MusicPlayerManager musicPlayerManager = FindObjectOfType<MusicPlayerManager>();
        if(track.artistName != "Skilsel")
        {
            string artistAndTrackName = track.artistName + " - " + track.trackName;
            AudioSource fielForTrack = Resources.LoadAll<AudioSource>("Tracks").FirstOrDefault(x => x.gameObject.name == artistAndTrackName);
            AudioSource spawnedAudioFile = Instantiate(fielForTrack, new Vector3(0, 0, 0), Quaternion.identity, musicPlayerManager.transform);
            if (track.abilities[0].cameraEffectName != "PostProcessVolumes/"
                && track.abilities[0].cameraEffectName != ""
                && track.abilities[0].cameraEffectName != null)
            {
                PostProcessVolume ppv = Resources.LoadAll<PostProcessVolume>("PostProcessVolumes").FirstOrDefault(
                    x => x.gameObject.name == track.abilities[0].cameraEffectName.Split("/")[1]);
                spawnedPpv = Instantiate(ppv, new Vector3(0, 0, 0), Quaternion.identity);
            }
            AbilityToSave ability = track.abilities[0];
            Track trackToAdd = new Track
            {
                audioFile = spawnedAudioFile,
                artistName = track.artistName,
                trackName = track.trackName,
                abilities = new List<Ability>
                {
                    new Ability
                    {
                        abilityName = ability.abilityName,
                        abilitySprite = Resources.Load<Sprite>(ability.pathToAbilitySprite),
                        cameraEffect = spawnedPpv,
                        duration = ability.duration,
                        stamina = ability.stamina
                    }
                }
            };
            musicPlayerManager.Tracks.Add(trackToAdd);
        }
    }
    public string GetNextLevelName(string currentLevelName)
    {
        int currentLevelIndex = levelDictinonary.First( x => x.Value == currentLevelName).Key;
        int nextLevelIndex = currentLevelIndex + 1;
        if(nextLevelIndex > levelDictinonary.Count)
        {
            return "MainMenu";
        }
        else
        {
            string nextLevelName = levelDictinonary[nextLevelIndex];
            return nextLevelName;
        }
    }
}