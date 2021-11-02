using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour
{

    public GameObject background;
    // Start is called before the first frame update
    //public void OnSelect(BaseEventData eventData)
    //{
    //    GetComponent<Image>().color = Color.white;
    //}

    // Update is called once per frame
    void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == gameObject)
        {
            GetComponent<Image>().color = Color.black;
        }
        else
        {
            GetComponent<Image>().color = new Color(0,0,0,0);
        }
    }
}
