using UnityEngine;

public class FishColor : MonoBehaviour {
    public Material[] colors; // Arreglo de materiales
    public float[] materialProbabilities = { 0.3f, 0.25f, 0.2f, 0.15f, 0.1f}; // Arreglo de probabilidades para cada material

    private Renderer material; // Declaración del campo material.
    private Transform fishSize;

    // Start is called before the first frame update
    void Start() {
        material = GetComponent<Renderer>(); // Asigna el Renderer al campo material.
        fishSize = GetComponent<Transform>();
        AssignRandomMaterial();
    }

    public void AssignRandomMaterial() {
        if (colors.Length == 0 || materialProbabilities.Length != colors.Length) {
            Debug.LogError("Asegúrate de que los arreglos de materiales y probabilidades sean válidos.");
            return;
        }

        // Calcula el rango de probabilidades acumulativas
        float[] cumulativeProbabilities = new float[materialProbabilities.Length];
        cumulativeProbabilities[0] = materialProbabilities[0];
        for (int i = 1; i < materialProbabilities.Length; i++) {
            cumulativeProbabilities[i] = cumulativeProbabilities[i - 1] + materialProbabilities[i];
        }

        // Genera un número aleatorio dentro del rango total de probabilidades
        float randomValue = Random.Range(0f, cumulativeProbabilities[cumulativeProbabilities.Length - 1]);

        // Encuentra el índice del material en función del valor aleatorio
        int selectedMaterialIndex = 0;
        for (int i = 0; i < cumulativeProbabilities.Length; i++) {
            if (randomValue <= cumulativeProbabilities[i]) {
                selectedMaterialIndex = i;
                break;
            }
        }

        // Obtiene el Renderer del objeto y asigna el material seleccionado
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null) {
            renderer.material = colors[selectedMaterialIndex];
            SetSize(selectedMaterialIndex);
        }
        else {
            Debug.LogError("El objeto no tiene un componente Renderer.");
        }
    }

    public void SetSize(int index) {
        fishSize.transform.localScale = SetValues(index);
    }

    public Vector3 SetValues(int index) {

        Vector3 vec = new Vector3(0, 0, 0);
        switch (index) {
            case 0:
                vec.x = Random.Range(1f, 1.5f);
                vec.y = Random.Range(1f, 1.5f);
                vec.z = Random.Range(2.5f, 3f);
                break;
            case 1:
                vec.x = Random.Range(0.7f, 1.2f);
                vec.y = Random.Range(0.8f, 1.1f);
                vec.z = Random.Range(2.2f, 2.6f);
                break;
            case 2:
                vec.x = Random.Range(0.5f, 1f);
                vec.y = Random.Range(0.6f, 0.8f);
                vec.z = Random.Range(2f, 2.4f);
                break;
            case 3:
                vec.x = Random.Range(0.3f, 0.6f);
                vec.y = Random.Range(0.3f, 0.6f);
                vec.z = Random.Range(1.6f, 2f);
                break;
            default:
                vec.x = Random.Range(0.4f, 0.6f);
                vec.y = Random.Range(0.1f, 0.2f);
                vec.z = Random.Range(1f, 1.5f);
                break;
        }
        return vec;
    }

    public float[] GetRarezas()
    {
        return materialProbabilities;
    }

    public void SetRarezas(float[] rarezas)
    {
        this.materialProbabilities = rarezas;
    }
}