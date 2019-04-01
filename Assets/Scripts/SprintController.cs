using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintController : MonoBehaviour
{

    GameObject gameController;
    Rigidbody rigidbody;

    int racePosition = 0;

    bool sprintSequenceStarted = false;
    //bool jumpReleased = false;
    //bool foulJump = false;
    bool lean = false;
    bool firstContact = false;
    bool pastFinishLine = false;

    float keyDownTime = 0.25f;
    float startTime = 0f;

    //public bool pastLongJumpLine = false;
    bool firstClicked = false;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController");
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetSprintInput();
        CheckPastLine();
    }

    private void GetSprintInput()
    {
        if (!sprintSequenceStarted) //if throw sequence isn't started get run up input and check for long press to start throw sequence
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
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
            gameController.GetComponent<GameController>().ProcessNewSprintScore(99f);
            rigidbody.drag = 3f;

            }

        }
    }
}