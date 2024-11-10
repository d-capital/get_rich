using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Computer : MonoBehaviour
{

    public string en;
    public string ru;
    public static KeyboardHint keyboardHint;
    private void Start()
    {
        keyboardHint = Resources.FindObjectsOfTypeAll<KeyboardHint>().FirstOrDefault();
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject target = collision.collider.gameObject;
        if (target.name == "Player" )
        {
            string keyboardHintText = en;
            if (Language.Instance.CurrentLanguage == "ru")
            {
                keyboardHintText = ru;
            }
            else if(Language.Instance.CurrentLanguage == "en")
            {
                keyboardHintText = en;
            }
            Debug.Log("Computer: calling keboard hint");
            Resources.FindObjectsOfTypeAll<KeyboardHint>().FirstOrDefault().ShowKeyboardHint(keyboardHintText);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject target = collision.collider.gameObject;
        if (target.name == "Player" && Input.GetKeyDown(KeyCode.F))
        {
            Resources.FindObjectsOfTypeAll<AbilityManager>().FirstOrDefault().ShowAbilityScreen();
        }
    }
}
