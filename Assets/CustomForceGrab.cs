using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomRayInteractor : XRRayInteractor
{
    private XRInteractorLineVisual lineVisual;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // Disable line visual
        lineVisual = GetComponent<XRInteractorLineVisual>();
        if (lineVisual != null)
        {
            lineVisual.enabled = false;
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        // Re-enable line visual
        if (lineVisual != null)
        {
            lineVisual.enabled = true;
        }
        lineVisual = null;
    }
}