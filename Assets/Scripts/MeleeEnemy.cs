using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour, IKillable {

    // Set in editor
    public AudioClip deathClip;

    private Overlord overlord;
    private CharController charController;

    // Use this for initialization
    void Start() {
        charController = GetComponent<CharController>();
        overlord = FindObjectOfType<Overlord>();
    }

    void FixedUpdate() {
        if (overlord != null) {
            MoveTowardsOverlord();
        }
    }

    // Called every frame - command from the overlord where we should walk towards
    public void MoveTowardsOverlord() {
        Vector3 toPos = (overlord.transform.position - transform.position).normalized;
        toPos.y = 0;
        charController.HandleMovement(toPos);
    }

    private void OnCollisionEnter(Collision collision) {
        // Handled in other classes
    }

    public void Kill() {
        StatsManager.meleeEnemiesDied++;
        Minion.PlayClip(deathClip, transform.position, 0.9f, 1.1f);
        Destroy(gameObject);
    }

    public bool IsFriendly() {
        return false;
    }
}
