using UnityEngine;
using System.Collections;

public class CharTriggerAnim : MonoBehaviour {

	Animator anim;
	public string animationIn;
	public string animationOut;

	void Start () {
		anim = GetComponent<Animator>();
	}

	void OnTriggerEnter (Collider col) {
		if (!enabled) return;
		if (animationIn!="" && col.CompareTag("Player")) 
			anim.CrossFade(animationIn,0.24f);
	}

	void OnTriggerExit (Collider col) {
		if (!enabled) return;
		if (animationOut!="" && col.CompareTag("Player")) 
			anim.CrossFade(animationOut,0.24f);
	}

}
