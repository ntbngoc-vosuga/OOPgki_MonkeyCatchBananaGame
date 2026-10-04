using System.Collections;
using UnityEngine;

public class ItemFallingSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject normalBananaPrefab;
    [SerializeField] private GameObject rottenBananaPrefab;
    [SerializeField] private GameObject bombPrefab;

    [Header("Spawn Bounds")]
    [SerializeField] private float minX = -4.5f;
    [SerializeField] private float maxX = 4.5f;
    [SerializeField] private float spawnY = 5.5f;

    [Header("Difficulty Settings")]
    [Tooltip("Thời gian nghỉ ban đầu giữa các lần rơi (giây)")]
    [SerializeField] private float baseSpawnInterval = 1.2f;

    [Tooltip("Thời gian nghỉ tối thiểu ở giai đoạn khó nhất (giây)")]
    [SerializeField] private float minSpawnInterval = 0.45f;

    [Tooltip("Tốc độ rơi cộng thêm ở giai đoạn khó nhất")]
    [SerializeField] private float maxExtraFallSpeed = 3.5f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (CatchGameManager.Instance != null && !CatchGameManager.Instance.isGameOver)
            {
                // Tính tiến trình trận đấu (từ 0.0 lúc bắt đầu đến 1.0 lúc gần hết 3 phút)
                float totalTime = CatchGameManager.Instance.totalGameTime;
                float remaining = CatchGameManager.Instance.timeRemaining;
                float progress = Mathf.Clamp01(1f - (remaining / totalTime));

                // 1. Càng về sau khoảng cách rơi càng dồn dập
                float currentInterval = Mathf.Lerp(baseSpawnInterval, minSpawnInterval, progress);

                // 2. Tạo vật phẩm ngẫu nhiên
                SpawnRandomItem(progress);

                yield return new WaitForSeconds(currentInterval);
            }
            else
            {
                yield return null;
            }
        }
    }

    private void SpawnRandomItem(float progress)
    {
        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPos = new Vector3(randomX, spawnY, 0f);

        // Tỷ lệ xuất hiện: Càng về sau tỷ lệ bom và chuối hư càng tăng nhẹ
        // Lúc đầu: Bom 20%, Chuối hư 20%, Chuối ngon 60%
        // Về cuối: Bom 35%, Chuối hư 25%, Chuối ngon 40%
        float bombChance = Mathf.Lerp(0.20f, 0.35f, progress);
        float rottenChance = Mathf.Lerp(0.20f, 0.25f, progress);

        float roll = Random.value;
        GameObject prefabToSpawn;

        if (roll < bombChance)
        {
            prefabToSpawn = bombPrefab;
        }
        else if (roll < bombChance + rottenChance)
        {
            prefabToSpawn = rottenBananaPrefab;
        }
        else
        {
            prefabToSpawn = normalBananaPrefab;
        }

        if (prefabToSpawn != null)
        {
            GameObject item = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

            // Tăng tốc độ rơi của vật phẩm bằng cách tăng trọng lực Rigidbody2D
            Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Trọng lực gốc tăng dần theo progress
                rb.gravityScale += progress * (maxExtraFallSpeed * 0.3f);
            }
        }
    }
}