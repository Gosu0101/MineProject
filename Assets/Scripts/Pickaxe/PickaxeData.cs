using UnityEngine;
[CreateAssetMenu(fileName = "New Pickaxe Data", menuName = "Data/Pickaxe")]
public class PickaxeData : ScriptableObject
{
    public int pickaxeID;
    public string pickaxeName;
    public int power; // 곡괭이의 파워
    public int cost;  // 구매 비용
}