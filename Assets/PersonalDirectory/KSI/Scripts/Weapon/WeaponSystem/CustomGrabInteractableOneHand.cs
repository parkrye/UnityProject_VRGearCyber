using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;
using PGR;

namespace KSI
{
	[RequireComponent(typeof(RayAttachModifier))]
	public class CustomGrabInteractableOneHand : CustomGrabInteractable
	{
		[Header("")]
		[SerializeField] bool onlyOneHandGrab;

		// 한 쪽 손에서 잡고 있는 중에는 다른 손에서는 잡지 못하게 함
		public override bool IsSelectableBy(IXRSelectInteractor interactor)
		{
			if (!onlyOneHandGrab)
				return base.IsSelectableBy(interactor);

			if (isSelected && !ReferenceEquals(interactor.transform, firstInteractorSelecting.transform))
				return false;
			else
				return base.IsSelectableBy(interactor);
		}
	}
}
