using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VolumeBox.Toolbox;

[RequireComponent(typeof(EdgeCollider2D))]
public class GameBoundsSetter : MonoCached
{
    private EdgeCollider2D edge;

    public override void Rise()
    {
        edge = GetComponent<EdgeCollider2D>();
        SetBounds();
    }

    public void SetBounds()
    {
        if (edge == null)
        {
            Debug.LogWarning("EdgeCollider2D is null");
            return;
        }

        Vector2[] points = new Vector2[5];

        float up = ScreenBounds.UpperPointWorld.y;
        float bottom = -up;
        float right = ScreenBounds.RightPointWorld.x;
        float left = -right;

        //upper left point
        points[0].x = left;
        points[0].y = up;
        
        //upper right point
        points[1].x = right;
        points[1].y = up;
        
        //lower right point
        points[2].x = right;
        points[2].y = bottom;
        
        //lower left point
        points[3].x = left;
        points[3].y = bottom;
        
        //end upper left point
        points[4].x = left;
        points[4].y = up;

        edge.points = points;
    }
}
