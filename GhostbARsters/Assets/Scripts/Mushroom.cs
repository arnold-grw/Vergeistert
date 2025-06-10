using System.Collections;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public float ttl = 4f; // Lebensdauer in Sekunden
    public float yUp = 0f;
    public float yDown = -0.25f;
    private float yTarget;

    void Start()
    {
        transform.position = new Vector3(transform.position.x, yDown, transform.position.z);

        yTarget = yUp;
        StartCoroutine(Despawn());
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = Mathf.Lerp(pos.y, yTarget, Time.deltaTime * 2f);
        transform.position = pos;
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(ttl);

        yTarget = yDown;

        yield return new WaitForSeconds(2f);
        Destroy(gameObject); // Entfernt den Pilz
    }
}
