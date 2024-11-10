using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DialogueLineInfo
{
    public string line;
    public Sprite avatarSource;
    public string name;
    public float duration;
}
public class DialogueBox : MonoBehaviour
{
    public TMP_Text CharacterName;
    public TMP_Text DialogueLine;
    public Image CharacterAvatar;

    public List<DialogueLineInfo> enDialogueLines;
    public List<DialogueLineInfo> ruDialogueLines;
    List<DialogueLineInfo> dialogueLines;

    public float timeTillSceneSwitch;

    [SerializeField] float delayBeforeStart = 0f;
	[SerializeField] float timeBtwChars = 0.01f;
	[SerializeField] string leadingChar = "";
	[SerializeField] bool leadingCharBeforeDelay = false;

    public string nextLevelName;

    string writer;

    // Start is called before the first frame update
    void Start()
    {
        if (Language.Instance.CurrentLanguage == "ru")
        {
            dialogueLines = ruDialogueLines;
        }
        else
        {
            dialogueLines = enDialogueLines;
        }
        foreach(DialogueLineInfo dli in dialogueLines)
        {
            StartCoroutine(ShowDialogueLine(dli.line, dli.duration, dli.avatarSource,dli.name));
        }
        if(SceneManager.GetActiveScene().name == "CutScene0" 
            || SceneManager.GetActiveScene().name == "CutScene1" 
            || SceneManager.GetActiveScene().name == "CutScene2"
            || SceneManager.GetActiveScene().name == "CutScene3" 
            || SceneManager.GetActiveScene().name == "CutScene4"
            || SceneManager.GetActiveScene().name == "CutScene5" 
            || SceneManager.GetActiveScene().name == "CutScene6"
            || SceneManager.GetActiveScene().name == "CutScene7"
            )
        {
            StartCoroutine(ToTheGame());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ShowDialogueLine(string line, float duration, Sprite imageSource, string name)
    {
        yield return new WaitForSeconds(duration);
        CharacterAvatar.sprite = imageSource;
        CharacterName.text = name;
        writer = line;
        DialogueLine.text = "";
        StartCoroutine(TypeWriterTMP(DialogueLine));
    }
    IEnumerator ToTheGame()
    {
        yield return new WaitForSeconds(timeTillSceneSwitch);
        SceneManager.LoadScene(nextLevelName);
    }

    IEnumerator TypeWriterTMP(TMP_Text _tmpProText)
    {
        _tmpProText.text = leadingCharBeforeDelay ? leadingChar : "";

        yield return new WaitForSeconds(delayBeforeStart);

		foreach (char c in writer)
		{
			if (_tmpProText.text.Length > 0)
			{
				_tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
			}
			_tmpProText.text += c;
			_tmpProText.text += leadingChar;
			yield return new WaitForSeconds(timeBtwChars);
		}

		if (leadingChar != "")
		{
			_tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
		}
	}
}
