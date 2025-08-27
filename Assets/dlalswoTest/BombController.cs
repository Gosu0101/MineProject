using Mono.Cecil;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public float radius = 1f;
    public LayerMask blockLayer;
    public float defaultPower = 1;
    //TODO: 생성 되고 카운트 다운 추가, 이펙트 추가
    void OnAttack()
    {
        Explosive();
        //Debug.Log("마우스 공격");
    }

    void Explosive()
    {
        RaycastHit[] hits;

        for (int i = 0; i < 5; i++)//중앙에 가까울수록 더 많은 데미지를 받음(데미지를 여러번 받는다)
        {
            hits = Physics.SphereCastAll(transform.position, radius * i * i / 10, Vector3.down, 0f, blockLayer);

            foreach (RaycastHit hit in hits)
            {
                BlockController block = hit.collider.GetComponent<BlockController>();
                if (block != null)
                {
                    float Power = Random.Range(defaultPower, defaultPower + radius);//폭탄의 힘을 일정한 범위 내에서 랜덤으로 정하는 코드
                    block.takeDamage(Power, hit);
                }
            }
        }
    }
}
