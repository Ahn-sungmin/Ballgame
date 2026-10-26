using UnityEngine;
using UnityEngine.InputSystem;

// WASD로 플레이어를 움직이고, Obstacle 태그 오브젝트에 닿으면 Game Over 처리한다.
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody rb;
    private Vector3 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        moveInput = Vector3.zero;
        if (IsGameOver()) return;

        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        if (kb.wKey.isPressed) moveInput.z += 1f;
        if (kb.sKey.isPressed) moveInput.z -= 1f;
        if (kb.dKey.isPressed) moveInput.x += 1f;
        if (kb.aKey.isPressed) moveInput.x -= 1f;
        moveInput = moveInput.normalized; // 대각선 이동이 더 빨라지지 않도록
    }

    void FixedUpdate()
    {
        if (rb.isKinematic) return;
        Vector3 v = moveInput * moveSpeed;
        v.y = rb.linearVelocity.y; // 중력에 의한 낙하 속도는 유지
        rb.linearVelocity = v;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (IsGameOver()) return;
        if (!collision.gameObject.CompareTag("Obstacle")) return;

        if (GameManager.Instance != null) GameManager.Instance.GameOver();

        // 이동 정지: 속도를 0으로 만들고 물리 영향(공에 밀리는 것)도 받지 않게 한다.
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
    }

    private bool IsGameOver()
    {
        return GameManager.Instance != null && GameManager.Instance.IsGameOver;
    }
}
