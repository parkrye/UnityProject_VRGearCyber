using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PID;
using UnityEngine.Events;

namespace PID
{
    public class SocketHelper : MonoBehaviour
    {
        UnityAction customTriggerCheck;
        Renderer _renderer; 
        const string playerTag = "Player";

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == playerTag)
            {
                //Turn on MeshRenderer, 
                //Invoke a event 
                _renderer.enabled = true;
                customTriggerCheck?.Invoke(); 
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == playerTag)
            {
                //Turn on MeshRenderer, 
                //Invoke a event 
                _renderer.enabled = false;
            }
        }
    }

}
