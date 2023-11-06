using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ImageQuality : MonoBehaviour{

    public TMP_Dropdown dropdown;
    public int quality;

    void Start(){
        quality = PlayerPrefs.GetInt("NumeroCalidad", 1);
        dropdown.value = quality;
        ChangeQuality();
    }

    public void ChangeQuality(){
        QualitySettings.SetQualityLevel(dropdown.value);
        PlayerPrefs.SetInt("NumeroCalidad", dropdown.value);

        quality = dropdown.value;

    }

}
