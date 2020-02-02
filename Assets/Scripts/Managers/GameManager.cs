using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameData data;

    private bool _isPaused;
    public bool IsPaused
    {
        get { return _isPaused; }
        set
        {
            if (_isPaused == value)
                return;

            _isPaused = value;
            if (OnPause != null)
                OnPause.Invoke(_isPaused);
        }
    }

    public UnityEngine.Events.UnityAction<bool> OnPause;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        data = new GameData();
    }

    public bool TogglePause()
    {
        return IsPaused = !IsPaused;
    }

    public bool SetPause(bool value)
    {
        return IsPaused = value;
    }

    public void QuitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }


}
