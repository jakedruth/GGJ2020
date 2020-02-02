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

        transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(GameManager.instance.data.musicIsMuted);
        transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(!GameManager.instance.data.musicIsMuted);
        
        transform.GetChild(0).GetChild(1).GetChild(1).gameObject.SetActive(GameManager.instance.data.sfxIsMuted);
        transform.GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(!GameManager.instance.data.sfxIsMuted);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            togglePause();
        }
    }
    public void ToggleMusic(string name)
    {
        SoundManager.instance.Play("Button");
        GameManager.instance.data.musicIsMuted = !GameManager.instance.data.musicIsMuted;

        transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(GameManager.instance.data.musicIsMuted);
        transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(!GameManager.instance.data.musicIsMuted);

        SoundManager.instance.Mute(name);
    }
    public void ToggleSFX()
    {
        SoundManager.instance.Play("Button");
        GameManager.instance.data.sfxIsMuted = !GameManager.instance.data.sfxIsMuted;

        transform.GetChild(0).GetChild(1).GetChild(1).gameObject.SetActive(GameManager.instance.data.sfxIsMuted);
        transform.GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(!GameManager.instance.data.sfxIsMuted);

        foreach (Sound s in SoundManager.instance.sounds)
        {
            if (s.name != "Background Music")
            {
                SoundManager.instance.Mute(s.name);
            }
        }
    }

    public void togglePause()
    {
        SoundManager.instance.Play("Button");
        GameManager.instance.TogglePause();
    }
    public void onTogglePause(bool value)
    {
        transform.GetChild(0).gameObject.SetActive(value);
    }
    public void ReturnHome()
    {
        SoundManager.instance.Play("Button");
        SceneManager.LoadScene("MainMenu");
    }
    public void RestartLevel()
    {
        SoundManager.instance.Play("Button");
        GameManager.instance.SetPause(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnDestroy()
    {
        GameManager.instance.OnPause -= onTogglePause;
    }
}
