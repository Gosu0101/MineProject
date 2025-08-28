using UnityEngine;
[CreateAssetMenu(fileName = "New Pickaxe Data", menuName = "Data/Pickaxe")]
public class PickaxeData : ScriptableObject
{
    public int pickaxeID;
    public string pickaxeName;
    public int power; // °î±ªÀÌÀÇ ÆÄ¿ö
    public int cost;  // ±¸¸Å ºñ¿ë
    public GameObject pickaxePrefab; // [Ãß°¡] ÀÌ °î±ªÀÌÀÇ 3D ¸ðµ¨ ÇÁ¸®ÆÕ
}
