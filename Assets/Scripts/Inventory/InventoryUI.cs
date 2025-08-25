using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public List<InventorySlot> slots = new List<InventorySlot>(); // 모든 인벤토리 슬롯 리스트

    void Start()
    {
        // 게임 시작 시 모든 슬롯을 일단 비워줍니다.
        ClearAllSlots();
    }

    // 인벤토리 UI를 PlayerManager의 데이터에 맞춰 새로고침하는 핵심 함수
    public void UpdateInventoryUI(Dictionary<BlockData, int> inventory)
    {
        ClearAllSlots(); // 모든 슬롯을 비우고 다시 그립니다.

        int slotIndex = 0;
        // 인벤토리에 있는 모든 아이템에 대해 반복
        foreach (var item in inventory)
        {
            // 슬롯 인덱스가 유효한 범위 내에 있다면
            if (slotIndex < slots.Count)
            {
                // 해당 슬롯에 아이템 정보(데이터, 개수)를 전달하여 그리도록 함
                slots[slotIndex].DrawSlot(item.Key, item.Value);
                slotIndex++;
            }
        }
    }

    // 모든 슬롯을 깨끗하게 비우는 함수
    private void ClearAllSlots()
    {
        foreach (var slot in slots)
        {
            slot.ClearSlot();
        }
    }
}
