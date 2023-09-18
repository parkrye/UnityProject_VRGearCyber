using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PID;
using UnityEngine.Events;

namespace PID
{
    public class SocketSupplement : MonoBehaviour
    {
        public UnityAction<bool> customTrigger;
        Renderer _renderer; 
        const string playerTag = "Player";

        private void Awake()
        {
            //_renderer = GetComponent<Renderer>();
        }

        public void HandEnter()
        {

        }

        public void HandExit()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == playerTag)
            {
                //Turn on MeshRenderer, 
                //Invoke a event 
                // _renderer.enabled = true;
                customTrigger?.Invoke(true); 
            }


        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == playerTag)
            {
                //Turn on MeshRenderer, 
                //Invoke a event 
                // _renderer.enabled = false;
                customTrigger?.Invoke(false);
            }
        }
    }

}
