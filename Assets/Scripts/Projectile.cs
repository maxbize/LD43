using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    // Set in editor
    public float speed;

    private GameObject spawner; // Who spawned me?
    private CharController charController;
    private Vector3 direction;

	// Use this for initialization
	void Start () {
        charController = GetComponent<CharController>();
	}
	
    public void Init(GameObject spawner, Vector3 direction) {
        this.spawner = spawner;
        this.direction = direction;
    }

    private void FixedUpdate() {
        if (transform.position.y < 1.5f) {
            direction.y = 0;
            transform.position = new Vector3(
                transform.position.x,
                1.5f,
                transform.position.z
            );
        }
        charController.HandleMovement(direction);
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
