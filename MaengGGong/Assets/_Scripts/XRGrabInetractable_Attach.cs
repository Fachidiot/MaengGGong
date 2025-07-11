using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class XRGrabInetractable_Attach : XRGrabInteractable
{
    [SerializeField] private Transform _lefHandAttachTransform;
    [SerializeField] private Transform _rightHandAttachTransform;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.CompareTag("LeftHand"))
            attachTransform = _lefHandAttachTransform;
        else if (args.interactorObject.transform.CompareTag("RightHand"))
            attachTransform = _rightHandAttachTransform;

        base.OnSelectEntered(args);
    }
}
