using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DistanceReleaseGrabInteractable : UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable
{
    public float maxGrabDistance = 0.25f;   // Max distance before auto-release
    private UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor cachedInteractor;   // To Keep track of the interactor

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        cachedInteractor = args.interactorObject;   // Cache the interactor when grabbed
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        cachedInteractor = null;    // Clear cache when released
    }

    void Update()
    {
        if (null != cachedInteractor)
        {   // Only check distance if currently grabbed
            if (Vector3.Distance(cachedInteractor.transform.position, colliders[0].transform.position) > maxGrabDistance)
                interactionManager.SelectExit(cachedInteractor, this);  // If the interactor is too far, force release.
        }
    }


    public void DetachInteractor()
    {
        if (null != cachedInteractor)
            interactionManager.SelectExit(cachedInteractor, this);
    }
}
