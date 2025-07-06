using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ArrowSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _arrowPrefab;    // Arrow Spawn Prefab
    [SerializeField] private GameObject _notchPoint;    // Arrow Spawn Point
    [SerializeField] private float _spawnDelay = 1f;

    private XRGrabInteractable _bow;
    private XRPullInteraction _pullInteractable;
    private bool _arrowNotched = false;
    private GameObject _currentArrow = null;

    void Start()
    {
        _bow = GetComponent<XRGrabInteractable>();
        _pullInteractable = GetComponentInChildren<XRPullInteraction>();

        if (null != _pullInteractable)
            _pullInteractable.PullActionReleased += NotchEmpty;
    }

    private void OnDestroy()
    {
        if (null != _pullInteractable)
            _pullInteractable.PullActionReleased -= NotchEmpty;
    }

    void Update()
    {
        if (_bow.isSelected && !_arrowNotched)
        {
            _arrowNotched = true;
            StartCoroutine(DelayedSpawn());
        }
        if (!_bow.isSelected && null != _currentArrow)
        {
            Destroy(_currentArrow);
            NotchEmpty(1f);
        }
    }

    private void NotchEmpty(float value)
    {
        _arrowNotched = false;
        _currentArrow = null;
    }

    IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(_spawnDelay);

        _currentArrow = Instantiate(_arrowPrefab, _notchPoint.transform);

        ArrowLauncher launcher = _currentArrow.GetComponent<ArrowLauncher>();
        if (null != launcher && null != _pullInteractable)
            launcher.Initialize(_pullInteractable);
    }
}
