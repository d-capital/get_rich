using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    public TMP_Text restartText;
    public TMP_Text mainMenuText;
    public TMP_Text resumeText;

    public string restartEn;
    public string restartRu;
    public string mainMenuEn;
    public string mainMenuRu;
    public string resumeEn;
    public string resumeRu;

    // Start is called before the first frame update
    void Start()
    {
        if (Language.Instance.CurrentLanguage == "ru")
        {
            restartText.text = restartRu;
            mainMenuText.text = mainMenuRu;
            resumeText.text = resumeRu;
        }
        else
        {
            restartText.text = restartEn;
            mainMenuText.text = mainMenuEn;
            resumeText.text = resumeEn;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
