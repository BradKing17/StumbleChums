using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
}
public class GameManager : MonoBehaviour
{
    [Header("Gamemode Settings")]
    public bool topDown;

    public bool doPlayersRespawn = true;
    public float levelKillY = -6.0F;

    [Header("Player Settings")]
    public GameObject playerPrefab;
    public PlayerType[] playerTypes;
    private List<GameObject> players = new List<GameObject>();

    [Header("Manager Settings")] 
    public Transform chumParent;
    public Transform cameraManager;
    
    // Start is called before the first frame update
    void Start()
    {
        if (topDown)
        {
            cameraManager.Rotate(new Vector3(90,0,0));
        }
    }

    private void Update()
    {
        foreach (GameObject player in players)
        {
            if (player.activeSelf && player.transform.position.y < levelKillY)
            {
                if (doPlayersRespawn)
                {
                    player.transform.position = Vector3.zero;
                    continue;
                }
                player.SetActive(false);
            }
        }
    }

    public void OnPlayerJoined(PlayerInput input)
    {
        int i = players.Count;
        GameObject thisGameObject = input.gameObject;
        PlayerType thisPlayerType = playerTypes[i];
        thisGameObject.name = thisPlayerType.name + " Chum (Player " + (i+1) + ")";
        thisGameObject.transform.parent = chumParent;
        players.Add(thisGameObject);
        PlayerMovement thisPlayer = thisGameObject.GetComponent<PlayerMovement>();
        thisPlayer.SetPlayerType(playerTypes[i]);
        thisPlayer.SetTopDown(topDown);
    }
}
