using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    Rigidbody rigidbody;
    public float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0.15f, 2.22f);
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isMoving = false;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigidbody.velocity = Vector3.left * speed;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rigidbody.velocity = Vector3.right * speed;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rigidbody.velocity = Vector3.forward * speed;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rigidbody.velocity = Vector3.back * speed;
            isMoving = true;
        }

        if(!isMoving)
        {
            rigidbody.velocity = Vector3.zero;
        }
    }

    private void Reset()
    {
        transform.position = new Vector3(0, 0.15f, 2.22f);
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.y);
    }
}
