using UnityEngine;
using System.Collections.Generic; // List를 사용하기 위해 추가

public class PickaxeController : MonoBehaviour
{
    private PlayerManager playerManager;
    private Collider pickaxeCollider; // 곡괭이의 콜라이더

    // 한 번의 스윙에 여러 번 피해가 들어가는 것을 방지하기 위한 리스트
    private List<Collider> alreadyHitBlocks;

    void Start()
    {
        // 부모 오브젝트에서 PlayerManager 정보를 가져옵니다.
        playerManager = GetComponentInParent<PlayerManager>();
        // 곡괭이의 콜라이더를 가져옵니다.
        pickaxeCollider = GetComponent<Collider>();
        // 리스트를 초기화합니다.
        alreadyHitBlocks = new List<Collider>();

        // 게임 시작 시에는 공격 판정이 없도록 콜라이더를 꺼둡니다.
        pickaxeCollider.enabled = false;
    }

    // 애니메이션 이벤트에서 호출할 함수 1: 공격 시작
    public void StartMining()
    {
        // 새로운 스윙이므로 이전에 맞았던 블록 기록을 초기화합니다.
        alreadyHitBlocks.Clear();
        // 공격 판정을 활성화합니다.
        pickaxeCollider.enabled = true;
    }

    // 애니메이션 이벤트에서 호출할 함수 2: 공격 끝
    public void EndMining()
    {
        // 공격 판정을 비활성화합니다.
        pickaxeCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 이번 스윙에서 이미 맞았던 블록이면 무시합니다.
        if (alreadyHitBlocks.Contains(other))
        {
            return;
        }

        Block targetBlock = other.GetComponent<Block>();
        if (targetBlock != null)
        {
            Debug.Log("곡괭이가 " + other.name + " 블록에 닿았습니다!");
            targetBlock.OnHit(playerManager.currentPickaxe.power);

            // 맞은 블록을 리스트에 추가하여 중복 피해를 방지합니다.
            alreadyHitBlocks.Add(other);
        }
    }
}
