using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleMeshManager : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] _meshRenderers;
    void Start()
    {
        foreach (var renderer in _meshRenderers)
            renderer.enabled = false;
    }
}
