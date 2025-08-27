using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade Data", menuName = "Data/Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    [TextArea] public string description;
    public int baseCost;
    public float costIncreaseRate; // 레벨당 비용 증가율 (예: 1.5)
    public float valueIncrease;    // 1회 강화 시 증가하는 값 (예: 가방 +10)
}
