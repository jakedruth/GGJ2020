using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHandler : MonoBehaviour
{
    public static LevelHandler current;
    public int TotalAnimals { get; private set; }
    public int SavedAnimals { get; private set; }
    public int RemaingAnimals { get { return TotalAnimals - SavedAnimals; } }

    private bool loadingNextScene = false;

    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        TotalAnimals = FindObjectsOfType<AnimalController>().Length;
        SavedAnimals = 0;
    }

    public void SaveAnimal(int amount)
    {
        SavedAnimals += amount;
        if(RemaingAnimals == 0)
        {
            if (!loadingNextScene)
            {
                loadingNextScene = true;
                if (GameManager.instance.data.levelsBeaten <= SceneManager.GetActiveScene().buildIndex - 2)
                {
                    GameManager.instance.data.levelsBeaten = SceneManager.GetActiveScene().buildIndex - 1;
                }

                StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
            }
        }
    }

    private IEnumerator LoadScene(int index)
    {
        EmoteSystemManager.instance.CreateEmote(FindObjectOfType<PlayerController>().transform, "faceHappy", 2f);
        
        yield return new WaitForSeconds(3f);

        if (index < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(index);
        else
            Debug.Log("Can't load next level");
    }
}
