using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Mushroom : MonoBehaviour
{
    public float ttl = 4f;
    public float yUp = 0f;
    public float yDown = -0.25f;
    private float yTarget;
    private bool grabbed = false;
    private bool firstGrabbed = false;
    private bool gettingSucked = false;
    private int type;

    public GameObject player;
    public GameObject flashlight;

    public AudioSource grabSound;

    public MeshRenderer greenRenderer;
    public MeshRenderer pinkRenderer;

    void Start()
    {
        transform.position = new Vector3(transform.position.x, yDown, transform.position.z);
        yTarget = yUp;
        StartCoroutine(Despawn());
    }

    void Update()
    {

        if (!gettingSucked)
        {
            if (!grabbed)
            {
                Vector3 pos = transform.position;
                pos.y = Mathf.Lerp(pos.y, yTarget, Time.deltaTime * 2f);
                transform.position = pos;
            }
            else
            {
                GameObject mouth = player.transform.Find("Mouth")?.gameObject;
                if (type == 0 && mouth !=null && Vector3.Distance(transform.position, mouth.transform.position) < 5f)
                {
                    //heal player
                    Player playerScript = player.GetComponent<Player>();
                    playerScript.AddHealth(10);

                    AudioSource munchSound = mouth.GetComponent<AudioSource>();
                    if (munchSound != null) munchSound.Play();
                    StartCoroutine(GetSuckedInto(mouth.transform.position));
                }

                GameObject flashlightCharger = flashlight.transform.Find("Charger")?.gameObject;
                if (type == 1 && flashlightCharger != null && Vector3.Distance(transform.position, flashlightCharger.transform.position) < 5f)
                {
                    //add energy
                    FlashlightGun flashlightScript = flashlight.GetComponent<FlashlightGun>();
                    flashlightScript.AddEnergy(25);

                    AudioSource chargeSound = flashlightCharger.GetComponent<AudioSource>();
                    if (chargeSound != null) chargeSound.Play();
                    StartCoroutine(GetSuckedInto(flashlightCharger.transform.position));
                }
            }
        }

    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(ttl);
        if (!grabbed)
        {
            yTarget = yDown;
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }
    }

    public void OnGrab()
    {
        grabbed = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        if (grabSound != null && !firstGrabbed)
        {
            grabSound.Play();
        }
        firstGrabbed = true;
    }

    public void SetType(int newType)
    {
        this.type = newType;

        if (type == 1)
        {
            pinkRenderer.enabled = true;
            greenRenderer.enabled = false;
        }
        else if (type == 0)
        {
            pinkRenderer.enabled = false;
            greenRenderer.enabled = true;
        }
    }

    private IEnumerator GetSuckedInto(Vector3 targetPos)
    {
        gettingSucked = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        float duration = 0.25f;
        float elapsed = 0f;
        Vector3 startPos = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            float scaleValue = Mathf.Lerp(1.0f, 0.0f, elapsed / duration);
            transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        Destroy(gameObject);
    }
}
