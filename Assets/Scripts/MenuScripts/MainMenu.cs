using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public void PlayGame()
  {
        GameManager gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        SceneManager.LoadScene(1);
        gameManager.sceneQueue.RemoveAt(0);
        
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }
}
