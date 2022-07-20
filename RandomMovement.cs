using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    Rigidbody rigidBody;
    public float accelerationTime = 1f;
    public float maxSpeed = 5f;
    private Vector3 movement;
    private float timeLeft = 5f;
    private float foodCounter = 10f;

    public GameObject food;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        foodCounter -= Time.deltaTime;

        if (timeLeft <= 0)
        {
            movement = new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f));
            timeLeft += 5f;
        }
        if(foodCounter <= 0)
        {
            Instantiate(food, this.transform.localPosition, this.transform.rotation);
            foodCounter += 10f;
        }
    }

    void FixedUpdate()
    {
        rigidBody.AddForce(movement * maxSpeed);
    }
}
