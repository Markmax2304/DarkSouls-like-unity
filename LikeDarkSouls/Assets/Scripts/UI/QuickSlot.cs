using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA.UI
{
	public class QuickSlot : MonoBehaviour 
	{
		public static QuickSlot singleton;

		void Awake ()
		{
			singleton = this;
		}

		public List<QSlots> q_slots = new List<QSlots>();

		public void Init ()
		{
			ClearIcons ();
		}

		public void ClearIcons ()
		{
			for (int i = 0; i < q_slots.Count; i++) {
				q_slots [i].icon.gameObject.SetActive (false);
			}
		}

		public void UpdateSlot (QSlotType stype, Sprite i)
		{
			QSlots q = GetSlot (stype);
			q.icon.sprite = i;
			q.icon.gameObject.SetActive (true);
		}

		public QSlots GetSlot (QSlotType t)
		{
			for (int i = 0; i < q_slots.Count; i++) {
				if (q_slots [i].type == t)
					return q_slots [i];
			}

			return null;
		}
	}

	public enum QSlotType
	{
		rh, lh, item, spell
	}

	[System.Serializable]
	public class QSlots
	{
		public Image icon;
		public QSlotType type;
	}
}
