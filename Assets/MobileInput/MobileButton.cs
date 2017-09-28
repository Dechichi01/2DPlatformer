using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean;

[RequireComponent(typeof(RectTransform))]
public class MobileButton : MonoBehaviour {

    private RectTransform rect;
    private Animator anim;
    private LeanFinger finger;
    private bool turnedOn = false;

    public System.Action OnPress;
    public System.Action OnRelease;
    public System.Action OnTap;
    public System.Action OnHold;
    
    private void Start()
    {
        rect = GetComponent<RectTransform>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerUp += OnFingerUp;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= OnFingerDown;
        LeanTouch.OnFingerUp -= OnFingerUp;
    }

    private void OnFingerDown(LeanFinger leanFinger)
    {
        if (finger == null && ContainsPoint(leanFinger.ScreenPosition))
        {
            finger = leanFinger;
            OnPressed();

            if (OnPress != null)
            {
                OnPress();
            }
        }
    }

    private void OnFingerUp(LeanFinger leanFinger)
    {
        if (finger == leanFinger)
        {
            if (ContainsPoint(leanFinger.ScreenPosition))
            {
                OnReleased();
                if (finger.Tap && OnTap != null)
                {
                    OnTap();
                }
                if (OnRelease != null)
                {
                    OnRelease();
                }
            }

            finger = null;
        }
    }

    private void OnFingerHoldDown(LeanFinger leanFinger)
    {
        if (finger == leanFinger)
        {
            if (OnHold != null)
            {
                OnHold();
            }
        }
    }

    private void OnPressed()
    {
        if (anim == null)
            return;

        anim.SetTrigger("Pressed");
    }

    private void OnReleased()
    {
        turnedOn = !turnedOn;
        if (anim == null)
            return;

        anim.SetTrigger("Released");
    }

    private bool ContainsPoint(Vector2 pos)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rect, pos);
    }

    public void SetEnabled(bool value)
    {
        gameObject.SetActive(value);
    }
}
