using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlord : MonoBehaviour {

    // Set in editor
    public Camera mainCamera;

    // Movement input
    private CharController charController;
    private Vector3 input;

    // Keep track of our minions
    List<Minion> minions = new List<Minion>();

	// Use this for initialization
	void Start () {
        charController = GetComponent<CharController>();

        minions = new List<Minion>(FindObjectsOfType<Minion>());
	}
	
	// Update is called once per frame
	void Update () {
        input = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0,
            Input.GetAxisRaw("Vertical")
        );
        input.Normalize();

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

        if (Physics.Raycast(ray, out hit)) {
            Vector3 point = hit.point;
            point.y = 0;
            return point;
        }

        return Vector3.zero;
    }

    private void FixedUpdate() {
        charController.HandleMovement(input);
    }
}
