using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;
using System.IO;
using System.Xml;
using System;
using System.Text;

namespace SA.Utilities
{
	[ExecuteInEditMode]
	public class XMLToResources : MonoBehaviour 
	{
		public bool load;

		public ResourcesManager resourcesManager;
		public string weaponFileName = "item_database.xml";

		void Update ()
		{
			if (!load)
				return;
			load = false;

			LoadWeaponData (resourcesManager);
		}

		public void LoadWeaponData (ResourcesManager rm)
		{
			string filePath = StaticStrings.SaveLocation () + StaticStrings.itemFolder;
			filePath += weaponFileName;

			XmlDocument doc = new XmlDocument ();
			doc.Load (filePath);

			foreach (XmlNode w in doc.DocumentElement.SelectNodes("//weapon")) {
				Weapon _w = new Weapon ();
				_w.actions = new List<Action> ();
				_w.twoHandedActions = new List<Action> ();

				//XmlNode weaponId = w.SelectSingleNode("weaponId");
				//_w.weaponID = weaponId.InnerText;
				XmlNode oh_idle = w.SelectSingleNode("oh_idle");
				_w.oh_idle = oh_idle.InnerText;
				XmlNode th_idle = w.SelectSingleNode("th_idle");
				_w.th_idle = th_idle.InnerText;

				XmlToActions (doc, "actions", ref _w);
				XmlToActions (doc, "twoHandedActions", ref _w);

				XmlNode parryMultiplier = w.SelectSingleNode("parryMultiplier");
				float.TryParse (parryMultiplier.InnerText, out _w.parryMultiplier);
				XmlNode backstabMultiplier = w.SelectSingleNode("backstabMultiplier");
				float.TryParse (backstabMultiplier.InnerText, out _w.backstabMultiplier);
				XmlNode leftHandMirror = w.SelectSingleNode("leftHandMirror");
				bool.TryParse (leftHandMirror.InnerText, out _w.leftHandMirror);

		/*		_w.model_pos = XmlToVector (w, "mp");
				_w.model_eulers = XmlToVector (w, "me");
				_w.model_scale = XmlToVector (w, "ms");
*/
				//resourcesManager.weaponList.Add (_w);
			}
		}

		Vector3 XmlToVector (XmlNode w, string prefix)
		{
			XmlNode _x = w.SelectSingleNode (prefix + "_x");
			float x = 0;
			float.TryParse (_x.InnerText, out x);
			XmlNode _y = w.SelectSingleNode (prefix + "_y");
			float y = 0;
			float.TryParse (_y.InnerText, out y);
			XmlNode _z = w.SelectSingleNode (prefix + "_z");
			float z = 0;
			float.TryParse (_z.InnerText, out z);

			return new Vector3 (x, y, z);
		}

		void XmlToActions (XmlDocument doc, string nodeName, ref Weapon _w)
		{
			foreach (XmlNode a in doc.DocumentElement.SelectNodes("//" + nodeName)) {
				Action _a = new Action ();

				XmlNode actionInput = a.SelectSingleNode("ActionInput");
				_a.input = (ActionInput)Enum.Parse (typeof(ActionInput), actionInput.InnerText);
				XmlNode actionType = a.SelectSingleNode("ActionType");
				_a.type = (ActionType)Enum.Parse (typeof(ActionType), actionType.InnerText);

				XmlNode targetAnim = a.SelectSingleNode("targetAnim");
				_a.targetAnim = targetAnim.InnerText;

				XmlNode mirror = a.SelectSingleNode("mirror");
				bool.TryParse (mirror.InnerText, out _a.mirror);
				XmlNode canBeParried = a.SelectSingleNode("canBeParried");
				bool.TryParse (canBeParried.InnerText, out _a.canBeParried);
				XmlNode changeSpeed = a.SelectSingleNode("changeSpeed");
				bool.TryParse (changeSpeed.InnerText, out _a.changeSpeed);
				XmlNode animSpeed = a.SelectSingleNode("animSpeed");
				float.TryParse (animSpeed.InnerText, out _a.animSpeed);
				XmlNode canParry = a.SelectSingleNode("canParry");
				bool.TryParse (canParry.InnerText, out _a.canParry);
				XmlNode canBackstab = a.SelectSingleNode("canBackstab");
				bool.TryParse (canBackstab.InnerText, out _a.canBackstab);
				XmlNode overrideDamageAnim = a.SelectSingleNode("overrideDamageAnim");
				bool.TryParse (overrideDamageAnim.InnerText, out _a.overrideDamageAnim);
				XmlNode damageAnim = a.SelectSingleNode("damageAnim");
				_a.damageAnim = damageAnim.InnerText;

				_a.weaponStats = new WeaponStats ();

				XmlNode physical = a.SelectSingleNode("physical");
				int.TryParse (physical.InnerText, out _a.weaponStats.physical);
				XmlNode strike = a.SelectSingleNode("strike");
				int.TryParse (strike.InnerText, out _a.weaponStats.strike);
				XmlNode slash = a.SelectSingleNode("slash");
				int.TryParse (slash.InnerText, out _a.weaponStats.slash);
				XmlNode thrust = a.SelectSingleNode("thrust");
				int.TryParse (thrust.InnerText, out _a.weaponStats.thrust);
				XmlNode magic = a.SelectSingleNode("magic");
				int.TryParse (magic.InnerText, out _a.weaponStats.magic);
				XmlNode fire = a.SelectSingleNode("fire");
				int.TryParse (fire.InnerText, out _a.weaponStats.fire);
				XmlNode lighting = a.SelectSingleNode("lighting");
				int.TryParse (lighting.InnerText, out _a.weaponStats.lighting);
				XmlNode dark = a.SelectSingleNode("dark");
				int.TryParse (dark.InnerText, out _a.weaponStats.dark);

				if (nodeName == "actions") {
					_w.actions.Add (_a);
				} else {
					_w.twoHandedActions.Add (_a);
				}
			}
		}
	}
}
