using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class SquirrelAI : Agent
{
    Rigidbody rBody;
    public Transform Target;
    public float forceMultiplier = 5;

    public static bool didMove;
    public static bool didLeap;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
    }

    public override void OnEpisodeBegin()
    {
        var rotation = Random.Range(0, 4) * 90f;
        this.transform.Rotate(new Vector3(0f, rotation, 0f));

        rBody.angularVelocity = Vector3.zero;

        this.transform.localPosition = new Vector3(Random.Range(-1.5f, 1.5f), 0.4f, Random.Range(-1.5f, 1.5f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        float[] floatObservations = { Target.localPosition.x,
            Target.localPosition.y,
            Target.localPosition.z,
            this.transform.localPosition.x,
            this.transform.localPosition.y,
            this.transform.localPosition.z,
            rBody.velocity.x,
            rBody.velocity.z
        };

        // normalize vecObservations
        for (int i = 0; i < floatObservations.Length; i++)
        {
            floatObservations[i] = (floatObservations[i] - 0.45f) / (5f - 0.45f);
        }

        // Add observations
        foreach (float f in floatObservations)
        {
            sensor.AddObservation(f);
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        didMove = true;
        didLeap = false;
        MoveAgent(actionBuffers.DiscreteActions);

        // Rewards  
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        // Discourage erratic movement
        if (!didMove)
        {
            AddReward(0.5f);
        }

        // Encourage leaping away from the sphere
        if (didLeap && distanceToTarget < 2f)
        {
            AddReward(0.5f);
        }

        // Collision
        if (distanceToTarget < 0.9f)
        {
            AddReward(-0.5f);
            EndEpisode();
        }

        // In range
        if (distanceToTarget < 2f)
        {
            AddReward(-(2f - Vector3.Distance(this.transform.localPosition, Target.localPosition)));
        }

        if(this.transform.localPosition.y <= -1)
        {
            EndEpisode();
        }
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = act[0];

        switch (action)
        {
            case 0:
                dirToGo = transform.forward * .5f;
                break;
            case 1:
                rotateDir = transform.up * .5f;
                break;
            case 2:
                rotateDir = transform.up * -.5f;
                break;
            case 3:
                dirToGo = transform.forward * 1.5f;
                didLeap = true;
                break;
        }

        transform.Rotate(rotateDir, Time.fixedDeltaTime * 50f);
        rBody.velocity = dirToGo;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal") * forceMultiplier;
        continuousActionsOut[1] = Input.GetAxis("Vertical") * forceMultiplier;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.StartsWith("FoodUnit"))
        {
            Debug.Log("Eat food");
            Destroy(collision.gameObject);
            AddReward(1f);
        }
    }
}
