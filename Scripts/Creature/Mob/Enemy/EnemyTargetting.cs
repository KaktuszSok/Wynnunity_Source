using UnityEngine;
using System.Collections;

public class EnemyTargetting : MonoBehaviour {

	public Enemy Mob;
	public Transform player;
	public Vector3 playerPos;
	// Use this for initialization
	void Start () {
		if (!Mob && transform.root.GetComponent<Enemy> ())
			Mob = transform.root.GetComponent<Enemy>();
	}
	
	// Update is called once per frame
	void Update () {
		if(player)
			Mob.player = player.GetComponent<Player> ();
		if (Player.Health.dead)
			player = null;
		if (player && Mob.GetComponent<Health>().health != 0 && !Mob.disableWalk) {
			playerPos = player.position;
			playerPos.y = Mob.transform.position.y;
			if(!Mob.disableTurn)
			{
				Mob.transform.LookAt (playerPos);
			}
			if(Vector3.Distance(Mob.transform.position, player.position) > Mob.minDist)
			{
				Vector3 changeVel = Mob.rb.velocity;
				changeVel.y = 0;
				Mob.rb.AddForce(-changeVel, ForceMode.VelocityChange);
				Mob.rb.AddRelativeForce(Vector3.forward*Mob.speed, ForceMode.VelocityChange);
			}
		} else if(Mob.GetComponent<Health>().health != 0 && !Mob.disableWalk)
		{
			Vector3 changeVel = Mob.rb.velocity;
			changeVel.y = 0;
			Mob.rb.AddForce(-changeVel, ForceMode.VelocityChange);
		}
	}

	void OnTriggerEnter (Collider col) {
		if (col.transform.tag == "Player")
			player = col.transform;
	}

	void OnTriggerExit (Collider col) {
		if (col.transform.tag == "Player")
			player = null;
	}
}
