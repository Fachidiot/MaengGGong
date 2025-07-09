using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class XRTagSocketInteractor : UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor
{
    [SerializeField]
    private string m_targetTag;

    public override bool CanHover(UnityEngine.XR.Interaction.Toolkit.Interactables.IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && interactable.transform.tag == m_targetTag;
    }

    public override bool CanSelect(UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.transform.tag == m_targetTag;
    }
}
