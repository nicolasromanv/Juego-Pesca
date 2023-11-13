using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGate1 : MonoBehaviour
{
    public Transform player;
    public Transform obstacle; 
    public float interactionDistance = 5.5f; 
    public GameObject obstacleObject; 
    public GameObject textoObject;
    public GameObject isWaypointGate1;

    public bool gate1Flag=false;

    void Update()
    {
        // Calcula la distancia entre el jugador y el obstáculo.
        float distance = Vector3.Distance(player.position, obstacle.position);

        if (distance <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.E)) //presiona E cerca de la "gate-1"
            {
                isWaypointGate1.gameObject.SetActive(true);
                obstacleObject.GetComponent<MeshCollider>().enabled = false;
                obstacleObject.GetComponent<MeshRenderer>().enabled = false;
                textoObject.GetComponent<MeshRenderer>().enabled = false;

                Debug.Log("PUERTA 1 OPEN :D!!");
                gate1Flag=true;
            }
        }
        //CAMBIAR COORDENADAS A LA VILLA !!!!
        if(gate1Flag==true && Input.GetKeyDown(KeyCode.Alpha2)){
            Debug.Log("Tecla 2 presionada");
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

