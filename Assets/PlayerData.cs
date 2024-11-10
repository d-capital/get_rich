using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int Kills;
    public int KillsTotal;
    public int Health;
    public int Stamina;
    public string CurrentLevelName;
    public bool HasWeapon;
    public bool HasKeyLevel;
    public int MaxHealth;
    public int MaxStamina;
    public string WeaponType;
    public string Language;
    public List<TrackToSave> Tracks;
    public List<WeaponInfoSourceToSave> weaponInfoSources;
}