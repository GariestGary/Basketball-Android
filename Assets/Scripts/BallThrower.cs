using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using VolumeBox.Toolbox;

public class BallThrower : MonoCached
{
    [SerializeField] private Rigidbody2D ball;
    [SerializeField] private float deadZoneRadius;
    [SerializeField] private float minForce;
    [SerializeField] private float maxForce;
    //max distance between point where touch starts and end point
    [SerializeField] private float maxTouchDistance;

    private Vector2 startPosition;
    private bool startPosWrited;
    private bool touched;
    private bool readyToThrow;
    private bool readyToThrowNotified;

    private Vector2 throwDirection;
    private float throwForce;
    private Vector2 prevPos;

    public void OnTouch(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (!touched)
            {
                touched = true;
            }
        }
        
        if (ctx.canceled)
        {
            touched = false;
            startPosWrited = false;
            
            if (readyToThrow)
            {
                Throw();
                readyToThrow = false;
            }
        }
    }
    
    public void OnPosition(InputAction.CallbackContext ctx)
    {
        if(!touched) return;
        
        Vector2 pos = ctx.ReadValue<Vector2>();
        
        if (ctx.performed)
        {
            if(pos == prevPos) return;

            if (touched && !startPosWrited)
            {
                startPosition = pos;
                startPosWrited = true;
            }

            float distFromStartPoint = Vector2.Distance(pos, startPosition);

            if (distFromStartPoint > deadZoneRadius)
            {
                if (!readyToThrowNotified)
                {
                    
                }
                
                readyToThrow = true;
                float t = Mathf.Clamp01(distFromStartPoint / maxTouchDistance);
                throwForce = Mathf.Lerp(minForce, maxForce, t);
                throwDirection = (pos - startPosition).normalized;
            }
            else
            {
                readyToThrow = false;
            }

            prevPos = pos;
        }
    }

    private void Throw()
    {
        ball.velocity = throwDirection * throwForce;
    }
}
