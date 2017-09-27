using UnityEngine;
using Lean;

public class FingerDragDetector : MonoBehaviour {

    [SerializeField] private MobileInputArea dragArea;

    private LeanFinger dragFinger;
    private float dragFingerDownTime;

    public System.Action<Vector2> onFingerDrag;

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

    private void RaiseOnFingerDrag(Vector2 delta)
    {
        if (onFingerDrag != null)
        {
            onFingerDrag(delta);
        }
    }

    private void OnFingerDown(LeanFinger finger)
    {
        if (dragFinger == null && dragArea.ContainsPoint(finger.ScreenPosition))
        {
            dragFinger = finger;
            dragFingerDownTime = Time.time;
        }
    }

    private void OnFingerUp(LeanFinger finger)
    {
        if (finger == dragFinger)
        {
            dragFinger = null;
        }
    }

    private void OnFingerDrag(LeanFinger finger)
    {
        if (finger == dragFinger)
        {
            RaiseOnFingerDrag(finger.DeltaScreenPosition);
        }
    }
}
