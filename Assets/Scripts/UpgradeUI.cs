using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 이 스크립트를 Upgrade_Panel에 붙여주세요.
public class UpgradeUI : MonoBehaviour
{
    [Header("탭 버튼 및 콘텐츠 패널")]
    public Button pickaxeTabButton;
    public Button bagTabButton;
    public Button speedTabButton;
    public GameObject pickaxeContent;
    public GameObject bagContent;
    public GameObject speedContent;

    [Header("곡괭이 UI 요소")]
    public TextMeshProUGUI currentPickaxeText;
    public TextMeshProUGUI nextPickaxeText;
    public TextMeshProUGUI pickaxeCostText;
    public Button pickaxeUpgradeButton;

    [Header("가방 UI 요소")]
    public TextMeshProUGUI currentBagText;
    public TextMeshProUGUI nextBagText;
    public TextMeshProUGUI bagCostText;
    public Button bagUpgradeButton;

    [Header("속도 UI 요소")]
    public TextMeshProUGUI currentSpeedText;
    public TextMeshProUGUI nextSpeedText;
    public TextMeshProUGUI speedCostText;
    public Button speedUpgradeButton;

    [Header("강화 데이터")]
    public UpgradeData bagUpgradeData; // 에디터에서 BagUpgradeData 에셋 연결
    public UpgradeData speedUpgradeData; // 에디터에서 SpeedUpgradeData 에셋 연결

    // 강화소 창이 켜질 때 외부에서 호출할 함수
    public void OpenUpgradePanel()
    {
        gameObject.SetActive(true);
        UpdateAllTabs(); // 모든 탭의 정보를 최신화
        ShowPickaxeTab(); // 처음에는 곡괭이 탭을 보여줌
    }

    // 모든 탭의 정보를 갱신
    public void UpdateAllTabs()
    {
        UpdatePickaxeTab();
        UpdateBagTab();
        UpdateSpeedTab();
    }

    // --- 탭 UI 갱신 함수들 ---
    void UpdatePickaxeTab()
    {
        var player = PlayerManager.Instance;
        PickaxeData nextTier = player.GetNextPickaxeTier();

        currentPickaxeText.text = "Current: " + player.currentPickaxe.pickaxeName;

        if (nextTier != null)
        {
            nextPickaxeText.text = "Next: " + nextTier.pickaxeName;
            pickaxeCostText.text = "Cost: " + nextTier.cost;
            pickaxeUpgradeButton.interactable = (player.currentGold >= nextTier.cost);
        }
        else
        {
            nextPickaxeText.text = "Max Level";
            pickaxeCostText.text = "";
            pickaxeUpgradeButton.interactable = false;
        }
    }

    void UpdateBagTab()
    {
        var player = PlayerManager.Instance;
        int cost = (int)(bagUpgradeData.baseCost * Mathf.Pow(bagUpgradeData.costIncreaseRate, player.bagLevel));

        currentBagText.text = "Current: " + player.inventorySize + " compartments (Lv." + (player.bagLevel + 1) + ")";
        nextBagText.text = "Next: " + (player.inventorySize + (int)bagUpgradeData.valueIncrease) + " compartments";
        bagCostText.text = "Cost: " + cost;
        bagUpgradeButton.interactable = (player.currentGold >= cost);
    }

    void UpdateSpeedTab()
    {
        var player = PlayerManager.Instance;
        int cost = (int)(speedUpgradeData.baseCost * Mathf.Pow(speedUpgradeData.costIncreaseRate, player.speedLevel));

        currentSpeedText.text = "Current: " + (player.miningSpeedModifier * 100).ToString("F0") + "% (Lv." + (player.speedLevel + 1) + ")";
        nextSpeedText.text = "Next: " + ((player.miningSpeedModifier + speedUpgradeData.valueIncrease) * 100).ToString("F0") + "%";
        speedCostText.text = "Cost: " + cost;
        speedUpgradeButton.interactable = (player.currentGold >= cost);
    }

    // --- 탭 보여주기 함수들 ---
    public void ShowPickaxeTab()
    {
        pickaxeContent.SetActive(true);
        bagContent.SetActive(false);
        speedContent.SetActive(false);
    }

    public void ShowBagTab()
    {
        pickaxeContent.SetActive(false);
        bagContent.SetActive(true);
        speedContent.SetActive(false);
    }

    public void ShowSpeedTab()
    {
        pickaxeContent.SetActive(false);
        bagContent.SetActive(false);
        speedContent.SetActive(true);
    }

    // --- 버튼 클릭 함수들 (수정된 부분) ---
    public void OnPickaxeUpgradeClick()
    {
        // [수정] 강화 성공 여부를 확인하고 효과음을 재생합니다.
        int goldBefore = PlayerManager.Instance.currentGold;
        PlayerManager.Instance.UpgradePickaxe();
        if (goldBefore > PlayerManager.Instance.currentGold) // 골드가 소모되었다면 성공
        {
            AudioManager.Instance.PlayUpgradeSuccessSound();
        }
        UpdateAllTabs();
    }

    public void OnBagUpgradeClick()
    {
        int goldBefore = PlayerManager.Instance.currentGold;
        // PlayerManager에게 어떤 데이터로 강화할지 알려줍니다.
        PlayerManager.Instance.UpgradeBag(bagUpgradeData);
        if (goldBefore > PlayerManager.Instance.currentGold) // 골드가 소모되었다면 성공
        {
            AudioManager.Instance.PlayUpgradeSuccessSound();
        }
        UpdateAllTabs();
    }

    public void OnSpeedUpgradeClick()
    {
        int goldBefore = PlayerManager.Instance.currentGold;
        // PlayerManager에게 어떤 데이터로 강화할지 알려줍니다.
        PlayerManager.Instance.UpgradeSpeed(speedUpgradeData);
        if (goldBefore > PlayerManager.Instance.currentGold) // 골드가 소모되었다면 성공
        {
            AudioManager.Instance.PlayUpgradeSuccessSound();
        }
        UpdateAllTabs();
    }
}
