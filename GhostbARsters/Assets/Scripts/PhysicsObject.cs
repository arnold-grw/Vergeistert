using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public void applyImpact(Vector3 direction, float force)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 impact = (direction.normalized + new Vector3(0f, 0.25f, 0f)) * force;
            rb.AddForce(impact, ForceMode.Impulse);
        }
    }
}
