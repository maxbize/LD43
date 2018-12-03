using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("Die", GetComponent<AudioSource>().clip.length);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Die() {
        Destroy(gameObject);
    }
}
