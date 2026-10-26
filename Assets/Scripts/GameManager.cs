using UnityEngine;
using UnityEngine.UI;

// 게임 상태(Game Over 여부)와 생존시간 UI를 관리한다.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Text timeText;

    private float survivalTime;
    public bool IsGameOver { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (IsGameOver) return; // Game Over 이후 시간 정지

        survivalTime += Time.deltaTime;
        if (timeText != null)
        {
            timeText.text = "Time: " + survivalTime.ToString("F1") + " s";
        }
    }

    public void GameOver()
    {
        if (IsGameOver) return; // 여러 번 부딪혀도 한 번만 처리
        IsGameOver = true;
        Debug.Log("Game Over");
    }
}
