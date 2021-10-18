using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeafloorWalkingBoots {
    /// <summary>
    /// Custom StatusEffect that enables seafloor walking while wearing the boots
    /// </summary>
    class SE_boots: StatusEffect {

        public override void Setup(Character character) {

            // /"What's swimming? never heard of that skill"
            Player.m_localPlayer.m_canSwim = false;

            // /camera magic that allows camera to go below water. Ruins the cool water shader tho. Normally .3f
            Camera.main.GetComponent<GameCamera>().m_minWaterDistance = -30f;

            base.Setup(character);
        }

        public override void Stop() {

            // /"Man I love swimming"
			Player.m_localPlayer.m_canSwim = true;

            // /hardcoding values, gotta love it
            Camera.main.GetComponent<GameCamera>().m_minWaterDistance = .3f;

            base.Stop();
        }
    }
}
