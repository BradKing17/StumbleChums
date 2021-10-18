using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexagonTile : MonoBehaviour
{

    float timer;
    public Sprite secondState;
    public float timeTo2ndState;
    public float timeToDestroy;
    bool startTimer;

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
                GetComponent<SpriteRenderer>().sprite = secondState;
            }

            if (timer >= timeTo2ndState + timeToDestroy)
            {
                Destroy(gameObject);
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startTimer = true;
        }
    }

}
