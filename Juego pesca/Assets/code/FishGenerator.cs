using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGenerator : MonoBehaviour {
    [Header("Prefab")]
    public GameObject fishObject;

    [Header("Valores")]
    public int nFish;
    public float rangoMin;
    public float rangoMax;
    public float spawnHeight;

    private GameObject[] fishes;
    
    void Start(){
        // fishes = FindObjectsByType<GameObject>(fishObject);
        SpawnGameObject(fishObject, nFish);
    }

    private void Update() {
        // 
    }
    public float GetSpawnHeight(){
        return spawnHeight;
    }

    public float GetRangoMin(){
        return rangoMin;
    }

    public float GetRangoMax(){
        return rangoMax;
    }

    private Vector3 SetRandomPosition(){
        float randomX = UnityEngine.Random.Range(rangoMin, rangoMax);
        float randomZ = UnityEngine.Random.Range(rangoMin, rangoMax);
        return new Vector3(randomX, spawnHeight, randomZ);
    }

    void SpawnGameObject(GameObject go, int cantidad) {
        for (int i = 0; i < cantidad; i++){
            Instantiate(go, SetRandomPosition(), Quaternion.identity);
        }
    }
}
