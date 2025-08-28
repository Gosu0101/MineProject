using UnityEngine;

public class ItemController : MonoBehaviour
{
    // [추가된 코드] 이 아이템이 어떤 블록의 정보를 담고 있는지 저장할 변수
    public BlockData blockData;
    

    Rigidbody rb;
    [SerializeField] private LayerMask blockLayer;
    [SerializeField] private LayerMask groundLayer;

    bool onBlock;
    bool onGround;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // [수정된 코드] 아이템이 생성될 때 자연스럽게 튀어오르도록 AddForce 사용
        float x = Random.Range(-2f, 2f);
        float z = Random.Range(-2f, 2f);
        rb.AddForce(new Vector3(x, 5f, z), ForceMode.Impulse);
    }

    void Update()
    {
        onBlock = Physics.Raycast(transform.position, Vector3.down, 0.25f, blockLayer);
        onGround = Physics.Raycast(transform.position, Vector3.down, 0.25f, groundLayer);
        if (onBlock || onGround)
        {
            
            rb.linearVelocity = new Vector3(0, 24f*Time.deltaTime, 0);//[수정]0.2로 하면 튀어오르지도 않고 제자리에 잘 있음
        }

        transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))//대소문자 주의
        {
            // [수정된 코드] PlayerManager를 통해 아이템을 인벤토리에 추가합니다.
            //점수 추가 함수 넣어주세요 -> 아이템 추가 로직으로 변경
            if (PlayerManager.Instance != null && blockData != null)
            {
                PlayerManager.Instance.AddItem(blockData, 1);
            }

            // 아이템 획득 후 오브젝트를 파괴합니다.
            Destroy(gameObject);
        }
    }
}
