using UnityEngine;

public class Block : MonoBehaviour
{
    public BlockData blockData; // 각 블록의 데이터를 담는 Scriptable Object
    private int currentHp;

    void Start()
    {
        // Scriptable Object에 정의된 hp값으로 현재 체력 초기화
        currentHp = blockData.hp;
    }

    // 플레이어로부터 공격(클릭)을 받았을 때 호출될 함수
    public void OnHit(int toolPower)
    {
        // 1. 도구의 파워가 블록의 경도(hardness)보다 낮은지 확인
        if (toolPower < blockData.hardness)
        {
            Debug.Log("더 강력한 도구가 필요합니다!");
            // TODO: 여기에 "팅!" 하는 효과음 및 UI 피드백 로직 추가
            return; // 파워가 부족하면 아무런 데미지를 주지 않고 함수 종료
        }

        // 2. 경도 조건을 만족하면 HP를 1 감소시킴
        currentHp--;
        Debug.Log(blockData.blockName + "의 현재 HP: " + currentHp);

        // 3. HP가 0 이하가 되면 블록 파괴 함수 호출
        if (currentHp <= 0)
        {
            DestroyBlock();
        }
    }

    private void DestroyBlock()
    {
        Debug.Log(blockData.blockName + " 블록 파괴됨!");
        
        // TODO: 여기에 아이템 생성 및 획득 로직 추가
        // TODO: 여기에 블록 파괴 파티클 이펙트 및 사운드 재생 로직 추가

        // 블록 게임 오브젝트를 씬에서 제거
        Destroy(gameObject);
    }
}
