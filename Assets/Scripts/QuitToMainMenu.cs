using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitToMainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void QuitToMenu()
    {
        Destroy(GameObject.Find("GameController"));
        Destroy(GameObject.Find("Canvas"));
        SceneManager.LoadScene(0);
    }
}
