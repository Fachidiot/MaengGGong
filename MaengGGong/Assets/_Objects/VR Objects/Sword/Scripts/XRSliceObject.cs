using UnityEngine;
using EzySlice;

public class XRSliceObject : MonoBehaviour
{
// If Error when slice 3D Model. please check model's Setting to Read/Write set true.
    [SerializeField] private Transform _startSlicePoint;    // startpoint is nearest at handle.
    [SerializeField] private Transform _endSlicePoint;  // endslicePoint must have velocityEstimator Component. (play on awake = true)
    [SerializeField] private VelocityEstimator _velocityEstimator;  // endslicePoint
    [Tooltip("can slice object's layer")]
    [SerializeField] private LayerMask _sliceableLayer; // sliceObjects & SliceableObjects Layer (ex. Sliceable)

    [Tooltip("sliced face's material")]
    [SerializeField] private Material _crossSectionMaterial;
    [Tooltip("slice force")]
    [SerializeField] private float _cutForce = 1000f;

    private void FixedUpdate()
    {
        // Raycast to Find Object.
        bool hasHit = Physics.Linecast(_startSlicePoint.position, _endSlicePoint.position, out RaycastHit hit, _sliceableLayer);

        if (hasHit)
        {
            GameObject target = hit.transform.gameObject;
            Slice(target);
        }
    }

    private void Slice(GameObject target)
    {
        // Calculate velocity & normal Vector
        Vector3 velocity = _velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(_endSlicePoint.position - _startSlicePoint.position, velocity);
        planeNormal.Normalize();

        SlicedHull hull = target.Slice(_endSlicePoint.position, planeNormal);

        if (null == hull)
            return;

        // Make Sliced Objects
        GameObject upperHull = hull.CreateUpperHull(target, _crossSectionMaterial);
        SetupSlicedComponent(upperHull);

        GameObject lowerHull = hull.CreateLowerHull(target, _crossSectionMaterial);
        SetupSlicedComponent(lowerHull);

        // Destroy Original
        Destroy(target);
    }

    private void SetupSlicedComponent(GameObject slicedObject)
    {
        Rigidbody rigidbody = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;
        rigidbody.AddExplosionForce(_cutForce, slicedObject.transform.position, 1);
    }
}