using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class XRAlyxGrabInteractable : XRGrabInteractable
{
    [SerializeField] private float _velocityThreshold = 2f;
    [SerializeField] private float _jumpAngleInDegree = 60f;

    private XRRayInteractor rayInteractor;
    private Vector3 previousPos;
    private Rigidbody interactableRigidbody;
    private bool canJump = true;

    protected override void Awake()
    {
        base.Awake();

        interactableRigidbody = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if (isSelected && firstInteractorSelecting is XRRayInteractor && canJump)
        {
            Vector3 velocity = (rayInteractor.transform.position - previousPos) / Time.deltaTime;
            previousPos = rayInteractor.transform.position;

            if (velocity.magnitude > _velocityThreshold)
            {
                Drop();
                interactableRigidbody.velocity = ComputeVelocity();
                canJump = false;
            }
        }
    }

    private Vector3 ComputeVelocity()
    {
        Vector3 diff = rayInteractor.transform.position - transform.position;
        Vector3 diffXZ = new Vector3(diff.x, 0, diff.z);
        float diffXZLength = diffXZ.magnitude;
        float diffYLength = diff.y;

        float angleInRadian = _jumpAngleInDegree * Mathf.Deg2Rad;

        float jumpSpeed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(diffXZLength, 2) /
        (2 * Mathf.Pow(Mathf.Cos(angleInRadian), 2) * (diffXZ.magnitude * Mathf.Tan(angleInRadian) - diffYLength)));

        Vector3 jumpVelocityVector = diffXZ.normalized * Mathf.Cos(angleInRadian) * jumpSpeed + Vector3.up * Mathf.Sin(angleInRadian) * jumpSpeed;
        
        return jumpVelocityVector;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactableObject is XRRayInteractor)
        {
            trackPosition = false;
            trackRotation = false;
            throwOnDetach = false;

            rayInteractor = (XRRayInteractor)args.interactorObject;
            previousPos = rayInteractor.transform.position;
            canJump = true;
        }
        else
        {
            trackPosition = true;
            trackRotation = true;
            throwOnDetach = true;
        }

        base.OnSelectEntered(args);
    }
}
