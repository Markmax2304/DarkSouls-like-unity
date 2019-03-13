using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
	public class ParryCollider : MonoBehaviour 
	{
		StateManager states;
		EnemyStates enStates;

		public float maxTimer = .6f;
		float timer = 0;

		void Update ()
		{
			if (states) {
				timer += states.delta;

				if (timer > maxTimer) {
					gameObject.SetActive (false);
					timer = 0;
				}
			}

			if (enStates) {
				timer += enStates.delta;

				if (timer > maxTimer) {
					gameObject.SetActive (false);
					timer = 0;
				}
			}
		}

		public void InitPlayer (StateManager st)
		{
			states = st;
		}

		public void InitEnemy (EnemyStates est)
		{
			enStates = est;
		}

		void OnTriggerEnter (Collider coll)
		{
			/*DamageCollider dm = coll.GetComponent<DamageCollider> ();
			if (dm == null)
				return;*/

			if (states) {
				EnemyStates e_st = coll.transform.GetComponentInParent<EnemyStates> ();

				if (e_st == null)
					return;

				e_st.CheckForParry (transform.root, states);
			}

			if (enStates) {
				StateManager st = coll.transform.GetComponentInParent<StateManager> ();

				if (st == null)
					return;


			}
		}
	}
}
