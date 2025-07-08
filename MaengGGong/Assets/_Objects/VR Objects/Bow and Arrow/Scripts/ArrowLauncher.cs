using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowLauncher : MonoBehaviour
{
    [Header("Launch Settings")]
    [SerializeField] private float speed = 10f;   // Arrow's Fire Speed
    [Header("Visual Effects")]
    [SerializeField] private GameObject _trailSystem;

    [Header("Impact Settings")]
    [SerializeField] private Transform _tip;
    [SerializeField] private LayerMask _ignoreHitLayerMask;

    private Rigidbody _rigidBody;
    private bool _inAir = false;
    private XRPullInteraction _pullInteractable;
    private Vector3 _lastPosition;

    private void Awake()
    {
        InitializeComponents();
        SetPhysics(false);
    }

    private void InitializeComponents()
    {
        _rigidBody = GetComponent<Rigidbody>();

        if (null == _rigidBody)
            Debug.LogError($"Rigidbody component not found on Arrow {gameObject.name}");
    }

    public void Initialize(XRPullInteraction pullInteractable)
    {
        _pullInteractable = pullInteractable;
        _pullInteractable.PullActionReleased += Release;
    }

    private void OnDestroy()
    {
        if (null != _pullInteractable)
            _pullInteractable.PullActionReleased -= Release;
    }

    private void Release(float value)
    {   // Release Physics and Init Arrow's Force 
        if (null != _pullInteractable)
            _pullInteractable.PullActionReleased -= Release;
            
        gameObject.transform.parent = null;
        _inAir = true;
        SetPhysics(true);

        // Init Force
        Vector3 force = transform.forward * value * speed;
        _rigidBody.AddForce(force, ForceMode.Impulse);

        // RoateUpdate
        StartCoroutine(RotateWithVelocity());

        _trailSystem.SetActive(true);
    }

    private IEnumerator RotateWithVelocity()
    {   // Arrow's Rotation Update every time in air
        yield return new WaitForFixedUpdate();  // till end FixedUpdate's CheckCollision
        while (_inAir)
        {
            if (null != _rigidBody && _rigidBody.velocity.sqrMagnitude > 0.01f)
            {
                // Rotate Arrow with force that arrow's go, and current up position
                transform.rotation = Quaternion.LookRotation(_rigidBody.velocity, transform.up);
            }
            yield return null;
        }
    }

    private void FixedUpdate()
    {
        if (_inAir)
        {
            CheckCollision();
            _lastPosition = _tip.position;
        }
    }

    private void CheckCollision()
    {
        if (Physics.Linecast(_lastPosition, _tip.position, out RaycastHit hitInfo))
        {   // if hit Something.
            if ((_ignoreHitLayerMask & (1 << hitInfo.transform.gameObject.layer)) == 0)
            {   // That Something can interact
                if (hitInfo.transform.TryGetComponent(out Rigidbody body))
                {
                    _rigidBody.interpolation = RigidbodyInterpolation.None;
                    // Arrow's Parent Set.
                    transform.parent = hitInfo.transform;
                    // Keep force to parent
                    body.AddForce(_rigidBody.velocity / 2, ForceMode.Impulse);
                }
                // Stop Arrow's Physics
                Stop();
            }
        }
    }

    public void Stop()
    {
        _inAir = false;
        SetPhysics(false);
        _trailSystem.SetActive(false);
    }

    private void SetPhysics(bool usePhysics)
    {
        if (null != _rigidBody)
        {
            _rigidBody.useGravity = usePhysics;
            _rigidBody.isKinematic = !usePhysics;
        }
    }
}
