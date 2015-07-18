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

	public Walk walk;
	public Punch punch;
	public Health health;
	public Mana mana;
	public MouseLook TB;
	public MouseLook TH;
	public LayerMask enemyLOS;

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (gameObject);
		Walk = GetComponent<Walk> ();
		Punch = GetComponent<Punch> ();
		Health = GetComponent<Health> ();
		Mana = GetComponent<Mana> ();
		TurnBody = TB;
		TurnHead = TH;
		EnemyLOS = enemyLOS;

		walk = Walk;
		health = Health;
		punch = Punch;
		mana = Mana;
	}

	void Update() {

	}
}
