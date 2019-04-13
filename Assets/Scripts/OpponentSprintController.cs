using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentSprintController : MonoBehaviour
{
    bool lean = false;
    bool pastFinishLine = false;
    bool airborne = false;
    bool readyToJump = true;
    bool jumping = false;
    bool collidedWithHurdle = false;

    float hurdle1 = -47.53f;
    float hurdle2 = -31.53638f;
    float hurdle3 = -15.53638f;
    float hurdle4 = 0.4636154f;
    float hurdle5 = 16.46362f;
    float hurdle6 = 32.46362f;

    float[] hurdles = new float[6];
    float opponentX;

    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(waiter());

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        opponentX = transform.position.x;


        hurdles[0] = hurdle1 - opponentX;
        hurdles[1] = hurdle2 - opponentX;
        hurdles[2] = hurdle3 - opponentX;
        hurdles[3] = hurdle4 - opponentX;
        hurdles[4] = hurdle5 - opponentX;
        hurdles[5] = hurdle6 - opponentX;

        for (int i = 0; i < 5; i++)
        {
            if (hurdles[i] < 0)
            {
                hurdles[i] = 1000f;
            }
        }
        CheckPastLine();
        CheckForJump();
        CheckAirborne();
    }
    

    private void CheckForJump()
    {
        float closestHurdle = hurdles[0];
        for(int i = 1; i <= 5; i++)
        {
            if(hurdles[i] < closestHurdle)
            {
                closestHurdle = hurdles[i];
            }
        }

        float maxDist = UnityEngine.Random.Range(2.1f, 2.85f);
        float blaaaa = UnityEngine.Random.Range(0f, 1f);
        float jumpVel = 8.8f;
        if (blaaaa <= 0.07f)
        {
            //maxDist = 0.05f;
            jumpVel = 3f;
        }
        if (((closestHurdle < maxDist) && (closestHurdle > 0.01f)) && readyToJump && !collidedWithHurdle)
        {

            float fff = hurdle1 - opponentX;
            //print("difference: "+fff.ToString());
            //print("Close to hurdle");
            Vector3 playerVelocity = rigidbody.velocity;
            float normalisedVelocity = playerVelocity.x; //only care about x value of player (run up speed)
            normalisedVelocity = Mathf.Abs(normalisedVelocity); //get abs value becaus it's negative
            //print("Normalised vel: " + normalisedVelocity.ToString());
            rigidbody.useGravity = true;
            
            rigidbody.AddRelativeForce(Vector3.up * jumpVel, ForceMode.VelocityChange);
            rigidbody.AddRelativeForce(Vector3.right * 3.2f, ForceMode.VelocityChange);
            Physics.gravity = new Vector3(0f, -22f, 0f);
            readyToJump = false;
            jumping = true;
        }

    }

    IEnumerator waiter()
    {
        for (int i = 0; i < 200; i++)
        {
            if (pastFinishLine == false && !airborne && !collidedWithHurdle)
            {
                rigidbody.AddRelativeForce(Vector3.right * UnityEngine.Random.Range(4300f, 4750f) * Time.deltaTime);

                if (lean)
                {
                    lean = false;
                }
                else
                {
                    lean = true;
                }

                if (lean)
                {
                    Quaternion _playerTargetLeftLean;
                    _playerTargetLeftLean = Quaternion.Euler(-6f, 0f, 0f);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, _playerTargetLeftLean, 560f * Time.deltaTime);
                }
                else if (!lean)
                {
                    Quaternion _playerTargetRightLean;
                    _playerTargetRightLean = Quaternion.Euler(6f, 0f, 0f);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, _playerTargetRightLean, 560f * Time.deltaTime);
                }

            }
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.12f, 0.35f));


        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Hurdle")
        {
            rigidbody.constraints = RigidbodyConstraints.None;
            collidedWithHurdle = true;
        }


    }

    void CheckPastLine()
    {
        if (transform.position.x > 45f)
        {
            pastFinishLine = true;
           
        }
    }

    void CheckAirborne()
    {
        if (transform.position.y < 1.25f && collidedWithHurdle == false)
        {
            airborne = false;
            rigidbody.useGravity = false;
            if(!readyToJump && !jumping)
            {
                readyToJump = true;
            }
        }
        else if (collidedWithHurdle ==false)
        {

            airborne = true;
            rigidbody.useGravity = true;
        }
        else if (collidedWithHurdle)
        {
            rigidbody.useGravity = true;
        }

        Vector3 playerVelocity = rigidbody.velocity;
        float normalisedVelocity = playerVelocity.y; //only care about x value of player (run up speed)
        if(normalisedVelocity  < -2f)
        {
            jumping = false;
        }
    }

}
