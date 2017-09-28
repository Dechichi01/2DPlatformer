using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(FieldOfView2D))]
public class PlayerAim : MonoBehaviour {

    [SerializeField] private float maxAngleDelta = 40;
    [SerializeField] private float smoothTime = .2f;

    [SerializeField] private Transform _spine;

    private FieldOfView2D _fov;

    private IDamageable currTarget;
    private Transform currTargetTransform;

    private float deltaRot;
    private float targetDeltaRot;
    private float smoothRot;

    private Dictionary<Transform, IDamageable> _cachedTargets = new Dictionary<Transform, IDamageable>();

    private void Start()
    {
        _fov = GetComponent<FieldOfView2D>();
    }

    private void LateUpdate()
    {
        currTarget = FindBestTarget();

        if (currTarget != null)
        {
            
        }
        else
        {
            if (targetDeltaRot != deltaRot)
            {
                Move();
            }
            _spine.rotation = Quaternion.Euler(Vector3.forward * deltaRot) * _spine.rotation;
        }
    }

    public void SetAimRotation(float angle)
    {
        if (currTarget != null)
            return;

        targetDeltaRot = Mathf.Clamp(angle, -maxAngleDelta, maxAngleDelta);
    }

    public void Move()
    {
        deltaRot = Mathf.SmoothDamp(deltaRot, targetDeltaRot, ref smoothRot, smoothTime);
    }

    private IDamageable FindBestTarget()
    {
        if (_fov.visibleTargets.Count == 0)
            return null;

        currTargetTransform = _fov.visibleTargets.OrderBy(t => (t.position - transform.position).sqrMagnitude).First();

        if (_cachedTargets.ContainsKey(currTargetTransform))
            return _cachedTargets[currTargetTransform];

        IDamageable target = currTargetTransform.GetComponent<IDamageable>();
        _cachedTargets.Add(currTargetTransform, target);

        return target;
    }
}
