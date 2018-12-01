using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardenedMinion : MonoBehaviour {

    // Set in editor
    public float animationTime; // Length of animation
    public float expansionSize; // Scale fully expanded
    public float hardenTime; // How long to stay hardened

    private float doneExpandingTime;
    private float doneHardeningTime;
    private float doneContractingTime;
    private Vector3 initialScale;

    private ExpansionState expansionState = ExpansionState.waiting;
    private enum ExpansionState {
        waiting,
        expanding,
        expanded,
        contracting
    }

    // Entry point for hardening
	public void Expand() {
        expansionState = ExpansionState.expanding;
        doneExpandingTime = Time.timeSinceLevelLoad + animationTime;
        initialScale = transform.localScale;
        Destroy(GetComponent<Rigidbody>());
        GetComponent<CapsuleCollider>().height = 1;
    }
	
	// Update is called once per frame
	void FixedUpdate() {
		if (expansionState == ExpansionState.waiting) {
            // Do nothing
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
