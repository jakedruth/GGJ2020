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
            animatingRopeCoroutine = StartCoroutine(AnimateRopeToPoint(point, onRopeDoneAnimating));
        }
    }

    private IEnumerator AnimateRopeToPoint(Vector3 point, Action<Transform> onRopeDoneAnimating)
    {
        Vector3 pos = Vector3.zero;
        Vector3 target = point - transform.position;

        while (pos != target)
        {
            pos = Vector3.MoveTowards(pos, target, ropeMoveSpeed * Time.deltaTime);
            _line.SetPosition(1, pos);
            yield return null;
        }

        if (onRopeDoneAnimating != null)
        {
            onRopeDoneAnimating(Physics2D.OverlapPoint(point).transform);
        }

        animatingRopeCoroutine = null;
    }
}
