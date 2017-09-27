using UnityEngine;

public class PlayerInputManager : MonoBehaviour {

    [SerializeField] MobileJoystick _joystick;
    [SerializeField] MobileButton _shootButton;
    [SerializeField] MobileButton _spHabilityButton;

    public FingerDragDetector DragDetect { get; private set; }
    public MobileButton ShootButton { get { return _shootButton; } }
    public MobileButton SpecialHabilityButton { get { return _spHabilityButton; } }

    public Vector2 GetMovementInput()
    {
        Vector2 _inputVal = _joystick.InputValue();
        return new Vector2(_inputVal.x, Input.GetKeyDown(KeyCode.Space) ? 1: 0);
    }
}
