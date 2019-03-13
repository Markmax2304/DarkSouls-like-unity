using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
	public static class StaticFunction 
	{
		public static void DeepCopyWeapon (Weapon from, Weapon to)
		{
			to.icon = from.icon;
			to.oh_idle = from.oh_idle;
			to.th_idle = from.th_idle;

			to.actions = new List<Action> ();
			for (int i = 0; i < from.actions.Count; i++) {
				Action a = new Action ();
				a.weaponStats = new WeaponStats ();
				DeepCopyActionToAction (a, from.actions [i]);
				to.actions.Add (a);
			}

			to.twoHandedActions = new List<Action> ();
			for (int i = 0; i < from.twoHandedActions.Count; i++) {
				Action a = new Action ();
				a.weaponStats = new WeaponStats ();
				DeepCopyActionToAction (a, from.twoHandedActions [i]);
				to.twoHandedActions.Add (a);
			}

			to.parryMultiplier = from.parryMultiplier;
			to.backstabMultiplier = from.backstabMultiplier;
			to.leftHandMirror = from.leftHandMirror;
			to.modelPrefab = from.modelPrefab;
			to.l_model_pos = from.l_model_pos;
			to.r_model_pos = from.r_model_pos;
			to.l_model_eulers = from.l_model_eulers;
			to.r_model_eulers = from.r_model_eulers;
			to.model_scale = from.model_scale;

		}

		public static void DeepCopyActionToAction (Action a, Action w_a)
		{
			a.targetAnim = w_a.targetAnim;
			a.type = w_a.type;
			a.canBeParried = w_a.canBeParried;
			a.animSpeed = w_a.animSpeed;
			a.changeSpeed = w_a.changeSpeed;
			a.canBackstab = w_a.canBackstab;
			a.overrideDamageAnim = w_a.overrideDamageAnim;
			a.damageAnim = w_a.damageAnim;

			DeepCopyWeaponStats (w_a.weaponStats, a.weaponStats);
		}

		public static void DeepCopyAction (Weapon w, ActionInput inp, ActionInput assign, List<Action> actionList, bool isLeftHand = false)
		{
			Action a = GetAction (assign, actionList);
			Action w_a = w.GetAction (w.actions, inp);

			if (w_a == null)
				return;

			if (isLeftHand) {
				a.mirror = true;
			}

			DeepCopyActionToAction (a, w_a);
		}

		public static void DeepCopyWeaponStats (WeaponStats from, WeaponStats to)
		{
			to.physical = from.physical;
			to.slash = from.slash;
			to.strike = from.strike;
			to.thrust = from.thrust;
			to.magic = from.magic;
			to.lighting = from.lighting;
			to.fire = from.fire;
			to.dark = from.dark;
		}

		public static Action GetAction (ActionInput input, List<Action> actionSlots)
		{
			for (int i = 0; i < actionSlots.Count; i++) {
				if (actionSlots [i].input == input)
					return actionSlots [i];
			}

			return null;
		}
	}
}
