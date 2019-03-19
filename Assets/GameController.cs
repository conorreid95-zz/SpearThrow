using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


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
        Vector3 desiredPosition = new Vector3(spear.transform.position.x + 4.78023f, spear.transform.position.y, 2.325678f);
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
        
        GameObject.Find("LastScore").GetComponent<TextMeshProUGUI>().text = "Last Score: " + newScore.ToString("0.00");
        if (newScore > highScore)
        {
            GameObject.Find("HighScore").GetComponent<TextMeshProUGUI>().text = "High Score: " + newScore.ToString("0.00");
            highScore = newScore;
        }

        Invoke("LoadCurrentLevel", 0.5f);
    }

    public void LoadCurrentLevel()
    {
        SceneManager.LoadScene(currentSceneIndex);
        followSpear = false;

    }

}
