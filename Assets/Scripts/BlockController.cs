//=====================================================================
//이 스크립트는 블럭 데이터 설정,블럭의 파괴와, 아이템 드랍 관련 스크립트입니다.
//=====================================================================

using UnityEngine;

public class BlockController : MonoBehaviour
{
    private int whatIsThisBlocK;
    public BlockData bd;

    int blockID;
    string blockName;
    int hardness; // 블록의 경도 (요구 곡괭이 파워)
    int hp;       // 블록의 내구도
    int value;    // 판매 시 가치



    ////이 함수는 Spawner에서 블럭 Id와 함께 불러와져야 합니다.
    ////사용법:case에 블럭 아이디를 넣고 bd에 id와 연결하고 싶은 블럭 데이터를 
    //public void SetThisBlock(int thisBlocIs)
    //{
    //    switch(whatIsThisBlocK)
    //    {
    //        case 0:
    //            bd = Resources.Load<BlockData>("Data/Block/Grain");
    //            break;

    //        case 1:
    //            bd = Resources.Load<BlockData>("Data/Block/Grain");
    //            break;
    //        default:
    //            Debug.Log("BC.SetThisBlock.default 발생");
    //            break;

    //    }
    //}

    void Start()
    {
        blockID = bd.blockID;
        blockName = bd.blockName;
        hardness = bd.hardness;
        hp = bd.hp;
        value = bd.value;
        
    }

    public void OnHitByRay(int damage, RaycastHit hit)//플레이어 레이케스트에 맞았는지 확인하는 함수
    {
        if(hit.collider.gameObject == this.gameObject)//Ray에 맞은게 자기 자신인지 확인
        {
            hp = damage - hardness;//후에 Set으로 교체 추천
        }
    }

    private void Update()
    {
        if (bd != null)//블럭데이터 없을 경우 발생(주의:작동안함)
        {
            Debug.Log("블럭 데이터 스크립트 손실");
            this.gameObject.SetActive(false);//디버그 로그 띄우고 비활성화
        }

        if(bd.hp <= 0)//체력이 0이하일경우
        {
            resourceDrop();//자원 드랍
            gameObject.SetActive(false);//비활성화(후에 파괴로 변경가능)
        }
    }

    void resourceDrop()//자원 드랍
    {
        Debug.Log("자원 드랍");
    }

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
        return bd.hp;
    }
}
