using System;
using UnityEngine;

public class Holster : MonoBehaviour
{
    [SerializeField] private GameObject _centerEyeAnchor;
    [SerializeField] private Vector3 _posOffset;
    [SerializeField] private float _rotOffset;
    [SerializeField] private float _rotationSpeed = 60f;

    void Update()
    {
        transform.position = new Vector3(_centerEyeAnchor.transform.position.x, _centerEyeAnchor.transform.position.y / 2, _centerEyeAnchor.transform.position.z) + _posOffset;

        float rotationDifference = Math.Abs(_centerEyeAnchor.transform.eulerAngles.y - transform.eulerAngles.y);
        float finalRotation = _rotationSpeed;

        if (rotationDifference > 60)
            finalRotation = _rotationSpeed * 2;
        else if (rotationDifference > 40 && rotationDifference > 20)
            finalRotation = _rotationSpeed / 2;
        else if (rotationDifference < 20 && rotationDifference > 0)
            finalRotation = _rotationSpeed / 4;

        float step = finalRotation * Time.deltaTime;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, _centerEyeAnchor.transform.eulerAngles.y + _rotOffset, 0), step);
    }
}
