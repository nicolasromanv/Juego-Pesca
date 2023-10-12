using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{

    public float speed = 1.0f;
    public FishGenerator fishGenScript;

    private Vector3 randomTarget;
    private float spawnHeight = 40;
    private float rangoMin;
    private float rangoMax;

    void Start(){
        SetRandomTarget();
        spawnHeight = fishGenScript.GetSpawnHeight();
        rangoMin = fishGenScript.GetRangoMin();
        rangoMax = fishGenScript.GetRangoMax();
    }

    void Update(){
        Move();
    }

    private void GoTo(Vector3 targetPos){
        Vector3 realTargetPos = targetPos;
        transform.LookAt(realTargetPos);

        transform.position = Vector3.MoveTowards(transform.position, realTargetPos, speed * Time.deltaTime);
    }

    private void SetRandomTarget(){
        // Generar un destino aleatorio dentro de un rango
        float randomX = Random.Range(rangoMin, rangoMax);
        float randomZ = Random.Range(rangoMin, rangoMax);
        randomTarget = new Vector3(randomX, spawnHeight, randomZ);
    }

    private void Move(){
        GoTo(randomTarget);
        if (Vector3.Distance(transform.position, randomTarget) <= 0.1f){
            SetRandomTarget();
        }
    }


}
