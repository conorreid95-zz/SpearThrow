using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerLongJump : MonoBehaviour
{
    GameObject gameController;
    Rigidbody rigidbody;

    bool runSequenceStarted = false;
    bool jumpReleased = false;
    bool lean = false;
    float keyDownTime = 0.25f;
    float startTime = 0f;

    public bool pastLine = false;
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
        GetLongJumpInput();
        CheckPastLongJumpLine();

    }

    private void CheckPastLongJumpLine()
    {
        if (transform.position.z < -10.80f && jumpReleased == false)
        {
            pastLine = true;
            print("PastLine");
            //Invoke("StartFaultProcess", 4f);
        }
    }

    private void GetLongJumpInput()
    {
        if (!runSequenceStarted) //if throw sequence isn't started get run up input and check for long press to start throw sequence
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                firstClicked = true;
                rigidbody.AddRelativeForce(Vector3.back * 4300f * Time.deltaTime);
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
                _playerTargetLeftLean = Quaternion.Euler(0f, 0f, -8f);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _playerTargetLeftLean, 260f * Time.deltaTime);
            }
            else if (!lean && firstClicked)
            {
                Quaternion _playerTargetRightLean;
                _playerTargetRightLean = Quaternion.Euler(0f, 0f, 8f);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _playerTargetRightLean, 260f * Time.deltaTime);
            }


            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0))
            {
                if ((startTime + keyDownTime) < Time.time) //check if held long enough
                {
                    print("Held for 0.25 second");
                    print("Start Time: " + startTime.ToString());

                    print("Time: " + Time.time.ToString());
                    runSequenceStarted = true;
                }
            }

        }
        else
        {
            if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0))) //if throw is released
            {
                if (!jumpReleased)
                {
                    jumpReleased = true;

                    Vector3 playerVelocity = rigidbody.velocity;
                    float normalisedVelocity = playerVelocity.z; //only care about x value of player (run up speed)
                    normalisedVelocity = Mathf.Abs(normalisedVelocity); //get abs value becaus it's negative
                    print("Normalised vel: " + normalisedVelocity.ToString());
                    //normalisedVelocity = normalisedVelocity * (8.25f + UnityEngine.Random.Range(0.5f, 2f)); //multiply to get a force to add onto throw
                    rigidbody.useGravity = true;
                    
                    rigidbody.AddRelativeForce(Vector3.up*normalisedVelocity, ForceMode.VelocityChange);
                    rigidbody.AddRelativeForce(Vector3.back * 2.5f, ForceMode.VelocityChange);
                    Physics.gravity = new Vector3(0f, -22f, 0f);

                    rigidbody.drag = 1f;

                }


            }
            else if (!jumpReleased)
            {
                Quaternion _playerTargetRotation;
                _playerTargetRotation = Quaternion.Euler(0f, -15f, 0f);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _playerTargetRotation, 120f * Time.deltaTime);

            }
        }
    }
}
