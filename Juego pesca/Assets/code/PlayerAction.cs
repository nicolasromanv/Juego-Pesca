using Grapple;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{

    [SerializeField] GameObject ObjectToShow;
    [SerializeField] GrapplingHookLogic hookLogic;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ObjectToShow.SetActive(true);
            hookLogic.inStore = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ObjectToShow.SetActive(false);
            hookLogic.inStore = false;
        }
    }
}
