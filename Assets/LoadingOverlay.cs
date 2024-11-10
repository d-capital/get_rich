using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingOverlay : MonoBehaviour
{
    public TMP_Text text;
    public string enText;
    public string ruText;
    // Start is called before the first frame update
    void Start()
    {
        if(Language.Instance.CurrentLanguage == "en")
        {
            text.text = enText;        
        }
        else if(Language.Instance.CurrentLanguage == "ru")
        {
            text.text = ruText;
        }
    }

    public void RemoveOveraly()
    {
        gameObject.SetActive(false);
    }
}
