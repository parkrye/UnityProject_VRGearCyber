using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PGR
{
    public class VendingMachine : XRSimpleInteractable, IHitable
    {
        [SerializeField] int maxHP, nowHP, least;
        [SerializeField] GameObject[] products;
        [SerializeField] Transform productOutTransform;

        protected override void Awake()
        {
            base.Awake();
            if (maxHP <= 0)
                maxHP = 1;
            if(least < 5)
                least = 5;
            nowHP = maxHP;
        }

        public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            nowHP -= damage;
            if (nowHP <= 0 || least <= 0)
                return;
            GiveProduct();
        }

        public void OnHoverEnterEvent(HoverEnterEventArgs args)
        {
            if (args.interactorObject.transform.TryGetComponent(out LineRenderer _))
                return;
            if (nowHP <= 0 || least <= 0)
                return;
            GiveProduct();
        }

        public void GiveProduct()
        {
            int productNum = Random.Range(0, products.Length * 5);
            if (productNum >= products.Length)
                return;

            least--;
            Instantiate(products[productNum], productOutTransform.position, Quaternion.identity);
        }
    }
}