using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelAnimation : MonoBehaviour
{
    Animator animator;
    Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rigidbody.velocity != Vector3.zero)
        {
            if (SquirrelRaycast.didLeap)
            {
                animator.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
            }
            else if(!SquirrelRaycast.isRotating)
            {
                animator.SetFloat("Speed", 0.81f, 0.1f, Time.deltaTime);
            } else
            {
                animator.SetFloat("Speed", 0.35f, 0.1f, Time.deltaTime);
            }
        }
        else
        {
            //animator.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
        }
    }
}
