using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Room : MonoBehaviour
{
    [SerializeField] GameObject topDoor;
    [SerializeField] GameObject downDoor;
    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject rightDoor;

    [SerializeField] GameObject topStaircase;
    [SerializeField] GameObject downStaircase;
    [SerializeField] GameObject leftStaircase;
    [SerializeField] GameObject rightStaircase;

    [SerializeField] GameObject finalTopDoor;
    [SerializeField] GameObject finalDownDoor;
    [SerializeField] GameObject finalLeftDoor;
    [SerializeField] GameObject finalRightDoor;

    [SerializeField] public SpriteRenderer floorObject;
    public Sprite[] floorSprites;

    public Transform[] enemySpawnPoints;
    public Transform bossTurretSpawnPoint;

    public Vector2Int RoomIndex { get; set; }
    public bool hasEnemies = false;
    public int intRoomIndex;

    public GameObject computer;

    public bool hasPlayer = false;

    private void Start()
    {
        floorObject.sprite = floorSprites[Random.Range(0,floorSprites.Length-1)];
    }
    public void OpenDoor (Vector2Int direction)
    {
        if (direction == Vector2Int.up)
        {
            topDoor.SetActive(false);
            topStaircase.SetActive(true);

        }
        if (direction == Vector2Int.down)
        {
            downDoor.SetActive(false);
            downStaircase.SetActive(true);
        }
        if (direction == Vector2Int.left)
        {
            leftDoor.SetActive(false);
            leftStaircase.SetActive(true);
        }
        if (direction == Vector2Int.right)
        {
            rightDoor.SetActive(false);
            rightStaircase.SetActive(true);
        }
    }

    public void SetFinalDoor()
    {
        List<GameObject> doors = new List<GameObject>();
        doors.Add(rightDoor);
        doors.Add(leftDoor);
        doors.Add(topDoor);
        doors.Add(rightDoor);

        List<GameObject> finalDoors = new List<GameObject>();
        finalDoors.Add(finalRightDoor);
        finalDoors.Add(finalLeftDoor);
        finalDoors.Add(finalTopDoor);
        finalDoors.Add(finalDownDoor);

        List<GameObject> closedDoors = doors.FindAll(x => x.activeInHierarchy);
        int randomNumber = Random.Range(0, closedDoors.Count - 1);
        GameObject doorToSetFinalDoor = closedDoors.ElementAt(randomNumber);
        string doorTag = doorToSetFinalDoor.tag;

        GameObject finalDoorToSet = finalDoors.FirstOrDefault(x => x.tag == doorTag);
        doorToSetFinalDoor.SetActive(false);
        finalDoorToSet.SetActive(true);

    }

    public List<GameObject> ReportClosedDoors()
    {
        List<GameObject> closedDoors = new List<GameObject>();
        if(!leftDoor.activeInHierarchy)
        {
            closedDoors.Add(leftDoor);
        }
        if(!rightDoor.activeInHierarchy)
        {
            closedDoors.Add(leftDoor);
        }
        if(!topDoor.activeInHierarchy)
        {
            closedDoors.Add(leftDoor);
        }
        if(!downDoor.activeInHierarchy)
        {
            closedDoors.Add(leftDoor);
        }
        return closedDoors;
    } 

}
