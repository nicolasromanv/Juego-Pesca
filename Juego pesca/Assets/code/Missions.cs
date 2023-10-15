using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Missions : MonoBehaviour {
    // Start is called before the first frame update
    [Header("Datos de misiones")]
    public Renderer parentRenderer;
    public int dificultad;
    public int cantidad;
    public string rareza;
    public int tiempo;

    List<string> rarezas = new List<string> { "comunes", "raros", "peculiares", "legendarios", "exóticos" };
    TextMeshPro missionsTMP;

    void Start() {
        missionsTMP = GetComponent<TextMeshPro>();
        missionsTMP.text = GenerateRandomMission();
    }

    // Update is called once per frame
    void Update() {
        missionsTMP = GetComponent<TextMeshPro>();
        // if (missionsTMP != null) {
        //     Debug.Log("missionsTMP tomada");
        // }
    }

    private int GetCantidadRandom(int dificultad) {
        switch (dificultad) {
            case 0:
                return Random.Range(5, 10);
            default:
                return Random.Range(8, 15);
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
        dificultad = Random.Range(0, 3);
        cantidad = GetCantidadRandom(dificultad);
        rareza = GetRarezaRandom(dificultad);
        tiempo = GetTiempoRandom(dificultad);
        switch (dificultad) {
            case 0:
                parentRenderer.material.SetColor("_EmissionColor", Color.green);
                break;
            case 1:
                parentRenderer.material.SetColor("_EmissionColor", Color.yellow);
                break;
            case 2:
                parentRenderer.material.SetColor("_EmissionColor", Color.red);
                break;
        }
        string missionsTMP = "Recolecta " + cantidad + " peces " + rareza + " en " + tiempo + " [min] o menos.";
        return missionsTMP;
    }

    public void SetRandomMission() {
        missionsTMP.text = GenerateRandomMission();
    }

    public string GetText() {
        return missionsTMP.text;
    }
}
