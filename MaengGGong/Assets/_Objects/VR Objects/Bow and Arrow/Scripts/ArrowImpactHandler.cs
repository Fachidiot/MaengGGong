using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowImpactHandler : MonoBehaviour
{
    [Header("Impact Settings")]
    [SerializeField] private bool _explodeOnImpact = false;
    [SerializeField] private float _stickDuration = 3f;
    [SerializeField] private float _minEmbedDepth = 0.05f;
    [SerializeField] private float _maxEmbedDepth = 0.15f;
    [SerializeField] private LayerMask _ignoreLayers;
    [SerializeField] private Transform _tip;

    [Header("Visual Effects")]
    [SerializeField] private GameObject _impactGameObject;
    [SerializeField] private MeshRenderer _arrowMeshRenderer;

    private ArrowLauncher _arrowLauncher;
    private Rigidbody _rigidBody;
    private bool _hasHit = false;

    private void Awake()
    {
        _arrowLauncher = GetComponent<ArrowLauncher>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasHit || ((1 << collision.gameObject.layer) & _ignoreLayers) != 0)
            return;

        _hasHit = true;
        _arrowLauncher.Stop();

        if (_explodeOnImpact)
            HandleExplosion();
        else
            HandleStick(collision);
    }

    private void HandleExplosion()
    {
        Debug.Log("Explosion Called");
        if (null != _arrowMeshRenderer)
            _arrowMeshRenderer.enabled = false;

        if (null != _impactGameObject)
            Instantiate(_impactGameObject, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void HandleStick(Collision collision)
    {
        Vector3 arrowDirection = transform.forward;
        Vector3 arrowUp = transform.up;
        ContactPoint contactPoint = collision.GetContact(0);

        float randomDepth = Random.Range(_minEmbedDepth, _maxEmbedDepth);
        Quaternion finalRotation = Quaternion.LookRotation(arrowDirection, arrowUp);
        Vector3 centerOffset = _tip.localPosition;
        Vector3 finalPosition = contactPoint.point - (finalRotation * centerOffset) + contactPoint.normal * -randomDepth;

        transform.SetPositionAndRotation(finalPosition, finalRotation);

        CreateStabJoint(collision, randomDepth);

        transform.SetParent(collision.transform, true);
        StartCoroutine(DespawnAfterDelay());
    }

    public ConfigurableJoint CreateStabJoint(Collision collision, float randomDepth)
    {
        var joint = gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedBody = collision.rigidbody;
        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;

        var limit = joint.linearLimit;
        limit.limit = randomDepth;
        joint.linearLimit = limit;

        return joint;
    }

    private IEnumerator DespawnAfterDelay()
    {
        yield return new WaitForSeconds(_stickDuration);
        Destroy(gameObject);
    }
}
