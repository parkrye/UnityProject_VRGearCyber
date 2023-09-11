using PM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PM
{
    public class Lazer : MonoBehaviour
    {
        Terminal terminal;

        private void Start()
        {
            GetTerminal();
        }
        private void GetTerminal()
        {
            foreach (Transform trans in transform.parent)
            {
                if (trans.GetComponent<Terminal>() != null)
                {
                    terminal = trans.GetComponent<Terminal>();
                    break;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                if (terminal != null)
                    StartCoroutine(terminal.CallSecurity(other.transform.position));
            }
        }
    }

}
