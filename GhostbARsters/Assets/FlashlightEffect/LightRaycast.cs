using UnityEngine;

public class LightRaycast : MonoBehaviour
{
    [Header("Einstellungen")]
    public float minDistance = 0f;
    public float maxDistance = 5f;
    public float smoothSpeed = 5f; // Glättungsgeschwindigkeit
    public LayerMask obstacleLayers;
    public SphereController sphere;

    private Vector3 initialLocalPosition;
    private Vector3 targetLocalPosition;

    void Start()
    {
        // Lokale Startposition merken
        initialLocalPosition = transform.localPosition;
        targetLocalPosition = transform.localPosition;
    }

    void Update()
    {
        // Ursprungspunkt des Raycasts (in Weltkoordinaten)
        Vector3 origin = transform.parent.TransformPoint(initialLocalPosition);
        Vector3 direction = transform.parent.forward;

        float targetDistance = maxDistance;

        // Raycast ausführen
        if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance, obstacleLayers))
        {
            // Zielentfernung innerhalb min/max clampen
            targetDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance) * 100f;
        }
        sphere.setSize(Mathf.Clamp(targetDistance/500f + 0.1f, 0.1f, 0.45f));

        // Zielposition in lokalen Koordinaten berechnen
        targetLocalPosition = initialLocalPosition + Vector3.forward * targetDistance;
        //Debug.Log(targetLocalPosition);

        // Weiche Bewegung zur Zielposition
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetLocalPosition, Time.deltaTime * smoothSpeed);
    }
}
