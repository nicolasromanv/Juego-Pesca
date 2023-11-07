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

    void Update()
    {
        // Calcula la distancia entre el jugador y el obstáculo.
        float distance = Vector3.Distance(player.position, obstacle.position);

        if (distance <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.E)) //presiona E cerca de la "gate-2"
            {
                obstacleObject.GetComponent<MeshCollider>().enabled = false;
                obstacleObject.GetComponent<MeshRenderer>().enabled = false;
                textoObject.GetComponent<MeshRenderer>().enabled = false;

                Debug.Log("PUERTA 2 OPEN :D!!");

            }
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
