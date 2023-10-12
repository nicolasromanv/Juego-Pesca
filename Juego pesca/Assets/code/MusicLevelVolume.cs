using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLevelVolume : MonoBehaviour {
    AudioSource musicSource;

    // Start is called before the first frame update
    void Start() {
        musicSource = GetComponent<AudioSource>();
        musicSource.volume = PlayerPrefs.GetFloat("volumenAudio");
    }

    // Update is called once per frame
    void Update() {
        
    }
}
