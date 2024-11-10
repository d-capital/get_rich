using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlHInt : MonoBehaviour
{
    
    public TMP_Text hintText;
    public string en;
    public string ru;
    // Start is called before the first frame update
    void Start()
    {
        if(Language.Instance.CurrentLanguage == "en")
        {
            hintText.text = en;
        }
        else if(Language.Instance.CurrentLanguage == "ru")
        {
            hintText.text = ru;
        }
        StartCoroutine(HideControlHintAsync());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator HideControlHintAsync()
    {
        yield return new WaitForSeconds(5.0f);
        gameObject.SetActive(false);
    }
}
