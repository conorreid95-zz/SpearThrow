using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerController : MonoBehaviour
{
    public GameObject spear;
    Renderer rend;
    Material white_Material;

    Rigidbody rigidbody;
    GameObject gameController;

    bool throwSequenceStarted = false;
    bool spearReleased = false;
    bool lean = false;
    float keyDownTime = 0.25f;
    float startTime = 0f;

    public bool pastLine = false;
    bool firstClicked = false;

    

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController");

        rend = GetComponent<Renderer>();
        white_Material = GetComponent<Renderer>().material;

        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        CheckPastLine();
        //Camera.main.transform.Translate(transform.position.x + 4.78023f, 3.21062f, 2.325678f);
    }

    void CheckPastLine()
    {
        if(transform.position.x < 41.09f)
        {
            pastLine = true;
            white_Material.color = Color.red;
            Invoke("StartFaultProcess", 4f);
        }
    }

    void StartFaultProcess()
    {
        gameController.GetComponent<GameController>().ProcessNewScore(0f);
    }


    private void GetInput()
    {
        if (!throwSequenceStarted) //if throw sequence isn't started get run up input and check for long press to start throw sequence
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                firstClicked = true;
                rigidbody.AddRelativeForce(Vector3.left * 4300f * Time.deltaTime);
                startTime = Time.time;
                if (lean)
                {
                    
                    lean = false;
                }
                else
                {
                    //Quaternion _playerTargetRightLean;
                    //_playerTargetRightLean = Quaternion.Euler(15f, 0f, 0f);
                    //transform.rotation = Quaternion.RotateTowards(transform.rotation, _playerTargetRightLean, 260f * Time.deltaTime);
                    //transform.eulerAngles = new Vector3(5f, 0f, 0f);
                    lean = true;
                }
            }

            if (lean && firstClicked)
            {
                Quaternion _playerTargetLeftLean;
                _playerTargetLeftLean = Quaternion.Euler(-8f, 0f, 0f);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _playerTargetLeftLean, 260f * Time.deltaTime);
                //transform.eulerAngles = new Vector3(-5f, 0f, 0f);
            }
            else if(!lean && firstClicked)
            {
                Quaternion _playerTargetRightLean;
                _playerTargetRightLean = Quaternion.Euler(8f, 0f, 0f);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _playerTargetRightLean, 260f * Time.deltaTime);
                //transform.eulerAngles = new Vector3(5f, 0f, 0f);
                
            }


            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0))
            {
                if ((startTime + keyDownTime) < Time.time) //check if held long enough
                {
                    print("Held for 0.25 second");
                    print("Start Time: " + startTime.ToString());

                    print("Time: " + Time.time.ToString());
                    //white_Material.color = Color.red;
                    throwSequenceStarted = true;
                    //rigidbody.AddRelativeForce(Vector3.left * 50000f * Time.deltaTime);
                }
            }

        }
        else
        {
            if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0)) || (!spearReleased && (spear.transform.rotation.eulerAngles.z < 1f))) //if throw is released
            {
                if (!spearReleased)
                {
                    spearReleased = true;
                    Invoke("ChangeCameraToSpear", 0.5f);

                    
                    //transform.eulerAngles = new Vector3(0f, 0f, 20f); //reset player stance to normal

                    Vector3 playerVelocity = rigidbody.velocity;
                    float normalisedVelocity = playerVelocity.x; //only care about x value of player (run up speed)
                    normalisedVelocity = Mathf.Abs(normalisedVelocity); //get abs value becaus it's negative
                    print("Normalised vel: " + normalisedVelocity.ToString());
                    normalisedVelocity = normalisedVelocity * (8.25f + UnityEngine.Random.Range(0.5f, 2f)); //multiply to get a force to add onto throw

                    white_Material.color = Color.white;
                    spear.transform.parent = null;
                    spear.GetComponent<FallTowardsGround>().enabled = true;
                    //spear.AddComponent<Rigidbody>();
                    //spear.GetComponent<Rigidbody>().mass = 0.1f;
                    spear.GetComponent<Rigidbody>().useGravity = true;
                    spear.GetComponent<Rigidbody>().isKinematic = false;
                    spear.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
                    spear.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * (35f + normalisedVelocity)); //add throw force to spear
                    rigidbody.drag = 5; //add drag to player after throw
                    print("Threw spear with extra velocity of " + normalisedVelocity.ToString());
                }

                
            }
            else if(!spearReleased)
            {
                Quaternion _playerTargetRotation;
                _playerTargetRotation = Quaternion.Euler(0f,0f,-15f);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _playerTargetRotation, 120f * Time.deltaTime);
                
                //transform.eulerAngles = new Vector3(0f, 0f, -20f);
                spear.transform.RotateAround(transform.localPosition, new Vector3(0f, 00f, -90f), Time.deltaTime * 45f); //spear not released yet to rotate it upwards until released
            }

            if (spearReleased)
            {
                Quaternion _playerThrowTargetRotation;
                _playerThrowTargetRotation = Quaternion.Euler(0f, 0f, 20f);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _playerThrowTargetRotation, 260f * Time.deltaTime);
            }
        }

    }

    private void ChangeCameraToSpear()
    {
        GameObject.Find("GameController").GetComponent<GameController>().followSpear = true;
    }



}
