using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    [SerializeField] private int _targetFramerate = 45;

    private void Awake() => Application.targetFrameRate = _targetFramerate;
}
