using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour {

	public Vector3 Destination;
	private HSBColor HSB;
	public bool activated = true;
	public bool running = false;
	public Texture texture;
	private Renderer rend;
	private Material mat;
	public bool colourRainbow = false;
	public float refreshDelay = 2;
	public float enableDistance = 32;
	public Camera cam;
	public bool relative;
	public bool keepVelocity;
	
	void Start() {
		rend = GetComponent<Renderer> ();
		mat = rend.material;
		mat.SetTexture ("_EmissionMap", texture);
	}

	void OnTriggerEnter (Collider col) {
		if (col.tag == "Player" && activated) {
			if(!relative) {
				col.transform.root.position = Destination;
				if(!keepVelocity) {
					col.transform.root.GetComponent<Rigidbody>().velocity = Vector3.zero;
					col.transform.root.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
				}
			} else {
				col.transform.root.Translate (Destination, Space.World);
				if(!keepVelocity) {
					col.transform.root.GetComponent<Rigidbody>().velocity = Vector3.zero;
					col.transform.root.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
				}
			}
		}
	}

	void Update () {

		float Closest = enableDistance + 1;
		foreach(Player Player in PlayerManager.Players) {
			float Distance = Vector3.Distance(Player.transform.position, transform.position);
			if(Distance <= enableDistance) {
				if(Distance < Closest) {
					Closest = Distance;
				}
			}
		}

		if (Closest <= enableDistance) {
			activated = true;
			if(cam) {
				cam.enabled = true;
			}
		} else {
			activated = false;
			if(cam) {
				cam.enabled = false;
			}
		}

		if (!running && activated && colourRainbow) {
			StartCoroutine (HueTransition());
		}
		if (!activated && rend.enabled) {
			rend.enabled = false;
		} else if(activated && !rend.enabled) {
			rend.enabled = true;
		}
	}

	public IEnumerator HueTransition () {
		running = true;
		HSB = new HSBColor (Color.red);
		HSB.h = 0;
		float emissionHue = 0f;
		while (activated) {
			emissionHue = Mathf.PingPong (Time.time, 1f);
			HSB.h = emissionHue;
			Color colour = HSBColor.ToColor(HSB);
			mat.SetColor ("_EmissionColor", colour);
			yield return new WaitForEndOfFrame();
		}
		running = false;
	}
}
