﻿using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Overlord : MonoBehaviour, IKillable {

    // Set in editor
    public Camera mainCamera;
    public AudioClip deathClip;

    // Movement input
    private CharController charController;
    private Vector3 input;
    private ArenaManager arenaManager; // HACK

    // Keep track of our minions
    HashSet<Minion> minions = new HashSet<Minion>();

	// Use this for initialization
	void Start () {
        charController = GetComponent<CharController>();
        arenaManager = FindObjectOfType<ArenaManager>();
	}
	
	// Update is called once per frame
	void Update () {
        input = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0,
            Input.GetAxisRaw("Vertical")
        );
        input.Normalize();

        Minion closestMinion = GetMinionClosestToMouse();
        if (closestMinion != null) {
            if (Input.GetKeyDown(KeyCode.E)) {
                closestMinion.Harden(GetMouseArenaPos());
            } else if (Input.GetKeyDown(KeyCode.Q)) {
                closestMinion.Kamikaze(GetMouseArenaPos());
            }
        }
    }

    private Minion GetMinionClosestToMouse() {
        Vector3 mousePos = GetMousePos();
        float distanceToClosest = float.PositiveInfinity;
        Minion closest = null;
        foreach (Minion minion in minions) {
            if (closest == null) {
                closest = minion;
            }
            float distanceToMinion = Vector3.Distance(mousePos, minion.transform.position);
            if (distanceToMinion < distanceToClosest) {
                distanceToClosest = distanceToMinion;
                closest = minion;
            }
        }
        return closest;
    }

    private void FixedUpdate() {
        charController.HandleMovement(input);
        MoveMinions();
    }

    // Tell the minions where to go
    private void MoveMinions() {
        Vector3 mouseHit = GetMousePos();
        foreach (Minion minion in minions) {
            minion.MoveTowards(mouseHit);
        }
    }

    // Gets the position of the mouse in the world (on the ground plane)
    private Vector3 GetMousePos() {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("MouseCast"))) {
            Vector3 point = hit.point;
            point.y = 0;
            return point;
        }

        return Vector3.zero;
    }

    // Constrained to arena bounds
    private Vector3 GetMouseArenaPos() {
        return arenaManager.constrainPointToArena(GetMousePos());
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.GetComponent<MeleeEnemy>() != null) {
            Kill();
        }
    }

    public void NotifyMinionDied(Minion deadMinion) {
        minions.Remove(deadMinion);
    }

    public void NotifyMinionControlled(Minion minion) {
        minions.Add(minion);
    }

    public void Kill() {
        Minion.PlayClip(deathClip, transform.position, 1.25f, 1.75f);
        FindObjectOfType<GameManager>().EndGame();
        Destroy(gameObject);
    }

    public bool IsFriendly() {
        return true;
    }
}
