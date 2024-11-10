using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestractableObject : MonoBehaviour
{
    public int health;
    public string objectName;
    public string objectNameRu;
    public string objectNameEn;
    // Start is called before the first frame update
    void Start()
    {
        if (Language.Instance.CurrentLanguage == "ru")
        {
            objectName = objectNameRu;
        }
        else
        {
            objectName = objectNameEn;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
