using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PGR
{
    public class KeyPadButton : MonoBehaviour
    {
        [SerializeField] SecretDoorKeypad keypad;
        [SerializeField] MeshRenderer m_Renderer;
        [SerializeField] AudioSource clickAudio;
        [SerializeField] int keyNum;

        void Start()
        {
            keypad.ResetEvent.AddListener(ResetInput);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<XRPokeInteractor>() == null)
                return;

            m_Renderer.enabled = true;
            clickAudio?.Play();
            keypad.InputPassword(keyNum);
        }

        void ResetInput()
        {
            m_Renderer.enabled = false;
            clickAudio?.Play();
        }
    }
}
