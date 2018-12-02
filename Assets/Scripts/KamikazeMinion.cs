using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeMinion : MonoBehaviour {

    // Set in editor
    public float deathRadius;
    public float pushRadius;
    public float pushImpulse;
    public float pushUpModifer;
    public float incendiaryTime; // therealhammer86gn might be on an FBI watchlist for knowing this ;)

    private float explodeAtTime = Mathf.Infinity;
    private CharController charController;
    private Vector3 target;

    private KamikazeState kamikazeState;
    private enum KamikazeState {
        waiting,
        moving,
        triggered
    }


	public void Init(Vector3 target) {
        charController = GetComponent<CharController>();
        this.target = target;
        kamikazeState = KamikazeState.moving;
        gameObject.layer = LayerMask.NameToLayer("Ignore All");
    }

    private void FixedUpdate() {

        if (kamikazeState == KamikazeState.waiting) {
            // Do nothing
        } else if (kamikazeState == KamikazeState.moving) {
            Vector3 toTarget = target - transform.position;
            toTarget.y = 0;
            charController.HandleMovement(toTarget.normalized);
            if (toTarget.magnitude < 0.1f) {
                explodeAtTime = Time.timeSinceLevelLoad + incendiaryTime;
                kamikazeState = KamikazeState.triggered;
                //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition; // Do we want this??
            }
        } else {
            if (Time.timeSinceLevelLoad > explodeAtTime) {
                Explode();
            }
        }
    }

    private void Explode() {
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
