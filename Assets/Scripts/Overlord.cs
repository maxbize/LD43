using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlord : MonoBehaviour {

    // Set in editor
    public float acceleration;
    public float dragCoefficient;
    public float frictionCoefficient;

    private Vector3 input;
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        input = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0,
            Input.GetAxisRaw("Vertical")
        );
        input.Normalize();
	}

    private void FixedUpdate() {
        HandleMovement();
    }

    // Takes the user input and makes the overlord actually move
    private void HandleMovement() {
        float force = input.magnitude * acceleration;
        force -= (rb.velocity.magnitude * rb.velocity.magnitude) * dragCoefficient;

        Vector3 friction = frictionCoefficient * -rb.velocity.normalized;

        rb.AddForce(input * force + friction);
    }
}
