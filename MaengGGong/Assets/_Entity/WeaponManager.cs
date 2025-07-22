using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Transform _attachPoint;

    private Transform _parent;
    private XRGrabInteractable _xrGrabInteractable;

    void Start()
    {
        _parent = transform.parent;
        _xrGrabInteractable = GetComponent<XRGrabInteractable>();
    }


    public void OnSelected()
    {
        transform.parent = _attachPoint;
        transform.position = Vector3.zero;
    }

    public void OffSeleceted()
    {
        // if (_xrGrabInteractable.)
        transform.parent = null;
    }
}
