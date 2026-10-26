using UnityEngine;

// 일정 간격으로 공중의 랜덤 위치에 Sphere를 생성한다. Rigidbody의 중력으로 낙하한다.
public class SphereSpawner : MonoBehaviour
{
    [SerializeField] private float spawnInterval = 3f;   // 생성 간격(초)
    [SerializeField] private float spawnHeight = 10f;    // 생성 높이
    [SerializeField] private float spawnRange = 4.5f;    // X/Z 랜덤 범위(바닥 10x10 안쪽)
    [SerializeField] private Material sphereMaterial;
    [SerializeField] private GameObject ground;          // 지면 판정 기준
    [SerializeField] private float destroyDelayAfterLanding = 3f; // 지면에 닿은 후 사라지기까지 시간(초)

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer -= spawnInterval;
            SpawnSphere();
        }
    }

    private void SpawnSphere()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.name = "FallingSphere";
        sphere.tag = "Obstacle";
        sphere.transform.position = new Vector3(
            Random.Range(-spawnRange, spawnRange),
            spawnHeight,
            Random.Range(-spawnRange, spawnRange));

        if (sphereMaterial != null)
        {
            sphere.GetComponent<Renderer>().sharedMaterial = sphereMaterial;
        }

        Rigidbody rb = sphere.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; // 빠르게 떨어져도 통과하지 않도록

        // 지면에 닿은 후 일정 시간 뒤 사라지도록 설정
        SphereLifetime lifetime = sphere.AddComponent<SphereLifetime>();
        lifetime.Init(ground, destroyDelayAfterLanding);
    }
}
