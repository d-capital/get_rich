using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoHint : MonoBehaviour
{
    
    public TMP_Text infoText;
    // Start is called before the first frame update

    void Start()
    {
        
    }
    public void ShowInfoHint(string text)
    {
        infoText.text = text;
        StartCoroutine(HideInfoTextAsync());

    }

    IEnumerator HideInfoTextAsync()
    {
        yield return new WaitForSeconds(5.0f);
        gameObject.SetActive(false);
    }
}
