using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{

    public TMP_Text tooltipText;
    public string en;
    public string ru;

    public void ShowTooltip()
    {
        HideOtherTooltips();
        gameObject.SetActive(true);
        if (Language.Instance.CurrentLanguage == "ru")
        {
            tooltipText.text = ru;
        }
        else
        {
            tooltipText.text = en;
        }
        StartCoroutine(hideTooltipWithDelay());
    }
    IEnumerator hideTooltipWithDelay()
    {
        yield return new WaitForSeconds(5.0f);
        gameObject.SetActive(false);
    }

    private void HideOtherTooltips()
    {
        Tooltip[] tooltips = Resources.FindObjectsOfTypeAll<Tooltip>();
        foreach(Tooltip t in tooltips)
        {
            t.gameObject.SetActive(false);
        }
    }
}
