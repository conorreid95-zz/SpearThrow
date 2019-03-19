using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallTowardsGround : MonoBehaviour
{
    Rigidbody rigidbody;
    bool collided = false;
    Vector3 spearVelocity;
    GameObject gameController;
    
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController");
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        spearVelocity = rigidbody.velocity;
        float normalisedVelocity = spearVelocity.y; //only care about x value of player (run up speed)

        //print(normalisedVelocity.ToString());
        if (normalisedVelocity < 5f && !collided)
        {
            print("Spear y is negative");
            transform.RotateAround(transform.localPosition, new Vector3(0f, 00f, 90f), Time.deltaTime * 35f); //spear not released yet to rotate it upwards until released
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        collided = true;
        rigidbody.useGravity = false;
        rigidbody.freezeRotation = true;
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        print("Contact point: "+collision.GetContact(0).point.ToString());
        float distance = Vector3.Distance(collision.GetContact(0).point, new Vector3(41.09f, 0f, 0f));

        if(collision.GetContact(0).point.x > 41.09f) { distance = 0f; }
        gameController.GetComponent<GameController>().ProcessNewScore(distance); //call function on gameController with distance data
        
    }

    

}
