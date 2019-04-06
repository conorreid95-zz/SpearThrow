using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintController : MonoBehaviour
{

    GameObject gameController;
    Rigidbody rigidbody;

    int racePosition = 0;

    bool sprintSequenceStarted = false;
    bool lean = false;
    bool pastFinishLine = false;
    float startTime = 0f;

    //public bool pastLongJumpLine = false;
    bool firstClicked = false;



    float[] opponentsDistToFinishLine = new float[7];
    public GameObject[] opponents;



    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController");
        rigidbody = GetComponent<Rigidbody>();
        opponents = GameObject.FindGameObjectsWithTag("Opponent");
    }

    // Update is called once per frame
    void Update()
    {
        GetSprintInput();
        CheckPastLine();
        UpdateRacePosition();
    }

    private void GetSprintInput()
    {
        if (!sprintSequenceStarted) //if throw sequence isn't started get run up input and check for long press to start throw sequence
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                firstClicked = true;
                rigidbody.AddRelativeForce(Vector3.right * 4300f * Time.deltaTime);
                startTime = Time.time;
                if (lean)
                {
                    lean = false;
                }
                else
                {
                    lean = true;
                }
            }

            if (lean && firstClicked)
            {
                Quaternion _playerTargetLeftLean;
                _playerTargetLeftLean = Quaternion.Euler(-6f, 0f, 0f);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _playerTargetLeftLean, 260f * Time.deltaTime);
            }
            else if (!lean && firstClicked)
            {
                Quaternion _playerTargetRightLean;
                _playerTargetRightLean = Quaternion.Euler(6f, 0f, 0f);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _playerTargetRightLean, 260f * Time.deltaTime);
            }
        }
    }

    void CheckPastLine()
    {
        if (transform.position.x > 45f)
        {
            if (!pastFinishLine)
            {

            pastFinishLine = true;
            gameController.GetComponent<GameController>().ProcessNewSprintScore(racePosition);
            rigidbody.drag = 3f;

            }

        }
    }

    void UpdateRacePosition()
    {
        if (!pastFinishLine)
        {
            racePosition = 1;
            float playerXTransform = transform.position.x;
            for (int i = 0; i < opponents.Length; i++)
            {
                opponentsDistToFinishLine[i] = opponents[i].transform.position.x;
            }

            for (int i = 0; i < opponents.Length; i++)
            {
                if(playerXTransform < opponentsDistToFinishLine[i])
                {
                    racePosition++;
                }
            }

            gameController.GetComponent<GameController>().sprintCurrentPos = racePosition;


        }

    }


    
}