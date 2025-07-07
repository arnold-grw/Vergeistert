using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabManager : MonoBehaviour
{
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    public AudioSource grabSound;
    void Awake()
    {
        //grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
    }


    void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log("I been grabbed");
        if (grabSound != null)
        {
            grabSound.Play();
        }
    }
}
