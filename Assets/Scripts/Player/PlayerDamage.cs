using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public int health = 100;
    public PlayerMovement playerMove;
    public PickUpSystem pickUpSystem;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            playerMove.enabled = false;
            
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        playerMove.enabled = false;
    }

    private void OnGUI()
    {
        if (health <= 0)
        {
            GUI.Label(new Rect(10, 10, 100, 20), "Health: 0, Game Over!");
        }
        else
        {
            GUI.Label(new Rect(10, 10, 100, 20), "Health: " + health);

        }
    }
}