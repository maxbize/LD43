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
    public AudioClip deliveryClip;

    private float nextMeleeEnemySpawnTime;
    private float nextMinionSpawnTime;
    private float nextRangedEnemySpawnTime;
    private float nextTrapSpawnTime;
    private float nextDifficultyRampTime;
    private int difficulty = 0;
    private Overlord overlord;
    private GameManager gameManager;

	// Use this for initialization
	void Start () {
        overlord = FindObjectOfType<Overlord>();
        gameManager = FindObjectOfType<GameManager>();
	}

    public void StartGame() {
        SpawnCluster(minionPrefab, 5, 5);
        PlaySpawnClip();
        nextMeleeEnemySpawnTime = Time.timeSinceLevelLoad + 5f;
        nextMinionSpawnTime = Time.timeSinceLevelLoad + 10f;
        nextRangedEnemySpawnTime = Time.timeSinceLevelLoad + 60f;
        nextTrapSpawnTime = Time.timeSinceLevelLoad + 30f;
    }
	
	// Update is called once per frame
	void Update () {
        if (overlord == null || !gameManager.gameStarted) {
            return;
        }

		if (Input.GetKeyDown(KeyCode.Space)) {
            //SpawnCluster(minionPrefab, 3, 6); // TODO: Remove this debug before publishing
        }

        if (Time.timeSinceLevelLoad > nextDifficultyRampTime) {
            difficulty++;
            nextDifficultyRampTime = Time.timeSinceLevelLoad + 40f;
        }

        if (Time.timeSinceLevelLoad > nextMinionSpawnTime) {
            int min = 3 + Mathf.FloorToInt(difficulty / 1.5f);
            int max = 4 + Mathf.FloorToInt(difficulty / 1.5f);
            SpawnCluster(minionPrefab, min, max);
            SpawnCluster(minionPrefab, min, max);
            nextMinionSpawnTime = Time.timeSinceLevelLoad + 5f;
            PlaySpawnClip();
        }

        if (Time.timeSinceLevelLoad > nextMeleeEnemySpawnTime) {
            int min = 2 + Mathf.FloorToInt(difficulty * 1.8f);
            int max = 3 + Mathf.FloorToInt(difficulty * 1.8f);
            SpawnCluster(meleeEnemyPrefab, min, max);
            SpawnCluster(meleeEnemyPrefab, min, max);
            nextMeleeEnemySpawnTime = Time.timeSinceLevelLoad + 9f;
            PlaySpawnClip();
        }

        if (Time.timeSinceLevelLoad > nextRangedEnemySpawnTime) {
            for (int i = 0; i < difficulty; i++) {
                SpawnSomething(rangedEnemyPrefab, GetRandomPointInArena());
            }
            PlaySpawnClip();
            nextRangedEnemySpawnTime = Time.timeSinceLevelLoad + 30f;
        }

        if (Time.timeSinceLevelLoad > nextTrapSpawnTime) {
            if (FindObjectsOfType<Trap>().Length <= 3) { // TODO: Refactor out FindObjectsOfType
                PlaySpawnClip();
                SpawnCluster(trapPrefab, 1, 2);
                SpawnCluster(trapPrefab, 1, 2);
            }
            nextTrapSpawnTime = Time.timeSinceLevelLoad + 30f;
        }
    }

    private void PlaySpawnClip() {
        Minion.PlayClip(deliveryClip, Vector3.zero, 1.25f, 1.75f);
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

    public Vector3 constrainPointToArena(Vector3 point) {
        point -= ground.transform.position;
        if (Mathf.Abs(point.x) > ground.transform.lossyScale.x * 4.5f) {
            point.x = ground.transform.lossyScale.x * 4.5f * Mathf.Sign(point.x);
        }
        if (Mathf.Abs(point.z) > ground.transform.lossyScale.z * 4.5f) {
            point.z = ground.transform.lossyScale.z * 4.5f * Mathf.Sign(point.z);
        }
        point += ground.transform.position;
        return point;
    }
}
