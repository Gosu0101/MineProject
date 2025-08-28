using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 이 스크립트를 Upgrade_Panel에 붙여주세요.
public class UpgradeUI : MonoBehaviour
{
    [Header("탭 버튼 및 콘텐츠 패널")]
    public Button pickaxeTabButton;
    public Button bagTabButton;
    public Button speedTabButton;
    public Text pickaxeButtonText;
    public Text bagButtonText;
    public Text speedButtonText;
    public GameObject pickaxeContent;
    public GameObject bagContent;
    public GameObject speedContent;

    [Header("곡괭이 UI 요소")]
    public Text currentPickaxeText;
    public Text nextPickaxeText;
    public Text pickaxeCostText;
    public Button pickaxeUpgradeButton;
    public Image currentPickaxeImage;
    public List<GameObject> upgradePickaxe;

    [Header("가방 UI 요소")]
    public Text currentBagText;
    public Text nextBagText;
    public Text bagCostText;
    public Button bagUpgradeButton;
    public Image currentBagImage;
    public List<GameObject> upgradeBag;

    [Header("속도 UI 요소")]
    public Text currentSpeedText;
    public Text nextSpeedText;
    public Text speedCostText;
    public Button speedUpgradeButton;
    public Image currentSpeedImage;
    public List<GameObject> upgradeSpeed;

    [Header("강화 데이터")]
    public UpgradeData bagUpgradeData; // 에디터에서 BagUpgradeData 에셋 연결
    public UpgradeData speedUpgradeData; // 에디터에서 SpeedUpgradeData 에셋 연결

    private Vector3 addVector = new Vector3(400f, 0f, 0f);
    private int pickaxeIndex = -1;
    private int bagIndex = -1;
    private int speedIndex = -1;

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

        if (pickaxeIndex == 0)
        {
            nextPickaxeText.GetComponent<Outline>().effectColor = Color.blue;
            currentPickaxeText.GetComponent<Outline>().effectColor = Color.forestGreen;
        }
        else if (pickaxeIndex == 1)
        {
            nextPickaxeText.GetComponent<Outline>().effectColor = Color.red;
            currentPickaxeText.GetComponent<Outline>().effectColor = Color.blue;
        }
        else if (pickaxeIndex == 2)
        {
            currentPickaxeText.GetComponent<Outline>().effectColor = Color.red;
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

        if (bagIndex == 4 || bagIndex == 5)
        {
            if (bagIndex == 4) nextBagText.GetComponent<Outline>().effectColor = Color.forestGreen;
            else currentBagText.GetComponent<Outline>().effectColor = Color.forestGreen;
        }
        else if (bagIndex == 8 || bagIndex == 9)
        {
            if (bagIndex == 8) nextBagText.GetComponent<Outline>().effectColor = Color.blue;
            else currentBagText.GetComponent<Outline>().effectColor = Color.blue;
        }
        else if (bagIndex == 12 || bagIndex == 13)
        {
            if (bagIndex == 12) nextBagText.GetComponent<Outline>().effectColor = Color.red;
            else currentBagText.GetComponent<Outline>().effectColor = Color.red;
        }
    }

    void UpdateSpeedTab()
    {
        var player = PlayerManager.Instance;
        int cost = (int)(speedUpgradeData.baseCost * Mathf.Pow(speedUpgradeData.costIncreaseRate, player.speedLevel));

        currentSpeedText.text = "Current: " + (player.miningSpeedModifier * 100).ToString("F0") + "% (Lv." + (player.speedLevel + 1) + ")";
        nextSpeedText.text = "Next: " + ((player.miningSpeedModifier + speedUpgradeData.valueIncrease) * 100).ToString("F0") + "%";
        speedCostText.text = "Cost: " + cost;
        speedUpgradeButton.interactable = (player.currentGold >= cost);

        /*
        if (bagIndex == 3) currentBagText.GetComponent<Outline>().effectColor = Color.forestGreen;
        else if (bagIndex == 8) currentBagText.GetComponent<Outline>().effectColor = Color.blue;
        else if (bagIndex == 14) currentBagText.GetComponent<Outline>().effectColor = Color.red;
        */

        if (speedIndex == 2 || speedIndex == 3)
        {
            if (speedIndex == 2) nextSpeedText.GetComponent<Outline>().effectColor = Color.forestGreen;
            else currentSpeedText.GetComponent<Outline>().effectColor = Color.forestGreen;
        }
        else if (speedIndex == 7 || speedIndex == 8)
        {
            if (speedIndex == 7) nextSpeedText.GetComponent<Outline>().effectColor = Color.blue;
            else currentSpeedText.GetComponent<Outline>().effectColor = Color.blue;
        }
        else if (speedIndex == 13 || speedIndex == 14)
        {
            if (speedIndex == 13) nextSpeedText.GetComponent<Outline>().effectColor = Color.red;
            else currentSpeedText.GetComponent<Outline>().effectColor = Color.red;
        }
    }

    // --- 탭 보여주기 함수들 ---
    public void ShowPickaxeTab()
    {
        pickaxeContent.SetActive(true);
        bagContent.SetActive(false);
        speedContent.SetActive(false);

        pickaxeButtonText.GetComponent<Outline>().effectColor = Color.red;
        bagButtonText.GetComponent<Outline>().effectColor = Color.black;
        speedButtonText.GetComponent<Outline>().effectColor = Color.black;
    }

    public void ShowBagTab()
    {
        pickaxeContent.SetActive(false);
        bagContent.SetActive(true);
        speedContent.SetActive(false);

        pickaxeButtonText.GetComponent<Outline>().effectColor = Color.black;
        bagButtonText.GetComponent<Outline>().effectColor = Color.red;
        speedButtonText.GetComponent<Outline>().effectColor = Color.black;
    }

    public void ShowSpeedTab()
    {
        pickaxeContent.SetActive(false);
        bagContent.SetActive(false);
        speedContent.SetActive(true);

        pickaxeButtonText.GetComponent<Outline>().effectColor = Color.black;
        bagButtonText.GetComponent<Outline>().effectColor = Color.black;
        speedButtonText.GetComponent<Outline>().effectColor = Color.red;
    }

    // --- 버튼 클릭 함수들 (수정된 부분) ---
    public void OnPickaxeUpgradeClick()
    {
        // [수정] 강화 성공 여부를 확인하고 효과음을 재생합니다.
        long goldBefore = PlayerManager.Instance.currentGold;
        PlayerManager.Instance.UpgradePickaxe();
        if (goldBefore > PlayerManager.Instance.currentGold) // 골드가 소모되었다면 성공
        {
            AudioManager.Instance.PlayUpgradeSuccessSound();
            currentPickaxeImage.transform.position += addVector;
            if (pickaxeIndex == -1) { upgradePickaxe[++pickaxeIndex].SetActive(true); }
            else
            {
                upgradePickaxe[pickaxeIndex++].SetActive(false);
                upgradePickaxe[pickaxeIndex].SetActive(true);
            }
        }
        UpdateAllTabs();
    }

    public void OnBagUpgradeClick()
    {
        long goldBefore = PlayerManager.Instance.currentGold;
        // PlayerManager에게 어떤 데이터로 강화할지 알려줍니다.
        PlayerManager.Instance.UpgradeBag(bagUpgradeData);
        if (goldBefore > PlayerManager.Instance.currentGold) // 골드가 소모되었다면 성공
        {
            AudioManager.Instance.PlayUpgradeSuccessSound();
            switch (++bagIndex)
            {
                case 5:
                    currentBagImage.transform.position += addVector;
                    upgradeBag[0].SetActive(true);
                    break;
                case 9:
                    currentBagImage.transform.position += addVector;
                    upgradeBag[0].SetActive(false);
                    upgradeBag[1].SetActive(true);
                    break;
                case 13:
                    currentBagImage.transform.position += addVector;
                    upgradeBag[1].SetActive(false);
                    upgradeBag[2].SetActive(true);
                    break;
            }
        }
        UpdateAllTabs();
    }

    public void OnSpeedUpgradeClick()
    {
        long goldBefore = PlayerManager.Instance.currentGold;
        // PlayerManager에게 어떤 데이터로 강화할지 알려줍니다.
        PlayerManager.Instance.UpgradeSpeed(speedUpgradeData);
        if (goldBefore > PlayerManager.Instance.currentGold) // 골드가 소모되었다면 성공
        {
            AudioManager.Instance.PlayUpgradeSuccessSound();
            switch (++speedIndex)
            {
                case 3:
                    currentSpeedImage.transform.position += addVector;
                    upgradeSpeed[0].SetActive(true);
                    break;
                case 8:
                    currentSpeedImage.transform.position += addVector;
                    upgradeSpeed[0].SetActive(false);
                    upgradeSpeed[1].SetActive(true);
                    break;
                case 14:
                    currentSpeedImage.transform.position += addVector;
                    upgradeSpeed[1].SetActive(false);
                    upgradeSpeed[2].SetActive(true);
                    break;
            }
        }
        UpdateAllTabs();
    }
}
