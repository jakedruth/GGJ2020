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
            _isPaused = value;
            if (OnPause != null)
                OnPause.Invoke();
        }
    }

    public UnityEngine.Events.UnityAction OnPause;

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
    }

    public bool TogglePause()
    {
        return IsPaused = !IsPaused;
    }

    public bool SetPause(bool value)
    {
        return IsPaused = value;
    }
}

public struct GameData
{
}
