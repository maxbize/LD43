using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeMinion : MonoBehaviour {

    // Set in editor
    public float deathRadius;
    public float pushRadius;
    public float pushImpulse;
    public float pushUpModifer;
    public float accel;
    public float drag;

    private Vector3 dir;
    private bool launched;
    private CharController charController;

	public void Init(Vector3 target) {
        Destroy(GetComponent<Minion>());
        GetComponent<Collider>().isTrigger = true;
        charController = GetComponent<CharController>();
        dir = (target - transform.position).normalized;
        dir.y = 0;
        launched = true;
        charController.DisableGravity();
        transform.position += Vector3.up * 0.2f; // Sometimes we clip the ground!
        charController.dragCoefficient = drag; // HACK - shouldn't modify this directly
        charController.acceleration = accel; // HACK - shouldn't modify this directly
        charController.frictionCoefficient = 0; // HACK - shouldn't modify this directly
    }

    private void FixedUpdate() {
        if (launched) {
            charController.HandleMovement(dir);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!launched) {
            return;
        }

        IKillable hitKillable = other.GetComponent<IKillable>();
        if (hitKillable != null && hitKillable.IsFriendly()) {
            return;
        }

        foreach (Collider hit in Physics.OverlapSphere(transform.position, deathRadius)) {
            IKillable killable = hit.transform.GetComponent<IKillable>();
            if (killable != null) {
                killable.Kill();
            }
        }

        foreach (Collider hit in Physics.OverlapSphere(transform.position, pushRadius)) {
            Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
            if (rb != null) {
                Vector3 toHit = (hit.transform.position - transform.position).normalized;
                rb.AddExplosionForce(pushImpulse, transform.position, pushRadius, pushUpModifer, ForceMode.Impulse);
            }
        }

        Destroy(gameObject);
    }
}
