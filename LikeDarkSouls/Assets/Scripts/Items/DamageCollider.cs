using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
	public class DamageCollider : MonoBehaviour
	{
		StateManager states;

		public void Init(StateManager st)
		{
			states = st;
		}

		void OnTriggerEnter (Collider coll)
		{
			EnemyStates enStates = coll.transform.GetComponent<EnemyStates> ();

			if (enStates == null)
				return;

			enStates.DoDamage (states.currentAction);
		}
	}
}
