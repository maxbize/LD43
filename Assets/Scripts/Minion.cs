using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour {

    private CharController charController;

	// Use this for initialization
	void Start () {
        charController = GetComponent<CharController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Called every frame - command from the overlord where we should walk towards
    public void MoveTowards(Vector3 pos) {
        Vector3 toPos = (pos - transform.position).normalized;
        toPos.y = 0;
        charController.HandleMovement(toPos);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.GetComponent<MeleeEnemy>() != null) {
            Overlord overlord = FindObjectOfType<Overlord>();
            if (overlord != null) {
                overlord.NotifyMinionDied(this);
            }
            Destroy(gameObject);
        }
    }

}
