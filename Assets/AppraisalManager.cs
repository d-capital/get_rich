using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppraisalManager : MonoBehaviour
{
    public List<Appraisal> appraisals;
    public void ShowAppraisal()
    {
        FindObjectOfType<HitCounter>().GetComponent<HitCounter>().hasToShowAppraisal = true;
        int randomNumber = Random.Range(0, appraisals.Count-1);
        Appraisal appraisal = appraisals[randomNumber];
        appraisal.gameObject.SetActive(true);
        StartCoroutine(HideAppraisal(appraisal));
    }

    IEnumerator HideAppraisal(Appraisal appraisal)
    {
        yield return new WaitForSeconds(2.0f);
        appraisal.gameObject.SetActive(false);
    }
}
