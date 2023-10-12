using UnityEngine;

public class FishColor : MonoBehaviour
{
    private Renderer material; // Declaración del campo material.

    public Material colors_white;
    public Material colors_green;
    public Material colors_blue;
    public Material colors_purple;
    public Material colors_yellow;

    private Material[] colors;
    private Transform fishSize;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>(); // Asigna el Renderer al campo material.
        fishSize = GetComponent<Transform>();
        colors = new Material[] { colors_white, colors_green, colors_blue, colors_purple, colors_yellow };
        AssignRandomMaterial();
    }
    public void AssignRandomMaterial()
    {
        // Selecciona un índice aleatorio dentro del rango de materiales
        int randomMaterialIndex = Random.Range(0, colors.Length);

        // Obtiene el Renderer del objeto y asigna el material seleccionado
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null){
            // Utiliza el material seleccionado del arreglo fishMaterials o de las variables públicas
            renderer.material = colors[randomMaterialIndex];
            SetSize(randomMaterialIndex);
        }
        else{
            Debug.LogError("El objeto no tiene un componente Renderer.");
        }
    }
    public void SetSize(int index)
    {
        fishSize.transform.localScale = SetValues(index);
    }
    public Vector3 SetValues(int index)
    {

        Vector3 vec = new Vector3(0, 0, 0);
        switch (index)
        {
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
}