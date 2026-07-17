using UnityEngine;
using System.Collections.Generic;

public class BatteryManager : MonoBehaviour
{
    public GameObject batteryPrefab;
    public int maxBatteries = 5;
    public float spawnInterval = 5f;
    public Vector2 spawnAreaMin = new Vector2(-9.12f, -5f);
    public Vector2 spawnAreaMax = new Vector2(9.12f, 5f);

    private List<GameObject> spawnedBatteries = new List<GameObject>();

    void Start()
    {
        Debug.Log("✅ BatteryManager started...");
        Debug.Log("Spawn interval: " + spawnInterval);

        if (batteryPrefab == null)
        {
            Debug.LogWarning("❌ batteryPrefab NOT assigned in the Inspector!");
            return;
        }

        InvokeRepeating(nameof(SpawnBattery), spawnInterval, spawnInterval);
    }

    void SpawnBattery()
    {
        // Clean up destroyed batteries
        spawnedBatteries.RemoveAll(b => b == null);

        if (batteryPrefab == null)
        {
            Debug.LogWarning("Battery prefab is null! Cannot spawn.");
            return;
        }

        Debug.Log("🔍 Currently spawned batteries: " + spawnedBatteries.Count);

        if (spawnedBatteries.Count < maxBatteries)
        {
            float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);

            Vector3 randomPos = new Vector3(randomX, randomY, 0f);

            Debug.Log($"Random spawn position: X={randomX}, Y={randomY}");

            GameObject newBattery = Instantiate(batteryPrefab, randomPos, Quaternion.identity);
            spawnedBatteries.Add(newBattery);

            Debug.Log("⚡ Spawned battery at: " + randomPos);
        }
        else
        {
            Debug.Log("🚫 Max batteries reached, not spawning more.");
        }
    }
}
