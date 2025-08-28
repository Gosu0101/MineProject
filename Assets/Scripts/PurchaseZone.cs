using UnityEngine;
using UnityEngine.UI;

public class PurchaseZone : MonoBehaviour
{
    [Header("구매 정보")]
    [SerializeField] private GameObject terrainSpawnerPrefab; // 생성할 스포너 프리팹
    [SerializeField] private int purchaseCost = 500; // 구매 비용
    [SerializeField] private string terrainName = "구리의 땅"; // 표시될 땅 이름
    [SerializeField] private Transform spawnPoint; // [추가] 스포너가 생성될 위치
    [SerializeField] private Transform playerSafePosition;

    [Header("오브젝트 연결")]
    [SerializeField] private GameObject purchasePromptUI; // "E키를 눌러 구매" 같은 안내 UI
    [SerializeField] private Text promptText; // 안내 UI의 텍스트

    private bool isPlayerInside = false; // 플레이어가 트리거 안에 있는지 확인

    void Start()
    {
        // 시작할 때 안내 UI는 꺼둡니다.
        if (purchasePromptUI != null)
        {
            purchasePromptUI.SetActive(false);
        }

        if (spawnPoint == null)
        {
            spawnPoint = this.transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 트리거에 들어온 오브젝트가 'Player' 태그를 가지고 있다면
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            if (purchasePromptUI != null)
            {
                // UI 텍스트를 현재 구매 정보에 맞게 업데이트
                promptText.text = $"[B] Key Pressed '{terrainName}' Purchase ({purchaseCost} Gold)";
                purchasePromptUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            if (purchasePromptUI != null)
            {
                purchasePromptUI.SetActive(false);
            }
        }
    }

    void Update()
    {
        // 플레이어가 트리거 안에 있고 B키를 눌렀다면
        if (isPlayerInside && Input.GetKeyDown(KeyCode.B))
        {
            // PlayerManager에서 현재 골드 정보를 가져와 비교
            if (PlayerManager.Instance.currentGold >= purchaseCost)
            {
                // 골드 차감
                PlayerManager.Instance.currentGold -= purchaseCost;

                //  스포너를 생성하기 직전에, 플레이어를 안전한 위치로 이동시킵니다.
                PlayerManager.Instance.transform.position = playerSafePosition.position;

                Instantiate(terrainSpawnerPrefab, spawnPoint.position, spawnPoint.rotation);


                // 구매 안내 UI와 구매 트리거 오브젝트 자신을 파괴해서 중복 구매 방지
                Destroy(purchasePromptUI);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("골드가 부족합니다!");
                // TODO: 골드 부족 시 피드백 (소리, 화면 메시지 등)
            }
        }
    }
}