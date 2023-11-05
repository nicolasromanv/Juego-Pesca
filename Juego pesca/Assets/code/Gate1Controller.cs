using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate1Controller : MonoBehaviour
{
    public float interactionDistance = 2f; // Distance for player interaction.
    private bool isLocked = true; // Initial state is locked.

    private Collider gateCollider;

    void Start()
    {
        gateCollider = GetComponent<Collider>();

        if (gateCollider == null)
        {
            Debug.LogError("Gate-1-NULL");
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
        // Disable the gate's collider to unlock it.
        gateCollider.enabled = false;

        // Add any additional logic you need for unlocking the gate here.
        isLocked = false;
        Debug.Log("Gate-1 desbloqueada, collider FALSE");
    }
}
