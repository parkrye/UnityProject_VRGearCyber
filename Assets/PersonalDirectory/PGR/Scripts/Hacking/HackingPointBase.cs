using UnityEngine;

namespace PGR
{
    public class HackingPointBase : MonoBehaviour
    {
        public void SelfDestroy()
        {
            GameManager.Resource.Destroy(gameObject);
        }
    }

}