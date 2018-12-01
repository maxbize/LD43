using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour {


    // Set in editor
    public float acceleration = 100;
    public float dragCoefficient = 1;
    public float frictionCoefficient = 10;

    private Rigidbody rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    // Takes the user input and makes the overlord actually move
    public void HandleMovement(Vector3 input) {
        float force = input.magnitude * acceleration;
        force -= (rb.velocity.magnitude * rb.velocity.magnitude) * dragCoefficient;

        Vector3 friction = frictionCoefficient * -rb.velocity.normalized;

        rb.AddForce(input * force + friction);
    }
}
