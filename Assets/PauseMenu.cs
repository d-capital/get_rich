using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public bool isPauseMenuOpen = false;

    public Texture2D cursorTextureNoWeapon;
    public Texture2D cursorTextureWithWeapon;

    public Vector2 hotSpotCenteredWithWeapon;
    public Vector2 hotSpotCenteredNoWeapon;

    public bool canOpenPauseMenu = true;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        //bool isLevelRewardScreenOpen = GameObject.FindObjectsOfType<LevelReward>().Length > 0;
        //bool isVictoryScreenOpen = GameObject.FindGameObjectsWithTag("victory").Length > 0;
        if (Input.GetKeyDown(KeyCode.Escape) && !isPauseMenuOpen && canOpenPauseMenu)
        {
            ShowPauseMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPauseMenuOpen && canOpenPauseMenu)
        {
            HidePauseMenu();
        }
    }

    public void ShowPauseMenu()
    {
        PauseMenuUI[] PauseMenuUI = Resources.FindObjectsOfTypeAll<PauseMenuUI>();
        foreach (PauseMenuUI i in PauseMenuUI)
        {
            if (i.gameObject.name == "PauseMenu")
            {
                i.gameObject.SetActive(true);
                isPauseMenuOpen = true;
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                Time.timeScale = 0f;
            }
        }
        FindObjectOfType<PlayerController>().isUiOpen = true;
    }

    public void HidePauseMenu()
    {
        GameObject.Find("PauseMenu").SetActive(false);
        isPauseMenuOpen = false;
        Time.timeScale = 1f;
        if (GameObject.Find("Player").GetComponentsInChildren<Weapon>().Length > 0)
        {
            hotSpotCenteredWithWeapon = new Vector2(cursorTextureWithWeapon.width / 2, cursorTextureWithWeapon.height / 2);
            Cursor.SetCursor(cursorTextureWithWeapon, hotSpotCenteredWithWeapon, CursorMode.Auto);
        }
        else
        {
            hotSpotCenteredNoWeapon = new Vector2(cursorTextureNoWeapon.width / 2, cursorTextureNoWeapon.height / 2);
            Cursor.SetCursor(cursorTextureNoWeapon, hotSpotCenteredNoWeapon, CursorMode.Auto);
        }
        FindObjectOfType<PlayerController>().isUiOpen = false;
    }

    public void ReloadLevel()
    {
        SaveSystem.Instance.playerData.Health = 500;
        SaveSystem.Instance.playerData.Stamina = 100;
        SaveSystem.Instance.playerData.HasKeyLevel = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowOrHidePauseMenu()
    {
        //bool isLevelRewardScreenOpen = GameObject.FindObjectsOfType<LevelReward>().Length > 0;
        //bool isVictoryScreenOpen = GameObject.FindGameObjectsWithTag("victory").Length > 0;
        if (!isPauseMenuOpen)
        {
            ShowPauseMenu();
        }
        else if (isPauseMenuOpen)
        {
            HidePauseMenu();
        }
    }

}
