using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public TMP_Text actualHealthText;

    public TMP_Text maxHealthText;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
        if(maxHealthText is not null)
        {
            maxHealthText.text = health.ToString();
        }
        if(actualHealthText is not null)
        {
            actualHealthText.text = health.ToString();
        }
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        if(actualHealthText is not null)
        {
            actualHealthText.text = health.ToString();
        }
    }

    public void ResetNameAndHealth(int health, string name)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        if(actualHealthText is not null)
        {
            actualHealthText.text = health.ToString();
        }
    }
}