using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour, IKillable {

    // Set in editor
    public float controlDistance;
    public Material claimedMat;

    public AudioClip deathClip;
    public AudioClip dispatchClip;

    private CharController charController;
    private Overlord overlord;
    public bool controlled { get; private set; } // Under control of the overlord
    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        charController = GetComponent<CharController>();
        overlord = FindObjectOfType<Overlord>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
		if (!controlled && overlord != null) {
            if (Vector3.Distance(transform.position, overlord.transform.position) < controlDistance) {
                controlled = true;
                overlord.NotifyMinionControlled(this);
                gameObject.layer = LayerMask.NameToLayer("Friendly");
                gameObject.transform.GetChild(0).GetComponent<Renderer>().material = claimedMat; // MASSIVE HACK
                charController.HandleMovement(Vector3.up * 10f);
                audioSource.pitch = Random.Range(1.25f, 1.75f);
                audioSource.Play();
            }
        }
	}

    private void FixedUpdate() {
        if (!controlled) {
            //MoveTowards(transform.position);
        }
    }

    // Called every frame - command from the overlord where we should walk towards
    public void MoveTowards(Vector3 pos) {
        Vector3 toPos = (pos - transform.position).normalized;
        toPos.y = 0;
        charController.HandleMovement(toPos);
    }

    private void OnCollisionEnter(Collision collision) {
        IKillable hit = collision.transform.GetComponent<IKillable>();
        if (hit != null && !hit.IsFriendly() && controlled) {
            hit.Kill();
            Kill();
        }
    }

    public void Harden(Vector3 target) {
        PlayDispatchClip();
        GetComponent<HardenedMinion>().Init(target);
        if (overlord != null) {
            overlord.NotifyMinionDied(this);
        }
        StatsManager.minionsDied++;
        Destroy(this);
    }

    public void Kamikaze(Vector3 target) {
        PlayDispatchClip();
        GetComponent<KamikazeMinion>().Init(target);
        if (overlord != null) {
            overlord.NotifyMinionDied(this);
        }
        StatsManager.minionsDied++;
        Destroy(this);
    }

    private void PlayDispatchClip() {
        PlayClip(dispatchClip, transform.position, 1.25f, 1.75f);
    }

    public void Kill() {
        if (overlord != null) {
            overlord.NotifyMinionDied(this);
        }
        StatsManager.minionsDied++;
        PlayClip(deathClip, transform.position, 1.25f, 1.75f);
        Destroy(gameObject);
    }

    public bool IsFriendly() {
        return true;
    }

    // HACK! This should really exist somewhere else but oh well
    public static void PlayClip(AudioClip clip, Vector3 pos, float pitchLow, float pitchHigh) {
        GameObject audioObject = new GameObject("Detached audio source");
        audioObject.transform.position = pos;
        AudioSource objectAudioSource = audioObject.AddComponent<AudioSource>();
        objectAudioSource.clip = clip;
        objectAudioSource.pitch = Random.Range(pitchLow, pitchHigh);
        objectAudioSource.Play();
        audioObject.AddComponent<AutoDestroy>();
    }
}
