using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    public GameManager gameManagerPrefab;
    
    [Header("Gamemode Settings")] 
    public bool topDown = false;
    public int numLives = 1;
    public float gameTime = 60.0F;
    public float levelKillY = -6.0F;
    public List<Vector3> spawnPoints;
    
    [Header("Setup")]
    public Transform cameraManager;
    public Text timeText;
    [FormerlySerializedAs("livesTexts")] public List<Text> scoreTexts;
    
    private GameManager gameManager;
    private List<PlayerManager> players;
    public AudioClip playerDeathSound;
    public AudioSource audioSource;
    void Start()
    {
        var gameManagerObj = GameObject.FindGameObjectWithTag("Game Manager");
        if (gameManagerObj == null)
        {
            gameManager = Instantiate(gameManagerPrefab);
        }
        else
        {
            gameManager = gameManagerObj.GetComponent<GameManager>();
        }
        
        players = gameManager.GetPlayers();
        if (topDown)
        {
            cameraManager.Rotate(new Vector3(90,0,0));
        }

        int i = 0;
        foreach (PlayerManager player in players)
        {
            player.SpawnPlayer(spawnPoints[i]);
            player.lives = numLives;
            i++;
        }
    }

    private void Update()
    {
        int i = 0;
        int numPlayers = players.Count;
        foreach (PlayerManager player in players)
        {
            if (player.movement.transform.position.y < levelKillY)
            {
                audioSource.PlayOneShot(playerDeathSound);
                if (/*player.lives > 1*/true)
                {
                    player.movement.transform.position = spawnPoints[i % spawnPoints.Count];
                }
                else
                {
                    player.DespawnPlayer();
                }
                //player.score--;
                //player.lives--;
            }

            /*if (player.lives <= 0)
            {
                numPlayers--;
            }*/

            scoreTexts[i].text = player.score.ToString();
            scoreTexts[i].color = player.type.color;
            i++;
        }
        gameTime -= Time.deltaTime;
        timeText.text = gameTime.ToString("F0");
        if (gameTime <= 0.0F)
        {
            NextGame();
        }
    }

    private void NextGame()
    {
        foreach (PlayerManager player in players)
        {
            player.DespawnPlayer();
        }
        SceneManager.LoadScene(2);
    }

    private void OnDrawGizmosSelected()
    {
        foreach (Vector3 pos in spawnPoints)
        {
            Gizmos.DrawSphere(pos,0.25F);
        }
    }
}
