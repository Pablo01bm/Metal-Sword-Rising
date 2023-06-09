using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributesControler : MonoBehaviour
{
    public float playerDamage;
    public float playerDamageMultiplier=1;
    public float playerHealth;
    public float playerSpeed = 1;
    public float meleeEnemyDamage = 1f;
    public float meleeEnemyDamageMultiplier = 1f;
    

    public void damagePowerUp()
    {
        playerDamageMultiplier = playerDamageMultiplier + 0.1f;
        playerDamage = 10f * playerDamageMultiplier;
        Debug.Log("Current Player Damage: " + playerDamage);
    }

    public void healPowerUp()
    {
        playerHealth = playerHealth + 20;
        if (playerHealth > 100) 
        {
            playerHealth = 100;
        }
        Debug.Log("Current Player Health: " + playerHealth);
    }



}
