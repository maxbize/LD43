using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCarrier : MonoBehaviour {

    // Set in editor
    public float coneAngle;
    public GameObject spriteObject;

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

        spriteObject.transform.rotation = Quaternion.Euler(35, 0, Random.Range(0, 360));
    }

    private void FixedUpdate() {
        charController.HandleMovement(transform.forward);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.GetComponent<SpawnCarrier>() != null) {
            return; // Don't trigger on other spawners
        }

        // MAJOR HACK!
        if (other.name.IndexOf("invisible") >= 0) {
            return; // Don't hit the invisible walls
        }

        Instantiate(prefabToSpawn, transform.position + Vector3.up * 3, Quaternion.identity);

        Destroy(gameObject);
    }
}
