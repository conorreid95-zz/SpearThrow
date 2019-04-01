using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameController : MonoBehaviour
{
    enum CurrentSport { None, Javelin, LongJump, Sprint}

    float highJavelinScore = 0f;
    float lastJavelinScore = 0f;
    float LJHighScore = 0f;
    float LJLastScore = 0f;
    float sprintHighScore = 0f;
    float sprintLastScore = 0f;


    int javelinAttempts = 0;
    int longJumpAttempts = 0;
    int sprintAttempts = 0;


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
        else if (currentSceneIndex == 2)
        {
            currentSport = CurrentSport.Sprint;
        }
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(currentSceneIndex == 0)
        {
            currentSport = CurrentSport.Javelin;
        }
        else if(currentSceneIndex == 1)
        {
            currentSport = CurrentSport.LongJump;
        }
        else if (currentSceneIndex == 2)
        {
            currentSport = CurrentSport.Sprint;
        }

        if (currentSport == CurrentSport.Javelin)
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
        else if (currentSport == CurrentSport.Sprint)
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
        else if (currentSport == CurrentSport.Sprint)
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
            Vector3 desiredPositionLongJump = new Vector3( -77f, player.transform.position.y + 2.5f, player.transform.position.z+5.12f);
            Vector3 smoothedPositionLongJump = Vector3.Lerp(Camera.main.transform.position, desiredPositionLongJump, 0.40f);
            Camera.main.transform.position = smoothedPositionLongJump;
        }
        else if (currentSport == CurrentSport.Sprint)
        {
            Vector3 desiredPositionSprint = new Vector3(player.transform.position.x - 5f, player.transform.position.y + 3f, -46.782f);
            Vector3 smoothedPositionSprint = Vector3.Lerp(Camera.main.transform.position, desiredPositionSprint, 0.40f);
            Camera.main.transform.position = smoothedPositionSprint;
        }

    }

    public void ProcessNewJavelinScore(float newScore)
    {
        javelinAttempts++;
        if(player.GetComponent<PlayerController>().pastLine == true)
        {
            GameObject.Find("LastScore").GetComponent<TextMeshProUGUI>().text = "Last: " + "Fault";
        }
        else
        {
            lastJavelinScore = newScore;
            GameObject.Find("LastScore").GetComponent<TextMeshProUGUI>().text = "Last: " + lastJavelinScore.ToString("0.00") + "m";
            if (lastJavelinScore > highJavelinScore)
            {
                GameObject.Find("HighScore").GetComponent<TextMeshProUGUI>().text = "Best: " + newScore.ToString("0.00") + "m";
                highJavelinScore = newScore;
            }
        }
        
        if(javelinAttempts < 3)
        {
            Invoke("LoadCurrentLevel", 0.5f);
        }
        else
        {
            Invoke("LoadScene1", 0.5f);
        }
        
    }

    public void ProcessNewLongJumpScore(float newScore)
    {
        LJLastScore = newScore;
        longJumpAttempts++;
        if (player.GetComponent<PlayerControllerLongJump>().pastLongJumpLine == true)
        {
            GameObject.Find("LastScore").GetComponent<TextMeshProUGUI>().text = "Last: " + "Fault";
        }
        else
        {
            GameObject.Find("LastScore").GetComponent<TextMeshProUGUI>().text = "Last: " + LJLastScore.ToString("0.00") + "m";
            if (LJLastScore > LJHighScore)
            {
                GameObject.Find("HighScore").GetComponent<TextMeshProUGUI>().text = "Best: " + LJLastScore.ToString("0.00") + "m";
                LJHighScore = LJLastScore;
            }
        }


        if (longJumpAttempts < 3)
        {
            Invoke("LoadCurrentLevel", 0.5f);
        }
        else
        {
            Invoke("LoadScene0", 0.5f);
        }
    }

    public void ProcessNewSprintScore(float newScore)
    {
        sprintLastScore = newScore;
        sprintAttempts++;

        GameObject.Find("LastScore").GetComponent<TextMeshProUGUI>().text = "Last: " + sprintLastScore.ToString("0.00") + " sec";
        if (sprintLastScore > sprintHighScore)
        {
            GameObject.Find("HighScore").GetComponent<TextMeshProUGUI>().text = "Best: " + sprintLastScore.ToString("0.00") + "sec";
            sprintHighScore = sprintLastScore;
        }

        if (sprintAttempts < 3)
        {
            Invoke("LoadCurrentLevel", 0.5f);
        }
        else
        {
            Invoke("LoadScene0", 1f);
        }
    }

    public void LoadCurrentLevel()
    {

        followSpear = false;
        SceneManager.LoadScene(currentSceneIndex);

    }

    public void LoadScene1()
    {

        followSpear = false;
        javelinAttempts = 0;
        longJumpAttempts = 0;
        SceneManager.LoadScene(1);
        UpdateHighScore();
    }

    public void LoadScene0()
    {

        followSpear = false;
        javelinAttempts = 0;
        longJumpAttempts = 0;
        Physics.gravity = new Vector3(0f, -9.81f, 0f);
        SceneManager.LoadScene(0);
        UpdateHighScore();
    }

    public void LoadScene2()
    {

        followSpear = false;
        javelinAttempts = 0;
        longJumpAttempts = 0;
        sprintAttempts = 0;
        Physics.gravity = new Vector3(0f, -9.81f, 0f);
        SceneManager.LoadScene(2);
        UpdateHighScore();
    }

    private void CheckDebugKeys()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    public void UpdateHighScore()
    {
        if(currentSport == CurrentSport.LongJump)
        {
            GameObject.Find("HighScore").GetComponent<TextMeshProUGUI>().text = "Best: " + highJavelinScore.ToString("0.00") + "m";
            GameObject.Find("LastScore").GetComponent<TextMeshProUGUI>().text = "Last: " + "0.00m";
        }
        else if(currentSport == CurrentSport.Javelin)
        {
            GameObject.Find("HighScore").GetComponent<TextMeshProUGUI>().text = "Best: " + LJHighScore.ToString("0.00") + "m";
            GameObject.Find("LastScore").GetComponent<TextMeshProUGUI>().text = "Last: " + "0.00m";
        }
    }


}
