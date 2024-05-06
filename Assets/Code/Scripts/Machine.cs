using UnityEngine;
using System;

public class Machine : MonoBehaviour
{
    public static event Action<Machine> OnAnyInitializeMachine;

    private Renderer[] _renderers;

    private void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();
    }

    private void Start()
    {
        OnAnyInitializeMachine?.Invoke(this);
    }

    public Vector3 GetBoundsCenterPosition()
    {
       Bounds bounds = _renderers[0].bounds;
       for(int i = 0; i < _renderers.Length; i++)
       {
            bounds = bounds.GrowBounds(_renderers[i].bounds);
       }
       Vector3 center = bounds.center;
       return center;
    }
}