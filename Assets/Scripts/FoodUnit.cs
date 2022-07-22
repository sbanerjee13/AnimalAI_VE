using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodUnit : MonoBehaviour
{
    private float timer = 0f;
    public GameObject food;

    // Update is called once per frame
    void Update()
    {
        if(timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            if (Input.GetKeyDown("f"))
            {
                Instantiate(food, this.transform.localPosition, this.transform.rotation);
                timer += 5f;
            }
        }
    }
}
