using System;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.XR.Interaction.Toolkit.UI;

[Serializable]
public class HapticSettings
{
    public bool active;
    [Range(0f, 1f)]
    public float intensity;
    public float duration;
}

public class XRUIHapticFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public HapticSettings OnSelectEnter;
    public HapticSettings OnSelectExit;
    public HapticSettings OnHoverEnter;
    public HapticSettings OnHoverExit;

    private XRUIInputModule InputModule => EventSystem.current.currentInputModule as XRUIInputModule;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (OnHoverEnter.active)
            TriggerHaptic(eventData, OnHoverEnter);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (OnHoverExit.active)
            TriggerHaptic(eventData, OnHoverExit);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (OnSelectEnter.active)
            TriggerHaptic(eventData, OnSelectEnter);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (OnSelectExit.active)
            TriggerHaptic(eventData, OnSelectExit);
    }

    private void TriggerHaptic(PointerEventData eventData, HapticSettings hapticSettings)
    {
        // Get Interactor to send haptic
        UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor interactor = InputModule.GetInteractor(eventData.pointerId) as UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor;

        if (!interactor) return;

        // Send Haptic to xrController
        interactor.xrController.SendHapticImpulse(hapticSettings.intensity, hapticSettings.duration);
    }
}
