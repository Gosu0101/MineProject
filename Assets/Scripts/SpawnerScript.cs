using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public int xWidth;//x크기
    public int yWidth;//y크기
    public float depth;//최대 깊이
    public List<GameObject> blockPrefabs;//블럭 프리팹
    public List<float> layerChangingTimeing;
    

    float currentDepth = 0;//현재 깊이

    private GameObject allBlock;

    


    public static SpawnerScript Instance { get; private set; }//싱클톤 사용

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }


    private void Start()
    {
        allBlock = new GameObject("Blocks");
        allBlock.transform.position = Vector3.zero;


        for (int i = 0; i < 10; i++)//시직시 10개의 층 생성
        {
            generateBlockLayer(i);
        }

        

    }

    //현재 단계에 맞는 레이어 생성
    public void generateBlockLayer(int locate)//층별로 블럭생성
    {
        int levelOfBlock = 0;
        if(currentDepth >=  depth)//현재 깊이가 초과하면 더이상 생성 안함
        {
            return;
        }
        //Debug.Log("generateLayer...:" + levelOfBlock);

        for (int i = layerChangingTimeing.Count - 1; i > 0; i--)//현재 어느 레벨인지 확인하기
        {
            if (locate > layerChangingTimeing[i])
            {
                levelOfBlock = i; //레벨 설정하기
                break;
            }
        }


        for (int i = 0; i < xWidth; i++)//x크기 만큼 반복
        {
            for (int j = 0; j < yWidth; j++)//y크기 만큼 반복
            {
                int ran = generateRandomBlock();//랜덤하게 광물 선택
                if(levelOfBlock+ran >blockPrefabs.Count-1)//생성할 프리펩이 없는 경우
                {
                    //아직 아무것도 안넣어둠, 아마도 빈공간으로 남겨둘듯
                }
                else
                {
                    
                    GameObject newBlock = 
                    Instantiate(blockPrefabs[levelOfBlock + ran],//랜덤으로 더 좋은 광물 생성
                        (gameObject.transform.position + new Vector3(i, 0 - currentDepth, j)),//위치 조정(스포너 기준으로 생성)
                        Quaternion.identity //회전(아마도 없어도 됨)
                        , allBlock.transform);

                    BlockController newBlockController = newBlock.GetComponent<BlockController>();
                    newBlockController.setLocate(currentDepth);//위치를 현재 깊이로 설정
                }
                
                    
            }
        }
        currentDepth += 1f;//깊이 추가
    }

    int generateRandomBlock()
    {
        float ran = Random.Range(0f,1f);
        if(ran <= 0.5f) return 0;//확률 조정할때 아래 마이너스 문이랑 같은 값으로 넣어주세요.
        else ran -= 0.5f;
        if (ran <= 0.3f) return 1;
        else ran -= 0.3f;
        if (ran <= 0.15f) return 2;
        else ran -= 0.15f;
        if (ran <= 0.04f) return 3;
        else ran -= 0.04f;
        if (ran <= 0.01f) return 4;

        Debug.Log("Error: generateRandomBlock");//어째선진 몰라도 다 위에 if에 안들어갔을 경우
        return 0;
    }

    public void refreshLayer(float locate)//현재 위치를 받아오기
    {
        if (locate < currentDepth)
        {
           
            generateBlockLayer((int)currentDepth);//싱글톤 사용
            Debug.Log("refreshLayer :" + locate);
            

        }
    }
}
