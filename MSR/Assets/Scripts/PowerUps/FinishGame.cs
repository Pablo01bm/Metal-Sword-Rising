using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : MonoBehaviour
{

    public GameObject pickupEffect;
    private AttributesControler atributesScript;
    private GameObject player;
    // Start is called before the first frame update
    private void Start()
    {
        GameObject aux = GameObject.Find("GameManager");
        atributesScript = aux.GetComponent<AttributesControler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            Pickup();
            FindObjectOfType<AudioManager>().Stop();
            FindObjectOfType<AudioManager>().Play("WinMusic");
        }
    }

    void Pickup()
    {
        //Pickup visual effect
        GameObject instatiatedGameObject = Instantiate(pickupEffect, player.transform.position + new Vector3(0, 1.2f, 0), player.transform.rotation, player.transform);

        //Apply the effect
        atributesScript.EndGame();

        // Remove power up object
        Destroy(gameObject);
        Destroy(instatiatedGameObject, 2f);
    }
}
