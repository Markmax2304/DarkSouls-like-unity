using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;
using System.IO;

namespace SA.Utilities
{
	[ExecuteInEditMode]
	public class ItemToXML : MonoBehaviour 
	{
		public bool make;
		public List<RuntimeWeapon> candidates = new List<RuntimeWeapon>();
		public string xml_version; //<?xml version = "1.0" encoding = "UTF-8"?>
		public string targetName;

		void Update ()
		{
			if (!make)
				return;
			make = false;

			string xml = xml_version;
			xml += "\n";
			xml += "<root>";

			for (int i = 0; i < candidates.Count; i++) {
				Weapon w = candidates [i].instance;

				xml += "<weapon>" + "\n";
				//xml += "<weaponId>" + w.weaponID + "</weaponId>" + "\n";
				xml += "<oh_idle>" + w.oh_idle + "</oh_idle>" + "\n";
				xml += "<th_idle>" + w.th_idle + "</th_idle>" + "\n";

				xml += ActionListToString (w.actions, "actions");
				xml += ActionListToString (w.twoHandedActions, "twoHandedActions");

				xml += "<parryMultiplier>" + w.parryMultiplier.ToString() + "</parryMultiplier>" + "\n";
				xml += "<backstabMultiplier>" + w.backstabMultiplier.ToString() + "</backstabMultiplier>" + "\n";
				xml += "<leftHandMirror>" + w.leftHandMirror + "</leftHandMirror>" + "\n";

			/*	xml += "<mp_x>" + w.model_pos.x + "</mp_x>" + "\n";
				xml += "<mp_y>" + w.model_pos.y + "</mp_y>" + "\n";
				xml += "<mp_z>" + w.model_pos.z + "</mp_z>" + "\n";

				xml += "<me_x>" + w.model_eulers.x + "</me_x>" + "\n";
				xml += "<me_y>" + w.model_eulers.y + "</me_y>" + "\n";
				xml += "<me_z>" + w.model_eulers.z + "</me_z>" + "\n";

				xml += "<ms_x>" + w.model_scale.x + "</ms_x>" + "\n";
				xml += "<ms_y>" + w.model_scale.y + "</ms_y>" + "\n";
				xml += "<ms_z>" + w.model_scale.z + "</ms_z>" + "\n";
*/
				xml += "</weapon>" + "\n";
			}

			xml += "</root>";

			string path = StaticStrings.SaveLocation () + StaticStrings.itemFolder;
			if (string.IsNullOrEmpty (targetName)) {
				targetName = "items_database.xml";
			}

			path += targetName;

			File.WriteAllText (path, xml);
		}

		string ActionListToString (List<Action> l, string nodeName)
		{
			string xml = null;

			for(int j = 0; j < l.Count; j++) {
				xml += "<" + nodeName + ">" + "\n";
				xml += "<ActionInput>" + l[j].input.ToString() + "</ActionInput>" + "\n";
				xml += "<ActionType>" + l[j].type.ToString() + "</ActionType>" + "\n";
				xml += "<targetAnim>" + l[j].targetAnim + "</targetAnim>" + "\n";
				xml += "<mirror>" + l[j].mirror + "</mirror>" + "\n";
				xml += "<canBeParried>" + l[j].canBeParried + "</canBeParried>" + "\n";
				xml += "<changeSpeed>" + l[j].changeSpeed + "</changeSpeed>" + "\n";
				xml += "<animSpeed>" + l[j].animSpeed.ToString() + "</animSpeed>" + "\n";
				xml += "<canParry>" + l[j].canParry + "</canParry>" + "\n";
				xml += "<canBackstab>" + l[j].canBackstab + "</canBackstab>" + "\n";
				xml += "<overrideDamageAnim>" + l[j].overrideDamageAnim + "</overrideDamageAnim>" + "\n";
				xml += "<damageAnim>" + l[j].damageAnim + "</damageAnim>" + "\n";

				WeaponStats s = l [j].weaponStats;
				xml += "<physical>" + s.physical + "</physical>" + "\n";
				xml += "<strike>" + s.strike + "</strike>" + "\n";
				xml += "<slash>" + s.slash + "</slash>" + "\n";
				xml += "<thrust>" + s.thrust + "</thrust>" + "\n";
				xml += "<magic>" + s.magic + "</magic>" + "\n";
				xml += "<fire>" + s.fire + "</fire>" + "\n";
				xml += "<lighting>" + s.lighting + "</lighting>" + "\n";
				xml += "<dark>" + s.dark + "</dark>" + "\n";

				xml += "</" + nodeName + ">" + "\n";
			}

			return xml;
		}
	}
}
