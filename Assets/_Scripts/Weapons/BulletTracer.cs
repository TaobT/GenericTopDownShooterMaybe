using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTracer : MonoBehaviour
{
    private Vector2 endPosition;
    private bool hasEndPosition;
    private float speed;

    private void Update()
    {
        if (hasEndPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
        }

        if (transform.position == (Vector3) endPosition) Destroy(gameObject);
    }

    public void InitializeTrace(Vector2 endPos, float speed)
    {
        this.endPosition = endPos;
        this.speed = speed;
        hasEndPosition = true;
    }
}
