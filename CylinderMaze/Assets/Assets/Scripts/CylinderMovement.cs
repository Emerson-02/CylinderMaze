using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderMovement : MonoBehaviour
{
    private float speed = 50.0f;
    private Vector3 lastPosition;
    public Transform cylinder;
    public Transform ball;
    public Transform ballPosition;
    public float rotationSpeed = 100f;
    public float smoothTime = 0.3F;
    private float yVelocity = 0.0F;
    public Transform camera;
    public Transform Marker;
    public Transform markerBall;
    public Transform yPosBall;
    public Vector3 frontCylinder;


    void Start()
    {
        // Armazene a posição original da bola
        // ballPosition = ball.position;

        // // Calcular a distância x e z entre a bola e o yPosBall
        // float distanceX = ball.position.x - yPosBall.position.x;
        // float distanceZ = ball.position.z - yPosBall.position.z;

        // frontCylinder = new Vector3(distanceX, 0, distanceZ);
    }

    // Update is called once per frame
    void Update()
    {

        
        ballPosition.position = new Vector3(ball.position.x, ball.position.y, ball.position.z);

        // y do yPosBall acompanha o y da bola
        //yPosBall.position = new Vector3(ball.position.x, ball.position.y, ball.position.z);


        


        // se o botão apertado for arrow left ou arrow right, o objeto gira em torno do eixo z
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, -Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0);

            Marker.Rotate(0, Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0);

    
        }
        // se o botão apertado for arrow up ou arrow down, o objeto pai gira em torno do eixo z
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.parent.Rotate(0, 0, -Input.GetAxis("Vertical") * speed * Time.deltaTime);

            
        }


        



        
        // // Calcular a distância x e z entre a bola e o yPosBall
        // float distanceX = ball.position.x - yPosBall.position.x;
        // float distanceZ = ball.position.z - yPosBall.position.z;

        // // bota os valores em um vetor
        // Vector3 distance = new Vector3(distanceX, 0, distanceZ);

        // // imprime da distância
        // Debug.Log(distance);


        // // Calcular os vetores em relação à posição do cilindro
        // Vector3 frontCylinderRelativeToCylinder = new Vector3(frontCylinder.x, 0, frontCylinder.z) - new Vector3(yPosBall.position.x, 0, yPosBall.position.z);
        // Vector3 distanceRelativeToCylinder = new Vector3(distance.x, 0, distance.z) - new Vector3(yPosBall.position.x, 0, yPosBall.position.z);

        // // Calcular o ângulo entre os dois vetores em graus
        // float angleInDegrees = Vector3.Angle(frontCylinderRelativeToCylinder, distanceRelativeToCylinder);

        // // Calcular o raio como a distância entre a posição do cilindro e o ponto representado pelo vetor distance
        // float radius = distanceRelativeToCylinder.magnitude;

        // // Calcular o arco
        // float arc = angleInDegrees * radius;

        // // Imprimir o arco
        // Debug.Log("Arco: " + arc);



        // TODO: fazer o y do yPosBall acompanhar o y da bola





    }


    void LateUpdate()
    {
        yPosBall.position = new Vector3(ballPosition.position.x - ballPosition.position.x, ballPosition.position.y, ballPosition.position.z - ballPosition.position.z);
    }
}