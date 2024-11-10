using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Language : MonoBehaviour
{
    public string CurrentLanguage;

    public static Language Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CurrentLanguage = "en";
            try
            {
                CurrentLanguage = SaveSystem.Instance.playerData.Language;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
