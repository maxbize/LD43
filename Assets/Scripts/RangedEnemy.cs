using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour, IKillable {

    // Set in editor
    public float attackFrequency; // Attack every x seconds
    public GameObject projectilePrefab;

    private float nextAttackTime;
    private Overlord overlord;

	// Use this for initialization
	void Start () {
        nextAttackTime = Time.timeSinceLevelLoad + attackFrequency;
        overlord = FindObjectOfType<Overlord>();
	}
	
	// Update is called once per frame
	void Update () {
        if (overlord == null) {
            return;
        }

        Vector3 toOverlord = overlord.transform.position - transform.position;
        toOverlord.y = 0;
        transform.rotation = Quaternion.LookRotation(toOverlord);

		if (Time.timeSinceLevelLoad > nextAttackTime) {
            Attack();
            nextAttackTime = Time.timeSinceLevelLoad + attackFrequency;
        }
    }

    // Spawn a projectile that homes in on the overlord
    private void Attack() {
        GameObject projectile = Instantiate(projectilePrefab, transform.position + transform.forward, transform.rotation);
        projectile.GetComponent<Projectile>().Init(gameObject);
    }

    public void Kill() {
        Destroy(gameObject);
    }

    public bool IsFriendly() {
        return false;
    }
}
