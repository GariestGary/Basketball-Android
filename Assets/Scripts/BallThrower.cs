using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using VolumeBox.Toolbox;
using Random = UnityEngine.Random;

public class BallThrower : MonoCached
{
    [SerializeField] private Rigidbody2D ball;
    [SerializeField] private float deadZoneRadius;
    [SerializeField] private float minForce;
    [SerializeField] private float maxForce;
    //max distance between point where touch starts and end point
    [SerializeField] private float maxTouchDistance;
    [SerializeField] private TrajectoryDrawer drawer;
    [SerializeField] private BoxCollider2D ballStartArea;

    [Inject] private Messager msg;

    private Rigidbody2D ballRB;
    private Vector2 startPosition;
    private Vector2 throwDirection;
    private float throwForce;
    private bool startPosWrited;
    private bool touched;
    private bool readyToThrow;
    private bool readyToThrowNotified;
    private bool throwed = true;


    public override void Rise()
    {
        ballRB = ball.GetComponent<Rigidbody2D>();
        ResetPosition();
    }

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
            readyToThrowNotified = false;
            
            if (readyToThrow)
            {
                Throw();
                readyToThrow = false;
                drawer.ClearTrajectory();
            }
        }
    }
    
    public void OnPosition(InputAction.CallbackContext ctx)
    {
        if(!touched) return;
        
        Vector2 pos = ctx.ReadValue<Vector2>();

        if (ctx.performed)
        {
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
                    ResetPosition();
                    throwed = false;
                    msg.Send(Message.READY_TO_THROW);
                    readyToThrowNotified = true;
                }
                    
                readyToThrow = true;
                float t = Mathf.Clamp01(distFromStartPoint / maxTouchDistance);
                throwForce = Mathf.Lerp(minForce, maxForce, t);
                throwDirection = (pos - startPosition).normalized;
                drawer.DrawTrajectory(ball.transform.position, throwDirection * throwForce);
            }
            else
            {
                readyToThrow = false;
                drawer.ClearTrajectory();
            }
        }
    }

    private void Throw()
    {
        ballRB.bodyType = RigidbodyType2D.Dynamic;
        ball.velocity = throwDirection * throwForce;
        throwed = true;
    }

    public void ResetPosition()
    {
        if(!throwed) return;
        
        ballRB.velocity = Vector2.zero;
        ballRB.bodyType = RigidbodyType2D.Static;
        Vector2 pos;
        Vector2 max = ballStartArea.bounds.max;
        Vector2 min = ballStartArea.bounds.min;
        pos.x = Random.Range(min.x, max.x);
        pos.y = Random.Range(min.y, max.y);
        ball.transform.position = pos;

        throwed = false;
    }
}
