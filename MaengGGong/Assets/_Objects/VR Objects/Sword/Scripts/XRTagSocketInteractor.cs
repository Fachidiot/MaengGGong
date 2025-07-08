using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class XRTagSocketInteractor : Interactors.XRSocketInteractor
{
    [SerializeField]
    private string m_targetTag;

    public override bool CanHover(Interactables.IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && interactable.transform.tag == m_targetTag;
    }

    public override bool CanSelect(Interactables.IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.transform.tag == m_targetTag;
    }
}
