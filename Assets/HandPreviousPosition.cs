using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPreviousPosition : MonoBehaviour {

    private Vector3 previousPosition;
    private Vector3 currentPosition;

    void Start() {
        previousPosition = transform.position;
    }

    void FixedUpdate() {
        // Debug.Log("before: " + previousPosition);
        previousPosition = currentPosition;
        currentPosition = transform.position;
        // Debug.Log("after: " + previousPosition);

        // GetComponent<Rigidbody>().velocity = (currentPosition - previousPosition) / Time.deltaTime;
        // I cant do this without another hand object with its own rigid body that is not kinematic
    }

    public Vector3 GetPreviousPosition() {
        return previousPosition;
    }
}