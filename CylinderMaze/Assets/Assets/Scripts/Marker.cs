using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    public Transform Cylinder;
    public CylinderMovement cylinderMovement;
    public GameObject plane;
    public float speed = 200f;
    private bool isBallInMarker = true;
    private Vector3 exitDirection;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isBallInMarker)
        {
            // verifica se o cilindro está de cabeça para baixo
            // if (Cylinder.rotation.eulerAngles.x > 90 && Cylinder.rotation.eulerAngles.x < 270)
            // {
            //     // gire o cilindro na direção oposta
            //     Cylinder.Rotate(0, -speed * Time.deltaTime * Mathf.Sign(exitDirection.x), 0);
            //     transform.parent.Rotate(0, speed * Time.deltaTime * Mathf.Sign(exitDirection.x), 0);
            // }
            // else
            // {
            //     // gire o cilindro na direção normal
            //     Cylinder.Rotate(0, speed * Time.deltaTime * Mathf.Sign(exitDirection.x), 0);
            //     transform.parent.Rotate(0, -speed * Time.deltaTime * Mathf.Sign(exitDirection.x), 0);
            // }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            isBallInMarker = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Ball") 
        {
            // debug
            Debug.Log("Ball left the marker");

            // calcula a direção da bola em relação ao marcador
            exitDirection = (other.transform.position - transform.position).normalized;

            isBallInMarker = false;
        
        }


        // TODO: anexar esse script ao filho marker
    }
    
}
