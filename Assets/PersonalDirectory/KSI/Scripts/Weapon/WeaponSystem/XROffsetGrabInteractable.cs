using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// 초기 값 유지
public class XROffsetGrabInteractable : XRGrabInteractable
{
	private Vector3 initialAttachLocalPos;
	private Quaternion initialAttachLocalRot;

	private void Start()
	{
		// AttachTransform 생성
		if (!attachTransform)
		{
			GameObject grab = new GameObject("Grab Pivot");
			grab.transform.SetParent(transform, false);
			attachTransform = grab.transform;
		}

		initialAttachLocalPos = attachTransform.localPosition;
		initialAttachLocalRot = attachTransform.localRotation;
	}

	protected override void OnSelectEntered(SelectEnterEventArgs args)
	{
		if (args is SelectEnterEventArgs)
		{
			attachTransform.position = args.interactorObject.transform.position;
			attachTransform.rotation = args.interactorObject.transform.rotation;
		}
		else
		{
			attachTransform.localPosition = initialAttachLocalPos;
			attachTransform.localRotation = initialAttachLocalRot;
		}

		base.OnSelectEntered(args);
	}
}
