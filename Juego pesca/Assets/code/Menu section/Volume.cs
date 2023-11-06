using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    public Slider sliderMusica;
    public Slider sliderFX;

    public float sliderValueMusica;
    public float sliderValueFX;

    public Image imageMute;
    void Start(){
        sliderMusica.value = PlayerPrefs.GetFloat("VolumenMusica", 0.1f);
        sliderMusica.value = PlayerPrefs.GetFloat("VolumenFX", 0.1f);
        AudioListener.volume = sliderMusica.value;
        GetOnMute();
    }

    public void ChangeSlider(float value){
        sliderValueMusica = value;
        PlayerPrefs.SetFloat("VolumenMusica", sliderValueMusica);
        AudioListener.volume = sliderMusica.value;
        GetOnMute();
    }

    public void GetOnMute(){
        imageMute.enabled = sliderValueMusica == 0? true : false;
    }
}
