using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Door : MonoBehaviour
{
    public static KeyboardHint keyboardHint;

    private void Start()
    {
        keyboardHint = Resources.FindObjectsOfTypeAll<KeyboardHint>().FirstOrDefault();
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject target = collision.collider.gameObject;
        if (target.name == "Player" && gameObject.activeInHierarchy)
        {
            Debug.Log("Door: calling keboard hint");
            keyboardHint.ShowKeyboardHint("Press [F]");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject target = collision.collider.gameObject;
        if (target.name == "Player")
        {
            if (SaveSystem.Instance.playerData.HasKeyLevel && Input.GetKeyDown(KeyCode.F))
            {
                PlayerController player = FindObjectOfType<PlayerController>();
                string nextLevelName = SaveSystem.Instance.GetNextLevelName(SaveSystem.Instance.playerData.CurrentLevelName);
                SaveSystem.Instance.playerData.CurrentLevelName = nextLevelName;
                SaveSystem.Instance.SavePlayer();
                SaveSystem.Instance.SaveToFile();
                SceneManager.LoadScene(player.nextLevelName);
            }
            else
            {
                if(SaveSystem.Instance.playerData.Language == "en")
                {
                    keyboardHint.ShowKeyboardHint("You need to have key to open that door.");
                }
                else if(SaveSystem.Instance.playerData.Language == "ru")
                {
                    keyboardHint.ShowKeyboardHint("Вам нужен ключь, чтобы открыть дверь.");
                }
            }
        }

    }
}
