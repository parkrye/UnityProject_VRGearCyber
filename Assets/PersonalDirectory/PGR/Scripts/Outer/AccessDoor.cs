using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

namespace PGR
{
    public class AccessDoor : SceneMover
    {
        [SerializeField] Rigidbody doorRb;
        [SerializeField] Image gaugeImage;
        [SerializeField] float closeTerm, accessGauge;
        [SerializeField] int locationNum;
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

            MoveScene(locationNum);
        }

    }

}