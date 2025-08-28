using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Pickaxe Tier List", menuName = "Data/Pickaxe Tier List")]
public class PickaxeTierList : ScriptableObject
{
    public List<PickaxeData> tiers;
}