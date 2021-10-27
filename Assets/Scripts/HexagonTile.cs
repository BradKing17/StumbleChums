using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexagonTile : MonoBehaviour
{

    public float timer;
    public GameObject secondState;
    public float timeTo2ndState;
    public float timeToDestroy;
    public bool startTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer)
        {
            timer += Time.deltaTime;
            if (timer >= timeTo2ndState)
            {
                GetComponent<SpriteRenderer>().color = Color.white;
            }

            if (timer >= timeTo2ndState + timeToDestroy)
            {
                Destroy(gameObject);
            }
        }

    }

    public void Colliding()
    {
        startTimer = true;
    }

    public void ExitedCollider()
    {
        if(timer < timeTo2ndState)
        {
            startTimer = false;
        }
    }



}
