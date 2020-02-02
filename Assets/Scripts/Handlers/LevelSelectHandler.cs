using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelSelectHandler : MonoBehaviour
{
    public Transform levelPanel;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < levelPanel.childCount; i++)
        {
            levelPanel.GetChild(i).GetComponent<Button>().interactable = (i <= GameManager.instance.data.levelsBeaten);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SelectLevel(int level)
    {
        SoundManager.instance.Play("Button");
        SceneManager.LoadScene(level + 1);
        GameManager.instance.SetPause(false);
    }
    public void ReturnHome()
    {
        SoundManager.instance.Play("Button");
        SceneManager.LoadScene("MainMenu");
    }
}
