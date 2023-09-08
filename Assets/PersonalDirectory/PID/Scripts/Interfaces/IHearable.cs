using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PID
{
    public interface IHearable
    {
        /// <summary>
        /// When Calling for listen
        /// </summary>
        /// <param name="soundPoint"></param>
        /// <param name="hasWall"></param>
        public void Heard(Vector3 soundPoint, bool hasWall);
        public void UnderRadio(bool underRadioSound); 
    }
}