using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour {


    // Set in editor
    public float acceleration = 100;
    public float dragCoefficient = 1;
    public float frictionCoefficient = 10;
    public float extraGravityMultiplier = 0;

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
        Vector3 inputForce = input * acceleration;

        Vector3 dragForce = -rb.velocity.normalized * (rb.velocity.sqrMagnitude * dragCoefficient);

        Vector3 frictionForce = frictionCoefficient * -rb.velocity.normalized;
        frictionForce.y = 0;

        Vector3 extraGravityForce = Physics.gravity * extraGravityMultiplier;

        rb.AddForce(inputForce + dragForce + frictionForce + extraGravityForce);
    }
}
