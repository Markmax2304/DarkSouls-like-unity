using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
	public class EnemyStates : MonoBehaviour
	{
		public int health;

		public CharacterStats characterStats;

		public bool canBeParried = true;
		public bool parryIsOn = true;
		//public bool doParry = false;
		public bool isInvincible;
		public bool dontDoAnything;
		public bool canMove;
		public bool isDead;

		public float delta;
		public float poiseDegrade = 2;

		public StateManager parriedBy;

		public Animator anim;
		public Rigidbody rigid;
		EnemyTarget enTarget;
		AnimatorHook a_hook;

		List<Rigidbody> ragdollRigids = new List<Rigidbody>();
		List<Collider> ragdollColliders = new List<Collider>();

		float timer = 0;

		void Start ()
		{
			health = 1000;
			anim = GetComponentInChildren<Animator> ();
			enTarget = GetComponent<EnemyTarget> ();
			enTarget.Init (this);

			rigid = GetComponent<Rigidbody> ();

			a_hook = anim.GetComponent<AnimatorHook> ();
			if(a_hook == null)
				a_hook = anim.gameObject.AddComponent<AnimatorHook> ();
			a_hook.Init (null, this);

			InitRagdoll ();
			parryIsOn = false;
		}

		void InitRagdoll ()
		{
			Rigidbody[] rigs = GetComponentsInChildren<Rigidbody> ();
			for (int i = 0; i < rigs.Length; i++) {
				if (rigs [i] == rigid)
					continue;

				ragdollRigids.Add (rigs [i]);
				rigs [i].isKinematic = true;

				Collider coll = rigs [i].gameObject.GetComponent<Collider> ();
				coll.isTrigger = true;
				ragdollColliders.Add (coll);
			}
		}

		public void EnableRagdoll ()
		{
			for (int i = 0; i < ragdollRigids.Count; i++) {
				ragdollRigids [i].isKinematic = false;
				ragdollColliders [i].isTrigger = false;
			}

			Collider controllerColleder = rigid.gameObject.GetComponent<Collider> ();
			controllerColleder.enabled = false;
			rigid.isKinematic = true;

			StartCoroutine (CloseAnimator ());
		}

		IEnumerator CloseAnimator ()
		{
			yield return new WaitForEndOfFrame ();
			anim.enabled = false;
			this.enabled = false;
		}

		void Update ()
		{
			delta = Time.deltaTime;
			canMove = anim.GetBool (StaticStrings.canMove);

			if (dontDoAnything) {
				dontDoAnything = !canMove;

				return;
			}

			if (health <= 0) {
				if (!isDead) {
					isDead = true;
					EnableRagdoll ();
				}
			}

			if (isInvincible) {
				isInvincible = canMove;
			}

			if (parriedBy != null && !parryIsOn) {
				//parriedBy.parryTarget = null;
				parriedBy = null;

			}

			if (canMove) {
				parryIsOn = false;
				anim.applyRootMotion = false;

				//Debug
				timer += Time.deltaTime;
				if (timer > 3) {
					DoAction ();
					timer = 0;
				}
			}

			characterStats.poise -= delta;
			if (characterStats.poise < 0)
				characterStats.poise = 0;
		}

		void DoAction ()
		{
			anim.Play ("oh_attack_1");
			anim.applyRootMotion = true;
			anim.SetBool (StaticStrings.canMove, false);
		}

		public void DoDamage (Action a)
		{
			if (isInvincible)
				return;

			int damage = StatsCalculation.CalculateBaseDamage (a.weaponStats, characterStats);

			characterStats.poise += damage;
			health -= damage;

			if (canMove || characterStats.poise > 100) {
				if (a.overrideDamageAnim) {
					anim.Play (a.damageAnim);
				} else {
					int rand = Random.Range (0, 100);
					string tA = (rand > 50) ? StaticStrings.damage1 : StaticStrings.damage2;
					anim.Play (tA);
				}
			}

			Debug.Log ("Damage " + damage + "Poise " + characterStats.poise);

			isInvincible = true;

			anim.applyRootMotion = true;
			anim.SetBool (StaticStrings.canMove, false);
		}

		public void CheckForParry (Transform target, StateManager states)
		{
			if (!canBeParried || !parryIsOn || isInvincible)
				return;

			Vector3 dir = transform.position - target.position;
			dir.Normalize ();
			float dot = Vector3.Dot (target.forward, dir);
			if (dot < 0)
				return;

			isInvincible = true;
			anim.Play (StaticStrings.attack_interrupt);
			anim.applyRootMotion = true;
			anim.SetBool (StaticStrings.canMove, false);
			//states.parryTarget = this;
			parriedBy = states;
		}

		public void IsGettingParried (Action a)
		{
			int damage = StatsCalculation.CalculateBaseDamage (a.weaponStats, characterStats, a.parryMultiplier);

			health -= damage;
			dontDoAnything = true;
			anim.SetBool (StaticStrings.canMove, false);
			anim.Play (StaticStrings.parry_received);
		}

		public void IsGettingBackstabbed (Action a)
		{
			int damage = StatsCalculation.CalculateBaseDamage (a.weaponStats, characterStats, a.backstabMultiplier);

			health -= damage;
			dontDoAnything = true;
			anim.SetBool (StaticStrings.canMove, false);
			anim.Play (StaticStrings.backstabbed);
		}
	}
}
