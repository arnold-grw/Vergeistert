using System.Collections;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    public GameObject ghostPrefab;
    public Transform player;

    [Header("Sound")]
    public AudioClip[] spawnSounds;

    [Header("Spawn Settings")]
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

        PlaySound(spawnPosition);
    }

    void PlaySound(Vector3 pos)
    {
        if (spawnSounds == null || spawnSounds.Length == 0) return;

        AudioClip chosenClip = spawnSounds[Random.Range(0, spawnSounds.Length)];
        if (chosenClip == null) return;

        GameObject soundObject = new GameObject("TempSound");
        soundObject.transform.position = pos;

        AudioSource source = soundObject.AddComponent<AudioSource>();
        source.clip = chosenClip;
        source.spatialBlend = 1f; // 3D sound
        source.Play();

        Destroy(soundObject, chosenClip.length);
    }
}
