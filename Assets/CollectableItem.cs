using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectableItem : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {

        GameObject target = collision.collider.gameObject;
        if (target.name == "Player")
        {
            Destroy(gameObject);
            SaveSystem.Instance.playerData.HasKeyLevel = true;
            SaveSystem.Instance.SavePlayer();
        }

    }
}
