using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 게임 상태(Game Over 여부)와 생존시간 UI, Game Over 화면(재시작/종료)을 관리한다.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Text timeText;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel; // Game Over 시 표시할 패널
    [SerializeField] private Text resultText;          // 이번 판 생존시간 기록 표시

    private float survivalTime;
    public bool IsGameOver { get; private set; }

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f; // 재시작 시 일시정지가 풀린 상태로 시작
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
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

        Time.timeScale = 0f; // 게임 일시정지(물리, 공 생성, 회전 모두 멈춤)

        if (resultText != null)
        {
            resultText.text = "Survival Time: " + survivalTime.ToString("F1") + " s";
        }
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    // Retry 버튼: 현재 씬을 다시 불러와 처음부터 시작
    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Quit 버튼: 빌드에서는 게임 종료, 에디터에서는 Play 모드 종료
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
