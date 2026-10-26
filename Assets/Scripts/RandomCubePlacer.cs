using System.Collections.Generic;
using UnityEngine;

// 게임이 시작될 때마다 지정한 Cube들을 Ground 위 랜덤 위치로 재배치한다.
// Cube 사이, Cube와 플레이어 시작점 사이에는 항상 플레이어가 지나갈 수 있는 틈을 보장한다.
public class RandomCubePlacer : MonoBehaviour
{
    [SerializeField] private Transform[] cubes;            // 재배치할 Cube들
    [SerializeField] private Renderer ground;              // 배치 범위 기준(Ground)
    [SerializeField] private CapsuleCollider player;       // 플레이어(크기와 시작 위치를 기준으로 사용)
    [SerializeField] private float edgeMargin = 0.7f;      // 바닥 가장자리와의 여유
    [SerializeField] private float extraGap = 0.4f;        // 플레이어 지름에 더할 추가 여유 틈
    [SerializeField] private int maxAttemptsPerCube = 100; // Cube 하나당 위치 시도 횟수
    [SerializeField] private int maxFullRetries = 50;      // 전체 배치 재시도 횟수

    void Awake()
    {
        PlaceCubes();
    }

    // Cube가 어느 방향으로 회전해 있어도 차지할 수 있는 최대 반경(XZ 평면 대각선의 절반)
    private float CubeRadius(Transform cube)
    {
        Vector3 s = cube.lossyScale;
        return Mathf.Sqrt(s.x * s.x + s.z * s.z) * 0.5f;
    }

    private float PlayerDiameter()
    {
        if (player == null) return 1f;
        Vector3 s = player.transform.lossyScale;
        return player.radius * 2f * Mathf.Max(s.x, s.z);
    }

    private void PlaceCubes()
    {
        if (ground == null || cubes == null) return;

        Bounds b = ground.bounds;
        float requiredGap = PlayerDiameter() + extraGap; // Cube 표면 사이에 확보할 최소 틈

        for (int retry = 0; retry < maxFullRetries; retry++)
        {
            List<Vector3> positions = new List<Vector3>();
            List<float> radii = new List<float>();
            bool success = true;

            foreach (Transform cube in cubes)
            {
                if (cube == null) continue;
                float r = CubeRadius(cube);
                float minX = b.min.x + edgeMargin, maxX = b.max.x - edgeMargin;
                float minZ = b.min.z + edgeMargin, maxZ = b.max.z - edgeMargin;

                bool found = false;
                for (int i = 0; i < maxAttemptsPerCube; i++)
                {
                    Vector3 candidate = new Vector3(Random.Range(minX, maxX), cube.position.y, Random.Range(minZ, maxZ));
                    if (IsValid(candidate, r, requiredGap, positions, radii))
                    {
                        positions.Add(candidate);
                        radii.Add(r);
                        found = true;
                        break;
                    }
                }
                if (!found) { success = false; break; }
            }

            if (success)
            {
                int idx = 0;
                foreach (Transform cube in cubes)
                {
                    if (cube == null) continue;
                    cube.position = positions[idx++];
                }
                return;
            }
        }

        Debug.LogWarning("RandomCubePlacer: 조건을 만족하는 배치를 찾지 못해 기존 위치를 유지합니다. extraGap이나 Cube 수를 줄여 보세요.");
    }

    private bool IsValid(Vector3 candidate, float r, float requiredGap, List<Vector3> positions, List<float> radii)
    {
        Vector2 c = new Vector2(candidate.x, candidate.z);

        // 플레이어 시작점: Cube 표면과 플레이어 사이에도 틈 확보
        if (player != null)
        {
            Vector2 p = new Vector2(player.transform.position.x, player.transform.position.z);
            if (Vector2.Distance(p, c) < r + PlayerDiameter() * 0.5f + requiredGap) return false;
        }

        // 다른 Cube: 표면 사이 거리(중심 거리 - 두 반경)가 requiredGap 이상이어야 함
        for (int i = 0; i < positions.Count; i++)
        {
            Vector2 o = new Vector2(positions[i].x, positions[i].z);
            if (Vector2.Distance(o, c) - r - radii[i] < requiredGap) return false;
        }
        return true;
    }
}
