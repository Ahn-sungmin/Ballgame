using UnityEngine;

// Sphere가 지면(Ground)에 처음 닿은 뒤 일정 시간이 지나면 스스로 사라지게 한다.
public class SphereLifetime : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 3f; // 지면에 닿은 후 사라지기까지 시간(초)

    private GameObject ground;
    private bool landed;

    // 스포너가 생성 직후 호출해 지면과 사라지는 시간을 알려준다.
    public void Init(GameObject groundObject, float delay)
    {
        ground = groundObject;
        destroyDelay = delay;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (landed) return; // 여러 번 튀어도 타이머는 처음 닿았을 때 한 번만 시작
        if (ground == null || collision.gameObject != ground) return;

        landed = true;
        Destroy(gameObject, destroyDelay);
    }
}
