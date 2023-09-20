using PGR;
using UnityEngine;

public class TestShotRobot : MonoBehaviour, IHitable
{
    [SerializeField] DisplayCanvas display;
    [SerializeField] int maxDamage;

    void Start()
    {
        display.ChangeMainText($"High: 0");
        display.ChangeSubText($"0");
    }

    public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if(damage > maxDamage)
        {
            maxDamage = damage;
            display.ChangeMainText($"High: {damage}");
        }
        display.ChangeSubText($"{damage}");
    }

}
