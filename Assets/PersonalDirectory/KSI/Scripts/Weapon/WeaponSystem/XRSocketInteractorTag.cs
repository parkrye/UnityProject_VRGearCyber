using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace KSI
{
	// Tag¿¡ ¸Â´Â MagazineÀ¸·Î¸¸ ÀåÂøµÊ
	public class XRSocketInteractorTag : XRSocketInteractor
	{
		[SerializeField] private string targetTag;

		public override bool CanSelect(IXRSelectInteractable interactable)
		{
			GameObject gameObject = interactable.transform.gameObject;

			if (targetTag == null)
				return false;

			return base.CanSelect(interactable) && gameObject.CompareTag(targetTag);
		}
	}
}