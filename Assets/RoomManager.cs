using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

class DoubleDistance
{
    public int roomIndexInt;
    public float avgDistance;
    public DoubleDistance(int roomIndexInt, float avgDistance)
    {
        this.roomIndexInt = roomIndexInt;
        this.avgDistance = avgDistance;
    }
}

public class RoomManager : MonoBehaviour
{
    [SerializeField] GameObject roomPrefab;
    [SerializeField] private int maxRooms = 15;
    [SerializeField] private int minRooms = 10;
    int roomWidth = 20;
    int roomHeight = 12;
    [SerializeField] int gridSizeX = 10;
    [SerializeField] int gridSizeY = 10;
    [SerializeField] public NavMeshAutoBaker autoBaker;

    private List<GameObject> roomObjects = new List<GameObject>();

    private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();

    private int[,] roomGrid;

    private bool generationComplete = false;

    private int roomCount;

    string previousSpawnedEnemyName;

    public int enemyLimit = 10;

    public int enemiesCount = 0;

    public LoadingOverlay loadingOverlay;

    public PlayerController player;

    public BossAI boss;

    Room roomWithBoss;
    Room roomWithFinalDoor;

    string levelName;

    private void Start()
    {
        roomGrid = new int[gridSizeX, gridSizeY];
        roomQueue = new Queue<Vector2Int>();
        Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);
        levelName = SceneManager.GetActiveScene().name;
    }

    private void StartRoomGenerationFromRoom(Vector2Int roomIndex)
    {
        roomQueue.Enqueue(roomIndex);
        int x = roomIndex.x;
        int y = roomIndex.y;
        roomGrid[x, y] = 1;
        roomCount++;
        var initalRoom = Instantiate(roomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        initalRoom.name = $"Room-{roomCount}";
        initalRoom.GetComponent<Room>().RoomIndex = roomIndex;
        initalRoom.GetComponent<Room>().intRoomIndex = roomCount;
        roomObjects.Add(initalRoom);
    }

    private Vector3 GetPositionFromGridIndex(Vector2Int gridIndex)
    {
        int gridX = gridIndex.x;
        int gridY = gridIndex.y;
        return new Vector3(roomWidth * (gridX - gridSizeX / 2), roomHeight * (gridY - gridSizeY / 2));
    }

    private void Update()
    {
        if(roomQueue.Count > 0 && roomCount < maxRooms && !generationComplete)
        {
            Vector2Int roomIndex = roomQueue.Dequeue();
            int gridX = roomIndex.x;
            int gridY = roomIndex.y;
            TryGenerateRoom(new Vector2Int(gridX - 1, gridY));
            TryGenerateRoom(new Vector2Int(gridX + 1, gridY));
            TryGenerateRoom(new Vector2Int(gridX, gridY + 1));
            TryGenerateRoom(new Vector2Int(gridX, gridY - 1));
        }
        else if (roomCount < minRooms)
        {
            Debug.Log("RoomCount was less then minimum amount of rooms. Trying again.");
            RegenarateRooms();
        }
        else if (!generationComplete)
        {
            Debug.Log("Generation completed");
            generationComplete = true;
            CheckDoors();
            autoBaker.Bake();
            SpawnBoss();
            if(levelName != "Bullethell")
            {
                SaveSystem.Instance.LoadPlayerData();
            }
            PlacePlayer();
            SpawnEnemies();
            SpawnTurret();
            ActivateComputer();
            FindObjectOfType<PlayerController>().InitializePlayer();
            SpawnKey();
            loadingOverlay.RemoveOveraly();
        }
        if(generationComplete && (levelName == "Level4" || levelName == "Bullethell"))
        {
            if(enemiesCount < 5 && GameObject.FindObjectOfType<HitCounter>().hits < 40)
            {
                SpawnEnemies();
            }
        }
        if(generationComplete && levelName == "Level7")
        {
            if(FindObjectsOfType<Edrink>().Length == 0)
            {
                SpawnEnergyDrink();
            }
        }

    }
    private void SpawnEnemies()
    {
        List<string> enemyNames =  new List<string>();
        if(levelName =="SampleScene")
        {
            enemyNames.Add("en_1");
            enemyNames.Add("en_2");
        }
        else if(levelName == "Level2" || levelName == "Level5")
        {
            enemyNames.Add("en_1");
            enemyNames.Add("en_2_lvl2");
        }
        else if(levelName == "Level3")
        {
            enemyNames.Add("en_1_lvl3");
            enemyNames.Add("en_2_lvl3");
        }
        else if(levelName == "Level4")
        {
            enemyNames.Add("en_1_lvl4");
            enemyNames.Add("en_2_lvl4");
        }
        else if(levelName == "Level6")
        {
            enemyNames.Add("en_1_lvl6");
            enemyNames.Add("en_2_lvl6");
        }
        else if(levelName == "Level7")
        {
            enemyNames.Add("en_1_lvl6");
            enemyNames.Add("en_1");
            enemyNames.Add("en_2");
            enemyNames.Add("en_1_lvl4");
            enemyNames.Add("en_2_lvl2");
        }
        else if(levelName == "Bullethell")
        {
            enemyNames.Add("en_1_lvl6");
            enemyNames.Add("en_1");
            enemyNames.Add("en_2");
            enemyNames.Add("en_1_lvl4");
            enemyNames.Add("en_2_lvl2");
            enemyNames.Add("en_1_lvl3");
            enemyNames.Add("en_2_lvl3");
            enemyNames.Add("en_1_lvl6");
            enemyNames.Add("en_2_lvl6");
        }
        Room[] rooms = Resources.FindObjectsOfTypeAll<Room>();
        foreach (Room room in rooms)
        {
            int probabilityOfRoomHavingEnemies = Random.Range(0,100);
            if(probabilityOfRoomHavingEnemies > 15 && !room.hasPlayer)
            {
                room.hasEnemies = true;
                string enemyNameToSpawn = enemyNames.ElementAt(Random.Range(0, enemyNames.Count - 1));
                if(previousSpawnedEnemyName == "en_1")
                {
                    if(levelName == "SampleScene")
                    {
                        enemyNameToSpawn = "en_2";
                    }
                    else if(levelName == "Level2" || levelName == "Level5")
                    {
                        enemyNameToSpawn = "en_2_lvl2";
                    }
                }
                if(previousSpawnedEnemyName == "en_1_lvl3" && levelName == "Level3")
                {
                    enemyNameToSpawn = "en_2_lvl3";
                }
                if(previousSpawnedEnemyName == "en_1_lvl4" && levelName == "Level4")
                {
                    enemyNameToSpawn = "en_2_lvl4";
                }
                if(previousSpawnedEnemyName == "en_1_lvl6" && levelName == "Level6")
                {
                    enemyNameToSpawn = "en_2_lvl6";
                }
                EnemyAI enemy = Resources.Load<EnemyAI>(enemyNameToSpawn);
                Transform[] enemySpawnPoints = room.enemySpawnPoints;
                int randomNumber = Random.Range(0, 3);
                Transform spawnPointToUse = enemySpawnPoints[randomNumber];
                Instantiate(enemy, spawnPointToUse.position, Quaternion.identity);
                previousSpawnedEnemyName = enemyNameToSpawn;
                enemiesCount += 1;
            }
        }
    }

    private void SpawnBoss()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        Room[] rooms = Resources.FindObjectsOfTypeAll<Room>();
        Vector3 playerPosition = player.transform.position;
        Dictionary<int, float> playerRoomsDistances = new Dictionary<int, float>();
        foreach(Room room in rooms)
        {
            Vector3 roomPosition = room.transform.position;
            float distance = Vector3.Distance(roomPosition, playerPosition);
            playerRoomsDistances.Add(room.intRoomIndex, distance);
        }
        int longestDistance = playerRoomsDistances.FirstOrDefault(x => x.Value.Equals(playerRoomsDistances.Values.Max())).Key;
        Room farestRoom = rooms.FirstOrDefault(r => r.intRoomIndex == longestDistance);
        roomWithBoss = farestRoom;
        Transform bossSpawnPoint = farestRoom.bossTurretSpawnPoint;
        //BossAI bossPrefab = Resources.Load<BossAI>("b_1");
        if(boss is not null)
        {
            Instantiate(boss, bossSpawnPoint.position, Quaternion.identity);
        }
        farestRoom.hasEnemies = true;
        SetFinalDoor(farestRoom);
    }

    public void SpawnEnergyDrink()
    {
        if(levelName == "Level7")
        {
            int randomNuber = Random.Range(0,3);
            Edrink edrink = Resources.Load<Edrink>("eDrinkBlock");
            Instantiate(edrink, roomWithBoss.enemySpawnPoints[randomNuber].position, Quaternion.identity);
        }
    }
    private void SetFinalDoor(Room bossRoom)
    {
        Room[] rooms = Resources.FindObjectsOfTypeAll<Room>();
        Vector3 boosRoomPosition = bossRoom.gameObject.transform.position;
        Dictionary<int, float> bossRoomsDistances = new Dictionary<int, float>();
        foreach (Room room in rooms)
        {
            Vector3 roomPosition = room.transform.position;
            float distance = Vector3.Distance(roomPosition, boosRoomPosition);
            bossRoomsDistances.Add(room.intRoomIndex, distance);
        }
        int longestDistance = bossRoomsDistances.FirstOrDefault(x => x.Value.Equals(bossRoomsDistances.Values.Max())).Key;
        Room farestRoomFromBoss = rooms.FirstOrDefault(r => r.intRoomIndex == longestDistance);
        if(SceneManager.GetActiveScene().name != "Level3" 
            && SceneManager.GetActiveScene().name != "Level4"
            && SceneManager.GetActiveScene().name != "Bullethell")
        {
            farestRoomFromBoss.SetFinalDoor();
        }
        roomWithFinalDoor = farestRoomFromBoss;
    }

    private void SpawnTurret()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        Room[] rooms = Resources.FindObjectsOfTypeAll<Room>();
        Vector3 playerPosition = player.transform.position;
        Dictionary<int, float> playerRoomsDistancesDict = new Dictionary<int, float>();
        foreach (Room room in rooms)
        {
            Vector3 roomPosition = room.transform.position;
            float distance = Vector3.Distance(roomPosition, playerPosition);
            playerRoomsDistancesDict.Add(room.intRoomIndex, distance);
        }
        int nearestRoomIndex = playerRoomsDistancesDict.FirstOrDefault(x => x.Value.Equals(playerRoomsDistancesDict.Values.Min())).Key;
        foreach(Room potentialTurretRoom in rooms)
        {
            if (potentialTurretRoom.intRoomIndex != nearestRoomIndex && !potentialTurretRoom.hasEnemies && Random.Range(0,100)>10)
            {
                Transform turretSpawnPoint = potentialTurretRoom.bossTurretSpawnPoint;
                LaserTuret laserTuret = Resources.Load<LaserTuret>("laser_turet");
                Instantiate(laserTuret, turretSpawnPoint.position, Quaternion.identity);
            }
        }

    }

    public void ActivateComputer()
    {
        
        if(levelName != "Level4" && levelName != "Level6" && levelName != "Bullethell")
        {
            BossAI boss = FindObjectOfType<BossAI>();
            Room[] rooms = Resources.FindObjectsOfTypeAll<Room>();
            Dictionary<int, float> bossRoomsDistances = new Dictionary<int, float>();
            foreach(Room room in rooms)
            {
                Vector3 roomPosition = room.transform.position;
                float distance = Vector3.Distance(roomPosition, boss.transform.position);
                bossRoomsDistances.Add(room.intRoomIndex, distance);
            }
            int longestDistance = bossRoomsDistances.FirstOrDefault(x => x.Value.Equals(bossRoomsDistances.Values.Max())).Key;
            Room farestRoom = rooms.FirstOrDefault(r => r.intRoomIndex == longestDistance);
            farestRoom.computer.SetActive(true);
            farestRoom.computer.GetComponent<BoxCollider2D>().enabled = true;
        }else
        {
            int randomRoomIndex = Random.Range(0, roomCount);
            Room[] rooms = Resources.FindObjectsOfTypeAll<Room>();
            Room farestRoom = rooms.FirstOrDefault(r => r.intRoomIndex == randomRoomIndex);
            farestRoom.computer.SetActive(true);
            farestRoom.computer.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    private bool TryGenerateRoom (Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;
        
        if(roomCount >= maxRooms)
        {
            return false;
        }

        if(Random.value < 0.5f && roomIndex != Vector2Int.zero)
        {
            return false;
        }

        if (CountAdjacentRooms(roomIndex) > 1)
        {
            return false;
        }
        roomQueue.Enqueue(roomIndex);
        roomGrid[x, y] = 1;
        roomCount++;
        var newRoom = Instantiate(roomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        newRoom.GetComponent<Room>().RoomIndex = roomIndex;
        newRoom.name = $"Room-{roomCount}";
        newRoom.GetComponent<Room>().intRoomIndex = roomCount;
        roomObjects.Add(newRoom);

        OpenDoors(newRoom, x, y);

        return true;
    }

    private void RegenarateRooms()
    {
        roomObjects.ForEach(Destroy);
        roomObjects.Clear();
        roomGrid = new int[gridSizeX, gridSizeY];
        roomQueue.Clear();
        roomCount = 0;
        generationComplete = false;

        Vector2Int initalRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        StartRoomGenerationFromRoom(initalRoomIndex);
    }

    void OpenDoors(GameObject room, int x, int y)
    {
        Room newRoomScript = room.GetComponent<Room>();
        //neighbours
        Room leftRoomScript = GetRoomScriptAt(new Vector2Int(x - 1, y));
        Room rightRoomScript = GetRoomScriptAt(new Vector2Int(x + 1, y));
        Room upRoomScript = GetRoomScriptAt(new Vector2Int(x, y+1));
        Room downRoomScript = GetRoomScriptAt(new Vector2Int(x, y-1));
        //wich doors to open

        if(x > 0 && roomGrid[x-1,y] != 0)
        {
            //neighbour to the left
            newRoomScript.OpenDoor(Vector2Int.left);
            leftRoomScript.OpenDoor(Vector2Int.right);
        }
        if(x < gridSizeX - 1 && roomGrid[x+1, y] != 0)
        {
            //neighbour to the right
            newRoomScript.OpenDoor(Vector2Int.right);
            rightRoomScript.OpenDoor(Vector2Int.left);
        }
        if(y > 0 && roomGrid[x, y-1] != 0)
        {
            //neighbour down
            newRoomScript.OpenDoor(Vector2Int.down);
            downRoomScript.OpenDoor(Vector2Int.up);
        }
        if(y < gridSizeY - 1 && roomGrid[x, y+1] != 0)
        {
            //neighbour top
            newRoomScript.OpenDoor(Vector2Int.up);
            upRoomScript.OpenDoor(Vector2Int.down);

        }
    }

    Room GetRoomScriptAt(Vector2Int index)
    {
        GameObject roomObject = roomObjects.Find(r => r.GetComponent<Room>().RoomIndex == index);
        if(roomObject != null)
        {
            return roomObject.GetComponent<Room>();
        }
        else
        {
            return null;
        }
    }

    private int CountAdjacentRooms(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;
        int count = 0;
        if (x > 0 && roomGrid[x-1,y] != 0)
        {
            count++; // left neighbour
        }
        if (x < gridSizeX - 1 && roomGrid[x+1, y] != 0)
        {
            count++; //right neighbour
        }

        if (y > 0 &&  roomGrid[x,y-1] != 0)
        {
            count++; // down neighbour
        }

        if (y < gridSizeY - 1 && roomGrid[x, y + 1] != 0)
        {
            count++; // down neighbour
        }
        return count;
    }

    private void OnDrawGizmos()
    {
        Color gizmoColor = new Color(0, 1, 1, 0.05f);
        Gizmos.color = gizmoColor;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 position = GetPositionFromGridIndex(new Vector2Int(x, y));
                Gizmos.DrawWireCube(position, new Vector3(roomWidth, roomHeight, 1));
            }
        }
    }

    private void CheckDoors()
    {
        Room[] rooms = FindObjectsOfType<Room>();
        foreach(Room r in rooms)
        {
            OpenDoors(r.gameObject, r.RoomIndex.x,r.RoomIndex.y);
        }

    }

    private void PlacePlayer()
    {
        Room[] rooms = FindObjectsOfType<Room>();
        Vector3 boosRoomPosition = roomWithBoss.transform.position;
        Vector3 finalDoorRoomPosition = roomWithFinalDoor.transform.position;
        List<DoubleDistance> distances = new List<DoubleDistance>();
        foreach (Room room in rooms)
        {
            Vector3 roomPosition = room.transform.position;
            float distanceToBoss = Vector3.Distance(roomPosition, boosRoomPosition);
            float distanceToFinalDoor = Vector3.Distance(roomPosition, finalDoorRoomPosition);
            if(distanceToBoss !=0 && distanceToFinalDoor !=0 && CountAdjacentRooms(room.RoomIndex)<2)
            {
                float avgDistance  = (distanceToBoss*1.7f + distanceToFinalDoor)/2;
                DoubleDistance doubleDistance = new DoubleDistance(room.intRoomIndex, avgDistance);
                distances.Add(doubleDistance);
            }
        }
        int longestDistance = distances.OrderByDescending(i=>i.avgDistance).First().roomIndexInt;
        Room farestRoomFromBossAndFinalDoor = rooms.FirstOrDefault(r => r.intRoomIndex == longestDistance);
        Debug.Log(farestRoomFromBossAndFinalDoor.intRoomIndex);
        farestRoomFromBossAndFinalDoor.hasPlayer = true;
        List<GameObject> closedDoors = farestRoomFromBossAndFinalDoor.ReportClosedDoors();
        int randomNuber = Random.Range(0,closedDoors.Count()-1);
        if(SceneManager.GetActiveScene().name != "Level4" && SceneManager.GetActiveScene().name != "Level6")
        {
            GameObject doorToOpen = closedDoors.ElementAt(randomNuber);
            doorToOpen.GetComponent<SpriteRenderer>().color = new Color(152,152,152,0);
        }
        player.transform.position = farestRoomFromBossAndFinalDoor.transform.position;
        SaveSystem.Instance.playerData.HasKeyLevel = false;
    }

    private void SpawnKey()
    {
        if(levelName == "Level6")
        {
            int randomRoomIndex = Random.Range(0, roomCount);
            Room[] rooms = Resources.FindObjectsOfTypeAll<Room>();
            Room randomRoom = rooms.FirstOrDefault(r => r.intRoomIndex == randomRoomIndex);
            Transform pointInRoom = randomRoom.enemySpawnPoints.FirstOrDefault();
            Vector3 spawnPointToUse = pointInRoom.position;
            CollectableItem keyPrefab = Resources.Load<CollectableItem>("key");
            Instantiate(keyPrefab, spawnPointToUse, Quaternion.identity);
        }
    }
}
