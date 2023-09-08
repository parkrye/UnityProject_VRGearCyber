using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

namespace PGR
{
    public class AccessDoor : MonoBehaviour
    {
        [SerializeField] Rigidbody doorRb;
        [SerializeField] Image gaugeImage;
        [SerializeField] float closeTerm, accessGauge;
        IEnumerator hoverRoutine;

        void Awake()
        {
            hoverRoutine = HoverRoutine();
        }

        public void OnHoverEntered(HoverEnterEventArgs hoverEnterEventArgs)
        {
            StopCoroutine(hoverRoutine);
            StartCoroutine(hoverRoutine);
        }

        public void OnHoverExited(HoverExitEventArgs hoverExitEventArgs)
        {
            StopCoroutine(hoverRoutine);
            accessGauge = 0f;
            gaugeImage.fillAmount = accessGauge;
        }

        IEnumerator HoverRoutine()
        {
            while(accessGauge < 1f)
            {
                accessGauge += Time.deltaTime;
                gaugeImage.fillAmount = accessGauge;
                yield return null;
            }
            accessGauge = 1f;
            gaugeImage.fillAmount = accessGauge;
            StartCoroutine(CloseRoutine());

            yield return new WaitForSeconds(1f);
            accessGauge = 0f;
            gaugeImage.fillAmount = accessGauge;
        }

        IEnumerator CloseRoutine()
        {
            doorRb.isKinematic = false;
            yield return new WaitForSeconds(closeTerm);
            doorRb.isKinematic = true;
            for (int i = 0; i < 10; i++)
            {
                doorRb.transform.localEulerAngles = Vector3.Lerp(doorRb.transform.localEulerAngles, Vector3.zero, 0.1f);
                yield return new WaitForSeconds(0.1f);
            }
            doorRb.transform.localEulerAngles = Vector3.zero;
        }
    }

}