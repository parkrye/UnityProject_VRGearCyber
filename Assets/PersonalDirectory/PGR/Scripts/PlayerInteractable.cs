using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PGR
{
    public class PlayerInteractable : XRGrabInteractable
    {
        [Header("Player Interactable Parameters")]
        [SerializeField][Range(1, 10)] uint grabPriority;
        public uint Priority 
        { 
            get 
            { 
                return grabPriority; 
            } 
            set 
            {
                if (value <= 0)
                    grabPriority = 1;
                else if (value > 10)
                    grabPriority = 10;
                else
                    grabPriority = value; 
            } 
        }
    }

}