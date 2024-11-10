using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WastedOverlay : MonoBehaviour
{
    public TMP_Text wastedText;
    public string enWastedText;
    public string ruWastedText;
    public TMP_Text toMainMenuBtnText;
    public string enToMainMenuBtnText;
    public string ruToMainMenuBtnText;
    public TMP_Text restartBtnText;
    public string enRestartBtnText;
    public string ruRestartBtnText;
    public PauseMenu pauseMenu;
    public TMP_Text yourScoreText;
    public string enYourScoreText;
    public string ruYourScoreText;
    public TMP_Text score;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        pauseMenu.canOpenPauseMenu = false;
        if(Language.Instance.CurrentLanguage == "en")
        {
            wastedText.text = enWastedText;
            toMainMenuBtnText.text = enToMainMenuBtnText;
            restartBtnText.text = enRestartBtnText;
            if(yourScoreText is not null)
            {
                yourScoreText.text = enYourScoreText;
                score.text = GameObject.FindObjectOfType<HitCounter>().hits.ToString();
            }
        }
        else if(Language.Instance.CurrentLanguage == "ru")
        {
            wastedText.text = ruWastedText;
            toMainMenuBtnText.text = ruToMainMenuBtnText;
            restartBtnText.text = ruRestartBtnText;
            if(yourScoreText is not null)
            {
                yourScoreText.text = ruYourScoreText;
                score.text = GameObject.FindObjectOfType<HitCounter>().hits.ToString();
            }
        }
        
    }

    public void ReloadLevel()
    {
        Time.timeScale = 1f;
        pauseMenu.canOpenPauseMenu = true;
        SaveSystem.Instance.playerData.Health = 500;
        SaveSystem.Instance.playerData.Stamina = 100;
        SaveSystem.Instance.playerData.HasKeyLevel = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1f;
        pauseMenu.canOpenPauseMenu = true;
        SceneManager.LoadScene("MainMenu");
    }

     public void ShowWastedScreen()
    {
        gameObject.SetActive(true);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Time.timeScale = 0f;
        pauseMenu.canOpenPauseMenu = false;
        if(yourScoreText is not null)
        {
            score.text = GameObject.FindObjectOfType<HitCounter>().hits.ToString();
        }
    }


}