using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sets orientation of 2D art to face camera with wobbling
public class ArtOrientator : MonoBehaviour {

    // Set in editor
    public float wobbleDistance;
    public float wobbleAngle;
    public float wobbleAngleModifier;
    public float minWobbleSpeed;
    public float maxWobbleSpeed;
    public float wobbleSpeedVelocityScale; // Rate we scale with rb.velocity

    private int wobbleDir = 1; // 1 or -1
    private float currentAngle;
    private float wobbleSpeed;
    private Vector3 initialRotation;
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        wobbleSpeed = Random.Range(minWobbleSpeed, maxWobbleSpeed);
        initialRotation = transform.localRotation.eulerAngles;
        rb = GetComponentInParent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        float wobbleSpeedScaled = wobbleSpeed * rb.velocity.magnitude * wobbleSpeedVelocityScale;
        wobbleSpeedScaled = Mathf.Max(50f, wobbleSpeedScaled); // HACK - magic number

        currentAngle += wobbleDir * wobbleSpeedScaled * Time.deltaTime;
        if (Mathf.Abs(currentAngle) > wobbleAngle) {
            currentAngle = wobbleAngle * wobbleDir;
            wobbleDir *= -1;
        }

        Vector3 rightOffset = Vector3.right * Mathf.Tan(currentAngle * Mathf.Deg2Rad);
        Vector3 offset = (Vector3.up + rightOffset).normalized * wobbleDistance;

        transform.localRotation = Quaternion.Euler(initialRotation.x, initialRotation.y, initialRotation.z - currentAngle * wobbleAngleModifier);
        transform.localPosition = offset;
    }
}
