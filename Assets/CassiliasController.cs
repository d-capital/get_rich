using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CassiliasController : MonoBehaviour
{
    public Animator animator;

    public string nextLevelName;
    public void HasToWalk()
    {
        animator.SetBool("hasToWalk", true);
    }

    public void SwitchBackToIdle()
    {
        animator.SetBool("hasToWalk", false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(nextLevelName);
        }
    }
}
