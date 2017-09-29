using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean;

public class MobileStaticJoyscitk : MonoBehaviour {

    [SerializeField] private StaticJoystickInputArea _inputArea;
    [SerializeField] private RectTransform _inputIndicator;

    private Vector2 _inputValue;
    private Vector2 _jumpInput;

    private LeanFinger _currFinger;

    public Vector2 InputValue
    {
        get
        {
            return _inputValue;
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
            _inputValue = _inputArea.GetMovementInput(_currFinger.ScreenPosition);
            _inputIndicator.position = (Vector2)_inputArea.Area.position +
                _inputValue * _inputArea.InputRadius;
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
            _inputValue = Vector2.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_inputArea.Area == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_inputArea.Area.position, _inputArea.InputRadius);
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
