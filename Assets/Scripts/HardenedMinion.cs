using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardenedMinion : MonoBehaviour {

    // Set in editor
    public float animationTime; // Length of animation
    public float expansionSize; // Scale fully expanded
    public float hardenTime; // How long to stay hardened
    public Sprite shieldSprite;
    public AudioClip expandClip;
    public AudioClip deflateClip;

    private float doneExpandingTime;
    private float doneHardeningTime;
    private float doneContractingTime;
    private Vector3 initialScale;
    private Vector3 target;
    private CharController charController;

    private ExpansionState expansionState = ExpansionState.waiting;
    private enum ExpansionState {
        waiting,
        moving,
        expanding,
        expanded,
        contracting
    }

    public void Init(Vector3 target) {
        this.target = target;
        charController = GetComponent<CharController>();
        gameObject.layer = LayerMask.NameToLayer("Ignore All");
        expansionState = ExpansionState.moving;
    }

	private void Expand() {
        expansionState = ExpansionState.expanding;
        doneExpandingTime = Time.timeSinceLevelLoad + animationTime;
        initialScale = transform.localScale;
        Destroy(GetComponent<Rigidbody>());
        GetComponent<CapsuleCollider>().height = 1;
        gameObject.layer = LayerMask.NameToLayer("Ignore Friendly");
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = shieldSprite; // HACK!
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = expandClip;
        audioSource.pitch = Random.Range(1.25f, 1.75f);
        audioSource.Play();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (expansionState == ExpansionState.waiting) {
            // Do nothing
        } else if (expansionState == ExpansionState.moving) {
            Vector3 toTarget = target - transform.position;
            toTarget.y = 0;
            charController.HandleMovement(toTarget.normalized);
            if (toTarget.magnitude < 0.1f) {
                Expand();
            }
        } else if (expansionState == ExpansionState.expanding) {
            float t = (doneExpandingTime - Time.timeSinceLevelLoad) / animationTime;
            float size = Mathf.Lerp(expansionSize, initialScale.magnitude, t);
            transform.localScale = initialScale * size;
            if (Time.timeSinceLevelLoad > doneExpandingTime) {
                expansionState = ExpansionState.expanded;
                doneHardeningTime = Time.timeSinceLevelLoad + hardenTime;
            }
        } else if (expansionState == ExpansionState.expanded) {
            if (Time.timeSinceLevelLoad > doneHardeningTime) {
                expansionState = ExpansionState.contracting;
                doneContractingTime = Time.timeSinceLevelLoad + animationTime;
                Minion.PlayClip(deflateClip, transform.position, 1.25f, 1.75f);
            }
        } else if (expansionState == ExpansionState.contracting) {
            float t = (doneContractingTime - Time.timeSinceLevelLoad) / animationTime;
            float size = Mathf.Lerp(0, expansionSize, t);
            transform.localScale = initialScale * size;
            if (Time.timeSinceLevelLoad > doneContractingTime) {
                Destroy(gameObject);
            }
        } else {
            Debug.LogError("You messed up!");
        }
    }
}
