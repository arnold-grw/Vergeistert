using UnityEngine;

public class LightRaycast : MonoBehaviour
{
    [Header("Einstellungen")]
    public float minDistance = 0f;
    public float maxDistance = 5f;
    public float smoothSpeed = 5f; // Glättungsgeschwindigkeit
    public LayerMask obstacleLayers;

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
            targetDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }

        // Zielposition in lokalen Koordinaten berechnen
        targetLocalPosition = initialLocalPosition + Vector3.forward * targetDistance;

        // Weiche Bewegung zur Zielposition
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetLocalPosition, Time.deltaTime * smoothSpeed);
    }
}
