using UnityEngine;

namespace PGR
{
    public class CableObject : PlayerInteractable
    {
        [Header ("Cable Object Parameters")]
        [SerializeField] Light pointLight;
        [SerializeField] bool isFixed;

        protected override void Awake()
        {
            base.Awake();
            pointLight = GetComponent<Light>();

            Priority = 10;
            pointLight.enabled = false;
            rb.isKinematic = false;
            isFixed = false;
        }

        public void FixEnter()
        {
            rb.isKinematic = true;
            isFixed = true;
        }

        public void FixExit()
        {
            if (!isFixed)
                return;

            pointLight.enabled = false;
            rb.isKinematic = false;
            isFixed = false;
        }

        public void ReadyToShot()
        {
            pointLight.enabled = true;
        }

        public void ShotCable(Vector3 shotDirection, float shotPower)
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            rb.isKinematic = false;
            rb.AddForce(shotDirection * shotPower, ForceMode.Impulse);
        }

        void OnCollisionEnter(Collision collision)
        {
            IHackable hackable = collision.gameObject.GetComponent<IHackable>();
            if (hackable == null)
                return;

            // Start Hacking Puzzle
            Debug.Log("OnCollisionEnter");
            transform.LookAt(transform.position - collision.contacts[0].normal);
            //FixEnter();
        }
    }
}
