using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

	public string visible_name;
	public int level = 1;
	public float speed = 2;
	public float minDist = 1f;
	public Health HP;
	public Transform inSomething;
	public Vector3 inSmthPos;
	public float repelForce = 25;
	public Collider groundCol;
	private float defDFric;
	private float defSFric;
	public Rigidbody rb;
	public GroundDetect GroundCheck;
	public bool disableWalk = false;
	public float disableTurnTime;
	public bool disableTurn = false;
	public float minWalkRecoverTime = 0.05f;
	public float walkRecoverTime = 0f;
	public float notFallingThreshold = 0.1f;
	public Color defColour;
	public float defColourTime;
	public ParticleSystem FX;
	public Player player;

	// Use this for initialization
	void Start () {
		defColour = GetComponent<Renderer> ().material.color;
		if (!HP)
			HP = GetComponent<Health> ();
		if (groundCol) {
			defDFric = groundCol.material.dynamicFriction;
			defSFric = groundCol.material.staticFriction;
		}
		if (!rb) {
			rb = GetComponent<Rigidbody>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<Renderer> () && Time.time > defColourTime && HP.health != 0)
			revertColour ();
		if (inSomething && inSomething.tag != "HitZone") {
			rb.AddForce ((transform.position - inSmthPos) * repelForce);
			if (groundCol) {
				groundCol.material.staticFriction = 1;
				groundCol.material.dynamicFriction = 1;
			}
		} else if (GroundCheck.chckdist () && disableWalk) {

			if (groundCol) {
				groundCol.material.staticFriction = defSFric;
				groundCol.material.dynamicFriction = defDFric;
			}
		} else if (GroundCheck.chckdist ()) {
			if (groundCol) {
				groundCol.material.staticFriction = 1;
				groundCol.material.dynamicFriction = 1;
			}
		}

		if(disableWalk && walkRecoverTime < Time.time - 0.25f)
		{
			walkRecoverTime = Time.time + minWalkRecoverTime;
		}
		if(disableWalk && Time.time >= walkRecoverTime && GetComponent<Rigidbody> ().velocity.y < notFallingThreshold && GroundCheck.chckdist ())
		{
			disableWalk = false;
		}
		if (disableTurnTime < Time.time) {
			disableTurn = false;
		} else {
			disableTurn = true;
		}
	}

	void OnTriggerEnter(Collider col) {
		if(col.tag != "NoPush" && col.tag != "HitZone")
		{
			inSomething = col.transform;
			inSmthPos = transform.position;
			inSmthPos.x = inSomething.position.x;
			inSmthPos.z = inSomething.position.z;
		}
	}

	void OnTriggerExit() {
		inSomething = null;
	}

	void revertColour() {
		GetComponent<Renderer> ().material.color = defColour;
	}
}
