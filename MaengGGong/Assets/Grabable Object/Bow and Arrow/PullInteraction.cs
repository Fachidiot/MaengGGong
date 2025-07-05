using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PullInteraction : XRBaseInteractable
{
    public static event Action<float> PullActionReleased;

    public Transform start, end;    // action Start, End Position
    public GameObject notch;        // Arrow Make Position
    public float pullAmount { get; private set; } = 0.0f;   // Player's Pull Amount

    private LineRenderer _lineRenderer; // Bow's string linerenderer
    private IXRSelectInteractor _pullingInteractor = null;   // Left hand, Right hand Check.
    private AudioSource _audioSource;
    protected override void Awake()
    {
        base.Awake();
        _lineRenderer = GetComponent<LineRenderer>();

        _audioSource = GetComponent<AudioSource>();
    }

    public void SetPullInteractor(SelectEnterEventArgs args)
    {   // Check Interactor Object and save. (Left or Right)
        _pullingInteractor = args.interactorObject;
    }

    public void Release()
    {   // Pull Action Release
        PullActionReleased?.Invoke(pullAmount); // All Arrow Shot Actions Invoke
        _pullingInteractor = null;   // Release then no hands left at string
        pullAmount = 0f;    // Release then, pull amount is zero.

        // notch position reset
        notch.transform.localPosition = new Vector3(notch.transform.localPosition.x, notch.transform.localPosition.y, 0f);
        UpdateString();
        PlayReleaseSound();
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {   // Update Arrow position
        base.ProcessInteractable(updatePhase);
        if (XRInteractionUpdateOrder.UpdatePhase.Dynamic == updatePhase)
        {
            if (isSelected)
            {
                Vector3 pullPosition = _pullingInteractor.transform.position;
                pullAmount = CalculatePull(pullPosition);

                UpdateString();

                HapticFeedback();
            }
        }
    }

    private float CalculatePull(Vector3 pullPosition)
    {
        Vector3 pullDirection = pullPosition - start.position;
        Vector3 targetDirection = end.position - start.position;
        float maxLength = targetDirection.magnitude;

        targetDirection.Normalize();
        float pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength;
        return Mathf.Clamp(pullValue, 0, 1);
    }

    private void UpdateString()
    {
        Vector3 linePosition = Vector3.back * Mathf.Lerp(start.transform.localPosition.x, end.transform.localPosition.x, pullAmount);
        notch.transform.localPosition = new Vector3(-linePosition.z - 0.19f, notch.transform.localPosition.y, notch.transform.localPosition.z);
        _lineRenderer.SetPosition(1, linePosition);
    }

    private void HapticFeedback()
    {
        if (null != _pullingInteractor)
        {
            ActionBasedController currentController = _pullingInteractor.transform.gameObject.GetComponent<ActionBasedController>();
            currentController.SendHapticImpulse(pullAmount, 0.1f);
        }
    }

    private void PlayReleaseSound()
    {
        _audioSource.Play();
    }
}
