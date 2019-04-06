using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurdleController : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject gameController;
    Rigidbody rigidbody;

    int racePosition = 0;

    bool readyToJump = false;
    bool lean = false;
    bool pastFinishLine = false;
    bool jumping = false;

    float keyDownTime = 0.4f;
    float startTime = 0f;
    
    bool firstClicked = false;
    bool airborne = false;

    float[] hurdleOpponentsDistToFinishLine = new float[7];
    public GameObject[] hurdleOpponents;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController");
        rigidbody = GetComponent<Rigidbody>();
        hurdleOpponents = GameObject.FindGameObjectsWithTag("Opponent");
    }

    // Update is called once per frame
    void Update()
    {
        GetSprintInput();
        CheckPastLine();
        CheckAirborne();
        UpdateRacePosition();
    }



    private void GetSprintInput()
    {
        if (!airborne)
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

            if (Input.GetKey(KeyCode.Mouse0))
            {
                if ((startTime + keyDownTime) < Time.time) //check if held long enough
                {
                    print("Held for 0.25 second");
                    print("Start Time: " + startTime.ToString());

                    print("Time: " + Time.time.ToString());
                    readyToJump = true;
                    transform.localScale = new Vector3(1f, 0.9f, 1f);
                }
                else
                {
                    readyToJump = false;
                }
            }
        }

        if (readyToJump && !airborne && !jumping)
        {
            if (Input.GetKeyUp(KeyCode.Mouse0)) //if throw is released
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                rigidbody.AddRelativeForce(Vector3.up * 8.5f, ForceMode.VelocityChange);
                rigidbody.AddRelativeForce(Vector3.right * 3.2f, ForceMode.VelocityChange);
                Physics.gravity = new Vector3(0f, -22f, 0f);
                readyToJump = false;
                airborne = true;
                jumping = true;

            }


        }
    }

    void CheckAirborne()
    {
        if (transform.position.y < 1.25f)
        {

            airborne = false;
            rigidbody.useGravity = false;
        }
        else
        {
            startTime = Time.time;
            //readyToJump = false;
            airborne = true;
            rigidbody.useGravity = true;
        }

        Vector3 playerVelocity = rigidbody.velocity;
        float normalisedVelocity = playerVelocity.y; //only care about x value of player (run up speed)
        if(normalisedVelocity < -2f)
        {
            jumping = false;
        }
    }


    void CheckPastLine()
    {
        if (transform.position.x > 45f)
        {
            if (!pastFinishLine)
            {

                pastFinishLine = true;
                gameController.GetComponent<GameController>().ProcessNewHurdleScore(racePosition);
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
            for (int i = 0; i < hurdleOpponents.Length; i++)
            {
                hurdleOpponentsDistToFinishLine[i] = hurdleOpponents[i].transform.position.x;
            }

            for (int i = 0; i < hurdleOpponents.Length; i++)
            {
                if (playerXTransform < hurdleOpponentsDistToFinishLine[i])
                {
                    racePosition++;
                }
            }

            gameController.GetComponent<GameController>().hurdleCurrentPos = racePosition;


        }

    }

    
}