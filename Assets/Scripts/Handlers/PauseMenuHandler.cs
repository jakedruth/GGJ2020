using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenuHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
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
        transform.GetChild(0).gameObject.SetActive(value);
    }
    public void ReturnHome()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void RestartLevel()
    {
        GameManager.instance.SetPause(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnDestroy()
    {
        GameManager.instance.OnPause -= onTogglePause;
    }
}
