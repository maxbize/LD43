using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour, IKillable {

    // Set in editor
    public float attackFrequency; // Attack every x seconds
    public GameObject projectilePrefab;
    public GameObject turretArm;
    public GameObject turretArmTip;

    public float debug;

    private float nextAttackTime;
    private Overlord overlord;
    private Camera mainCamera;

	// Use this for initialization
	void Start () {
        mainCamera = FindObjectOfType<Camera>();
        nextAttackTime = Time.timeSinceLevelLoad + attackFrequency;
        overlord = FindObjectOfType<Overlord>();
        transform.position = new Vector3(
            transform.position.x,
            1.7f,
            transform.position.z
        );
	}
	
	// Update is called once per frame
	void Update () {
        if (overlord == null) {
            return;
        }

        Vector2 turretScreenPos = mainCamera.WorldToScreenPoint(transform.position);
        Vector2 overlordScreenPos = mainCamera.WorldToScreenPoint(overlord.transform.position);
        Vector2 turretToOverlord = overlordScreenPos - turretScreenPos;
        float angle = Vector2.Angle(Vector2.right, turretToOverlord);
        turretArm.transform.rotation = Quaternion.Euler(35, 0, turretToOverlord.y > 0 ? angle : -angle);

		if (Time.timeSinceLevelLoad > nextAttackTime) {
            Attack();
            nextAttackTime = Time.timeSinceLevelLoad + attackFrequency;
        }
    }

    // Spawn a projectile that homes in on the overlord
    private void Attack() {
        Vector3 toOverlord = (overlord.transform.position - turretArmTip.transform.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, turretArmTip.transform.position, turretArm.transform.rotation);
        projectile.GetComponent<Projectile>().Init(gameObject, toOverlord);
    }

    public void Kill() {
        Destroy(gameObject);
    }

    public bool IsFriendly() {
        return false;
    }
}
