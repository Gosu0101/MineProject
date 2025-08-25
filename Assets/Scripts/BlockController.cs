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
    int hardness; // 블록의 경도 (요구 곡괭이 파워)
    int hp;       // 블록의 내구도
    int value;    // 판매 시 가치

    float locate; //현재 위치(스포너가 현재 어디까지 캐졌는지 확인용)



    

    void Start()
    {
        if(blockData != null)//블럭 데이터가 존재할 경우(블럭데이터는 이때 초기값만 가져옵니다.)
        {
            blockID = blockData.blockID;
            blockName = blockData.blockName;
            hardness = blockData.hardness;
            hp = blockData.hp;
            value = blockData.value;
        }
        else
        {
            Debug.Log("블럭 데이터 스크립트 손실");//블럭데이터 없을 경우 발생
            this.gameObject.SetActive(false);//디버그 로그 띄우고 비활성화
        }
        
        
    }

    public void takeDamage(int damage, RaycastHit hit)//플레이어 레이케스트에 맞았는지 확인하는 함수
    {
        //Debug.Log("takeDamageStart");
        if (hit.collider.gameObject == this.gameObject)//Ray에 맞은게 자기 자신인지 확인
        {
            if (damage > hardness)//데미지가 경도보다 클 경우(회복방지)
                hp -= damage - hardness;
            //Debug.Log("takeDamage :" + hp);
            
        }
    }

    //후에 폭탄이 추가 되면 ray로 해결 안되거나 트리거로 처리하는게 편할 수 있음

    private void Update()
    {
        if(hp <= 0)//체력이 0이하일경우
        {
            //Debug.Log("Break");
            resourceDrop();//자원 드랍
            SpawnerScript.Instance.refreshLayer(locate);
            gameObject.SetActive(false);//비활성화(후에 파괴로 변경가능)
            //GameObject.Destroy(gameObject);
        }
    }

    void resourceDrop()//자원 드랍
    {
        if (resourcePrefab != null)
        {
            Debug.Log("자원 드랍");
            Instantiate(resourcePrefab);
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
    public int GetBlockHardness()
    {
        return hardness;
    }
    public int GetBlockHp()
    {
        return hp;
    }
    public int GetBlockMaxHp()
    {
        return blockData.hp;
    }

    
    public void setLocate(float n)//블럭의 현재 높이를 설정
    {
        locate = n;
        return;
    }
}
