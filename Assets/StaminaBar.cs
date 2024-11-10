using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public TMP_Text actualStaminaText;
    public TMP_Text maxStaminaText;
    
    public void SetMaxStamina(float stamina)
    {
        slider.maxValue = stamina;
        slider.value = stamina;
        fill.color = gradient.Evaluate(1f);
        if(actualStaminaText is not null)
        {
            actualStaminaText.text = stamina.ToString();
        }
        if(maxStaminaText is not null)
        {
            maxStaminaText.text = stamina.ToString();
        }
    }

    public void SetStamina(int stamina)
    {
        slider.value = stamina;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        if(actualStaminaText is not null)
        {
            actualStaminaText.text = stamina.ToString();
        }
    }
}
