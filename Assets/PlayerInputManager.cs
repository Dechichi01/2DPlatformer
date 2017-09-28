using UnityEngine;

public class PlayerInputManager : MonoBehaviour {

    [SerializeField] MobileStaticJoyscitk _joystick;
    [SerializeField] MobileButton _jumpButton;
    [SerializeField] MobileButton _shootButton;
    [SerializeField] MobileButton _spHabilityButton;

    public FingerDragDetector DragDetect { get; private set; }
    public MobileButton ShootButton { get { return _shootButton; } }
    public MobileButton JumpButton { get { return _jumpButton; } }
    public MobileButton SpecialHabilityButton { get { return _spHabilityButton; } }

    public Vector2 GetJoystickInput()
    {
        return _joystick.InputValue;
    }
}
