using UnityEngine;
using UnityEngine.SceneManagement; // [추가] 씬 관리를 위해 필요합니다.

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private GameObject gameClearPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // [추가] 매 프레임 키 입력을 감지하기 위한 Update 함수
    private void Update()
    {
        // 만약 'T' 키를 누르면
        if (Input.GetKeyDown(KeyCode.T))
        {
            RestartGame();
        }
    }

    public void GameClear()
    {
        if (gameClearPanel != null)
        {
            gameClearPanel.SetActive(true);
        }

        // 게임 클리어 시 시간을 멈춥니다.
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.enabled = false;
        }
    }

    // [추가] 게임을 재시작하는 함수
    public void RestartGame()
    {
        // 멈췄던 게임 시간을 다시 흐르게 합니다. (매우 중요!)
        Time.timeScale = 1f;

        // 현재 씬의 이름을 가져와서 다시 로드합니다.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}