using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(FieldOfView2D))]
public class PlayerAim : MonoBehaviour {

    [SerializeField] private Transform _spine;

    private FieldOfView2D _fov;

    private IDamageable currTarget;

    private Dictionary<Transform, IDamageable> _cachedTargets = new Dictionary<Transform, IDamageable>();

    private void Start()
    {
        _fov = GetComponent<FieldOfView2D>();
    }

    private void Update()
    {
        currTarget = FindBestTarget();

    }

    private IDamageable FindBestTarget()
    {
        if (_fov.visibleTargets.Count == 0)
            return null;

        Transform closest = _fov.visibleTargets.OrderBy(t => (t.position - transform.position).sqrMagnitude).First();

        if (_cachedTargets.ContainsKey(closest))
            return _cachedTargets[closest];

        IDamageable target = closest.GetComponent<IDamageable>();
        _cachedTargets.Add(closest, target);

        return target;
    }
}
