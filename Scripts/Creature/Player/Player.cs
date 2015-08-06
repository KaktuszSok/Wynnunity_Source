using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public static Walk Walk;
	public static Punch Punch;
	public static Health Health;
	public static Mana Mana;
	public static MouseLook TurnBody;
	public static MouseLook TurnHead;
	public static LayerMask EnemyLOS;
	public static LayerMask SpawnLOS;
	public static float CamRevertTime;

	public Walk walk;
	public Punch punch;
	public Health health;
	public Mana mana;
	public MouseLook TB;
	public MouseLook TH;
	public LayerMask enemyLOS;
	public LayerMask spawnLOS;

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (gameObject);
		PlayerManager.Players.Add (this);
		Walk = GetComponent<Walk> ();
		Punch = GetComponent<Punch> ();
		Health = GetComponent<Health> ();
		Mana = GetComponent<Mana> ();
		TurnBody = TB;
		TurnHead = TH;
		EnemyLOS = enemyLOS;
		SpawnLOS = spawnLOS;

		walk = Walk;
		health = Health;
		punch = Punch;
		mana = Mana;
	}

	void Update() {
		if (Camera.main.transform.parent.localRotation.z != 0 && Time.time > CamRevertTime) {
			Vector3 rotAngle = Camera.main.transform.parent.localEulerAngles;
			rotAngle.z = 0;
			Camera.main.transform.parent.localEulerAngles = rotAngle;
		}
	}
}
