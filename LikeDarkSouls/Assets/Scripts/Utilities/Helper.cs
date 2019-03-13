using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
	public class Helper : MonoBehaviour 
	{
		[Range(-1, 1)] public float vertical;
		[Range(-1, 1)] public float horizontal;

		public bool playAnim;
		public string[] oh_attacks;
		public string[] th_attacks;

		public bool twoHanded;
		public bool enableRootMotion;
		public bool useItem;
		public bool interacting;
		public bool lockon;

		Animator anim;

		void Start ()
		{
			anim = GetComponent<Animator> ();
		}

		void Update ()
		{
			enableRootMotion = !anim.GetBool ("canMove");
			anim.applyRootMotion = enableRootMotion;
			interacting = anim.GetBool ("interacting");

			if (enableRootMotion)
				return;

			if (!lockon) {
				horizontal = 0;
				vertical = Mathf.Clamp01 (vertical);
			}

			anim.SetBool ("lockon", lockon);

			if (useItem) {
				anim.Play ("use_item");
				useItem = false;
			}

			if (interacting) {
				playAnim = false;
				vertical = Mathf.Clamp (vertical, 0, .5f);
			}

			anim.SetBool ("two_handed", twoHanded); 

			if (playAnim) {

				string targetAnim;
				if (twoHanded) {
					int r = Random.Range (0, th_attacks.Length);
					targetAnim = th_attacks [r];
				} else {
					int r = Random.Range (0, oh_attacks.Length);
					targetAnim = oh_attacks [r];
				}

				if (vertical > .5f) {
					targetAnim = "oh_attack_3";
				}

				vertical = 0f;
				anim.CrossFade (targetAnim, 0.2f);
				//anim.SetBool ("canMove", false);
				//enableRootMotion = true;
				playAnim = false;
			}

			anim.SetFloat ("vertical", vertical);
			anim.SetFloat ("horizontal", horizontal);

		}
	}
}