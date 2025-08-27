using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryUI : MonoBehaviour
{
    public List<InventorySlot> slots = new List<InventorySlot>();

    void OnEnable()
    {
        if (PlayerManager.Instance != null)
        {
            UpdateInventoryUI(PlayerManager.Instance.inventory);
        }
    }

    // [수정] 슬롯 활성화/비활성화 로직 추가
    public void UpdateInventoryUI(Dictionary<BlockData, int> inventory)
    {
        // 1. 플레이어의 현재 가방 크기에 맞춰 슬롯을 켜고 끕니다.
        for (int i = 0; i < slots.Count; i++)
        {
            // i가 현재 가방 크기보다 작으면 (허용된 슬롯이면)
            if (i < PlayerManager.Instance.inventorySize)
            {
                slots[i].gameObject.SetActive(true);
            }
            else // 허용되지 않은 슬롯이면
            {
                slots[i].gameObject.SetActive(false);
            }
        }

        // 2. 활성화된 슬롯에 아이템을 그립니다.
        List<BlockData> itemList = inventory.Keys.ToList();
        for (int i = 0; i < slots.Count; i++)
        {
            // 슬롯이 꺼져있으면 그릴 필요가 없으므로 건너뜁니다.
            if (!slots[i].gameObject.activeSelf) continue;

            if (i < itemList.Count)
            {
                BlockData currentItemData = itemList[i];
                int currentItemCount = inventory[currentItemData];
                slots[i].DrawSlot(currentItemData, currentItemCount);
            }
            else
            {
                // 이 슬롯은 열려있지만 비어있는 상태입니다.
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
