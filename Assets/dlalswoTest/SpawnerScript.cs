using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public int xWidth;
    public int yWidth;
    public int height;
    public List<GameObject> blockPrefabs;
    public List<GameObject> blockWithResourcePrefabs;

    private void Start()
    {
        generateBlockLayer(0);
    }

    //현재 단계에 맞는 레이어 생성
    void generateBlockLayer(int levelOfBlock)
    {
        Debug.Log("generateLayer...:" + levelOfBlock);
        for (int i = 0; i < xWidth; i++)
        {
            for (int j = 0; j < yWidth; j++)
            {
                float ran = Random.Range(0f,1f);
                if(ran < 0.5f) { }
                else
                {
                    Instantiate(blockPrefabs[levelOfBlock], (gameObject.transform.position + new Vector3(i, 0, j)), Quaternion.identity);
                }
                    
            }
        }
    }
}
