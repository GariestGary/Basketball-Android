using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using VolumeBox.Toolbox;

public class Basket : MonoCached
{
    [SerializeField] private float heightOffset;
    [SerializeField] [Tag] private string ballTag;
    [SerializeField] private Transform ringPoint;

    [Inject] private Messager msg;
    
    public override void Rise()
    {
        SetPosition();
    }

    private void SetPosition()
    {
        transform.position = new Vector3(-ScreenBounds.RightPointWorld.x, heightOffset, 0);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(!other.gameObject.CompareTag(ballTag)) return;

        if (other.transform.position.y < ringPoint.position.y)
        {
            msg.Send(Message.BALL_PASSED_RING);
        }
    }
}
