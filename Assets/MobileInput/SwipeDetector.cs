using UnityEngine;
using Lean;
using System.Collections;
using System;

public class SwipeDetector : MonoBehaviour {

    [SerializeField] MobileInputArea swipeArea;

    private SwipeDirection sSwipeDirection; //receives swipe output

    protected virtual void OnEnable()
    {
        LeanTouch.OnFingerSwipe += OnFingerSwipe;     
    }

    protected virtual void OnDisable()
    {
        LeanTouch.OnFingerSwipe -= OnFingerSwipe;
    }

    // Use this for initialization
    protected virtual void Start()
    {
        sSwipeDirection = SwipeDirection.Null;
    }

    public SwipeDirection GetSwipeDirection() // to be used by Update()
    {
        if (sSwipeDirection != SwipeDirection.Null)//if a swipe is detected
        {
            SwipeDirection etempSwipeDirection = sSwipeDirection;
            sSwipeDirection = SwipeDirection.Null;

            return etempSwipeDirection;
        }
        else
            return SwipeDirection.Null;//if no swipe was detected
    }

    void OnFingerSwipe(LeanFinger finger)
    {
        if (!swipeArea.ContainsPoint(finger.StartScreenPosition))
            return;

        // Store the swipe delta in a temp variable
        var swipe = finger.SwipeDelta;

        if (swipe.x < -Mathf.Abs(swipe.y))
        {
            sSwipeDirection = SwipeDirection.Left;
        }

        if (swipe.x > Mathf.Abs(swipe.y))
        {
            sSwipeDirection = SwipeDirection.Right;
        }

        if (swipe.y < -Mathf.Abs(swipe.x))
        {
            sSwipeDirection = SwipeDirection.Down;
        }

        if (swipe.y > Mathf.Abs(swipe.x))
        {
            sSwipeDirection = SwipeDirection.Up;
        }
    }
}

public enum SwipeDirection
{
    Null = 0, //no swipe detected
    Down = 1, //swipe down detected
    Up = 2, //swipe up detected
    Right = 3, //swipe right detected
    Left = 4 //swipe left detected
}

