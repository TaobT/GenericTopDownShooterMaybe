using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTracerController : MonoBehaviour
{
    [SerializeField] private GameObject trailPf;
    [SerializeField] private float trailSpeed;

    public void DrawTrace(Vector2 startPos, Vector2 endPos)
    {
        Instantiate(trailPf, startPos, Quaternion.identity).GetComponent<BulletTracer>().InitializeTrace(endPos, trailSpeed);
    }
}
