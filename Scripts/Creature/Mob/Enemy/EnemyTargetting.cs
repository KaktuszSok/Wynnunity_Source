using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyTargetting : MonoBehaviour {

	public Enemy Mob;
	public Transform player;
	public Vector3 playerPos;
	public Vector3 localVel;
	public float ViewRange = 16;
	private float nextCheckTime;
	// Use this for initialization
	void Start () {
		if (!Mob && transform.root.GetComponent<Enemy> ())
			Mob = transform.root.GetComponent<Enemy>();
	}
	
	// Update is called once per frame
	void Update () {
		if (player) {
			Mob.player = player.GetComponent<Player> ();
			if (player.root.GetComponent<Health> ().health == 0)
				player = null;
		}
		if (player && Mob.GetComponent<Health>().health != 0 && !Mob.disableWalk) {
			playerPos = player.position;
			playerPos.y = Mob.transform.position.y;
			if(!Mob.disableTurn)
			{
				Mob.transform.LookAt (playerPos);
			}
			if(Vector3.Distance(Mob.transform.position, player.position) > Mob.minDist && !Mob.inSomething)
			{
				localVel = Quaternion.FromToRotation(transform.forward, Vector3.forward) * Mob.rb.velocity;
				localVel.y = 0;
				Mob.rb.AddForce(transform.right*-localVel.x + transform.forward*Mathf.Clamp (Mob.trueSpeed, 0, Mathf.Clamp (Mob.trueSpeed - localVel.z, 0, Mob.trueSpeed)), ForceMode.VelocityChange);
				if(!Mob.WalkAnim.isPlaying && Mob.trueSpeed != 0)
					Mob.WalkAnim.Play ();
			}
		} else if(Mob.GetComponent<Health>().health != 0 && !Mob.disableWalk)
		{
			Vector3 changeVel = Mob.rb.velocity;
			changeVel.y = 0;
			Mob.rb.AddForce(-changeVel, ForceMode.VelocityChange);
		}

		if (Time.time >= nextCheckTime) {
			nextCheckTime = Time.time + 0.25f;
			float Closest = ViewRange + 1;
			Player ClosestPlayer = null;
			foreach(Player Player in PlayerManager.Players) {
				if(Player.health.health != 0) {
					float Distance = Vector3.Distance(Player.transform.position, transform.position);
					if(Distance > 128) {
						Destroy (transform.root.gameObject);
					}
					if(Distance <= ViewRange) {
						if(Distance < Closest) {
							Closest = Distance;
							ClosestPlayer = Player;
						}
					}
				}
			}
			if(ClosestPlayer != null)
				player = ClosestPlayer.transform;
			else
				player = null;
		}
	}
}
