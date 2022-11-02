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
    [SerializeField] private TrajectoryDrawer drawer;
    [SerializeField] private BoxCollider2D ballStartArea;

    [Inject] private Messager msg;

    private Rigidbody2D ballRB;
    private Vector2 startPosition;
    private bool startPosWrited;
    private bool touched;
    private bool readyToThrow;
    private bool readyToThrowNotified;

    private Vector2 throwDirection;
    private float throwForce;

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

    private void Throw()
    {
        ballRB.bodyType = RigidbodyType2D.Dynamic;
        ball.velocity = throwDirection * throwForce;
    }

    public void ResetPosition()
    {
        ballRB.velocity = Vector2.zero;
        ballRB.bodyType = RigidbodyType2D.Static;
        Vector2 pos;
        float x = ballStartArea.bounds.max.x;
        float y = ballStartArea.bounds.max.y;
        pos.x = Random.Range(-x, x);
        pos.y = Random.Range(-y, y);
        ball.transform.position = pos;
    }
}
