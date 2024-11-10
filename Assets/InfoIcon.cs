using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoIcon : MonoBehaviour
{
    public Tooltip tooltipText;

    public void ShowOrHideTooltip()
    {
        if(!tooltipText.isActiveAndEnabled)
        {
            tooltipText.ShowTooltip();
        }else
        {
            tooltipText.gameObject.SetActive(false);
        }
    }
}
