using PID;
using UnityEngine;

namespace PGR
{
    public class TestHitableObject : HittableItem
    {
        [SerializeField] DisplayCanvas displayCanvas;
        public override void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            base.TakeDamage(damage, hitPoint, hitNormal);
            displayCanvas.ChangeSubText(damage.ToString());
        }

        public override void TakeStrike(Transform hitter, float damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            base.TakeStrike(hitter, damage, hitPoint, hitNormal);   
            displayCanvas.ChangeSubText(damage.ToString());
        }
    }

}