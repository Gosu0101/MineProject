using UnityEngine;
[CreateAssetMenu(fileName = "New Block Data", menuName = "Data/Block")]
public class BlockData : ScriptableObject
{ 
    public int blockID;
    public string blockName;
    public int hardness; // 블록의 경도 (요구 곡괭이 파워)
    public int hp;       // 블록의 내구도
    public int value;    // 판매 시 가치
}