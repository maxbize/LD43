﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

    // Set in editor
    public int maxVictims;
    public AudioClip destroyedClip;

    private int victims = 0;

	// Use this for initialization
	void Start () {
        transform.position = new Vector3(
            transform.position.x,
            0.25f,
            transform.position.z
        );
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        IKillable killable = other.transform.GetComponent<IKillable>();
        if (killable != null) {
            killable.Kill();
            victims++;
            if (victims == maxVictims) {
                StatsManager.trapsDestroyed++;
                Minion.PlayClip(destroyedClip, transform.position, 0.9f, 1.1f);
                Destroy(gameObject);
            }
        }
    }
}
