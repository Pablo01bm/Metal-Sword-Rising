using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonCreator : MonoBehaviour
{
    public int dungeonWidth, dungeonLength;
    public int roomWidthMin, roomlengthMin;
    public int maxIterations;
    public int corridorWidth;
    public Material material;
    [Range(0.0f, 0.3f)]
    public float roomBottomCornerModifier;
    [Range(0.7f, 1.0f)]
    public float roomTopCornerModifier;
    [Range(0, 2)]
    public int roomOffset;
    public GameObject wallVertical, wallHorizontal;
    public GameObject playerPrefab;
    public GameObject FinishGameObj;
    List<Vector3Int> possibleDoorVerticalPosition;
    List<Vector3Int> possibleDoorHorizontalPosition;
    List<Vector3Int> possibleWallHorizontalPosition;
    List<Vector3Int> possibleWallVerticalPosition;
    bool firstRoom = true;
    bool lastRoom = false;
    public int maxObjectsPerRoom;

    Vector3 playerPosition; 
    Vector3 farPositionFromPlayer;

    public GameObject MeleeEnemy;

    //Furniture
    public GameObject[] dungeonObjects;
    // Start is called before the first frame update
    void Start()
    {
        CreateDungeon();
    }

    public void CreateDungeon()
    {
        DestroyAllChildren();
        DungeonGenerator generator = new DungeonGenerator(dungeonWidth, dungeonLength);
        var listOfRooms = generator.CalculateDungeon(maxIterations, 
            roomWidthMin, 
            roomlengthMin, 
            roomBottomCornerModifier, 
            roomTopCornerModifier, 
            roomOffset,
            corridorWidth);

        GameObject wallParent = new GameObject("WallParent");
        wallParent.transform.parent = transform;
        possibleDoorVerticalPosition = new List<Vector3Int>();
        possibleDoorHorizontalPosition = new List<Vector3Int>();
        possibleWallHorizontalPosition = new List<Vector3Int>();
        possibleWallVerticalPosition = new List<Vector3Int>();

        for (int i = 0; i < listOfRooms.Count; i++)
        {
            CreateMesh(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner);
        }
        Instantiate(FinishGameObj, farPositionFromPlayer + new Vector3(0, 1f, 0), Quaternion.identity);
        createWalls(wallParent);
    }

    private void createWalls(GameObject wallParent)
    {
        foreach (var wallPosition in possibleWallHorizontalPosition)
        {
            createWallH(wallParent, wallPosition, ref wallHorizontal);
        }
        foreach (var wallPosition in possibleWallVerticalPosition)
        {
            createWallV(wallParent, wallPosition, ref wallVertical);
        }
    }

    private void createWallH(GameObject wallParent, Vector3Int wallPosition, ref GameObject wallPrefab)
    {
        Instantiate(wallPrefab, wallPosition, Quaternion.Euler(0, 90, 0), wallParent.transform);
    }

    private void createWallV(GameObject wallParent, Vector3Int wallPosition, ref GameObject wallPrefab)
    {
        Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);
    }

    private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0, topRightCorner.y);

        Vector3[] vertices = new Vector3[]
        {
            topLeftV,
            topRightV,
            bottomLeftV,
            bottomRightV
        };

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        { 
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
        {
            0,
            1,
            2,
            2,
            1,
            3
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        GameObject dungeonFloor = new GameObject("Mesh"+bottomLeftCorner, typeof(MeshFilter), typeof(MeshRenderer));

        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = material;
        dungeonFloor.layer = LayerMask.NameToLayer("whatIsGround"); //Own
        dungeonFloor.AddComponent<MeshCollider>().sharedMesh = mesh; //Own
        dungeonFloor.transform.parent = transform;

        for (int row = (int)bottomLeftV.x ; row < (int)bottomRightV.x; row++)
        {
            var wallPosition = new Vector3(row, 0, bottomLeftV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }

        for (int row = (int) topLeftV.x ; row < (int)topRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row, 0, topRightV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int col = (int)bottomLeftV.z; col < (int)topLeftV.z; col++)
        {
            var wallPosition = new Vector3(bottomLeftV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
        for (int col = (int)bottomRightV.z; col < (int)topRightV.z; col++)
        {
            var wallPosition = new Vector3(bottomRightV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }

        //Own Code, we place in the first room the player spawn point
        if (firstRoom)
        {
            firstRoom = false;
            Vector3 roomCenter = new Vector3((bottomLeftCorner.x + topRightCorner.x) / 2f, 0f, (bottomLeftCorner.y + topRightCorner.y) / 2f);
            Vector3 roomSize = new Vector3(topRightCorner.x - bottomLeftCorner.x, 0f, topRightCorner.y - bottomLeftCorner.y);
            Bounds roomBounds = new Bounds(roomCenter, roomSize);

            PlaceRespawnPoint(roomBounds);
            farPositionFromPlayer = roomCenter;
        }
        else 
        {

            Vector3 roomCenter = new Vector3((bottomLeftCorner.x + topRightCorner.x) / 2f, 0f, (bottomLeftCorner.y + topRightCorner.y) / 2f);
            Vector3 roomSize = new Vector3(topRightCorner.x - bottomLeftCorner.x, 0f, topRightCorner.y - bottomLeftCorner.y);
            Bounds roomBounds = new Bounds(roomCenter, roomSize);

            // Calculate the distance vector between playerPosition and roomCenter
            Vector3 distanceVector = playerPosition - roomCenter;

            // Calculate the magnitude (distance) of the distance vector
            float distance = distanceVector.magnitude;

            // Check if the distance is greater than the magnitude of farPositionFromPlayer
            if (distance > farPositionFromPlayer.magnitude)
            {
                print("Distance: " + distance);
                farPositionFromPlayer = roomCenter;
            }

            SpawnEnemies(MeleeEnemy, roomBounds, 5f, Random.Range(1, 5));
        }

        int instantiatedObjects = 0;

        for (int i = 0; i < dungeonObjects.Length; i++)
        {
            if (instantiatedObjects >= maxObjectsPerRoom)
            {
                break; // Limit reached, exit the loop
            }

            Vector3 objectPosition = GetRandomPosition(bottomLeftCorner, topRightCorner);

            // Check if the object position is valid
            if (IsValidPosition(objectPosition))
            {
                Vector3 spawnPosition = objectPosition + new Vector3(0f, 2f, 0f); // Offset the position by 2 units above
                GameObject instantiatedObject = Instantiate(dungeonObjects[i], spawnPosition, Quaternion.identity);
                instantiatedObject.transform.parent = dungeonFloor.transform;
                instantiatedObjects++;

            }
        }



    }
    //Own
    private Vector3 GetRandomPosition(Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        float x = Random.Range(bottomLeftCorner.x, topRightCorner.x);
        float z = Random.Range(bottomLeftCorner.y, topRightCorner.y);
        return new Vector3(x, 0f, z);
    }
    //Own
    private bool IsValidPosition(Vector3 position)
    {
        // Check if position is inside another object
        Collider[] colliders = Physics.OverlapSphere(position, 1f); // Adjust the radius as needed

        foreach (var collider in colliders)
        {
            // Skip if the collider belongs to an object on the "whatIsGround" layer
            if (collider.gameObject.layer == LayerMask.NameToLayer("whatIsGround"))
            {
                continue;
            }

            // Position is invalid if any collider overlaps
            return false;
        }

        // Check if position is at least 2 units away from other objects
        Collider[] nearbyColliders = Physics.OverlapSphere(position, 2f); // Adjust the radius as needed

        foreach (var collider in nearbyColliders)
        {
            // Skip if the collider belongs to an object on the "whatIsGround" layer
            if (collider.gameObject.layer == LayerMask.NameToLayer("whatIsGround"))
            {
                continue;
            }

            // Position is invalid if any collider is within the specified radius
            return false;
        }

        return true;
    }

    //Own Code, method which place the respawn point
    private void PlaceRespawnPoint(Bounds roomBounds)
    {
        // Generate a random position within the room bounds
        Vector3 respawnPosition = new Vector3(
            Random.Range(roomBounds.min.x, roomBounds.max.x),
            0f,
            Random.Range(roomBounds.min.z, roomBounds.max.z)
        );

        // Instantiate the respawn point object
        GameObject respawnPoint = new GameObject("Respawn Point");
        respawnPoint.transform.position = respawnPosition;
        

        // Add any necessary components to the respawn point object
        // For example, you might want to add a script to handle respawning the player
        PlayerRespawn respawnScript = respawnPoint.AddComponent<PlayerRespawn>();
        respawnScript.respawnPoint = respawnPoint.transform;
        playerPosition = respawnPoint.transform.position;
        respawnScript.playerPrefab = playerPrefab;
    }

    //Own code. Function which place enemies inside the room
    private void SpawnEnemies(GameObject enemyPrefab, Bounds roomBounds, float minDistanceBetweenEnemies, int numEnemies)
    {
        // Loop through the number of enemies to spawn
        for (int i = 0; i < numEnemies; i++)
        {
            // Generate a random position within the room bounds
            Vector3 enemyPosition = new Vector3(
                Random.Range(roomBounds.min.x, roomBounds.max.x),
                enemyPrefab.transform.position.y,
                Random.Range(roomBounds.min.z, roomBounds.max.z)
            );

            // Check if the enemy is too close to any previously spawned enemies
            bool isTooClose = false;
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if (Vector3.Distance(enemy.transform.position, enemyPosition) < minDistanceBetweenEnemies)
                {
                    isTooClose = true;
                    break;
                }
            }

            // If the enemy is too close to any previously spawned enemies, skip this iteration
            if (isTooClose)
            {
                continue;
            }

            // Instantiate the enemy object
            GameObject enemyObject = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);

            // Add any necessary components to the enemy object
            // For example, you might want to add an enemy AI script
            // or set its health and damage values
            // ...

            // Tag the enemy object with the "Enemy" tag
            enemyObject.tag = "Enemy";
        }
    }


    private void AddWallPositionToList(Vector3 wallPosition, List<Vector3Int> wallList, List<Vector3Int> doorList)
    {
        Vector3Int point = Vector3Int.CeilToInt(wallPosition);
        if (wallList.Contains(point))
        {
            doorList.Add(point);
            wallList.Remove(point);
        }
        else
        { 
            wallList.Add(point);   
        }
    }

    private void DestroyAllChildren()
    {
        while (transform.childCount != 0)
        {
            foreach (Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }
}
