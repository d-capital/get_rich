using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AbilityDuration : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetDuration(float duration)
    {
        slider.maxValue = duration;
        slider.value = duration;
        fill.color = gradient.Evaluate(1f);
    }

    // Update is called once per frame
    public void UpdateDuration(float newDuration)
    {
        if (slider.value > 0)
        {
            slider.value = newDuration;
        }
        else
        {
            slider.value = 0;
        }
    }
}
