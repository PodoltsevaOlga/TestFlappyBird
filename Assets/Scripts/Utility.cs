using UnityEngine;

public static class Utility
{
    public static bool IsInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            return true;
        }

        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                return true;
            }
        }

        if (Input.GetMouseButton(0))
        {
            return true;
        }

        return false;
    }
}
