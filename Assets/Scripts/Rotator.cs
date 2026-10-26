using UnityEngine;

// 붙어 있는 오브젝트를 Y축 기준으로 계속 회전시킨다.
public class Rotator : MonoBehaviour
{
    // 초당 회전 각도(도). private이지만 [SerializeField]로 Inspector에서 수정 가능.
    [SerializeField] private float rotationSpeed = 90f;

    void Update()
    {
        // Time.deltaTime(지난 프레임 이후 경과 시간)을 곱해 프레임 속도와 관계없이 "초당 90도"가 되게 한다.
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World);
    }
}
