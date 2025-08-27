//=====================================================================
//이 스크립트는 블럭 데이터 설정,블럭의 파괴와, 아이템 드랍 관련 스크립트입니다.
//=====================================================================

using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class BlockController : MonoBehaviour
{
    public BlockData blockData;
    public GameObject resourcePrefab;

    int blockID;
    string blockName;
    float hardness; // 블록의 경도 (요구 곡괭이 파워)
    float hp;       // 블록의 내구도
    float value;    // 판매 시 가치

    float locate = 0; //현재 위치(스포너가 현재 어디까지 캐졌는지 확인용)

    // [추가된 코드] 파괴 처리가 중복 실행되는 것을 방지하기 위한 변수
    private bool isDestroyed = false;

    void Start()
    {
        if (blockData != null)//블럭 데이터가 존재할 경우(블럭데이터는 이때 초기값만 가져옵니다.)
        {
            blockID = blockData.blockID;
            blockName = blockData.blockName;
            hardness = (float)blockData.hardness;
            hp = (float)blockData.hp;
            value = (float)blockData.value;
        }
        else
        {
            Debug.Log("블럭 데이터 스크립트 손실");//블럭데이터 없을 경우 발생
            this.gameObject.SetActive(false);//디버그 로그 띄우고 비활성화
        }
    }

    public void takeDamage(float damage, RaycastHit hit)//플레이어 레이케스트에 맞았는지 확인하는 함수
    {
        // [추가된 코드] 이미 파괴되었거나, Ray에 맞은 오브젝트가 자신이 아니면 함수 종료
        if (isDestroyed || hit.collider.gameObject != this.gameObject)
        {
            return;
        }

        //Debug.Log("takeDamageStart");
        if (damage >= hardness)//데미지가 경도보다 클 경우
        {
            hp -= damage - hardness; //곡갱이 효율관련 코드(추후 수정가능)
            //Debug.Log("takeDamage :" + hp);
        }

        // [추가된 코드] 체력이 0 이하로 떨어졌다면 파괴 함수를 즉시 호출
        if (hp <= 0)
        {
            DestroyBlock();
        }
    }

    //후에 폭탄이 추가 되면 ray로 해결 안되거나 트리거로 처리하는게 편할 수 있음

    // [수정된 코드] Update 함수는 더 이상 파괴 로직을 담당하지 않으므로 삭제합니다.
    // private void Update() { ... }

    // [추가된 코드] 파괴 관련 로직을 처리하는 별도의 함수
    private void DestroyBlock()
    {
        isDestroyed = true; // 파괴 상태로 변경하여 중복 호출 방지

        resourceDrop();//자원 드랍

        // SpawnerScript 인스턴스가 존재하는지 확인하여 오류를 방지
        if (SpawnerScript.Instance != null)
        {
            SpawnerScript.Instance.refreshLayer(locate);
        }
        else
        {
            Debug.LogError("SpawnerScript 인스턴스를 찾을 수 없습니다!", gameObject);
        }

        gameObject.SetActive(false);//비활성화(후에 파괴로 변경가능)
    }

    void resourceDrop()//자원 드랍
    {
        if (resourcePrefab != null)
        {
            //Debug.Log("자원 드랍");
            // [수정된 코드] 생성된 아이템 오브젝트의 정보를 가져와서 데이터를 넘겨줍니다.
            GameObject droppedItem = Instantiate(resourcePrefab, gameObject.transform.position, Quaternion.identity);

            ItemController itemController = droppedItem.GetComponent<ItemController>();
            if (itemController != null)
            {
                // 생성된 아이템에게 "너는 이 블록에서 나왔어" 라고 정보를 알려줍니다.
                itemController.blockData = this.blockData;
            }
        }
    }
    //Get 함수들(아직 사용은 안됨)
    public int GetBlockID()
    {
        return blockID;
    }

    public string GetBlockName()
    {
        return blockName;
    }
    public float GetBlockHardness()
    {
        return hardness;
    }
    public float GetBlockHp()
    {
        return hp;
    }
    public int GetBlockMaxHp()
    {
        return blockData.hp;
    }
    public float GetBlockValue()
    {
        return value;
    }

    public void setLocate(float n)//블럭의 현재 높이를 설정
    {
        locate = n;
        return;
    }
}
