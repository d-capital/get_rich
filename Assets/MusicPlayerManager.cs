using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Track
{
    public AudioSource audioFile;
    public string artistName;
    public string trackName;
    public List<Ability> abilities;
}
[System.Serializable]
public class TrackToSave
{
    public string artistName;
    public string trackName;
    public List<AbilityToSave> abilities;
}
public class MusicPlayerManager : MonoBehaviour
{
    public List<Track> Tracks;
    AudioSource currentAudioSource;
    int currentAudioIndex;
    float currentAudioLenght;
    bool isMusicPlaying;

    public TMP_Text artistName;
    public TMP_Text songTitle;

    public SpriteRenderer abilityIconSprite;
    public TMP_Text abilityName;

    public PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name != "Level3")
        {
            Tracks[0].audioFile.Play();
            currentAudioIndex = Tracks.IndexOf(Tracks[0]);
            currentAudioSource = Tracks[0].audioFile;
            currentAudioLenght = currentAudioSource.clip.length;
            isMusicPlaying = true;
            artistName.text = Tracks[0].artistName;
            songTitle.text = Tracks[0].trackName;
            abilityIconSprite.sprite = Tracks[0].abilities[0].abilitySprite;
            abilityName.text = Tracks[0].abilities[0].abilityName;
            StopAnyAbility();
            playerController.activeAbility = Tracks[0].abilities[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name != "Level3")
        {
            if (isMusicPlaying)
            {
                currentAudioLenght -= Time.deltaTime;
            }
            if (currentAudioLenght<0f)
            {
                currentAudioSource.Stop();
                int newAudioIndex = currentAudioIndex + 1;
                if (newAudioIndex < Tracks.Count)
                {
                    Tracks[newAudioIndex].audioFile.Play();
                    currentAudioSource = Tracks[newAudioIndex].audioFile;
                    currentAudioIndex = newAudioIndex;
                    currentAudioLenght = currentAudioSource.clip.length;
                    artistName.text = Tracks[newAudioIndex].artistName;
                    songTitle.text = Tracks[newAudioIndex].trackName;
                    abilityIconSprite.sprite = Tracks[newAudioIndex].abilities[0].abilitySprite;
                    abilityName.text = Tracks[newAudioIndex].abilities[0].abilityName;
                    StopAnyAbility();
                    playerController.activeAbility = Tracks[newAudioIndex].abilities[0];
                }
                else
                {
                    Tracks[0].audioFile.Play();
                    currentAudioSource = Tracks[0].audioFile;
                    currentAudioIndex = 0;
                    currentAudioLenght = currentAudioSource.clip.length;
                    artistName.text = Tracks[0].artistName;
                    songTitle.text = Tracks[0].trackName;
                    abilityIconSprite.sprite = Tracks[0].abilities[0].abilitySprite;
                    abilityName.text = Tracks[0].abilities[0].abilityName;
                    StopAnyAbility();
                    playerController.activeAbility = Tracks[0].abilities[0];
                }
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                PlayPrevAudio();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlayNextAudio();
            }
        }
    }

    public void PlayNextAudio()
    {
        currentAudioSource.Stop();
        int newAudioIndex = currentAudioIndex + 1;
        if (newAudioIndex < Tracks.Count)
        {
            Tracks[newAudioIndex].audioFile.Play();
            currentAudioSource = Tracks[newAudioIndex].audioFile;
            currentAudioIndex = newAudioIndex;
            currentAudioLenght = currentAudioSource.clip.length;
            artistName.text = Tracks[newAudioIndex].artistName;
            songTitle.text = Tracks[newAudioIndex].trackName;
            abilityIconSprite.sprite = Tracks[newAudioIndex].abilities[0].abilitySprite;
            abilityName.text = Tracks[newAudioIndex].abilities[0].abilityName;
            StopAnyAbility();
            playerController.activeAbility = Tracks[newAudioIndex].abilities[0];
        }
        else
        {
            Tracks[0].audioFile.Play();
            currentAudioSource = Tracks[0].audioFile;
            currentAudioIndex = 0;
            currentAudioLenght = currentAudioSource.clip.length;
            artistName.text = Tracks[0].artistName;
            songTitle.text = Tracks[0].trackName;
            abilityIconSprite.sprite = Tracks[0].abilities[0].abilitySprite;
            abilityName.text = Tracks[0].abilities[0].abilityName;
            StopAnyAbility();
            playerController.activeAbility = Tracks[0].abilities[0];
        }
        isMusicPlaying = true;
    }
    public void PlayPrevAudio()
    {
        currentAudioSource.Stop();
        int newAudioIndex = currentAudioIndex - 1;
        if (newAudioIndex >= 0 && newAudioIndex < Tracks.Count)
        {
            Tracks[newAudioIndex].audioFile.Play();
            currentAudioSource = Tracks[newAudioIndex].audioFile;
            currentAudioIndex = newAudioIndex;
            currentAudioLenght = currentAudioSource.clip.length;
            artistName.text = Tracks[newAudioIndex].artistName;
            songTitle.text = Tracks[newAudioIndex].trackName;
            abilityIconSprite.sprite = Tracks[newAudioIndex].abilities[0].abilitySprite;
            abilityName.text = Tracks[newAudioIndex].abilities[0].abilityName;
            StopAnyAbility();
            playerController.activeAbility = Tracks[newAudioIndex].abilities[0];
        }
        else
        {
            Tracks[Tracks.Count-1].audioFile.Play();
            currentAudioSource = Tracks[Tracks.Count - 1].audioFile;
            currentAudioIndex = Tracks.Count - 1;
            currentAudioLenght = currentAudioSource.clip.length;
            artistName.text = Tracks[Tracks.Count - 1].artistName;
            songTitle.text = Tracks[Tracks.Count - 1].trackName;
            abilityIconSprite.sprite = Tracks[Tracks.Count - 1].abilities[0].abilitySprite;
            abilityName.text = Tracks[Tracks.Count - 1].abilities[0].abilityName;
            StopAnyAbility();
            playerController.activeAbility = Tracks[Tracks.Count - 1].abilities[0];
        }
        isMusicPlaying = true;
    }

    public void PlayStop()
    {
        if (isMusicPlaying)
        {
            currentAudioSource.Pause();
            isMusicPlaying = false;
            StopAnyAbility();
        }
        else
        {
            currentAudioSource.Play();
            isMusicPlaying = true;
            StopAnyAbility();
        }
    }

    public void StopAnyAbility()
    {
        playerController.StopAnyAbility();
    }
}
