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
        _line.SetPosition(0, Vector3.forward * -0.25f);
        _line.SetPosition(1, Vector3.forward * -0.25f);
    }

    public void SetRopeEndPoint(Vector3 point)
    {
        _line.SetPosition(1, point + Vector3.forward * -0.25f);
    }

    public void SendOutRopeToPoint(Vector3 point)
    {
        if (animatingRopeCoroutine == null)
        {
            animatingRopeCoroutine = StartCoroutine(AnimateRopeToPoint(point));
        }
    }

    public void SendOutRopeToEntity(EntityBase entity, Action<EntityBase> CallBackOnHit)
    {
        if (animatingRopeCoroutine == null)
        {
            animatingRopeCoroutine = StartCoroutine(AnimateRopeToEntity(entity, CallBackOnHit));
        }
    }

    public void SendOutRopeAndReturn(Vector3 point, Action callBackOnComplete)
    {
        if (animatingRopeCoroutine == null)
        {
            animatingRopeCoroutine = StartCoroutine(AnimateRopeToPointAndBack(point, callBackOnComplete));
        }
    }

    private IEnumerator AnimateRopeToPoint(Vector3 point)
    {
        Vector3 pos = _line.GetPosition(1);
        Vector3 target = point;

        while (pos != target)
        {
            pos = Vector3.MoveTowards(pos, target, ropeMoveSpeed * Time.deltaTime);
            _line.SetPosition(1, pos + Vector3.forward * -0.25f);

            yield return null;
        }

        animatingRopeCoroutine = null;
    }

    private IEnumerator AnimateRopeToPointAndBack(Vector3 point, Action callBackOnComplete)
    {
        Vector3 start = Vector3.zero;
        Vector3 pos = start;
        Vector3 target = point - transform.position;

        while (pos != target)
        {
            pos = Vector3.MoveTowards(pos, target, ropeMoveSpeed * Time.deltaTime);
            _line.SetPosition(1, pos + Vector3.forward * -0.25f);
            yield return null;
        }

        while (pos != start)
        {
            pos = Vector3.MoveTowards(pos, start, ropeMoveSpeed * Time.deltaTime);
            _line.SetPosition(1, pos + Vector3.forward * -0.25f);
            yield return null;
        }

        callBackOnComplete?.Invoke();
        animatingRopeCoroutine = null;
    }

    private IEnumerator AnimateRopeToEntity(EntityBase entity, Action<EntityBase> callBackOnHit)
    {
        Vector3 pos = Vector3.zero;
        Vector3 target = entity.transform.position - transform.position;

        while (pos != target)
        {
            pos = Vector3.MoveTowards(pos, target, ropeMoveSpeed * Time.deltaTime);
            _line.SetPosition(1, pos + Vector3.forward * -0.25f);
            yield return null;
        }

        animatingRopeCoroutine = null;

        callBackOnHit?.Invoke(entity);
    }

    private Transform _followTarget;

    public void RemoveFollowTarget()
    {
        _followTarget = null;
        animatingRopeCoroutine = null;
    }

    public void AnimateRopeFollowTransform(Transform target)
    {
        if(animatingRopeCoroutine == null)
        {
            animatingRopeCoroutine = StartCoroutine(FollowTransform(target));
        }
    }

    private IEnumerator FollowTransform(Transform target)
    {
        _followTarget = target;
        Vector3 pos = _line.GetPosition(1);

        while (_followTarget != null)
        {
            Vector3 offset = _followTarget.position - transform.position;
            pos = Vector3.MoveTowards(pos, offset, ropeMoveSpeed * Time.deltaTime);
            _line.SetPosition(1, pos + Vector3.forward * -0.25f);
            yield return null;
        }

        //_followTarget = null;
        animatingRopeCoroutine = null;
    }
}
