using UnityEngine;
using System.Collections;

public class Effects : MonoBehaviour {

	public Vector2 Slowness;
	public Vector2 Stun;

	void FixedUpdate() {
		if (Slowness.x > 0)
			Slowness.x -= Time.fixedDeltaTime;
		else if (Slowness.y != 0)
			Slowness.y = 0;

		if (Stun.x > 0)
			Stun.x -= Time.fixedDeltaTime;
		else if (Stun.y != 0)
			Stun.y = 0;
	}

	public void Apply(string effect, float time, float power) {
		if (effect == "Slowness") {
			if(power >= Slowness.y) {
				Slowness.x = time;
				Slowness.y = power;
			}
		}
		if (effect == "Stun") {
			if(power >= Stun.y) {
				Stun.x = time;
				Stun.y = power;
			}
		}
	}


}
