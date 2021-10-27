using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonTileManager : MonoBehaviour
{
    public GameObject[] tiles;
    public float timeUntilWhite;
    [Header("The time to destroy after turning white")]
    public float timeToDestroy;

    // Start is called before the first frame update
    void Start()
    {
        tiles = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            tiles[i] = transform.GetChild(i).gameObject;
            tiles[i].GetComponent<HexagonTile>().timeTo2ndState = timeUntilWhite;
            tiles[i].GetComponent<HexagonTile>().timeToDestroy = timeToDestroy;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
