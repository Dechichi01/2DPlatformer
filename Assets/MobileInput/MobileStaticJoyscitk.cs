using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean;

public class MobileStaticJoyscitk : MonoBehaviour {

    [SerializeField] private StaticJoystickInputArea _inputArea;
    [SerializeField] private RectTransform _inputIndicator;

    private Vector2 _movementInput;
    private Vector2 _jumpInput;

    private LeanFinger _currFinger;

    public Vector2 MovementInput
    {
        get
        {
            Vector2 result = _movementInput;
            _movementInput = Vector2.zero;
            return result;
        }
    }
    public Vector2 JumpInput
    {
        get
        {
            Vector2 result = _jumpInput;
            _jumpInput = Vector2.zero;
            return result;
        }
    }

    private void OnEnable()
    {
        _inputIndicator.gameObject.SetActive(false);
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerUp += OnFingerUp;
    }

    private void OnDisable()
    {
        _inputIndicator.gameObject.SetActive(false);
        LeanTouch.OnFingerDown -= OnFingerDown;
        LeanTouch.OnFingerUp -= OnFingerUp;
    }

    public void Update()
    {
        if (_currFinger != null)
        {
            _movementInput = _inputArea.GetMovementInput(_currFinger.ScreenPosition);
            _inputIndicator.position = (Vector2)_inputArea.Area.position +
                _movementInput * _inputArea.InputRadius;
        }
    }

    private void OnFingerDown(LeanFinger finger)
    {
        if (_currFinger == null && _inputArea.ContainsPoint(finger.ScreenPosition))
        {
            _currFinger = finger;
            _inputIndicator.gameObject.SetActive(true);
        }
    }

    private void OnFingerUp(LeanFinger finger)
    {
        if (_currFinger == finger)
        {
            _currFinger = null;
            _inputIndicator.gameObject.SetActive(false);
            if (finger.Tap)
            {
                _jumpInput = _inputArea.GetMovementInput(finger.StartScreenPosition);
            }
            _movementInput = Vector2.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_inputArea.Area == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_inputArea.Area.position, _inputArea.InputRadius);

        if (_currFinger == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawCube(
            (Vector2)_inputArea.Area.position + _inputArea.GetMovementInput(_currFinger.ScreenPosition) * _inputArea.InputRadius,
            Vector3.one * 50);
    }
}

[System.Serializable]
public class StaticJoystickInputArea : MobileInputArea
{
    [SerializeField] private float _inputRadius = 50;

    public float InputRadius { get { return _inputRadius; } }

    public Vector2 GetMovementInput(Vector2 reference)
    {
        return (reference - (Vector2)area.position).normalized;
    }

    public override bool ContainsPoint(Vector2 point)
    {
        return base.ContainsPoint(point) &&
            (point - (Vector2) area.position).sqrMagnitude < _inputRadius*_inputRadius;
    }
}
