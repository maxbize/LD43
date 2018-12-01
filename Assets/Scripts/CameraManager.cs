using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    // Set in editor
    public float boomHeight;
    public float boomDistance;

    private Rigidbody overlordRb;
    private Vector3 lookTarget;

	// Use this for initialization
	void Start () {
        overlordRb = FindObjectOfType<Overlord>().GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 overlordDir = overlordRb.velocity.normalized;
        overlordDir.y = 0;

        transform.position = overlordRb.position - overlordDir * boomDistance + Vector3.up * boomHeight;

        Vector3 toOverlord = (overlordRb.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(toOverlord);
	}
}
