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
    public float explostionDuration; // visual
    public Sprite explodedSprite;
    public AudioClip beepClip;
    public AudioClip boomClip;

    private float explodeAtTime = Mathf.Infinity;
    private float destroyAtTime = Mathf.Infinity;
    private CharController charController;
    private Vector3 target;
    private Material spriteMaterial;
    private Color startingColor;
    private bool beepedThisSin;

    private KamikazeState kamikazeState;
    private enum KamikazeState {
        waiting,
        moving,
        triggered,
        exploded,
    }

	public void Init(Vector3 target) {
        charController = GetComponent<CharController>();
        this.target = target;
        kamikazeState = KamikazeState.moving;
        gameObject.layer = LayerMask.NameToLayer("Ignore All");
        spriteMaterial = transform.GetChild(0).GetComponent<SpriteRenderer>().material; // HACK
        startingColor = spriteMaterial.color;
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
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            }
        } else if (kamikazeState == KamikazeState.triggered) {
            float t = 1f - (explodeAtTime - Time.timeSinceLevelLoad) / incendiaryTime;
            float sin = Mathf.Sin(Time.timeSinceLevelLoad + (1 + 7 * t) * (1 + 7 * t));
            if (beepedThisSin) {
                if (sin < 0.8f) {
                    beepedThisSin = false;
                }
            } else {
                if (sin > 0.8f) {
                    Minion.PlayClip(beepClip, transform.position, 1.25f + 0.5f * t, 1.25f + 0.5f * t);
                    beepedThisSin = true;
                }
            }
            spriteMaterial.color = new Color(startingColor.r + sin, startingColor.g, startingColor.b);
            if (Time.timeSinceLevelLoad > explodeAtTime) {
                Explode();
                destroyAtTime = Time.timeSinceLevelLoad + explostionDuration;
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = explodedSprite;
                transform.localScale *= (deathRadius + pushRadius) / 2f;
                kamikazeState = KamikazeState.exploded;
                Minion.PlayClip(boomClip, transform.position, 1.25f, 1.75f);
            }
        } else if (kamikazeState == KamikazeState.exploded) {
            if (Time.timeSinceLevelLoad > destroyAtTime) {
                Destroy(gameObject);
            }
        } else {
            Debug.LogError("Unexpected state");
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
    }
}
