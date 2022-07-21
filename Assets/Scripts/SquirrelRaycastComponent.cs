using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class SquirrelRaycastComponent : Agent
{
    Rigidbody rBody;
    public Transform Target;
    public float forceMultiplier = 5;

    public static bool didLeap;
    public static bool isRotating;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        var rotation = Random.Range(0, 4) * 90f;
        this.transform.Rotate(new Vector3(0f, rotation, 0f));

        rBody.angularVelocity = Vector3.zero;

        this.transform.localPosition = new Vector3(Random.Range(-0.5f, 0.5f), 0.0142f - 0.3f, Random.Range(-0.5f, 0.5f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {

        ArrayList floatObservations = new ArrayList();
        ArrayList normalizedObservations = new ArrayList();
        floatObservations.Add(Target.localPosition.x);
        floatObservations.Add(Target.localPosition.y);
        floatObservations.Add(Target.localPosition.y);
        floatObservations.Add(this.transform.localPosition.x);
        floatObservations.Add(this.transform.localPosition.y);
        floatObservations.Add(this.transform.localPosition.z);
        floatObservations.Add(rBody.velocity.x);
        floatObservations.Add(rBody.velocity.z);

        // normalize vecObservations
        foreach (float f in floatObservations)
        {
            normalizedObservations.Add((f - 0.9f) / (2f - 0.45f));
        }

        // padding
        for (int i = 12; i >= floatObservations.Count; i--)
        {
            floatObservations.Add(0f);
        }

        // Add observations
        foreach (float f in floatObservations)
        {
            sensor.AddObservation(f);
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        didLeap = false;
        MoveAgent(actionBuffers.DiscreteActions);

        // Rewards  
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        // Encourage leaping away from the sphere
        if (didLeap && distanceToTarget < 1f)
        {
            AddReward(0.5f);
        }

        // Collision

        // In range
        if (distanceToTarget < 1f)
        {
            AddReward(-(1f - Vector3.Distance(this.transform.localPosition, Target.localPosition)));
        }

        if (this.transform.localPosition.y <= -1)
        {
            EndEpisode();
        }
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = act[0];

        didLeap = false;
        isRotating = false;

        switch (action)
        {
            case 0:
                dirToGo = transform.forward * 1f;
                break;
            case 1:
                rotateDir = transform.up * 1f;
                isRotating = true;
                break;
            case 2:
                rotateDir = transform.up * -1f;
                isRotating = true;
                break;
            case 3:
                dirToGo = transform.forward * 1.75f;
                didLeap = true;
                break;
        }

        transform.Rotate(rotateDir, Time.fixedDeltaTime * 100f);
        rBody.velocity = dirToGo;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.StartsWith("FoodUnit"))
        {
            Destroy(collision.gameObject);
            AddReward(1f);
        }

        if (collision.gameObject.name.StartsWith("Sphere"))
        {
            AddReward(-0.5f);
            EndEpisode();
        }
    }
}
