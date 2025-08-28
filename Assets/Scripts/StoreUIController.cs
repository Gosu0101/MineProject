using UnityEngine;

public class StoreUIController : MonoBehaviour
{
    [Header("UI 오브젝트 연결")]
    [SerializeField] private GameObject interaction; // 상호작용 UI ON/OFF
    [SerializeField] private GameObject store;       // 각 Store별 UI ON/OFF
    [SerializeField] private GameObject main;        // Main UI ON/OFF
    [SerializeField] private GameObject cancel;

    private bool isPlayerInRange = false; // 플레이어가 상호작용 범위 내에 있는지 여부

    void Update()
    {
        // 플레이어가 범위 내에 있고, 마우스 우클릭을 했다면
        if (isPlayerInRange && Input.GetMouseButtonDown(1))
        {
            ToggleShopPanel();
        }
    }

    // Store 창을 켜고 끄는 함수
    private void ToggleShopPanel()
    {
        // Store 창을 현재 상태의 반대로 설정 (켜져있으면 끄고, 꺼져있으면 켠다)
        store.SetActive(!store.activeSelf);
        main.SetActive(!main.activeSelf);
        cancel.SetActive(!cancel.activeSelf);

        // Store 창이 켜졌다면, 게임을 잠시 멈추고 마우스 커서를 보이게 합니다.
        if (store.activeSelf)
        {
            Time.timeScale = 0f; // 시간 정지
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else // Store 창이 꺼졌다면, 시간을 다시 흐르게 하고 커서를 숨깁니다.
        {
            Time.timeScale = 1f; // 시간 다시 흐름
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // 플레이어가 감지 영역에 들어왔을 때 호출
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            interaction.SetActive(true);
        }
    }

    // 플레이어가 감지 영역에서 나갔을 때 호출
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interaction.SetActive(false);

            // 만약 상점 창을 열어둔 상태로 멀어진다면 강제로 닫습니다.
            if (store.activeSelf)
            {
                ToggleShopPanel();
            }
        }
    }
}
