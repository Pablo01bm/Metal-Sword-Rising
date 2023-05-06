using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Transform respawnPoint;
    public GameObject playerPrefab;

    private GameObject playerInstance;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RespawnPlayer();
        }
    }

    void SpawnPlayer()
    {
        playerInstance = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
    }

    void RespawnPlayer()
    {
        Destroy(playerInstance);

        // Get a new random position within the respawn square
        Vector3 newPosition = new Vector3(
            Random.Range(respawnPoint.position.x - 2f, respawnPoint.position.x + 2f),
            respawnPoint.position.y,
            Random.Range(respawnPoint.position.z - 2f, respawnPoint.position.z + 2f)
        );

        // Respawn the player at the new position
        playerInstance = Instantiate(playerPrefab, newPosition, Quaternion.identity);
    }
}
