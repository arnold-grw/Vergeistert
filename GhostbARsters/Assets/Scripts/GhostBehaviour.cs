using UnityEngine;

public class GhostBehavior : MonoBehaviour
{
    //der Spieler
    public Transform target; 
    private float speed = 0.1f;
    private float currentSpeed;


    void Start()
    {
    }

    void Update()
{
    Vector3 diff = target.position - transform.position;
    diff.y = 0; // Nur horizontale Bewegung

    if (diff != Vector3.zero)
    {
        Quaternion targetRotation = Quaternion.LookRotation(diff);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
    }

    float distance = diff.magnitude;

    if (distance > 1.0f)
    {
        currentSpeed = speed;
        float brakeDistance = 2.5f;
        if (distance < brakeDistance){
            currentSpeed = currentSpeed * (distance / brakeDistance);
        }

        transform.position += transform.forward * currentSpeed * Time.deltaTime * 5f;
    }
}

}
