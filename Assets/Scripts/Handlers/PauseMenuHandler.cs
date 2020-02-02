﻿using System.Collections;
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
    public void ToggleMusic(string name)
    {
        SoundManager.instance.Play("Button");
        transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(!transform.GetChild(0).GetChild(0).GetChild(0).gameObject.activeInHierarchy);
        transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(!transform.GetChild(0).GetChild(0).GetChild(1).gameObject.activeInHierarchy);
        SoundManager.instance.Mute(name);
    }
    public void ToggleSFX(string name)
    {
        SoundManager.instance.Play("Button");
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
