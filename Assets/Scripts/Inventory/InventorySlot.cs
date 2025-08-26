using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemCountText;

    // 코드가 시작될 때 한 번만 색상 정보를 미리 저장해둡니다.
    private Color opaqueColor = Color.white;
    private Color transparentColor = new Color(1, 1, 1, 0);

    void Awake()
    {
        // 게임이 시작될 때 슬롯을 확실히 비우고 시작합니다.
        ClearSlot();
    }

    // 슬롯을 비우는 함수 (수정)
    public void ClearSlot()
    {
        // 아이콘의 색상을 완전히 투명하게 만들어 보이지 않게 합니다.
        itemIcon.color = transparentColor;
        // 텍스트는 비활성화합니다.
        itemCountText.enabled = false;
    }

    // 슬롯에 아이템 정보를 채우는 함수 (수정)
    public void DrawSlot(BlockData data, int count)
    {
        // 아이콘의 스프라이트를 설정하고,
        itemIcon.sprite = data.itemIcon;
        // 색상을 다시 불투명하게 만들어 보이게 합니다.
        itemIcon.color = opaqueColor;

        // 텍스트를 활성화하고 내용을 설정합니다.
        itemCountText.enabled = true;
        itemCountText.text = count.ToString();
    }
}
