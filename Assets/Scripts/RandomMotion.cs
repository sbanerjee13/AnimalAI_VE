using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMotion : MonoBehaviour
{
    Rigidbody rigidBody;
    public float accelerationTime = 1f;
    private Vector3 movement;
    private float timeLeft = 5f;
    private float foodCounter = 30f;

    public GameObject food;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        foodCounter -= Time.deltaTime;
        rigidBody.velocity = movement * 0.5f;

        if (timeLeft <= 0)
        {
            movement = new Vector3(Random.Range(-1.5f, 1.5f), -0.3f, Random.Range(-1.5f, 1.5f));
            timeLeft += 3f;
        }
        if (foodCounter <= 0)
        {
            Instantiate(food, this.transform.localPosition, this.transform.rotation);
            foodCounter += 10f;
        }
        if(this.transform.localPosition.y <= -1)
        {
            this.transform.localPosition = new Vector3(0.122f, 0.066f, 0.495f);
        }
        if (this.transform.localPosition.x <= -1 || this.transform.localPosition.x >= 1 || this.transform.localPosition.z <= -1 || this.transform.localPosition.z >= 1)
        {
            this.transform.localPosition = new Vector3(0.122f, 0.066f, 0.495f);
        }
    }
}
