using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA 
{
	public class CameraManager : MonoBehaviour 
	{
		public bool lockOn;
		public float followSpeed = 9f;
		public float mouseSpeed = 2f;
		public float controllerSpeed = 7f;

		public Transform target;
		public EnemyTarget lockOnTarget;
		public Transform lockOnTransform;

		[HideInInspector] public Transform pivot;
		[HideInInspector] public Transform camTrans;
		StateManager states;

		float turnSmoothing = .1f;
		public float minAngle = -35f;
		public float maxAngle = 35f;

		float smoothX;
		float smoothY;
		float smoothXVelocity;
		float smoothYVelocity;
		public float lookAngle;
		public float tiltAngle;

		bool usedRightAxis;

		bool changeTargetLeft;
		bool changeTargetRight;

		public void Init (StateManager st)
		{
			target = st.transform;
			states = st;

			camTrans = Camera.main.transform;
			pivot = camTrans.parent;
		}

		public void Tick (float d)
		{
			float h = Input.GetAxis ("Mouse X");
			float v = Input.GetAxis ("Mouse Y");

			float c_h = Input.GetAxis ("RightAxis Y");
			float c_v = Input.GetAxis ("RightAxis Y");

			float targetSpeed = mouseSpeed;

			changeTargetLeft = Input.GetKey (KeyCode.V);
			changeTargetRight = Input.GetKey (KeyCode.B);

			if (lockOnTarget != null) {
				if (lockOnTransform == null) {
					lockOnTransform = lockOnTarget.GetTarget ();
					states.lockOnTransform = lockOnTransform;
				}

				if (Mathf.Abs (c_h) > .6f) {
					if (!usedRightAxis) {
						lockOnTransform = lockOnTarget.GetTarget ((c_h > 0));
						states.lockOnTransform = lockOnTransform;
						usedRightAxis = true;
					}
				}

				if (changeTargetLeft || changeTargetRight) {
					lockOnTransform = lockOnTarget.GetTarget (changeTargetLeft);
					states.lockOnTransform = lockOnTransform;
				}
			}

			if (usedRightAxis) {
				if (Mathf.Abs (c_h) < .6f) {
					usedRightAxis = false;
				}
			}

			if (c_h != 0 || c_v != 0) {
				h = c_h;
				v = -c_v;
				targetSpeed = controllerSpeed;
			}

			FollowTarget (d);
			HandleRotations (d, h, v, targetSpeed);
		}

		void FollowTarget (float d)
		{
			float speed = d * followSpeed;
			Vector3 targetPosition = Vector3.Lerp (transform.position, target.position, speed);
			transform.position = targetPosition;
		}

		void HandleRotations (float d, float h, float v, float targetSpeed)
		{
			if (turnSmoothing > 0) {
				smoothX = Mathf.SmoothDamp (smoothX, h, ref smoothXVelocity, turnSmoothing);
				smoothY = Mathf.SmoothDamp (smoothY, v, ref smoothYVelocity, turnSmoothing);
			} 
			else {
				smoothX = h;
				smoothY = v;
			}

			tiltAngle -= smoothY * targetSpeed;
			tiltAngle = Mathf.Clamp (tiltAngle, minAngle, maxAngle);
			pivot.localRotation = Quaternion.Euler (tiltAngle, 0, 0);

			if (lockOn && lockOnTarget != null) {
				Vector3 targetDir = lockOnTransform.position - transform.position;
				targetDir.Normalize ();
				//targetDir.y = 0;

				if (targetDir == Vector3.zero)
					targetDir = transform.forward;
				Quaternion targetRot = Quaternion.LookRotation (targetDir);
				transform.rotation = Quaternion.Slerp (transform.rotation, targetRot, d * 9);
				lookAngle = transform.eulerAngles.y;
				return;
			}
				
			lookAngle += smoothX * targetSpeed;
			transform.rotation = Quaternion.Euler (0, lookAngle, 0);
		}

		public static CameraManager singleton;

		void Awake ()
		{
			if (singleton == null) {
				singleton = this;
			}
		}
	}
}