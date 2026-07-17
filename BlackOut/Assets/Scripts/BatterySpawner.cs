using UnityEngine;
using System.Collections;

public class BatterySpawner : MonoBehaviour
{
    public GameObject batteryPrefab;
    public int maxBatteries = 5;
    public float spawnInterval = 10f;
    public Vector2 spawnAreaMin = new Vector2(-10, -5);
    public Vector2 spawnAreaMax = new Vector2(10, 5);

    void Start()
    {
        Debug.Log("BatterySpawner Start called");

        if (batteryPrefab == null)
        {
            Debug.LogError("⚠️ batteryPrefab is NOT assigned!");
            return;
        }

        StartCoroutine(SpawnBatteriesOverTime());
    }

    IEnumerator SpawnBatteriesOverTime()
    {
        while (true)
        {
            Debug.Log("Waiting to spawn battery...");
            yield return new WaitForSeconds(spawnInterval);

            GameObject[] existingBatteries = GameObject.FindGameObjectsWithTag("Battery");
            Debug.Log("🔋 Found " + existingBatteries.Length + " active batteries in scene.");

            // List each one found
            foreach (var battery in existingBatteries)
            {
                Debug.Log("→ Existing battery: " + battery.name);
            }

            if (existingBatteries.Length < maxBatteries)
            {
                Vector2 randomPos = new Vector2(
                    Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                    Random.Range(spawnAreaMin.y, spawnAreaMax.y)
                );

                GameObject newBattery = Instantiate(batteryPrefab, randomPos, Quaternion.identity);
                newBattery.tag = "Battery"; // ✅ Ensure tag in case prefab loses it
                Debug.Log("✅ Spawned new battery at: " + randomPos);
            }
            else
            {
                Debug.Log("❌ Max batteries reached. Not spawning.");
            }
        }
    }
}
