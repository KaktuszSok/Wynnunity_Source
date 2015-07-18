using UnityEngine;
using System.Collections;

public class Walk : MonoBehaviour {

	public Player player;
	public float baseSpeed = 3.5f;
	public float sprintSpeed = 4.2f;
	public float speed = 3.5f;
	public float jumpHeight = 1.25f;
	public Rigidbody rb;
	public GroundDetect GD;
	public float minJumpDelay = 0.05f;
	public float nextJump = 0;

	public float ButtonCooler;
	public float ButtonCount;
	public float SprintTapTime = 0.375f;

	public float defFOV = 70;
	public float sprintFOV = 80;

	public bool moving = false;

	private float defFric = 1;

	// Use this for initialization
	void Start () {
		if (!player)
			player = GetComponent<Player> ();
		if(!rb)
			rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (moving) {
			GetComponent<Collider> ().material.staticFriction = 0;
			GetComponent<Collider> ().material.dynamicFriction = 0;
		} else {
			GetComponent<Collider> ().material.staticFriction = defFric;
			GetComponent<Collider> ().material.dynamicFriction = defFric;
		}
		Vector3 localVel = Quaternion.FromToRotation(transform.forward, Vector3.forward) * rb.velocity;
		float ClampedVelchangeNX = Mathf.Clamp (-speed - localVel.x, -speed, 0);
		float ClampedVelchangePX = Mathf.Clamp (speed - localVel.x, 0, speed);
		float ClampedVelchangeNZ = Mathf.Clamp (-speed - localVel.z, -speed, 0);
		float ClampedVelchangePZ = Mathf.Clamp (speed - localVel.z, 0, speed);
		if(moving)
			rb.AddRelativeForce (Mathf.Clamp (Input.GetAxis ("Horizontal") * speed, ClampedVelchangeNX, ClampedVelchangePX), 0, Mathf.Clamp (Input.GetAxis ("Vertical") * speed, ClampedVelchangeNZ, ClampedVelchangePZ), ForceMode.VelocityChange);

		if(Input.GetKey("space") && GD.chckdist() && Time.time >= nextJump)
		{
			nextJump = Time.time + minJumpDelay;
			rb.velocity = Vector3.zero;
			rb.AddForce(Vector3.up*Mathf.Sqrt(2*-Physics.gravity.y*jumpHeight), ForceMode.VelocityChange);
		}

		if (Input.GetKeyDown ("w")){
			
			if (ButtonCooler > 0 && ButtonCount == 1 && player.mana.MP > player.mana.StopSprintThreshold){
				speed = sprintSpeed;
				Camera.main.fieldOfView = sprintFOV;
			}else{
				ButtonCooler = SprintTapTime; 
				ButtonCount += 1;
				speed = baseSpeed;
				Camera.main.fieldOfView = defFOV;
			}
		}

		if (player.mana.MP <= player.mana.StopSprintThreshold) {
			ButtonCooler = 0;
			speed = baseSpeed;
			Camera.main.fieldOfView = defFOV;
		}
		
		if ( ButtonCooler > 0 )
		{
			
			ButtonCooler -= 1 * Time.deltaTime ;
			
		}else{
			ButtonCount = 0 ;
		}

		if(Input.GetKeyUp ("w")) {
			Camera.main.fieldOfView = defFOV;
			speed = baseSpeed;
		}

		if (Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0) {
			moving = true;
		} else {
			moving = false;
		}
	}
}
