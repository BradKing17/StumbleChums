using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

    public enum Direction {UP, DOWN, LEFT, RIGHT, FAIL}


    public GameObject[,] tiles;
    public GameObject tilePrefab;
    public float colourValue;
    public float colourChangeTime;
    public int pathIndex = 0;
    public bool showPath;
    // Start is called before the first frame update
    void Start()
    {
        tiles = new GameObject[6,6];

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                tiles[i, j] = Instantiate(tilePrefab, new Vector3(0, 0, 0), transform.rotation);
                tiles[i, j].GetComponent<TipToeTile>().tManager = this;
                tiles[i, j].transform.SetParent(transform);
                tiles[i, j].transform.localPosition = new Vector3(i * 2.15f, j * 2.15f, 0);
                tiles[i, j].GetComponent<TipToeTile>().x = i;
                tiles[i, j].GetComponent<TipToeTile>().y = j;
                tiles[i, j].GetComponent<TipToeTile>().tilePosition = "(" + i.ToString() + ", " + j.ToString() + ")";
                tiles[i, j].GetComponent<TipToeTile>().showPath = showPath;

                
            }
        }


        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                SetTileNeighbours(i, j);
            }
        }

        GameObject startTile = tiles[Random.Range(0, 6), 0];
        startTile.GetComponent<TipToeTile>().isInPath = true;
        int x = startTile.GetComponent<TipToeTile>().x;
        int y = 0;

        while(y < 5)
        {
            if (CreatePath(tiles[x, y]) == Direction.RIGHT)
            {
                x += 1;
                tiles[x, y].GetComponent<TipToeTile>().neighbours.left = null;
            }
            else if (CreatePath(tiles[x, y]) == Direction.LEFT)
            {
                x -= 1;
                tiles[x, y].GetComponent<TipToeTile>().neighbours.right = null;
            }
            else if (CreatePath(tiles[x, y]) == Direction.UP)
            {
                y += 1;
            }
            tiles[x, y].GetComponent<TipToeTile>().isInPath = true;
        }


    }

    Direction CreatePath(GameObject tile)
    {
        List<Direction> directions = new List<Direction>();
        if(tile.GetComponent<TipToeTile>().neighbours.right != null)
        {
            directions.Add(Direction.RIGHT);
        }
        if (tile.GetComponent<TipToeTile>().neighbours.left != null)
        {
            directions.Add(Direction.LEFT);
        }
        if (tile.GetComponent<TipToeTile>().neighbours.up != null)
        {
            directions.Add(Direction.UP);
        }

        int randomNumber = Random.Range(0, directions.Count);
        

        if (directions[randomNumber] == Direction.RIGHT) 
        {
            return Direction.RIGHT;
        }
        if (directions[randomNumber] == Direction.LEFT)
        {
            return Direction.LEFT;
        }
        if (directions[randomNumber] == Direction.UP)
        {
            return Direction.UP;
        }
        Debug.Log("here2");
        return Direction.FAIL;
    }

    void SetTileNeighbours(int i, int j)
    {

        if(i == 0 && j == 0)
        {
            tiles[i, j].GetComponent<TipToeTile>().neighbours.right = tiles[i + 1, j];
            tiles[i, j].GetComponent<TipToeTile>().neighbours.up = tiles[i, j + 1];
        }

        if(i == 5 && j == 5)
        {
            tiles[i, j].GetComponent<TipToeTile>().neighbours.left = tiles[i - 1, j];
        }

        if(i == 0 && j == 5)
        {
            tiles[i, j].GetComponent<TipToeTile>().neighbours.right = tiles[i + 1, j];
        }

        if(i == 5 && j == 0)
        {
            tiles[i, j].GetComponent<TipToeTile>().neighbours.left = tiles[i - 1, j];
            tiles[i, j].GetComponent<TipToeTile>().neighbours.up = tiles[i, j + 1];
        }

        if(i == 0 && j > 0 && j < 5)
        {
            tiles[i, j].GetComponent<TipToeTile>().neighbours.right = tiles[i+1,j];
            tiles[i, j].GetComponent<TipToeTile>().neighbours.up = tiles[i, j + 1];
        }
        else if(i == 5 && j > 0 && j < 5)
        {
            tiles[i, j].GetComponent<TipToeTile>().neighbours.left = tiles[i - 1, j];
            tiles[i, j].GetComponent<TipToeTile>().neighbours.up = tiles[i, j + 1];
        }
        
        if(j == 0 && i > 0 && i < 5)
        {
            tiles[i, j].GetComponent<TipToeTile>().neighbours.right = tiles[i + 1, j];
            tiles[i, j].GetComponent<TipToeTile>().neighbours.up = tiles[i, j + 1];
            tiles[i, j].GetComponent<TipToeTile>().neighbours.left = tiles[i - 1, j];
        }
        else if(j == 5 && i > 0 && i < 5)
        {
            tiles[i, j].GetComponent<TipToeTile>().neighbours.right = tiles[i + 1, j];
            tiles[i, j].GetComponent<TipToeTile>().neighbours.left = tiles[i-1, j];
        }
        
        if(i != 0 && i != 5 && j != 0 && j != 5)
        {
            
            tiles[i, j].GetComponent<TipToeTile>().neighbours.right = tiles[i + 1, j];
            tiles[i, j].GetComponent<TipToeTile>().neighbours.left = tiles[i - 1, j];
            tiles[i, j].GetComponent<TipToeTile>().neighbours.up = tiles[i, j + 1];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
