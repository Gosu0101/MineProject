//using Mono.Cecil;
using System.Collections;
using UnityEngine;
//TODO: 생성 되고 카운트 다운 추가, 이펙트 추가
public class BombController : MonoBehaviour
{
    [SerializeField] public float radius = 1f;
    [SerializeField] public LayerMask blockLayer;
    [SerializeField] public float defaultPower = 1;
    [SerializeField] public float timer = 1;
    [SerializeField] public GameObject effect;
    void OnEnable()
    {
        ignite();
    }
    

    void ignite()
    {
        //코루틴 함수는 반드시 StartCoroutine을 통해서 호출
        //사용이유는 메인 쓰레드와 별개의 비동기 처리를 위해서
        StartCoroutine(WaitForExplosive(timer));
        Debug.Log("ignite");
    }
    
    //코루틴 함수 작성
    IEnumerator WaitForExplosive(float time)
    {
        Debug.Log("waitforexplosive");
        //이미지의 Fill Amount
        yield return new WaitForSeconds(time);
        Explosive();
        
    }

    void Explosive()
    {
        Debug.Log("explosive");
        RaycastHit[] hits;

        //for (int i = 0; i < 5; i++)//중앙에 가까울수록 더 많은 데미지를 받음(데미지를 여러번 받는다)
        //{
        //    hits = Physics.SphereCastAll(transform.position, radius * i * i / 10, Vector3.down, 0f, blockLayer);//구체로 전체 적용
            hits = Physics.SphereCastAll(transform.position, radius, Vector3.down, 0f, blockLayer);//구체로 전체 적용
            foreach (RaycastHit hit in hits)//레이에 맞은 모든 객체 하나 하나에 적용
            {
                BlockController block = hit.collider.GetComponent<BlockController>();
                if (block != null)
                {
                    float Power = Random.Range(defaultPower, defaultPower + radius);//폭탄의 힘을 일정한 범위 내에서 랜덤으로 정하는 코드
                    block.takeDamage(Power, hit);
                }
            }
        //}

        GameObject usedEffect = Instantiate(effect, transform.position, transform.rotation);//이펙트 프립팹 복사본 생성, 생성한 오브젝 참조
        StartCoroutine(WaitForDestroy(usedEffect));//제거하는 코루틴 시작
    }

    IEnumerator WaitForDestroy(GameObject obj)//오브젝트 5초후 제거하는 함수
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(obj);
    }
}
