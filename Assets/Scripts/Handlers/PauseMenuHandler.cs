using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenuHandler : MonoBehaviour
{
    public GameObject pausePanel;
    // Start is called before the first frame update
    void Start()
    {
        pausePanel.SetActive(false);
        GameManager.instance.OnPause += onTogglePause;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            togglePause();
        }
    }

    public void togglePause()
    {
        GameManager.instance.TogglePause();
    }
    public void onTogglePause(bool value)
    {
        pausePanel.SetActive(value);
    }
    public void ReturnHome()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
