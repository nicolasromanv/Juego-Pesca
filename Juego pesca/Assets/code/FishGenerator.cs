using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGenerator : MonoBehaviour
{
    public GameObject fishObject;
    public int nFish;
    public float rangoMin;
    public float rangoMax;

    public float spawnHeight;

    public float GetSpawnHeight(){
        return spawnHeight;
    }
    public float GetRangoMin(){
        return rangoMin;
    }
    public float GetRangoMax(){
        return rangoMax;
    }
    
    void Start(){
        SpawnGameObject(fishObject, nFish);
    }
    private Vector3 SetRandomPosition(){
        float randomX = UnityEngine.Random.Range(rangoMin, rangoMax);
        float randomZ = UnityEngine.Random.Range(rangoMin, rangoMax);
        return new Vector3(randomX, spawnHeight, randomZ);
    }

    void SpawnGameObject(GameObject go, int cantidad)
    {
        for (int i = 0; i < cantidad; i++){
            Instantiate(go, SetRandomPosition(), Quaternion.identity);
        }
    }
}
