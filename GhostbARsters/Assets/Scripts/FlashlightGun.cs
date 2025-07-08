using UnityEngine;
using UnityEngine.InputSystem;
using Unity.XR.Oculus;

public class FlashlightGun : MonoBehaviour
{
    public float energy = 100f;
    public LayerMask obstacleLayers;

    public AudioSource shootAudioSource;
    public ParticleSystem shootEffect;

    public float fireRate = 0.1f; // 10 Schüsse pro Sekunde

    private InputAction shootAction;
    private bool isShooting = false;
    private float nextShotTime = 0f;

    void Awake()
    {
        shootAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
        shootAction.Enable();
    }

    void Update()
    {
        // Eingabe prüfen
        bool triggerHeld = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) ||
                           shootAction.IsPressed();
        if (triggerHeld)
        {
            if (shootAudioSource != null && !shootAudioSource.isPlaying)
            {
                shootAudioSource.Play();
            }
            isShooting = true;

            if (Time.time >= nextShotTime)
            {
                nextShotTime = Time.time + fireRate;
                Shoot();
            }

            // Partikeleffekt starten, falls nicht schon aktiv
            if (shootEffect != null && !shootEffect.isPlaying)
            {
                shootEffect.Play();
            }
        }
        else
        {
            isShooting = false;

            // Partikeleffekt stoppen
            if (shootEffect != null && shootEffect.isPlaying)
            {
                shootEffect.Stop();
                shootAudioSource.Stop();
            }
        }
    }

    void Shoot()
    {
        //if (energy <= 0f) return;
        //energy -= 5;
        //PlayAudio(shotAudio);

        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, 20f, obstacleLayers))
        {
            Ghost g = hit.collider.GetComponent<Ghost>();
            if (g != null)
            {
                g.TakeDamage(1.0f);
            }

            PhysicsObject po = hit.collider.GetComponent<PhysicsObject>();
            if (po != null)
            {
                po.applyImpact(direction, 4f);
            }
        }
    }

}
