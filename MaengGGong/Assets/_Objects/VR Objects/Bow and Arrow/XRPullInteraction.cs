using System;
using System.Drawing;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRPullInteraction : XRBaseInteractable
{
    public event Action<float> PullActionReleased;
    public event Action<float> PullUpdated;
    public event Action PullStarted;
    public event Action PullEnded;

    [Header("Pull Settings")]
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;    // action Start, End Position
    [SerializeField] private GameObject _notch;        // Arrow Make Position
    public float pullAmount { get; private set; } = 0.0f;   // Player's Pull Amount

    private LineRenderer _lineRenderer; // Bow's string linerenderer
    private IXRSelectInteractor _pullingInteractor = null;   // Left hand, Right hand Check.
    protected override void Awake()
    {
        base.Awake();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetPullInteractor(SelectEnterEventArgs args)
    {   // Check Interactor Object and save. (Left or Right)
        _pullingInteractor = args.interactorObject;
        PullStarted?.Invoke();
    }

    public void Release()
    {   // Pull Action Release
        PullActionReleased?.Invoke(pullAmount); // All Arrow Shot Actions Invoke
        PullEnded?.Invoke();

        _pullingInteractor = null;   // Release then no hands left at string
        pullAmount = 0f;    // Release then, pull amount is zero.
        _notch.transform.localPosition = new Vector3(0.19f, _notch.transform.localPosition.y, _notch.transform.localPosition.z);   // notch position reset

        UpdateStringAndNotch();
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {   // Update Arrow position
        base.ProcessInteractable(updatePhase);

        if (XRInteractionUpdateOrder.UpdatePhase.Dynamic == updatePhase)
        {
            if (isSelected && null != _pullingInteractor)
            {
                Vector3 pullPosition = _pullingInteractor.GetAttachTransform(this).position;
                float prevPull = pullAmount;
                pullAmount = CalculatePull(pullPosition);

                if (prevPull != pullAmount)
                    PullUpdated?.Invoke(pullAmount);

                UpdateStringAndNotch();
                HapticFeedback();
            }
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        SetPullInteractor(args);
    }

    private float CalculatePull(Vector3 pullPosition)
    {
        Vector3 pullDirection = pullPosition - _startPoint.position;
        Vector3 targetDirection = _endPoint.position - _startPoint.position;
        float maxLength = targetDirection.magnitude;

        targetDirection.Normalize();
        float pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength;
        return Mathf.Clamp(pullValue, 0, 1);
    }

    private void UpdateStringAndNotch()
    {
        Vector3 linePosition = Vector3.back * Mathf.Lerp(_startPoint.transform.localPosition.x, _endPoint.transform.localPosition.x, pullAmount);
        _notch.transform.localPosition = new Vector3(-linePosition.z, _notch.transform.localPosition.y, _notch.transform.localPosition.z);
        _lineRenderer.SetPosition(1, linePosition);
    }

    private void HapticFeedback()
    {
        if (null != _pullingInteractor && _pullingInteractor is ActionBasedController controllerInteractor)
            controllerInteractor.SendHapticImpulse(pullAmount, 0.1f);
    }
}
