using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA 
{
	public class InputHandler : MonoBehaviour 
	{
		float vertical;
		float horizontal;
		bool a_input;
		bool b_input;
		bool x_input;
		bool y_input;

		bool rb_input;
		float rt_axis;
		bool rt_input;
		bool lb_input;
		float lt_axis;
		bool lt_input;

		bool leftAxis_down;
		bool rightAxis_down;

		float b_timer;
		float rt_timer;
		float lt_timer;

		float delta;

		StateManager states;
		CameraManager camManager;

		void Start () 
		{
			UI.QuickSlot.singleton.Init ();

			states = GetComponent<StateManager> ();
			states.Init ();

			camManager = CameraManager.singleton;
			camManager.Init (states);
		}

		void FixedUpdate () 
		{
			delta = Time.fixedDeltaTime;
			GetInput ();
			UpdateStates ();
			states.FixedTick (delta);
			camManager.Tick (delta);
		}

		void Update ()
		{
			delta = Time.deltaTime;
			states.Tick (delta);
			ResetInputNStates ();
		}
			
		void GetInput ()
		{
			vertical = Input.GetAxis (StaticStrings.Vertical);
			horizontal = Input.GetAxis (StaticStrings.Horizontal);
			b_input = Input.GetButton (StaticStrings.B);
			a_input = Input.GetButton (StaticStrings.A);
			x_input = Input.GetButton (StaticStrings.X);
			y_input = Input.GetButtonUp (StaticStrings.Y);

			rb_input = Input.GetButton (StaticStrings.RB);
			rt_axis = Input.GetAxis (StaticStrings.RT);
			rt_input = (rt_axis != 0) ? true : false;

			lb_input = Input.GetButton (StaticStrings.LB);
			lt_axis = Input.GetAxis (StaticStrings.LT);
			lt_input = (lt_axis != 0) ? true : false;

			rightAxis_down = Input.GetButtonUp (StaticStrings.L) || Input.GetKeyUp(KeyCode.T);

			if (b_input)
				b_timer += delta;
		}

		void UpdateStates ()
		{
			states.horizontal = horizontal;
			states.vertical = vertical;

			Vector3 v = vertical * camManager.transform.forward;
			Vector3 h = horizontal * camManager.transform.right;
			states.moveDir = (v + h).normalized;
			float m = Mathf.Abs (horizontal) + Mathf.Abs (vertical);
			states.moveAmount = Mathf.Clamp01 (m);

			if (x_input)
				b_input = false;

			if (b_input && b_timer > .5f) {
				states.run = (states.moveAmount > 0);
			}

			if (!b_input && b_timer > 0 && b_timer < .5f) {
				states.rollInput = true;
			}

			states.itemInput = x_input;
			states.rb = rb_input;
			states.rt = rt_input;
			states.lb = lb_input;
			states.lt = lt_input;

			if (y_input) {
				states.isTwoHanded = !states.isTwoHanded;
				states.HandleTwoHanded ();
			}

			if (states.lockOnTarget != null) {
				if (states.lockOnTarget.enStates.isDead) {
					states.lockOn = false;
					states.lockOnTarget = null;
					states.lockOnTransform = null;
					camManager.lockOn = false;
					camManager.lockOnTarget = null;
				}
			} else {
				states.lockOn = false;
				states.lockOnTarget = null;
				states.lockOnTransform = null;
				camManager.lockOn = false;
				camManager.lockOnTarget = null;
			}

			if (rightAxis_down) {
				states.lockOn = !states.lockOn;

				states.lockOnTarget = EnemyManager.singleton.GetEnemy (transform.position);
				if (states.lockOnTarget == null) {
					states.lockOn = false;
				}
					
				camManager.lockOnTarget = states.lockOnTarget;
				states.lockOnTransform = states.lockOnTarget.GetTarget ();
				camManager.lockOnTransform = states.lockOnTransform;
				camManager.lockOn = states.lockOn;
			}
		}

		void ResetInputNStates ()
		{
			if (!b_input)
				b_timer = 0;

			if (states.rollInput)
				states.rollInput = false;

			if (states.run)
				states.run = false;
		}
	}
}