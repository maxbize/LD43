using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

    // Set in editor
    public int maxVictims;

    private int victims = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        IKillable hit = other.transform.GetComponent<IKillable>();
        if (hit != null) {
            hit.Kill();
            victims++;
            if (victims == maxVictims) {
                Destroy(gameObject);
            }
        }

    }
}
