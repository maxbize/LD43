using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    // Set in editor
    public float speed;

    private GameObject spawner; // Who spawned me?
    private CharController charController;

	// Use this for initialization
	void Start () {
        charController = GetComponent<CharController>();
	}
	
    public void Init(GameObject spawner) {
        this.spawner = spawner;
    }

    private void FixedUpdate() {
        if (transform.position.y < 1f) {
            transform.position = new Vector3(
                transform.position.x,
                1,
                transform.position.z
            );
        }
        charController.HandleMovement(transform.forward);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == spawner || other.GetComponent<Trap>() != null) {
            return;
        }

        IKillable hit = other.GetComponent<IKillable>();
        if (hit != null) {
            hit.Kill();
        }

        // TODO: Should this be a deathray?
        Destroy(gameObject);
    }
}
