using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    public float health = 100f ;
    public float damage = 40f;
    public GameObject ragdollGameObject;
    public GameObject camera;
    private bool oneTime = false;


    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.setMaxHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 && !oneTime ) 
        {
            oneTime = true;
            Ragdoll ragdollScript = ragdollGameObject.GetComponent<Ragdoll>();
            ragdollScript.playerDead();
            PlayerMovement playerMovementScript = gameObject.GetComponent<PlayerMovement>();
            //We make the player can't move or jump anymore
            playerMovementScript.MoveSpeed = 0;
            playerMovementScript.jumpForce = 0;

            ThirdPersonCam camScript = camera.GetComponent<ThirdPersonCam>();
            camScript.enabled = false;


        }
    }


    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("weapon"))
        {
            health = health - 20f;
            healthBar.setHealth(health);
            print("Damage");
        }

    }
}
