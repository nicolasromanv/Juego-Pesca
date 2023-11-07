using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGate2 : MonoBehaviour
{
    public Transform player;
    public Transform obstacle; 
    public float interactionDistance = 5.5f; 
    public GameObject obstacleObject; 
    public GameObject textoObject;
    public GameObject isWaypointGate2;

    public bool gate2Flag=false;


    void Update()
    {
        // Calcula la distancia entre el jugador y el obstáculo.
        float distance = Vector3.Distance(player.position, obstacle.position);

        if (distance <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.E)) //presiona E cerca de la "gate-2"
            {
                isWaypointGate2.gameObject.SetActive(true);
                obstacleObject.GetComponent<MeshCollider>().enabled = false;
                obstacleObject.GetComponent<MeshRenderer>().enabled = false;
                textoObject.GetComponent<MeshRenderer>().enabled = false;

                Debug.Log("PUERTA 2 OPEN :D!!");
                gate2Flag=true;

            }
        }
        //CAMBIAR COORDENADAS A LA MONTAÑA!!!!
        if(gate2Flag==true && Input.GetKeyDown(KeyCode.Alpha3)){
            Debug.Log("Tecla 3 presionada");
            player.position = new Vector3(-176.4f, -1.28f, 234f);
        }
    }
    //función para detectar colisión que hice en la entrega 1 y quedó ahí...
    void OnCollisionEnter (Collision collisionInfo){
        //Debug.Log(collisionInfo.collider.name);
        if (collisionInfo.collider.tag =="obstaculo"){
            Debug.Log("hit!!");
        }
    }
}
