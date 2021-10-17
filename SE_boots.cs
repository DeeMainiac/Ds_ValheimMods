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

            // /makes player act like a draugr in the sense that they don't know how to swim
            Player.m_localPlayer.m_canSwim = false;

            // /camera magic that allows camera to go below water. normally .3f
            Camera.main.GetComponent<GameCamera>().m_minWaterDistance = -30f;

            base.Setup(character);
        }

        public override void Stop() {

			Player.m_localPlayer.m_canSwim = true;

            // /hardcoding values, gotta love it
            Camera.main.GetComponent<GameCamera>().m_minWaterDistance = .3f;

            base.Stop();
        }
    }
}