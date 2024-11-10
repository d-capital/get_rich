using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
[System.Serializable]
public class Ability
{
    public Sprite abilitySprite;
    public string abilityName;
    public bool isActive;
    public float duration;
    public int stamina;
    public PostProcessVolume cameraEffect;
    public string cameraEffectName;
}

[System.Serializable]
public class AbilityToSave
{
    public string pathToAbilitySprite;
    public string abilityName;
    public bool isActive;
    public float duration;
    public int stamina;
    public string cameraEffectName;
}
