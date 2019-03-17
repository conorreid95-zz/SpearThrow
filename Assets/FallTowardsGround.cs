using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallTowardsGround : MonoBehaviour
{
    Rigidbody rigidbody;
    bool collided = false;
    Vector3 spearVelocity;

    int currentSceneIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
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
            transform.RotateAround(transform.localPosition, new Vector3(0f, 00f, 90f), Time.deltaTime * 45f); //spear not released yet to rotate it upwards until released
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        collided = true;
        rigidbody.useGravity = false;
        rigidbody.freezeRotation = true;
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        Invoke("LoadCurrentLevel", 5f);
    }

    private void LoadCurrentLevel()
    {
        SceneManager.LoadScene(currentSceneIndex);

    }

}
