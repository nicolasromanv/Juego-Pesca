using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate2Controller : MonoBehaviour
{
    public float interactionDistance = 2f; // Distance for player interaction.
    private bool isLocked = true; // Initial state is locked.

    private Collider gateCollider;

    void Start()
    {
        gateCollider = GetComponent<Collider>();

        if (gateCollider == null)
        {
            Debug.LogError("Gate-2-NULL");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // E para interactuar con la puerta
        {
            if (IsPlayerNearGate())
            {
                if (isLocked)
                {
                    UnlockGate();
                }
                else
                {
                    Debug.Log("La puerta ya está desbloqueada :)");
                }
            }
        }
    }

    bool IsPlayerNearGate()
    {
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);

        return distance <= interactionDistance;
    }

    void UnlockGate()
    {
        gateCollider.enabled = false;

        isLocked = false;
        Debug.Log("Gate-2 desbloqueada, collider FALSE");
    }
}
