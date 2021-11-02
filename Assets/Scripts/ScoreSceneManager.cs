using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreSceneManager : MonoBehaviour
{
    public List<Text> scores;

    private float timer = 0;
    private GameManager gameManager;
    private List<PlayerManager> players;
    
    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        players = gameManager.GetPlayers();
        for (int i = 0; i < players.Count; i++)
        {
            scores[i].color = players[i].type.color;
        }
    }
    
    private void Update()
    {
        timer += Time.deltaTime;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].score >= timer * 2.0F)
            {
                scores[i].text = players[i].score.ToString();
            }
        }

        if (timer > 5.0F)
        {
            SceneManager.LoadScene(gameManager.sceneQueue.Dequeue());
        }
    }
}
