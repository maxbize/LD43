using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropCreator : MonoBehaviour {

    // Set in editor
    public GameObject[] grassPrefabs;
    public GameObject[] rockPrefabs;
    public GameObject[] flowerPrefabs;

    private Camera mainCamera;

	// Use this for initialization
	void Start () {
        mainCamera = FindObjectOfType<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Y)) {
            SpawnProp(grassPrefabs[Random.Range(0, grassPrefabs.Length)], 0f);
        } else if (Input.GetKeyDown(KeyCode.U)) {
            SpawnProp(rockPrefabs[Random.Range(0, rockPrefabs.Length)], 0.1f);
        } else if (Input.GetKeyDown(KeyCode.I)) {
            SpawnProp(flowerPrefabs[Random.Range(0, flowerPrefabs.Length)], 0.5f);
        }
    }

    private void SpawnProp(GameObject propPrefab, float y) {
        Vector3 spawnPoint = GetMousePos();
        spawnPoint.y = y;
        Quaternion rotation = Quaternion.Euler(35, Random.Range(-30, 30), 0);
        Vector3 scale = Vector3.one;
        if (Random.Range(0f, 1f) > 0.4f) {
            scale.x *= -1;
        }
        scale *= Random.Range(0.8f, 1.2f);

        GameObject prop = Instantiate(propPrefab, spawnPoint, rotation);
        prop.transform.localScale = scale;
    }

    // Gets the position of the mouse in the world (on the ground plane)
    private Vector3 GetMousePos() {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("MouseCast"))) {
            Vector3 point = hit.point;
            point.y = 0;
            return point;
        }

        return Vector3.zero;
    }
}
