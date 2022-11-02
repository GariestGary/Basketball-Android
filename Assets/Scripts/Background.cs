using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using VolumeBox.Toolbox;

[RequireComponent(typeof(SpriteRenderer))]
public class Background : MonoCached
{
    private SpriteRenderer sr;
    
    public override void Rise()
    {
        ResizeSpriteToScreen();
    }
    
    [Button("Resize Sprite To Screen")]
    public void ResizeSpriteToScreen() 
    {
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }
        
        float spriteWidth = sr.sprite.bounds.size.x;
        var width = ScreenBounds.RightPointWorld.x * 2;
        
        transform.localScale = Vector3.one * width / spriteWidth;
    }
}
