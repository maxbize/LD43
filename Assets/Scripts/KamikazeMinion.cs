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

	public void Init(Vector3 target) {
        Destroy(GetComponent<Minion>());
        explodeAtTime = Time.timeSinceLevelLoad + incendiaryTime;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
    }

    private void Update() {
        if (Time.timeSinceLevelLoad > explodeAtTime) {
            Explode();
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
