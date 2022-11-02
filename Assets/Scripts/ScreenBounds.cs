using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VolumeBox.Toolbox;

public static class ScreenBounds
{
    public static Vector2 UpperPointWorld => Camera.main.ViewportToWorldPoint(Vector2.up);
    public static Vector2 BottomPointWorld => -UpperPointWorld;
    public static Vector2 RightPointWorld => Camera.main.ViewportToWorldPoint(Vector2.right);
    public static Vector2 LeftPointWorld => -RightPointWorld;
}
