using System.Collections;
using UnityEngine;

public class MushroomSpawner : MonoBehaviour
{
    public GameObject mushroomPrefab;
    public GameObject playerMouth;

    public float spawnRadius = 5f;
    public float startDelay = 10f;
    public float spawnRate = 5f;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            SpawnMushroom();

            // Zufälliges Intervall zwischen 0.5x und 1.5x spawnRate
            float randomInterval = spawnRate * Random.Range(0.5f, 1.5f);
            yield return new WaitForSeconds(randomInterval);
        }
    }

    void SpawnMushroom()
    {
        // Zufällige Richtung auf dem Boden
        Vector2 randomCircle = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(0f, spawnRadius);

        Vector3 spawnOffset = new Vector3(randomCircle.x, 0f, randomCircle.y) * randomDistance;
        Vector3 spawnPosition = transform.position + spawnOffset;

        GameObject mushroomInstance = Instantiate(mushroomPrefab, spawnPosition, Quaternion.identity);

        //Zugriff auf Mushroom-Script
        Mushroom mushroomScript = mushroomInstance.GetComponent<Mushroom>();
        mushroomScript.mouth = this.playerMouth;
        int randomValue = Random.Range(0, 2);
        mushroomScript.SetType(randomValue);
    }
}
