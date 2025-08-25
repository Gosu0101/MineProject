using UnityEngine;
using UnityEngine.UI; // UI 관련 클래스 사용을 위해 추가
using TMPro; // TextMeshPro 사용을 위해 추가

public class InventorySlot : MonoBehaviour
{
    public Image itemIcon; // 아이템의 이미지를 표시할 UI Image 컴포넌트
    public TextMeshProUGUI itemCountText; // 아이템 개수를 표시할 TextMeshPro UI 컴포넌트

    // 슬롯을 비우는 함수
    public void ClearSlot()
    {
        itemIcon.enabled = false; // 아이콘 비활성화
        itemCountText.enabled = false; // 텍스트 비활성화
    }

    // 슬롯에 아이템 정보를 채우는 함수
    public void DrawSlot(BlockData data, int count)
    {
        // BlockData에 아이템 아이콘 정보가 필요합니다. (아래 설명 참고)
        // itemIcon.sprite = data.itemIcon; 
        itemIcon.enabled = true; // 아이콘 활성화
        itemCountText.text = count.ToString(); // 개수 표시
        itemCountText.enabled = true; // 텍스트 활성화
    }
}
