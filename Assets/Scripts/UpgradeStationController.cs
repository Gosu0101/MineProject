using UnityEngine;

public class UpgradeStationController : MonoBehaviour
{
    [Header("UI 오브젝트 연결")]
    [SerializeField] private GameObject interactionPromptUI; // "((우클릭) 강화)" UI
    [SerializeField] private UpgradeUI upgradeUI; // Upgrade_Panel에 있는 UpgradeUI 스크립트
    [SerializeField] private GameObject MainUI;
    [SerializeField] private GameObject interactionCancel;

    private bool isPlayerInRange = false;

    void Start()
    {
        interactionPromptUI.SetActive(false);
        upgradeUI.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetMouseButtonDown(1))
        {
            ToggleUpgradePanel();
        }
    }

    private void ToggleUpgradePanel()
    {
        bool isPanelActive = !upgradeUI.gameObject.activeSelf;

        if (isPanelActive)
        {
            // 패널을 켜기 전에 UI를 최신 정보로 업데이트합니다.
            upgradeUI.OpenUpgradePanel();
        }
        else
        {
            upgradeUI.gameObject.SetActive(false);
        }

        // 시간 및 커서 제어
        Time.timeScale = isPanelActive ? 0f : 1f;
        Cursor.lockState = isPanelActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPanelActive;

        MainUI.SetActive(!isPanelActive);
        interactionCancel.SetActive(isPanelActive);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            interactionPromptUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactionPromptUI.SetActive(false);
            if (upgradeUI.gameObject.activeSelf)
            {
                ToggleUpgradePanel();
            }
        }
    }
}
