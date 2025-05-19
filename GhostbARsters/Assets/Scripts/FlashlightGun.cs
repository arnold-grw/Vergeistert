using UnityEngine;
using UnityEngine.InputSystem;
using Unity.XR.Oculus;

public class FlashlightGun : MonoBehaviour
{
    public float energy = 100;
    public LayerMask obstacleLayers;

    public AudioClip shot_audio;

    private InputAction shootAction;

    void Awake()
    {
        // Neue InputAction für linke Maustaste
        shootAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
        shootAction.Enable();
    }

    void Start()
    {

    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            Shoot();
        }

        if (shootAction.WasPressedThisFrame())
        {
            Shoot();
        }
    }

    void Shoot()
    {
        energy -= 5;
        PlayAudio(shot_audio);

        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;
        if (Physics.Raycast(origin, direction, out RaycastHit hit, 20f, obstacleLayers))
        {
            GhostBehaviour ghost = hit.collider.GetComponent<GhostBehaviour>();
            if (ghost != null)
            {
                ghost.TakeDamage(2f);
            }

            PhysicsObject po = hit.collider.GetComponent<PhysicsObject>();
            if (po != null)
            {
                po.applyImpact(direction, 4f);
            }
        }
    }

    void PlayAudio(AudioClip clip)
{
    if (clip == null) return;

    GameObject tempAudio = new GameObject("TempAudio");
    tempAudio.transform.position = transform.position;

    AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
    audioSource.clip = clip;
    audioSource.Play();

    Destroy(tempAudio, clip.length);
}
}