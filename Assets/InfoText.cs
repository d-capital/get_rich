using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoText : MonoBehaviour
{
    public TMP_Text infoText;
    public string en;
    public string ru;
    // Start is called before the first frame update
    void Start()
    {
        if (Language.Instance.CurrentLanguage == "ru")
        {
            infoText.text = ru;
        }
        else
        {
            infoText.text = en;
        }
        StartCoroutine(hideInfoTextWithDelay());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator hideInfoTextWithDelay()
    {
        yield return new WaitForSeconds(5.0f);
        gameObject.SetActive(false);
    }

    public void ShowInfoText(string text)
    {
        infoText.text = text;
        gameObject.SetActive(true);
        StartCoroutine(hideInfoTextWithDelay());
    }
}
