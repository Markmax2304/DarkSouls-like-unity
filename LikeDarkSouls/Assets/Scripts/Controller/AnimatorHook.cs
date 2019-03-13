using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA 
{
	public class AnimatorHook : MonoBehaviour 
	{
		Animator anim;
		StateManager states;
		EnemyStates enStates;
		Rigidbody rigid;

		public float rm_multi;
		bool rolling;
		float roll_t;
		float delta;
		AnimationCurve roll_curve;

		public void Init (StateManager st, EnemyStates eSt)
		{
			states = st;
			if (states != null) {
				anim = st.anim;
				rigid = st.rigid;
				roll_curve = st.roll_curve;
				delta = st.delta;
			}
			enStates = eSt;
			if (enStates != null) {
				anim = eSt.anim;
				rigid = eSt.rigid;
				delta = eSt.delta;
			}
		}

		public void InitForRoll()
		{
			rolling = true;
			roll_t = 0;
		}

		public void CloseRoll ()
		{
			if (!rolling)
				return;

			rm_multi = 1f;
			rolling = false;
		}

		void OnAnimatorMove ()
		{
			if (states == null && enStates == null)
				return;

			if (rigid == null)
				return;

			if (states != null) {
				if (states.canMove)
					return;

				delta = states.delta;
			}

			if (enStates != null) {
				if (enStates.canMove)
					return;

				delta = enStates.delta;
			}

			rigid.drag = 0;

			if (rm_multi == 0)
				rm_multi = 1f;

			if (!rolling) {
				Vector3 delta2 = anim.deltaPosition;
				delta2.y = 0;
				Vector3 v = (delta2 * rm_multi) / delta;
				rigid.velocity = v;
			}
			else {
				roll_t += delta;
				roll_t = Mathf.Min (roll_t, 1f);

				if (states == null)
					return;

				float zValue = roll_curve.Evaluate (roll_t);
				Vector3 v1 = Vector3.forward * zValue;
				Vector3 relative = transform.TransformDirection (v1);
				Vector3 v2 = (relative * rm_multi);
				rigid.velocity = v2;
			}
		}

		public void OpenDamageColliders ()
		{
			if (states) {
				states.inventoryManager.OpenAllDamageColliders ();
			}

			OpenParryFlag ();
		}

		public void CloseDamageColliders ()
		{
			if (states) {
				states.inventoryManager.CloseAllDamageColliders ();
			}

			CloseParryFlag ();
		}

		public void OpenParryCollider ()
		{
			if (states == null)
				return;

			states.inventoryManager.OpenParryCollider ();
		}

		public void CloseParryCollider ()
		{
			if (states == null)
				return;

			states.inventoryManager.CloseParryCollider ();
		}

		public void OpenParryFlag ()
		{
			if (states) {
				states.parryIsOn = true;
			}

			if (enStates) {
				enStates.parryIsOn = true;
			}
		}

		public void CloseParryFlag ()
		{
			if (states) {
				states.parryIsOn = false;
			}

			if (enStates) {
				enStates.parryIsOn = false;
			}
		}
	}
}
