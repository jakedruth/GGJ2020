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
        SoundManager.instance.Play("Button");
        SceneManager.LoadScene("LevelSelectMenu");
    }
    public void closeGame()
    {
        SoundManager.instance.Play("Button");
        GameManager.instance.QuitGame();
    }
}
