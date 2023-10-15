using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Middleware : MonoBehaviour {
    public TextMeshPro texto;
    public string GetText() {
        return texto.text;
    }
}
