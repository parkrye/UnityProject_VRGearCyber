using UnityEngine;

namespace PGR
{
    public class Wine : MonoBehaviour
    {
        [SerializeField] GameObject liquid;
        [SerializeField] ParticleSystem drop;
        [SerializeField] float content;
        [SerializeField] bool isCovered;

        private void Awake()
        {
            if (content <= 1f)
                content = 10f;
        }

        void LateUpdate()
        {
            if (content <= 0f)
                return;
            if (isCovered)
                return;

            if(Trigonometrics.Cos(Vector3.Angle(Vector3.up, transform.up)) <= 0.5f)
            {
                drop.Play();
                content -= Time.deltaTime;
                if(content < 0f)
                {
                    liquid?.SetActive(false);
                    drop.Stop();
                }
            }
            else
            {
                drop.Stop();
            }
        }

        public void CoverOff()
        {
            isCovered = false;
        }
    }

}