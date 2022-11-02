using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VolumeBox.Toolbox;

public class TrajectoryDrawer : MonoCached
{
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private int dotsCount;
    [SerializeField] private float dotsMinInterval;
    [SerializeField] private float dotsMaxInterval;
    [SerializeField] private float maxVelocityMagnitude;

    private GameObject[] dots;
    private bool cleared;
    
    public override void Rise()
    {
        dots = new GameObject[dotsCount];

        for (int i = 0; i < dotsCount; i++)
        {
            dots[i] = Instantiate(dotPrefab, Vector3.zero, Quaternion.identity, transform);
            dots[i].SetActive(false);
        }
    }

    public void DrawTrajectory(Vector2 initialPosition, Vector2 velocity)
    {
        cleared = false;
        
        for (int i = 0; i < dotsCount; i++)
        {
            float t = Mathf.Clamp01(velocity.magnitude / maxVelocityMagnitude);
            float step = Mathf.Lerp(dotsMinInterval, dotsMaxInterval, t);
            step *= i;
            Vector2 calculatedPosition = initialPosition + velocity * step;
            calculatedPosition.y += Physics2D.gravity.y / 2 * Mathf.Pow(step, 2);
            
            dots[i].SetActive(true);
            dots[i].transform.position = new Vector3(calculatedPosition.x, calculatedPosition.y, 0);
        }
    }

    public void ClearTrajectory()
    {
        if(cleared) return;
        
        foreach (var dot in dots)
        {
            dot.SetActive(false);
        }

        cleared = true;
    }
}
