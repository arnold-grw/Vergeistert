using System.Collections;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    public GameObject ghostPrefab;
    public Transform player;

    public float spawnRadius = 5f;
    public float startDelay = 10f;
    public float startRate = 20f;    // z. B. alle 10 Sekunden am Anfang
    public float endRate = 5f;       // z. B. alle 3 Sekunden am Ende
    public float duration = 180f;    // nach 2 Minuten ist die max Rate erreicht

    private float startTime;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(startDelay);
        startTime = Time.time;

        while (true)
        {
            SpawnGhost();

            float elapsed = Time.time - startTime;
            float t = Mathf.Clamp01(elapsed / duration); // 0 bis 1
            float currentInterval = Mathf.Lerp(startRate, endRate, t); // linear interpoliert

            yield return new WaitForSeconds(currentInterval);
        }
    }

    void SpawnGhost()
    {
        Vector3 randomDirection = Random.onUnitSphere;
        randomDirection.y = 0f;
        Vector3 spawnPosition = transform.position + randomDirection.normalized * spawnRadius;

        GameObject ghostInstance = Instantiate(ghostPrefab, spawnPosition, Quaternion.identity);

        Ghost ghostScript = ghostInstance.GetComponent<Ghost>();
        if (ghostScript != null)
        {
            ghostScript.setTarget(player);
        }
    }
}
