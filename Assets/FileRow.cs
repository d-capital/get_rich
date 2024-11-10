using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Rendering.PostProcessing;
using System.IO;

public class FileRow : MonoBehaviour
{
    public TMP_Text trackName;
    public TMP_Text artistName;
    public string enTrackName;
    public string enArtistName;

    public Ability ability;

    public AudioSource audioFile;

    public string postProcessEffectName;
    public bool shouldHavePostProcessingEffect;
    public PostProcessVolume spawnedPpv;

    public Sprite abilityImage;

    public GameObject downloadButton;
    public GameObject doneButton;
    // Start is called before the first frame update
    void Start()
    {

            trackName.text = enTrackName;
            artistName.text = enArtistName;
    }

    public void DownloadTrack()
    {
        //add ability to player
        MusicPlayerManager musicPlayerManager = FindObjectOfType<MusicPlayerManager>();
        string artistAndTrackName = enArtistName + " - " + enTrackName;
        AudioSource fielForTrack = Resources.LoadAll<AudioSource>("Tracks").FirstOrDefault(x => x.gameObject.name == artistAndTrackName);
        AudioSource spawnedAudioFile = Instantiate(fielForTrack, new Vector3(0, 0, 0), Quaternion.identity, musicPlayerManager.transform);
        if (shouldHavePostProcessingEffect)
        {
            PostProcessVolume ppv = Resources.LoadAll<PostProcessVolume>("PostProcessVolumes").FirstOrDefault(
                x => x.gameObject.name == postProcessEffectName);
            spawnedPpv = Instantiate(ppv, new Vector3(0, 0, 0), Quaternion.identity);
        }
        Track trackToAdd = new Track
        {
            audioFile = spawnedAudioFile,
            artistName = enArtistName,
            trackName = enTrackName,
            abilities = new List<Ability>
            {
                new Ability
                {
                    abilityName = ability.abilityName,
                    abilitySprite = ability.abilitySprite,
                    cameraEffect = spawnedPpv,
                    cameraEffectName = ability.cameraEffectName,
                    duration = ability.duration,
                    stamina = ability.stamina
                }
            }
        };
        musicPlayerManager.Tracks.Add(trackToAdd);
        //TODO: play some nice sound.
        //deactivate all the same tracks in computer.
        List<FileRow> sameFileRows = FindObjectOfType<AbilityManager>().gameObject.
            GetComponentsInChildren<FileRow>().Where(x=> x.enArtistName == enArtistName 
                && x.enTrackName == enTrackName).ToList();
        foreach(FileRow fr in sameFileRows)
        {
            fr.changeTrackStatusToDone();
        }
    }

    public void changeTrackStatusToDone()
    {
        downloadButton.SetActive(false);
        doneButton.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
