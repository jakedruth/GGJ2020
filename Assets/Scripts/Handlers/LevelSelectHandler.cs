using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelSelectHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SelectLevel(string level)
    {
        SceneManager.LoadScene(level);
    }
    public void ReturnHome()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
