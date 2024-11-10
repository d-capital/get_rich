using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavMeshPlus.Components;
using NavMeshPlus.Extensions;
using NavMeshPlus;
public class NavMeshAutoBaker : MonoBehaviour
{

    public NavMeshSurface[] surfaces;
    public NavMeshModifier[] objectsToRotate;

    // Use this for initialization
    public void Bake()
    {

        objectsToRotate = GameObject.FindObjectsOfType<NavMeshModifier>();
        for (int j = 0; j < objectsToRotate.Length; j++)
        {
            objectsToRotate[j].transform.localRotation = Quaternion.Euler(new Vector3(0, 50 * Time.deltaTime, 0) + objectsToRotate[j].transform.localRotation.eulerAngles);
        }
        GameObject.FindObjectOfType<NavMeshSurface>().BuildNavMesh();
    }

}