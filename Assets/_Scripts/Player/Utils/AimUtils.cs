using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AimUtils
{
    private static Vector2 lastMousePosition;
    private static Vector2 localPosition;

    //Returns mouse position every time is different
    //transformed to local position according to a transform
    public static Vector2 GetLocalMousePosition(Transform t)
    {
        if(InputManager.mousePosition != lastMousePosition)
        {
            localPosition = t.InverseTransformPoint(InputManager.mousePosition);
            lastMousePosition = InputManager.mousePosition;
        }

        return localPosition;
    }
}
