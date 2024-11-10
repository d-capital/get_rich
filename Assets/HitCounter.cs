using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitCounter : MonoBehaviour
{
    public TMP_Text hitCouner;
    public int hits = 0;
    public bool hasToShowAppraisal = false;
    public TMP_Text hitCounterTitle;
    public string hitCounterTitleEn;
    public string hitCounterTitleRu;

     public void Start()
    {
        SaveSystem.Instance.playerData.Kills = 0;
        hits = SaveSystem.Instance.playerData.Kills;
        hitCouner.text = hits.ToString();
        if(Language.Instance.CurrentLanguage == "ru")
        {
            hitCounterTitle.text = hitCounterTitleRu;
        }
        else
        {
            hitCounterTitle.text = hitCounterTitleEn;
        }
    }

    public void UpdateHitCounter()
    {
        hits += 1;
        hitCouner.text = hits.ToString();
        SaveSystem.Instance.playerData.Kills = hits;
    }

    private void Update()
    {
        if(hits % 5 != 0)
        {
            hasToShowAppraisal = false;
        }
        else if(hits !=0 && hits % 5 == 0 && !hasToShowAppraisal)
        {
            hasToShowAppraisal = true;
            GameObject.FindObjectOfType<AppraisalManager>().GetComponent<AppraisalManager>().ShowAppraisal();
        }
    }

}
