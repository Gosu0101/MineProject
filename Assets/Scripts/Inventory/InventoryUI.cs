using System.Collections.Generic;
using UnityEngine;
using System.Linq; // Linq 사용을 위해 추가

public class InventoryUI : MonoBehaviour
{
    public List<InventorySlot> slots = new List<InventorySlot>();

    void Start()
    {
        ClearAllSlots();
    }

    // 인벤토리 UI를 PlayerManager의 데이터에 맞춰 새로고침하는 핵심 함수 (수정)
    public void UpdateInventoryUI(Dictionary<BlockData, int> inventory)
    {
        // Dictionary의 Key(아이템 종류)들을 리스트로 변환하여 순서를 고정시킵니다.
        List<BlockData> itemList = inventory.Keys.ToList();

        // 모든 UI 슬롯을 순회합니다.
        for (int i = 0; i < slots.Count; i++)
        {
            // 현재 슬롯 인덱스(i)에 해당하는 아이템이 itemList에 존재한다면
            if (i < itemList.Count)
            {
                // 해당 아이템의 정보와 수량을 가져옵니다.
                BlockData currentItemData = itemList[i];
                int currentItemCount = inventory[currentItemData];
                // 슬롯에 아이템을 그립니다.
                slots[i].DrawSlot(currentItemData, currentItemCount);
            }
            else // itemList에 더 이상 아이템이 없다면
            {
                // 나머지 슬롯들은 비워줍니다.
                slots[i].ClearSlot();
            }
        }
    }

    private void ClearAllSlots()
    {
        foreach (var slot in slots)
        {
            slot.ClearSlot();
        }
    }
}
