using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour {

    // Set in editor
    public GameObject spawnCarrierPrefab;
    public GameObject meleeEnemyPrefab;
    public GameObject rangedEnemyPrefab;
    public GameObject trapPrefab;
    public GameObject ground;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Space)) {
            SpawnSomething(); // TODO: Remove this debug before publishing
        }
	}

    private void SpawnSomething() {
        // Groud object is scaled 10:1 for real world size & don't let it go to the edge
        Vector3 spawnPoint = new Vector3(
            Random.Range(-ground.transform.localScale.x * 4.5f, ground.transform.localScale.x * 4.5f),
            0,
            Random.Range(-ground.transform.localScale.z * 4.5f, ground.transform.localScale.z * 4.5f)
        );
        spawnPoint += ground.transform.position;

        GameObject spawnCarrier = Instantiate(spawnCarrierPrefab, Vector3.down * 10, Quaternion.identity);
        spawnCarrier.GetComponent<SpawnCarrier>().Init(meleeEnemyPrefab, spawnPoint);
    }

}
