using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHandler : MonoBehaviour
{
    public List<AnimalController> animalList;
    // Start is called before the first frame update
    void Start()
    {
        foreach (AnimalController animal in FindObjectsOfType<AnimalController>())
        {
            animalList.Add(animal);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (animalList.Count != 0)
        {
            for (int i = animalList.Count - 1; i >= 0; i--)
            {
                if (animalList[i] == null)
                {
                    animalList.Remove(animalList[i]);
                }
            }
        }
        else
        {
            if (GameManager.instance.data.levelsBeaten <= SceneManager.GetActiveScene().buildIndex - 2)
            {
                GameManager.instance.data.levelsBeaten = SceneManager.GetActiveScene().buildIndex - 1;
            }

            EmoteSystemManager.instance.CreateEmote(FindObjectOfType<PlayerController>().transform, "faceHappy");
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }

    private IEnumerator LoadScene(int index)
    {
        yield return new WaitForSeconds(3f);

        if (index < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(index);
        else
            Debug.Log("Can't load next level");
    }
}
