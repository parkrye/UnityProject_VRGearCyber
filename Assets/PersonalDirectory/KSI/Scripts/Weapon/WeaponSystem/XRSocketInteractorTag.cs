using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace KSI
{
	// Tag¿¡ ¸Â´Â MagazineÀ¸·Î¸¸ ÀåÂøµÊ
	//public class XRSocketInteractorTag : XRSocketInteractor
	//{
	//	[SerializeField] private string targetTag;

	//	public override bool CanSelect(IXRSelectInteractable interactable)
	//	{
	//		GameObject gameObject = interactable.transform.gameObject;

	//		if (targetTag == null)
	//			return false;

	//		return base.CanSelect(interactable) && gameObject.CompareTag(targetTag);
	//	}
	//}

	// Tag¿¡ ¸Â´Â MagazineÀ¸·Î¸¸ ÀåÂøµÊ
	public class XRSocketInteractorTag : XRSocketInteractor
	{
		[SerializeField] private string targetTag;

		public override bool CanSelect(IXRSelectInteractable interactable)
		{
			GameObject gameObject = interactable.transform.gameObject;

			if (string.IsNullOrEmpty(targetTag))
			{
				Debug.Log("Target tag is null or empty.");
				return false;
			}

			if (!base.CanSelect(interactable))
			{
				Debug.Log($"Base CanSelect method returned false for gameObject: {gameObject.name}");
				return false;
			}

			if (!gameObject.CompareTag(targetTag))
			{
				Debug.Log($"gameObject {gameObject.name} does not have the target tag: {targetTag}");
				return false;
			}

			return true;
		}
	}
}
