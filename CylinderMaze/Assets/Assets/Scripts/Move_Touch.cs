using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Touch : MonoBehaviour
{
    void update()
    {
        Debug.Log("o");
        if (Input.touchCount > 0)
        {
            Debug.Log("Move");
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Moved)
            {
                Vector2 rot = new Vector2(t.deltaPosition.x, t.deltaPosition.y);
                transform.Rotate(rot * 10f * Time.deltaTime, Space.World);
            }
        }
    }
}
