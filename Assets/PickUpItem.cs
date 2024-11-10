using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    
    public string enInfoHint;
    public string ruInfoHint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            InfoHint ih = Resources.FindObjectsOfTypeAll<InfoHint>().FirstOrDefault();
            ih.gameObject.SetActive(true);
            if (Language.Instance.CurrentLanguage == "ru")
            {
                ih.ShowInfoHint(ruInfoHint);
            }
            else 
            {
                ih.ShowInfoHint(enInfoHint);
            }

            Destroy(gameObject);
        }
    }
}
