using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    // Set in editor
    public float boomHeight;
    public float boomDistance;

    private Overlord overlord;
    private Rigidbody overlordRb;
    private Vector3 lookTarget;

	// Use this for initialization
	void Start () {
        overlord = FindObjectOfType<Overlord>();
        overlordRb = overlord.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = overlord.transform.position + Vector3.back * boomDistance + Vector3.up * boomHeight;

        Vector3 toOverlord = (overlordRb.position - transform.position).normalized;
        //transform.rotation = Quaternion.LookRotation(toOverlord);
	}
}
