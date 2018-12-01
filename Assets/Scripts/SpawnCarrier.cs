﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCarrier : MonoBehaviour {

    // Set in editor
    public float coneAngle;

    private GameObject prefabToSpawn;
    private CharController charController;

    // Use this for initialization
    void Start() {
        charController = GetComponent<CharController>();
    }

    public void Init(GameObject prefabToSpawn, Vector3 target) {
        this.prefabToSpawn = prefabToSpawn;

        // Pick some angle vector to spawn on that's within the cone
        float angle = Random.Range(0f, coneAngle) * Mathf.Deg2Rad;
        Vector3 right = Vector3.right * Mathf.Tan(angle);
        Vector3 targetToSpawn = Vector3.up + right;
        targetToSpawn = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up) * targetToSpawn;

        transform.position = target + targetToSpawn * 20f;
        transform.rotation = Quaternion.LookRotation(target - transform.position);
    }

    private void FixedUpdate() {
        charController.HandleMovement(transform.forward);
    }

    private void OnTriggerEnter(Collider other) {
        Instantiate(prefabToSpawn, transform.position + Vector3.up * 3, Quaternion.identity);

        Destroy(gameObject);
    }
}
