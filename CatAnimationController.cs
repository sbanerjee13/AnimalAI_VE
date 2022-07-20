using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimationController : MonoBehaviour
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
        if(rigidbody.velocity != Vector3.zero)
        {
            if(CatAI.didLeap)
            {
                animator.SetFloat("Speed", .75f, 0.1f, Time.deltaTime);
            } else
            {
                animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
            }
        }
        else
        {
            //animator.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
        }
    }
}
