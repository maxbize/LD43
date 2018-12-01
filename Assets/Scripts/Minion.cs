using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour {

    // Set in editor
    public float controlDistance;

    private CharController charController;
    private Overlord overlord;
    public bool controlled { get; private set; } // Under control of the overlord

	// Use this for initialization
	void Start () {
        charController = GetComponent<CharController>();
        overlord = FindObjectOfType<Overlord>();
    }

    // Update is called once per frame
    void Update () {
		if (!controlled) {
            if (Vector3.Distance(transform.position, overlord.transform.position) < controlDistance) {
                controlled = true;
                overlord.NotifyMinionControlled(this);
            }
        }
	}

    private void FixedUpdate() {
        if (!controlled) {
            MoveTowards(transform.position);
        }
    }

    // Called every frame - command from the overlord where we should walk towards
    public void MoveTowards(Vector3 pos) {
        Vector3 toPos = (pos - transform.position).normalized;
        toPos.y = 0;
        charController.HandleMovement(toPos);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.GetComponent<MeleeEnemy>() != null && controlled) {
            if (overlord != null) {
                overlord.NotifyMinionDied(this);
            }
            Destroy(gameObject);
        }
    }

}
