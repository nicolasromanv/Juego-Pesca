using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Missions : MonoBehaviour {
    // Start is called before the first frame update
    List<string> misiones = new List<string>();
    List<string> rarezas = new List<string> { "comunes", "raros", "peculiares", "legendarios", "exóticos" };
    public TextMeshPro misionesList;
    int totalMissions = 4;
    int misionesCount = 0;

    void Start() {
        misiones.Add(GenerateRandomMission());
        misionesList.text = GetRandomMission();
    }

    // Update is called once per frame
    void Update() {
        misionesCount = misiones.Count;
        if (misionesCount < totalMissions ) {
            misiones.Add(GenerateRandomMission());
        }
        Debug.Log(misionesCount);
            misionesList.text = GetRandomMission();
    }

    private int GetCantidadRandom(int dificultad) {
        switch (dificultad) {
            case 0:
                return Random.Range(5, 10);
            default:
                return Random.Range(15, 20);
        }
    }
    private string GetRarezaRandom(int dificultad) {
        int index;
        switch (dificultad) {
            case 0:
                index = Random.Range(0, 1);
                break;
            case 1:
                index = Random.Range(1, 3);
                break;
            default:
                index = Random.Range(3, 5);
                break;
        }
        return rarezas[index];
    }
    private int GetTiempoRandom(int dificultad) {
        switch (dificultad) {
            case 0:
                return Random.Range(3, 4);
            case 1:
                return Random.Range(2, 3);
            default:
                return Random.Range(1, 2);
        }
    }
    private string GenerateRandomMission() {
        misionesCount++;
        int dificultad = Random.Range(0, 2);
        int cantidad = GetCantidadRandom(dificultad);
        string rareza = GetRarezaRandom(dificultad);
        int tiempo = GetTiempoRandom(dificultad);
        string mision = "Recolecta " + cantidad + " peces " + rareza + " en " + tiempo + " [min] o menos.";
        return mision;
    }

    private string GetRandomMission() {
        int index = Random.Range(0, misiones.Count);
        Debug.Log("Index: "+index+". Mision: " + misiones[index]);
        return misiones[index];
    }
}
