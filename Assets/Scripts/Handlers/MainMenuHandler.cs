using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    public void playGame()
    {
        SceneManager.LoadScene("LevelSelectMenu");
    }
    public void closeGame()
    {
        GameManager.instance.QuitGame();
    }
}
