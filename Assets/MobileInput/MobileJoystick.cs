using UnityEngine;
using Lean;

public class MobileJoystick : MonoBehaviour
{
    [SerializeField] private JoystickInputArea joystick;

    private LeanFinger joystickFinger;

    private void Start()
    {
        joystick.Reset();
    }

    private void OnEnable()
    {
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerDrag += OnFingerDrag;
        LeanTouch.OnFingerUp += OnFingerUp;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= OnFingerDown;
        LeanTouch.OnFingerDrag -= OnFingerDrag;
        LeanTouch.OnFingerUp -= OnFingerUp;
    }

    public Vector2 InputValue()
    {
        return joystick.InputValue;
    }

    private void OnFingerDown(LeanFinger finger)
    {
        if (joystickFinger == null && joystick.ContainsPoint(finger.ScreenPosition))
        {
            joystick.Setup(finger.ScreenPosition);
            joystickFinger = finger;
        }
    }

    private void OnFingerUp(LeanFinger finger)
    {
        if (finger == joystickFinger)
        {
            joystick.Reset();
            joystickFinger = null;
        }
    }

    private void OnFingerDrag(LeanFinger finger)
    {
        if (finger == joystickFinger)
        {
            joystick.UpdateRectPos(finger.ScreenPosition);
        }
    }
}

[System.Serializable]
public class MobileInputArea
{
    [SerializeField] protected RectTransform area;

    public RectTransform Area { get { return area; } }

    public virtual bool ContainsPoint(Vector2 point)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(area, point);
    }
}

[System.Serializable]
public class JoystickInputArea : MobileInputArea
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private int range = 50;
    private Vector3 startPos;

    public Vector2 InputValue
    {
        get { return rect.gameObject.activeInHierarchy ?
                (rect.position - startPos) / range :
                Vector3.zero; }
    }

    public void Setup(Vector2 screenPos)
    {
        startPos = screenPos;
        rect.position = startPos;
        rect.gameObject.SetActive(true);
    }

    public void Reset()
    {
        rect.position = startPos;
        rect.gameObject.SetActive(false);
    }

    public void UpdateRectPos(Vector2 newPos)
    {
        Vector2 delta = new Vector2(newPos.x - startPos.x, newPos.y - startPos.y);
        float magnitude = Mathf.Min(delta.magnitude, range);
        rect.position = (Vector2)startPos + delta.normalized * magnitude;
    }
}


