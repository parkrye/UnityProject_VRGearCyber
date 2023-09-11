using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PM
{
    public class RoomData : MonoBehaviour
    {
        public int x;
        public int z;
        public int aggregate;   // 엘러의 알고리즘을 위한 변수
        public bool right;
        public bool left;
        public bool up;
        public bool down;
    }
}
