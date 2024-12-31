using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableCustom : XRGrabInteractable
{

    public Transform leftAttachTransform;
    public Transform rightAttachTransform;
    

    protected override void OnSelectEntered(SelectEnterEventArgs args) {

        // Debug.Log(args.interactableObject.transform.tag);   
        // Debug.Log(args.interactableObject.transform.name);   
        Debug.Log(args.interactorObject.transform.name);
        Debug.Log(args.interactorObject.transform.tag);

        if (args.interactorObject.transform.CompareTag("left hand")) {
            attachTransform = leftAttachTransform;
        }
        else if (args.interactorObject.transform.CompareTag("right hand")) {
            attachTransform = rightAttachTransform;
        }

        base.OnSelectEntered(args);
    }
}
