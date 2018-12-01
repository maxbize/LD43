using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour {

    // Set in editor
    public GameObject spawnCarrierPrefab;
    public GameObject minionPrefab;
    public GameObject meleeEnemyPrefab;
    public GameObject rangedEnemyPrefab;
    public GameObject trapPrefab;
    public GameObject ground;

    private float nextMeleeEnemySpawnTime;
    private float nextMinionSpawnTime;
    private float nextRangedEnemySpawnTime;
    private float nextTrapSpawnTime;

	// Use this for initialization
	void Start () {
        SpawnCluster(minionPrefab, 5, 5);
        nextMeleeEnemySpawnTime = Time.timeSinceLevelLoad + 3f;
        nextMinionSpawnTime = Time.timeSinceLevelLoad + 5f;
        nextRangedEnemySpawnTime = Time.timeSinceLevelLoad + 20f;
        nextTrapSpawnTime = Time.timeSinceLevelLoad + 10f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
            SpawnCluster(minionPrefab, 3, 6); // TODO: Remove this debug before publishing
        }

        if (Time.timeSinceLevelLoad > nextMeleeEnemySpawnTime) {
            SpawnCluster(meleeEnemyPrefab, 2, 3);
            SpawnCluster(meleeEnemyPrefab, 2, 3);
            nextMeleeEnemySpawnTime = Time.timeSinceLevelLoad + 7f;
        }

        if (Time.timeSinceLevelLoad > nextMinionSpawnTime) {
            SpawnCluster(minionPrefab, 2, 3);
            SpawnCluster(minionPrefab, 2, 3);
            nextMinionSpawnTime = Time.timeSinceLevelLoad + 10f;
        }

        if (Time.timeSinceLevelLoad > nextRangedEnemySpawnTime) {
            SpawnSomething(rangedEnemyPrefab, GetRandomPointInArena());
            SpawnSomething(rangedEnemyPrefab, GetRandomPointInArena());
            SpawnSomething(rangedEnemyPrefab, GetRandomPointInArena());
            nextRangedEnemySpawnTime = Time.timeSinceLevelLoad + 15f;
        }

        if (Time.timeSinceLevelLoad > nextTrapSpawnTime) {
            if (FindObjectsOfType<Trap>().Length <= 5) { // TODO: Refactor out FindObjectsOfType
                SpawnCluster(trapPrefab, 1, 2);
                SpawnCluster(trapPrefab, 1, 2);
            }
            nextTrapSpawnTime = Time.timeSinceLevelLoad + 10f;
        }
    }

    private void SpawnCluster(GameObject prefab, int min, int max) {
        int numToSpawn = Random.Range(min, max + 1);
        Vector3 baseSpawnPos = GetRandomPointInArena();
        for (int i = 0; i < numToSpawn; i++) {
            Vector3 random = Random.onUnitSphere;
            random.y = 0;
            random *= 3f;
            SpawnSomething(prefab, baseSpawnPos + random);
        }
    }

    private void SpawnSomething(GameObject prefab, Vector3 pos) {
        GameObject spawnCarrier = Instantiate(spawnCarrierPrefab, Vector3.up * 30, Quaternion.identity);
        spawnCarrier.GetComponent<SpawnCarrier>().Init(prefab, pos);
    }

    private Vector3 GetRandomPointInArena() {
        // Groud object is scaled 10:1 for real world size & don't let it go to the edge
        Vector3 point = new Vector3(
            Random.Range(-ground.transform.lossyScale.x * 4.5f, ground.transform.lossyScale.x * 4.5f),
            0,
            Random.Range(-ground.transform.lossyScale.z * 4.5f, ground.transform.lossyScale.z * 4.5f)
        );
        point += ground.transform.position;
        return point;
    }
}
