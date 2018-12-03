using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour {

    // HACK HACK HACK
    public static int minionsDied;
    public static int meleeEnemiesDied;
    public static int rangedEnemiesDied;
    public static int trapsDestroyed;
    public static float survivalLength;

	// Use this for initialization
	void Start () {
        Debug.Log("I better be called on reload scene!");
        ResetStats();
	}

    private void ResetStats() {
        minionsDied = 0;
        meleeEnemiesDied = 0;
        rangedEnemiesDied = 0;
        trapsDestroyed = 0;
        survivalLength = 0;
    }
}
