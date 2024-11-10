using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class AbilityManager : MonoBehaviour
{
    public GridLayoutGroup abilityGrid;
    public MusicPlayerManager musicPlayerManager;
    // Start is called before the first frame update
    void Start()
    {
        FileRow[] fileRows  = Resources.LoadAll<FileRow>("FileRows");
        int numberOfTracks = 2;
        List<int> numbers = new List<int>();
        for (int i = 0; i < numberOfTracks; i++)
        {
            numbers.Add(Random.Range(0, fileRows.Length - 1));
        }
        foreach(int rn in numbers)
        {
            Instantiate(fileRows[rn], new Vector3 (0,0,0), Quaternion.identity, abilityGrid.transform);
        }

        FileRow[] spawnedFileRows = abilityGrid.GetComponentsInChildren<FileRow>();
        List<string> currentUserAbilities = new List<string>();
        foreach(Track t in musicPlayerManager.Tracks)
        {
            currentUserAbilities.Add(t.abilities[0].abilityName);
        }
        foreach(FileRow sfr in spawnedFileRows)
        {
            if(currentUserAbilities.Any(cua => cua.Contains(sfr.ability.abilityName)))
            {
                sfr.changeTrackStatusToDone();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowAbilityScreen()
    {
        Time.timeScale = 0f;
        FindObjectOfType<PlayerController>().isUiOpen = true;
        gameObject.SetActive(true);
    }

    public void HideAbilityScreen()
    {
        Time.timeScale = 1f;
        FindObjectOfType<PlayerController>().isUiOpen = false;
        gameObject.SetActive(false);
        if(SceneManager.GetActiveScene().name == "Bullethell")
        {
            GameObject.FindObjectOfType<Computer>().gameObject.SetActive(false);
            GameObject.FindObjectOfType<RoomManager>().ActivateComputer();
        }
    }

}
