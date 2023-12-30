using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderMovement2 : MonoBehaviour
{
    private float rotationSpeed = 50.0f; 
    public Transform center, ballPosition, ball, cylinder, yPosBall, front;
    public float radius;


    // Start is called before the first frame update
    void Start()
    {
        // Raio do cilindro
        radius = cylinder.localScale.x / 2;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 relativePosition = cylinder.InverseTransformPoint(ball.position);
        //Debug
        Debug.Log("relativePosition: " + relativePosition);

        // Atribuir a posição de 'ballPosition' para ser a mesma que a posição de 'ball', mas em relação a 'cylinder'
        ballPosition.position = cylinder.TransformPoint(relativePosition);

        // relative position de front no cilindro
        Vector3 relativePositionFront = cylinder.InverseTransformPoint(front.position);

        //Debug
        Debug.Log("relativePositionFront: " + relativePositionFront);



        

        // calcula o angulo entre relativePosition e relativePositionFront
        float angle = Mathf.Atan2(relativePositionFront.z - relativePosition.z, relativePositionFront.x - relativePosition.x);


        // Calcular a posição x e y do arco
        float x = radius * Mathf.Cos(angle);
        float z = radius * Mathf.Sin(angle);

        // A posição do arco no plano xy do cilindro
        Vector3 arcPosition = new Vector3(x, 0, z);

        // debug
        // Calcular a distância do arco
        float arcDistance = radius * angle;

        // debug
        Debug.Log("Arc Distance: " + arcDistance);


  
        // gira o cilindro no eixo y local somando arcDistance
        cylinder.Rotate(0, arcDistance, 0, Space.Self);

        
        // Mover yPosBall no eixo y com base na posição y de relativePosition
        //yPosBall.position = new Vector3(yPosBall.position.x, relativePosition.y, yPosBall.position.z);



        // Verifica se a tecla esquerda está sendo pressionada
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // Gira o cilindro em torno do eixo y em uma direção em relação ao objeto local
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);
        }

        // Verifica se a tecla direita está sendo pressionada
        if (Input.GetKey(KeyCode.RightArrow))
        {
            // Gira o cilindro em torno do eixo y na direção oposta em relação ao objeto local
            transform.Rotate(-Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);
        }

        // Verifica se a tecla para cima está sendo pressionada
        if (Input.GetKey(KeyCode.UpArrow))
        {
            // Gira o cilindro em torno do eixo z em uma direção em relação ao mundo
            transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime, Space.World);
        }

        // Verifica se a tecla para baixo está sendo pressionada
        if (Input.GetKey(KeyCode.DownArrow))
        {
            // Gira o cilindro em torno do eixo z na direção oposta em relação ao mundo
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}
