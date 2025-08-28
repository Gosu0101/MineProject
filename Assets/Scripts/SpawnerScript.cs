using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [Header("영역 설정")]
    public int xWidth = 10; //x크기
    public int yWidth = 10; //y크기
    public float depth = 100; //최대 깊이
    public int initialLayers = 10; // [개선] 처음에 생성할 레이어 수

    [Header("블록 설정")]
    public List<GameObject> blockPrefabs; //블럭 프리팹
    public List<float> layerChangingTimeing;

    [Header("지형 정리 설정")] // [추가]
    public LayerMask groundLayer; // [추가] 파괴할 땅의 레이어를 지정합니다.


    private float currentDepth = 0; //현재 깊이
    private GameObject allBlock; // 생성된 모든 블록을 담을 부모 오브젝트


    // [주석] 싱글톤 인스턴스: 다른 스크립트에서 SpawnerScript.Instance로 쉽게 접근하기 위해 사용합니다.
    // 하지만 현재 게임에서는 여러 스포너를 사용할 것이므로, 이 싱글톤은 '가장 마지막에 활성화된 스포너'를 가리키게 됩니다.
    // 이는 블록 파괴 시 새 레이어를 생성하는 refreshLayer 기능이 마지막 스포너에만 작동하게 할 수 있으므로 주의가 필요합니다.
    public static SpawnerScript Instance { get; private set; }

    void Awake()
    {
        // 싱글톤 인스턴스를 설정합니다.
        Instance = this;
    }


    private void Start()
    {
        // --- 스포너 영역의 기존 땅을 제거하는 로직 ---
        ClearGroundInArea();
        // -------------------------------------------

        // [개선] 생성되는 블록들을 정리하기 위해, 이 스포너의 자식으로 Blocks 부모 오브젝트를 생성합니다.
        allBlock = new GameObject("Blocks");
        allBlock.transform.SetParent(this.transform);
        allBlock.transform.position = Vector3.zero;

        // [개선] 설정된 initialLayers 값만큼 초기 레이어를 생성합니다.
        for (int i = 0; i < initialLayers; i++)
        {
            generateBlockLayer(i);
        }
    }

    // --- 영역 내의 'Ground' 레이어를 가진 오브젝트를 제거하는 함수 (수정) ---
    private void ClearGroundInArea()
    {
        // 스포너의 위치를 기준으로 생성 영역을 정의합니다.
        Vector3 areaCenter = transform.position + new Vector3((xWidth - 1) / 2f, 0, (yWidth - 1) / 2f);
        Vector3 areaSize = new Vector3(xWidth, 5f, yWidth); // Y축 크기는 넉넉하게 줍니다.

        // 해당 영역 내의 모든 콜라이더를 가져옵니다.
        Collider[] collidersInArea = Physics.OverlapBox(areaCenter, areaSize / 2, Quaternion.identity);

        foreach (var col in collidersInArea)
        {
            // [수정] 콜라이더를 가진 오브젝트의 레이어가 groundLayer에 포함되는지 확인하여 파괴합니다.
            if ((groundLayer.value & (1 << col.gameObject.layer)) > 0)
            {
                Destroy(col.gameObject);
            }
        }
    }
    // -----------------------------------------------------------------

    //현재 단계에 맞는 레이어 생성
    public void generateBlockLayer(int locate)
    {
        int levelOfBlock = 0;
        if (currentDepth >= depth)
        {
            return;
        }

        for (int i = layerChangingTimeing.Count - 1; i >= 0; i--) // [수정] i > 0 조건을 i >= 0 으로 변경하여 0번째 요소도 포함하도록 수정
        {
            if (locate >= layerChangingTimeing[i]) // [수정] > 조건을 >= 으로 변경하여 정확한 깊이에서 레벨이 바뀌도록 수정
            {
                levelOfBlock = i;
                Debug.Log("levelOfBlock :" + i);
                break;
                
            }
        }

        for (int i = 0; i < xWidth; i++)
        {
            for (int j = 0; j < yWidth; j++)
            {
                int ran = generateRandomBlock();
                if (levelOfBlock + ran > blockPrefabs.Count - 1)
                {
                    // Do nothing
                }
                else
                {
                    GameObject newBlock =
                    Instantiate(blockPrefabs[levelOfBlock + ran],
                        (gameObject.transform.position + new Vector3(i, 0 - currentDepth, j)),
                        Quaternion.identity
                        , allBlock.transform);

                    BlockController newBlockController = newBlock.GetComponent<BlockController>();
                    newBlockController.setLocate(currentDepth);
                }
            }
        }
        currentDepth += 1f;
    }

    int generateRandomBlock()
    {
        float ran = Random.Range(0f, 1f);
        if (ran <= 0.5f) return 0;
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

    public void refreshLayer(float locate)
    {
        if (locate + 10> currentDepth)
        {
            generateBlockLayer((int)currentDepth);
            Debug.Log("refreshLayer :" + locate); // 너무 자주 호출될 수 있으므로 주석 처리
        }
    }
}
