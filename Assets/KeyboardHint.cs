using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyboardHint : MonoBehaviour
{
    public TMP_Text hintText;
    public string en;
    public string ru;
    void Start()
    {
        if (Language.Instance.CurrentLanguage == "ru")
        {
            hintText.text = ru;
        }
        else
        {
            hintText.text = en;
        }
    }

    IEnumerator hideKeyboardHintWithDelay()
    {
        yield return new WaitForSeconds(5.0f);
        gameObject.SetActive(false);
    }

    public void ShowKeyboardHint(string text)
    {
        hintText.text = text;
        gameObject.SetActive(true);
        StartCoroutine(hideKeyboardHintWithDelay());
    }
}
