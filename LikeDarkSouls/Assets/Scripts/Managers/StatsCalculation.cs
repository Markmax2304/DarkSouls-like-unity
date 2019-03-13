using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
	public static class StatsCalculation  
	{
		public static int CalculateBaseDamage (WeaponStats w, CharacterStats st, float multiplier = 1)
		{
			float attackPhysical = (w.physical + w.strike + w.slash + w.thrust) * multiplier;
			float defPhysical = st.physical + st.vs_strike + st.vs_slash + st.vs_thrust;

			float sum = attackPhysical - defPhysical;

			float attackMagic = (w.magic + w.fire + w.lighting + w.dark) * multiplier;
			float defMagic = st.magic + st.fire + st.lightin + st.dark;

			sum += attackMagic - defMagic;

			if (sum <= 0)
				sum = 1;

			return Mathf.RoundToInt(sum);
		}
	}
}