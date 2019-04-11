using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(0f, -9.81f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadJavelinScene()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadLJScene()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadSprintScene()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadHurdlesScene()
    {
        SceneManager.LoadScene(4);
    }
}
