using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderMovement : MonoBehaviour
{
    private float speed = 50.0f;
    private Vector3 lastPosition;
    public GameObject cylinder;

    // Update is called once per frame
    void Update()
    {
        // eixo de rotação x e z do objeto recebem 0 para que o objeto não se mova
        //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        // eixo de rotação x e y do objeto pai recebem 0 para que o objeto não se mova
        //transform.parent.eulerAngles = new Vector3(0, 0, transform.parent.eulerAngles.z);

        // if (Input.touchCount > 0)
        // {
        //     Touch touch = Input.GetTouch(0);

        //     if (touch.phase == TouchPhase.Began)
        //     {
        //         lastPosition = touch.position;
        //     }
        //     else if (touch.phase == TouchPhase.Moved)
        //     {
        //         Vector3 touchPosition3D = new Vector3(touch.position.x, touch.position.y, 0);
        //         Vector3 delta = touchPosition3D - lastPosition;

        //         // se o movimento for na horizontal, o objeto gira em torno do eixo y
        //         if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        //         {
        //             transform.Rotate(0, -delta.x * speed * Time.deltaTime, 0);
        //         }
        //         // se o movimento for na vertical, o objeto pai gira em torno do eixo z
        //         else
        //         {
        //             transform.parent.Rotate(0, 0, delta.y * speed * Time.deltaTime);
        //         }

        //         lastPosition = touchPosition3D;
        //     }
        // }


        // se o botão apertado for arrow left ou arrow right, o objeto gira em torno do eixo z
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, 0, -Input.GetAxis("Horizontal") * speed * Time.deltaTime);
        }
        // se o botão apertado for arrow up ou arrow down, o objeto pai gira em torno do eixo z
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.parent.Rotate(0, 0, -Input.GetAxis("Vertical") * speed * Time.deltaTime);
        }


    }
}