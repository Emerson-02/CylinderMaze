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
            // gire o cilindro em torno do eixo y na direção em que a bola saiu do marcador
            Cylinder.Rotate(0, speed * Time.deltaTime * exitDirection.x, 0);
            
            // o pai do marcador gira em torno do eixo y a uma velocidade constante contrária à do cilindro
            transform.parent.Rotate(0, -speed * Time.deltaTime * exitDirection.x, 0);
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
