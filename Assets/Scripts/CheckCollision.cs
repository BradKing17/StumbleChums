using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {

            transform.parent.GetComponent<HexagonTile>().Colliding();
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            transform.parent.GetComponent<HexagonTile>().ExitedCollider();
        }
    }
}

//&& timer < timeTo2ndState
