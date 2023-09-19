using PID;
using UnityEngine;

namespace PGR
{
    public class SceneMover : MonoBehaviour, IHitable, IStrikable
    {
        [SerializeField] string SceneName;
        [SerializeField] int life;

        public void MoveScene(int locationNum)
        {
            if (life > 0)
                return;

            PlayerPrefs.SetInt("Load Location", locationNum);
            GameManager.Scene.LoadScene(SceneName);
        }

        public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            life -= damage;
        }

        public void TakeStrike(Transform hitter, float damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            life -= (int)damage;
        }
    }

}