using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeController : MonoBehaviour
{
    private LineRenderer _line;
    public float ropeMoveSpeed;

    public Coroutine animatingRopeCoroutine { get; private set; }

    void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _line.SetPosition(0, Vector3.zero);
        _line.SetPosition(1, Vector3.zero);
    }

    public void MoveToPoint(Vector3 point, Action<Transform> onRopeDoneAnimating)
    {
        if (animatingRopeCoroutine == null)
        {
            animatingRopeCoroutine = StartCoroutine(AnimateRopeToPoint(point));
        }
    }

    private IEnumerator AnimateRopeToPoint(Vector3 point)
    {
        yield return null;
    }
}
