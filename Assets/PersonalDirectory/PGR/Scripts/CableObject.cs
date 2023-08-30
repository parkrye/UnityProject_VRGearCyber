using UnityEngine;

namespace PGR
{
    public class CableObject : PlayerInteractable
    {
        [Header ("Cable Object Parameters")]
        [SerializeField] Light pointLight;

        protected override void Awake()
        {
            base.Awake();
            pointLight = GetComponent<Light>();

            Priority = 10;
            pointLight.enabled = false;
            rb.isKinematic = false;
        }

        public void FixEnter()
        {
            rb.isKinematic = true;
        }

        public void FixExit()
        {
            pointLight.enabled = false;
            rb.isKinematic = false;
        }

        public void ReadyToShot()
        {
            pointLight.enabled = true;
        }

        public void ShotCable(Vector3 shotDirection, float shotPower)
        {
            rb.isKinematic = false;
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            rb.AddForce(shotDirection * shotPower, ForceMode.Impulse);
        }

        void OnCollisionEnter(Collision collision)
        {
            IHackable hackable = collision.gameObject.GetComponent<IHackable>();
            if (hackable == null)
                return;

            // Start Hacking Puzzle
            transform.LookAt(transform.position - collision.contacts[0].normal);
            FixEnter();
        }
    }
}
