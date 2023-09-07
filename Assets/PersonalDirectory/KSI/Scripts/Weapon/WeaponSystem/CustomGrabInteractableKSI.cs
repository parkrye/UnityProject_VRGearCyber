using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

namespace KSI
{
	[RequireComponent(typeof(RayAttachModifier))]
	public class CustomGrabInteractableKSI : XRGrabInteractable
	{
		[Header("Custom Grab Interactable Parameters")]
		[SerializeField][Range(1, 10)] int grabPriority;
		[SerializeField] protected Rigidbody rb;
		[SerializeField] GameData.InteractableType interactableType;
		[SerializeField] int defaultLayer, ignoreColliderLayer;

		[SerializeField] bool onlyOneHandGrab;

		public GameData.InteractableType InteractableType { get { return interactableType; } }

		public int Priority
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

		protected override void Awake()
		{
			base.Awake();
			rb = GetComponent<Rigidbody>();
			defaultLayer = gameObject.layer;
			ignoreColliderLayer = Mathf.RoundToInt(Mathf.Log(LayerMask.GetMask("Ignore Collider"), 2));
		}

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

		protected override void OnSelectEntered(SelectEnterEventArgs args)
		{
			base.OnSelectEntered(args);
			foreach (Transform go in GetComponentsInChildren<Transform>())
			{
				if (1 << go.gameObject.layer == LayerMask.GetMask("DisplayUI"))
					continue;
				go.gameObject.layer = ignoreColliderLayer;
			}
		}

		protected override void OnSelectExited(SelectExitEventArgs args)
		{
			base.OnSelectExited(args);
			foreach (Transform go in GetComponentsInChildren<Transform>())
			{
				if (1 << go.gameObject.layer == LayerMask.GetMask("DisplayUI"))
					continue;
				go.gameObject.layer = defaultLayer;
			}
		}
	}
}
