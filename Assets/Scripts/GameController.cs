using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameController : MonoBehaviour
{
    enum CurrentSport { None, Javelin, LongJump, Sprint, Hurdle }

    float highJavelinScore = 0f;
    float lastJavelinScore = 0f;
    float LJHighScore = 0f;
    float LJLastScore = 0f;


    int sprintHighScore = 0;
    int sprintLastScore = 0;
    public int sprintCurrentPos = 0;

    int hurdleHighScore = 0;
    int hurdleLastScore = 0;
    public int hurdleCurrentPos = 0;


    int javelinAttempts = 0;
    int longJumpAttempts = 0;
    int sprintAttempts = 0;
    int hurdleAttempts = 0;


    int currentSceneIndex;

    GameObject[] objects;

    GameObject player;
    GameObject spear;
    public bool followSpear = false;

    GameObject highScoreText;
    GameObject lastScoreText;

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

        highScoreText = GameObject.Find("HighScore");
        lastScoreText = GameObject.Find("LastScore");

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(currentSceneIndex == 1)
        {
            spear = GameObject.Find("Spear");
            currentSport = CurrentSport.Javelin;
        }
        else if(currentSceneIndex == 2)
        {
            currentSport = CurrentSport.LongJump;
        }
        else if (currentSceneIndex == 3)
        {
            currentSport = CurrentSport.Sprint;
        }
        else if (currentSceneIndex == 4)
        {
            currentSport = CurrentSport.Hurdle;
        }
        DontDestroyOnLoad(this);

        highJavelinScore = PlayerPrefs.GetFloat("JavelinHighScore", 0f);
        LJHighScore = PlayerPrefs.GetFloat("LongJumpHighScore", 0f);
        sprintHighScore = PlayerPrefs.GetInt("SprintHighScore", 0);
        hurdleHighScore = PlayerPrefs.GetInt("HurdlesHighScore", 0);
    }

    // Update is called once per frame
    void Update()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(currentSceneIndex == 1)
        {
            currentSport = CurrentSport.Javelin;
        }
        else if(currentSceneIndex == 2)
        {
            currentSport = CurrentSport.LongJump;
        }
        else if (currentSceneIndex == 3)
        {
            currentSport = CurrentSport.Sprint;
        }
        else if (currentSceneIndex == 4)
        {
            currentSport = CurrentSport.Hurdle;
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
        else if (currentSport == CurrentSport.Hurdle)
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
        else if (currentSport == CurrentSport.Hurdle)
        {
            FollowPlayer();
        }

        UpdateHighScore();

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
        else if (currentSport == CurrentSport.Hurdle)
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
            lastJavelinScore = 0f;
        }
        else
        {
            lastJavelinScore = newScore;
            if (lastJavelinScore > highJavelinScore)
            {
                highJavelinScore = newScore;
                PlayerPrefs.SetFloat("JavelinHighScore", highJavelinScore);
            }
        }
        
        if(javelinAttempts < 50)
        {
            Invoke("LoadCurrentLevel", 0.5f);
        }
        else
        {
            Invoke("LoadLJScene", 0.5f);
        }
        
    }

    public void ProcessNewLongJumpScore(float newScore)
    {
        LJLastScore = newScore;
        longJumpAttempts++;

        if (player.GetComponent<PlayerControllerLongJump>().pastLongJumpLine == true)
        {
            LJLastScore = 0f;
        }
        else
        {
            if (LJLastScore > LJHighScore)
            {
                LJHighScore = LJLastScore;
                PlayerPrefs.SetFloat("LongJumpHighScore", LJHighScore);
            }
        }


        if (longJumpAttempts < 50)
        {
            Invoke("LoadCurrentLevel", 0.5f);
        }
        else
        {
            Invoke("LoadSprintScene", 0.5f);
        }
    }

    public void ProcessNewSprintScore(int newPlace)
    {
        sprintLastScore = newPlace;
        sprintAttempts++;
        
        if (sprintLastScore < sprintHighScore || sprintHighScore == 0)
        {
            sprintHighScore = sprintLastScore;
            PlayerPrefs.SetInt("SprintHighScore", sprintHighScore);
        }

        if (sprintAttempts < 50)
        {
            Invoke("LoadCurrentLevel", 0.5f);
        }
        else
        {
            Invoke("LoadHurdleScene", 1f);
        }
    }

    public void ProcessNewHurdleScore(int newPlace)
    {
        hurdleLastScore = newPlace;
        hurdleAttempts++;
        
        if (hurdleLastScore < hurdleHighScore || hurdleHighScore == 0)
        {
            hurdleHighScore = hurdleLastScore;
            PlayerPrefs.SetInt("HurdlesHighScore", hurdleHighScore);
        }

        if (hurdleAttempts < 50)
        {
            Invoke("LoadCurrentLevel", 0.5f);
        }
        else
        {
            Invoke("LoadJavelinScene", 1f);
        }
    }

    public void LoadCurrentLevel()
    {

        followSpear = false;
        SceneManager.LoadScene(currentSceneIndex);

    }

    public void LoadJavelinScene()
    {

        followSpear = false;
        javelinAttempts = 0;
        longJumpAttempts = 0;
        Physics.gravity = new Vector3(0f, -9.81f, 0f);
        SceneManager.LoadScene(1);
    }
    public void LoadLJScene()
    {

        followSpear = false;
        javelinAttempts = 0;
        longJumpAttempts = 0;
        SceneManager.LoadScene(2);
    }

    

    public void LoadSprintScene()
    {

        followSpear = false;
        javelinAttempts = 0;
        longJumpAttempts = 0;
        sprintAttempts = 0;
        Physics.gravity = new Vector3(0f, -9.81f, 0f);
        SceneManager.LoadScene(3);
    }

    public void LoadHurdleScene()
    {

        followSpear = false;
        javelinAttempts = 0;
        longJumpAttempts = 0;
        sprintAttempts = 0;
        hurdleAttempts = 0;
        Physics.gravity = new Vector3(0f, -9.81f, 0f);
        SceneManager.LoadScene(4);
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
        if(currentSport == CurrentSport.Javelin)
        {
            highScoreText.GetComponent<TextMeshProUGUI>().text = "Best: " + highJavelinScore.ToString("0.00") + "m";
            lastScoreText.GetComponent<TextMeshProUGUI>().text = "Last: " + lastJavelinScore.ToString("0.00") + "m";
        }
        else if(currentSport == CurrentSport.LongJump)
        {
            highScoreText.GetComponent<TextMeshProUGUI>().text = "Best: " + LJHighScore.ToString("0.00") + "m";
            lastScoreText.GetComponent<TextMeshProUGUI>().text = "Last: " + LJLastScore.ToString("0.00") + "m";
        }
        else if (currentSport == CurrentSport.Sprint)
        {
            highScoreText.GetComponent<TextMeshProUGUI>().text = "Best: " + AddOrdinal(sprintHighScore);
            lastScoreText.GetComponent<TextMeshProUGUI>().text = "Pos: " + AddOrdinal(sprintCurrentPos);
        }
        else if (currentSport == CurrentSport.Hurdle)
        {
            highScoreText.GetComponent<TextMeshProUGUI>().text = "Best: " + AddOrdinal(hurdleHighScore);
            lastScoreText.GetComponent<TextMeshProUGUI>().text = "Pos: " + AddOrdinal(hurdleCurrentPos);
        }
    }


    public static string AddOrdinal(int num)
    {
        if (num <= 0) return num.ToString();

        switch (num % 100)
        {
            case 11:
            case 12:
            case 13:
                return num + "th";
        }

        switch (num % 10)
        {
            case 1:
                return num + "st";
            case 2:
                return num + "nd";
            case 3:
                return num + "rd";
            default:
                return num + "th";
        }

    }


}
