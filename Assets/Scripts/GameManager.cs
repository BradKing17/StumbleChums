using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerType
{
    [System.Serializable]
    public class PlayerSprites
    {
        public Sprite front;
        public Sprite left;
        public Sprite right;
        public Sprite back;
    }
    public string name;
    public PlayerSprites sprites;
    public Color color;
}
public class GameManager : MonoBehaviour
{
    [Header("Player Settings")]
    public PlayerManager playerPrefab;
    public PlayerType[] playerTypes;
    private List<PlayerManager> players = new List<PlayerManager>();

    [Header("Manager Settings")] 
    public Transform chumParent;

    public List<int> sceneQueue;
    
    // Start is called before the first frame update
    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void OnPlayerJoined(PlayerInput input)
    {
        int i = players.Count;
        PlayerManager thisPlayer = input.GetComponent<PlayerManager>();
        PlayerType thisPlayerType = playerTypes[i];
        thisPlayer.name = thisPlayerType.name + " Chum (Player " + (i+1) + ")";
        thisPlayer.transform.parent = chumParent;
        thisPlayer.type = thisPlayerType;
        players.Add(thisPlayer);
    }

    public List<PlayerManager> GetPlayers()
    {
        return players;
    }
}
