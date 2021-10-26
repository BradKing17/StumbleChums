using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Neighbours
{


     public GameObject left;
     public GameObject right;
     public GameObject up;
    

}
public class TipToeTile : MonoBehaviour
{
    Color defaultColour;
    Color childDefaultColour;
    [HideInInspector]
    public TileManager tManager;
    SpriteRenderer sr;
    SpriteRenderer csr;
    public bool isInPath;
    public string tilePosition;
    public int x;
    public int y;

    [HideInInspector]
    public bool showPath;

    bool playerOnTile;
    bool changeColour;
    GameObject childTile;
    public Neighbours neighbours;


    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        childTile = transform.GetChild(0).gameObject;
        csr = childTile.GetComponent<SpriteRenderer>();


        defaultColour = sr.color;
        childDefaultColour = csr.color;

        float red = Random.Range(defaultColour.r - tManager.colourValue, defaultColour.r + tManager.colourValue);
        float green = Random.Range(defaultColour.g - tManager.colourValue, defaultColour.g + tManager.colourValue);
        float blue = Random.Range(defaultColour.b - tManager.colourValue, defaultColour.b + tManager.colourValue);

        sr.color = new Color(red,green,blue,1);
    }

    // Update is called once per frame
    void Update()
    {
        if (showPath)
        {
            if (isInPath)
            {
                sr.color = Color.white;
            }
            
        }

        if(changeColour)
        {
            csr.sortingOrder = -1;
            Color lerpedColour = Color.Lerp(sr.color, defaultColour, Time.deltaTime/tManager.colourChangeTime);
            Color childLerpedColour = Color.Lerp(csr.color, childDefaultColour, Time.deltaTime/tManager.colourChangeTime);
            sr.color = lerpedColour;
            csr.color = childLerpedColour;


            if(CompareColour(sr.color, defaultColour))
            {
                changeColour = false;
            }
        }

    }


    bool CompareColour(Color colour1, Color colour2)
    {
        return Mathf.Round(colour1.r * 100) == Mathf.Round(colour2.r * 100) && Mathf.Round(colour1.g * 100) == Mathf.Round(colour2.g * 100) && Mathf.Round(colour1.b * 100) == Mathf.Round(colour2.b * 100);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            if (!isInPath)
            {
                Destroy(gameObject);
            }
            else
            {
                playerOnTile = true;
                sr.color = Color.white;
                csr.color = new Color(158/100, 158/100, 158/100, 1);
                csr.sortingOrder = 0;
                
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && playerOnTile)
        {
            changeColour = true;
            playerOnTile = false;
            Debug.Log("left tile");
        }
    }
}
