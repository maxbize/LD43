using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour {

    // Set in editor
    public GameObject spawnCarrierPrefab;
    public GameObject meleeEnemyPrefab;
    public GameObject rangedEnemyPrefab;
    public GameObject trapPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
            SpawnSomething(); // TODO: Remove this debug before publishing
        }
	}

    private void SpawnSomething() {
        Vector3 spawnPoint = Vector3.zero;

        GameObject spawnCarrier = Instantiate(spawnCarrierPrefab, Vector3.down * 10, Quaternion.identity);
        spawnCarrier.GetComponent<SpawnCarrier>().Init(meleeEnemyPrefab, spawnPoint);
    }

}
