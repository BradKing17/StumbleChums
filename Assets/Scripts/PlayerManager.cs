using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerMovement movement;
    public int score = 0;
    public int lives = 3;
    public PlayerType type;
    
    public void SpawnPlayer(Vector3 spawnPoint)
    {
        movement.transform.position = spawnPoint;
        movement.gameObject.SetActive(true);
    }

    public void DespawnPlayer()
    {
        movement.gameObject.SetActive(false);
    }
}
