using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Mushroom : MonoBehaviour
{
    public float ttl = 4f; // Lebensdauer in Sekunden
    public float yUp = 0f;
    public float yDown = -0.25f;
    private float yTarget;
    private bool grabbed = false;
    private bool firstGrabbed = false;
    private int type;

    public GameObject mouth;

    public AudioSource grabSound;

    public MeshRenderer greenRenderer;
    public MeshRenderer pinkRenderer;


    void Awake()
    {

    }

    void Start()
    {
        transform.position = new Vector3(transform.position.x, yDown, transform.position.z);

        yTarget = yUp;
        StartCoroutine(Despawn());
    }

    void Update()
    {

        if (!grabbed)
        {
            Vector3 pos = transform.position;
            pos.y = Mathf.Lerp(pos.y, yTarget, Time.deltaTime * 2f);
            transform.position = pos;
        }
        else
        {
            if (Vector3.Distance(transform.position, mouth.transform.position) < 0.175f)
            {
                AudioSource munchSound = mouth.GetComponent<AudioSource>();
                if (munchSound != null) { munchSound.Play(); }
                Destroy(gameObject);
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
            Destroy(gameObject); // Entfernt den Pilz 
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

        if (type == 0)
        {
            pinkRenderer.enabled = true;
            greenRenderer.enabled = false;
        }
        else if (type == 1)
        {
            pinkRenderer.enabled = false;
            greenRenderer.enabled = true;
        }
        
    }
}
