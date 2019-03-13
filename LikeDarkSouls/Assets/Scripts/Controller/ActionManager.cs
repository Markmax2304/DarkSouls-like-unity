using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA 
{
	public class ActionManager : MonoBehaviour 
	{
		public List<Action> actionSlots = new List<Action> ();

		public ItemAction consumbleItem;

		StateManager states;

		public void Init (StateManager st)
		{
			states = st;

			UpdateActionsOneHanded ();
		}

		public void UpdateActionsOneHanded ()
		{
			EmptyAllSlots ();

			StaticFunction.DeepCopyAction (states.inventoryManager.rightHandWeapon.instance, ActionInput.rb, ActionInput.rb, actionSlots);
			StaticFunction.DeepCopyAction (states.inventoryManager.rightHandWeapon.instance, ActionInput.rt, ActionInput.rt, actionSlots);

			if (states.inventoryManager.hasLeftHandWeapon) {
				StaticFunction.DeepCopyAction (states.inventoryManager.leftHandWeapon.instance, ActionInput.rb, ActionInput.lb, actionSlots, true);
				StaticFunction.DeepCopyAction (states.inventoryManager.leftHandWeapon.instance, ActionInput.rt, ActionInput.lt, actionSlots, true);
			} else {
				StaticFunction.DeepCopyAction (states.inventoryManager.rightHandWeapon.instance, ActionInput.lb, ActionInput.lb, actionSlots);
				StaticFunction.DeepCopyAction (states.inventoryManager.rightHandWeapon.instance, ActionInput.lt, ActionInput.lt, actionSlots);
			}
		}

		public void UpdateActionsTwoHanded ()
		{
			EmptyAllSlots ();
			Weapon w = states.inventoryManager.rightHandWeapon.instance;
			for (int i = 0; i < w.twoHandedActions.Count; i++) {
				Action a = StaticFunction.GetAction (w.twoHandedActions [i].input, actionSlots);
				a.targetAnim = w.twoHandedActions [i].targetAnim;
				a.type = w.twoHandedActions [i].type;
			}
		}

		void EmptyAllSlots ()
		{
			for (int i = 0; i < 4; i++) {
				Action a = StaticFunction.GetAction ((ActionInput)i, actionSlots);
				a.targetAnim = null;
				a.mirror = false;
				a.type = ActionType.attack;
			}
		}

		ActionManager()
		{
			for (int i = 0; i < 4; i++) {
				Action a = new Action ();
				a.input = (ActionInput)i;
				actionSlots.Add (a);
			}
		}

		public Action GetActionSlot (StateManager st)
		{
			ActionInput a_input = GetActionInput (st);
			return StaticFunction.GetAction (a_input, actionSlots);
		}

		public ActionInput GetActionInput (StateManager st)
		{
			if (st.rb)
				return ActionInput.rb;
			if (st.lb)
				return ActionInput.lb;
			if (st.rt)
				return ActionInput.rt;
			if (st.lt)
				return ActionInput.lt;

			return ActionInput.rb;
		}

		public bool IsLeftHandSlot (Action slot) 
		{
			return (slot.input == ActionInput.lb || slot.input == ActionInput.lt);
		}
	}

	public enum ActionInput 
	{ 
		rb, lb, rt, lt, x
	}

	public enum ActionType
	{
		attack, block, spells, parry
	}

	[System.Serializable]
	public class Action 
	{
		public ActionInput input;
		public ActionType type;
		public string targetAnim;
		public bool mirror = false;
		public bool canBeParried = true;
		public bool changeSpeed = false;
		public float animSpeed = 1f;
		public bool canParry = false;
		public bool canBackstab = false;

		[HideInInspector] public float parryMultiplier;
		[HideInInspector] public float backstabMultiplier;

		public bool overrideDamageAnim;
		public string damageAnim;

		public WeaponStats weaponStats;
	}

	[System.Serializable]
	public class ItemAction
	{
		public string targetAnim;
		public string itemID;
	}
}
