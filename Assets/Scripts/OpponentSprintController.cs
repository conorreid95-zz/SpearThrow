using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentSprintController : MonoBehaviour
{
    bool sprintSequenceStarted = false;
    //bool jumpReleased = false;
    //bool foulJump = false;
    bool lean = false;
    bool firstContact = false;
    bool pastFinishLine = false;
    bool firstClicked = false;

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
        CheckPastLine();
        GetSprintInput();
    }


    public void GetSprintInput()
    {
        firstClicked = true;
    }

    IEnumerator waiter()
    {
        for(int i = 0; i <50; i++)
        {


            if (pastFinishLine == false)
            {
                rigidbody.AddRelativeForce(Vector3.right * UnityEngine.Random.Range(4100f, 4800f) * Time.deltaTime);

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
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, _playerTargetLeftLean, 260f * Time.deltaTime);
                }
                else if (!lean)
                {
                    Quaternion _playerTargetRightLean;
                    _playerTargetRightLean = Quaternion.Euler(6f, 0f, 0f);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, _playerTargetRightLean, 260f * Time.deltaTime);
                }
                yield return new WaitForSeconds(UnityEngine.Random.Range(0.10f, 0.25f));
            }
        }
    }

    void CheckPastLine()
    {
        if (transform.position.x > 45f)
        {
            pastFinishLine = true;
           
        }
    }

}
