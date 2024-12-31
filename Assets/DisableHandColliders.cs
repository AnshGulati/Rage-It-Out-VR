using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DisableHandColliders : MonoBehaviour
{
    // set this variable by checking "use reference" and then search "select value"
    public InputActionProperty gripAnimationAction;

    private bool collidersEnabled = false;
    // set this variable on select for ray and grab interact 
    private bool isHoldingObject = false;

    private Collider[] handColliders;
    // Start is called before the first frame update
    void Start()
    {
        handColliders = GetComponentsInChildren<Collider>();
        DisableColliders();
    }

    public void SetHoldingObject(bool isHolding) {
        isHoldingObject = isHolding;
    }

    private void DisableColliders() {
        foreach (Collider c in handColliders) {
            // dont disable the direct interactor collider
            if (c.gameObject.name == "Direct Interactor") {
                continue;
            }
            c.enabled = false;
        }
    }

    private void EnableColliders() {
        foreach (Collider c in handColliders) {
            c.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if grip val is near 1, then hand is in fist
        float gripVal = gripAnimationAction.action.ReadValue<float>();
        bool inFist = gripVal > 0.9f;
        // if hand is in fist position and is not holding object, then enable colliders. otherwise disable colliders
        if (inFist && !isHoldingObject) {
            if (!collidersEnabled) {
                EnableColliders();
                collidersEnabled = true;
            }
        } else {
            if (collidersEnabled) {
                DisableColliders();
                collidersEnabled = false;
            }
        }

    }
}
