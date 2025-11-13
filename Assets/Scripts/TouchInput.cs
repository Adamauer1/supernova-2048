using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class TouchInput : MonoBehaviour
{
    
    public static TouchInput Instance { get; private set; }
    
    public event EventHandler OnSwipeRight;
    public event EventHandler OnSwipeLeft;
    public event EventHandler OnSwipeUp;
    public event EventHandler OnSwipeDown;

    
    private Vector2 m_startTouchPosition;
    private Vector2 m_endTouchPosition;

    private bool m_isSwiping = false;
    
    [SerializeField] private float m_minSwipeDistance = 100f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        TouchControl touch = Touchscreen.current?.primaryTouch;

        if (touch == null)
        {
            return;
        }

        if (touch.press.wasPressedThisFrame)
        {
            m_startTouchPosition = touch.position.ReadValue();
            m_isSwiping = true;
        }
        
        else if (touch.press.wasReleasedThisFrame)
        {
            m_endTouchPosition = touch.position.ReadValue();
            if (m_isSwiping)
            {
                DetectSwipe();
            }
            m_isSwiping = false;
        }
    }

    private void DetectSwipe()
    {
        Vector2 swipeDelta = m_endTouchPosition - m_startTouchPosition;

        if (swipeDelta.magnitude < m_minSwipeDistance)
        {
            return;
        }

        float x = swipeDelta.x;
        float y = swipeDelta.y;

        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            if (x > 0)
            {
                Debug.Log("Swipe Right");
                OnSwipeRight?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Debug.Log("Swipe Left");
                OnSwipeLeft?.Invoke(this, EventArgs.Empty);
            }
        }
        else
        {
            if (y > 0)
            {
                Debug.Log("Swipe Up");
                OnSwipeUp?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Debug.Log("Swipe Down");
                OnSwipeDown?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    
    //private void OnSwipeRight() => Debug.Log("Swipe Right");
    // private void OnSwipeLeft() => Debug.Log("Swipe Left");
    //private void OnSwipeUp() => Debug.Log("Swipe Up");
    //private void OnSwipeDown() => Debug.Log("Swipe Down");
}
