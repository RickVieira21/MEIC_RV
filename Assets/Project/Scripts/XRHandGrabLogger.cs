using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class XRHandGrabLogger : XRGrabInteractable
{
    private InteractableObject interactableObject;

    protected override void Awake()
    {
        base.Awake();
        interactableObject = GetComponent<InteractableObject>();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // Determine which hand is grabbing the object
        interactableObject.SetGrabbingHand(args.interactorObject.transform.parent.name.Contains("Left") ? "Left Hand" : "Right Hand");
        Debug.Log(args.interactorObject.transform.parent.name);

        // Log the grabbing hand
        interactableObject.IsSelected();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        // Handle the release of the object
        interactableObject.IsNotSelected();
    }
}
