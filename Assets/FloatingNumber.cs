using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class FloatingNumber : MonoBehaviour
{
    public TMP_Text numberToShow;
    public Animator numberAnimator;
    
    
    void Start()
    {
        GetComponentInChildren<Canvas>().worldCamera = FindObjectsOfType<Camera>().FirstOrDefault(x => x.gameObject.tag == "MainCamera");
    }
    public void DestroyFloatingNumber()
    {
        Destroy(gameObject);
    }

    public void ShowNumber(int number)
    {
        numberToShow.text = "+" + number.ToString();
        numberAnimator.Play("Floating");
    }

}
