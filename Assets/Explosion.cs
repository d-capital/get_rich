using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public ParticleSystem thisParticleSystem;
    // Start is called before the first frame update
    void Start()
    {
        thisParticleSystem.Play();
        StartCoroutine(DestoryWithDelay());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator DestoryWithDelay()
    {
        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
    }
}
