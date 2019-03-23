using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameController : MonoBehaviour
{
    float highScore = 0f;
    float lastScore = 0f;

    int currentSceneIndex;

    GameObject[] objects;

    GameObject player;
    GameObject spear;
    public bool followSpear = false;

    private void Awake()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("GameController"); //find all background music objects
        if (objects.Length > 1) //if more than one object destroy others
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Capsule");
        spear = GameObject.Find("Spear");
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            player = GameObject.Find("Capsule");
        }
        if(spear == null)
        {
            spear = GameObject.Find("Spear");
        }


        CheckDebugKeys();

    }

    
    void FixedUpdate()
    {
        if (!followSpear)
        {
            FollowPlayer();
        }
        else if (followSpear)
        {
            FollowSpear();
        }
    }

    private void FollowSpear()
    {
        
        if (spear == null)
        {
            spear = GameObject.Find("Spear");
        }
        Vector3 desiredPosition = new Vector3(spear.transform.position.x + 3f, spear.transform.position.y + 0.5f, 2f);
        Vector3 smoothedPosition = Vector3.Lerp(Camera.main.transform.position, desiredPosition, 0.3f);
        Camera.main.transform.position = smoothedPosition;
    }

    private void FollowPlayer()
    {
        if (player == null)
        {
            player = GameObject.Find("Capsule");
        }
        Vector3 desiredPosition = new Vector3(player.transform.position.x + 4.78023f, 3.21062f, 2.325678f);
        Vector3 smoothedPosition = Vector3.Lerp(Camera.main.transform.position, desiredPosition, 0.30f);
        Camera.main.transform.position = smoothedPosition;
    }

    public void ProcessNewScore(float newScore)
    {
        if(player.GetComponent<PlayerController>().pastLine == true)
        {
            GameObject.Find("LastScore").GetComponent<TextMeshProUGUI>().text = "Last: " + "Fault";
        }
        else
        {
            GameObject.Find("LastScore").GetComponent<TextMeshProUGUI>().text = "Last: " + newScore.ToString("0.00") + "m";
            if (newScore > highScore)
            {
                GameObject.Find("HighScore").GetComponent<TextMeshProUGUI>().text = "Best: " + newScore.ToString("0.00") + "m";
                highScore = newScore;
            }
        }
        

        Invoke("LoadCurrentLevel", 0.5f);
    }

    public void LoadCurrentLevel()
    {
        SceneManager.LoadScene(currentSceneIndex);
        followSpear = false;

    }

    private void CheckDebugKeys()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }


}
