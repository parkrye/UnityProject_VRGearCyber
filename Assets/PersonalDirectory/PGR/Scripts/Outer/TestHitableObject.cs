using UnityEngine;

namespace PGR
{
    public class TestHitableObject : MonoBehaviour, IHitable
    {
        [SerializeField] DisplayCanvas displayCanvas;
        public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            displayCanvas.ChangeSubText(damage.ToString());
        }
    }

}