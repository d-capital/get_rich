using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCutSceneController : MonoBehaviour
{
    public Animator animator;
    public string nextLevelName;
    public float timeTillSceneSwitch;

    public void HasToWalk()
    {
        animator.SetBool("isMoving",true);
        animator.SetBool("hasWeapon",false);
        animator.SetBool("hasRifle",false);
    }

    public void BackToIdle()
    {
        animator.SetBool("isMoving",false);
        animator.SetBool("hasWeapon",false);
        animator.SetBool("hasRifle",false);
    }

    public void Start()
    {
        if(timeTillSceneSwitch != 0)
        {
            StartCoroutine(ToTheGame());
        }
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && nextLevelName is not null)
        {
            SceneManager.LoadScene(nextLevelName);
        }

    }

    IEnumerator ToTheGame()
    {
        yield return new WaitForSeconds(timeTillSceneSwitch);
        SceneManager.LoadScene(nextLevelName);
    }

}
