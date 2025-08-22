using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public int xWidth;
    public int yWidth;
    public float height;
    public List<GameObject> blockPrefabs;
    

    private float currentHeight = 0;

    //TODO:랜덤 구현, 위쪽 블럭 파괴 확인하여 아래쪽 미리 생성하기

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            generateBlockLayer(0);
        }
        
    }

    //현재 단계에 맞는 레이어 생성
    void generateBlockLayer(int levelOfBlock)
    {
        
        if(currentHeight >=  height)
        {
            return;
        }
        Debug.Log("generateLayer...:" + levelOfBlock);
        for (int i = 0; i < xWidth; i++)
        {
            for (int j = 0; j < yWidth; j++)
            {
                int ran = generateRandomBlock();
                if(levelOfBlock+ran >blockPrefabs.Count-1)//생성할 프리펩이 없는 경우
                {
                    Debug.Log("nothing");
                }
                else
                {
                    Instantiate(blockPrefabs[levelOfBlock + ran],//랜덤으로 더 좋은 광물 생성
                        (gameObject.transform.position + new Vector3(i, 0 - currentHeight, j)),
                        Quaternion.identity);
                }
                
                    
            }
        }
        currentHeight += 1f;
    }

    int generateRandomBlock()
    {
        float ran = Random.Range(0f,1f);
        if(ran <= 0.5f) return 0;
        else ran -= 0.5f;
        if (ran <= 0.3f) return 1;
        else ran -= 0.3f;
        if (ran <= 0.15f) return 2;
        else ran -= 0.15f;
        if (ran <= 0.04f) return 3;
        else ran -= 0.04f;
        if (ran <= 0.01f) return 4;

        Debug.Log("Error: generateRandomBlock");
        return 0;
    }
}
