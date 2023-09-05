using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PID
{
    public interface IStrikable
    {
        /// <summary>
        /// Expanding from Ihitable, temporarily detached due to collaboration/ to avoid any git crashes. 
        /// 
        /// </summary>
        /// <param name="hitter"></param>
        /// <param name="damage">
        /// Where damage can be moderated/expanded through velocity in which hitter swings, damage is float. </param>
        /// <param name="hitPoint"></param>
        /// <param name="hitNormal"></param>
        /// <param name="hitInfo"></param>
        public void TakeStrike(Transform hitter, float damage, Vector3 hitPoint, Vector3 hitNormal); 
    }
}

