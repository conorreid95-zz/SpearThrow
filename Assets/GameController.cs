using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameController : MonoBehaviour
{
    enum CurrentSport { None, Javelin, LongJump}

    float highScore = 0f;
    float lastScore = 0f;

    int currentSceneIndex;

    GameObject[] objects;

    GameObject player;
    GameObject spear;
    public bool followSpear = false;

    CurrentSport currentSport = CurrentSport.None;
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
        if(currentSceneIndex == 0)
        {
            currentSport = CurrentSport.Javelin;
        }
        else if(currentSceneIndex == 1)
        {
            currentSport = CurrentSport.LongJump;
        }
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentSport == CurrentSport.Javelin)
        {
            if (player == null)
            {
                player = GameObject.Find("Capsule");
            }
            if (spear == null)
            {
                spear = GameObject.Find("Spear");
            }
        }
        else if(currentSport == CurrentSport.LongJump)
        {
            if (player == null)
            {
                player = GameObject.Find("Capsule");
            }
        }
        
        CheckDebugKeys();

    }

    
    void FixedUpdate()
    {
        if(currentSport == CurrentSport.Javelin)
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
        else if(currentSport == CurrentSport.LongJump)
        {
            FollowPlayer();
        }
        
    }

    private void FollowSpear()
    {
        
        if (spear == null)
        {
            spear = GameObject.Find("Spear");
        }
        Vector3 desiredPosition = new Vector3(spear.transform.position.x + 3.4f, spear.transform.position.y + 0.6f, 1.6f);
        Vector3 smoothedPosition = Vector3.Lerp(Camera.main.transform.position, desiredPosition, 0.3f);
        Camera.main.transform.position = smoothedPosition;
    }

    private void FollowPlayer()
    {
        if (player == null)
        {
            player = GameObject.Find("Capsule");
        }
        if(currentSport == CurrentSport.Javelin)
        {
            Vector3 desiredPosition = new Vector3(player.transform.position.x + 5.4f, 3.21062f, 1.39f);
            Vector3 smoothedPosition = Vector3.Lerp(Camera.main.transform.position, desiredPosition, 0.30f);
            Camera.main.transform.position = smoothedPosition;
        }
        else if (currentSport == CurrentSport.LongJump)
        {
            Vector3 desiredPositionLongJump = new Vector3( -77.27f, 3.258f, player.transform.position.z+5.12f);
            Vector3 smoothedPositionLongJump = Vector3.Lerp(Camera.main.transform.position, desiredPositionLongJump, 0.70f);
            Camera.main.transform.position = smoothedPositionLongJump;
        }
        
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
