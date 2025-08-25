using UnityEngine;

public class ItemController : MonoBehaviour
{ 
    //TODO: 아이템 획득, 생성될때 옆으로 살짝 튕기기

    Rigidbody rb;
    [SerializeField] private LayerMask blockLayer;
    [SerializeField] private LayerMask groundLayer;

    bool onBlock;
    bool onGround;

    void Start()
    {
        rb = GetComponent<Rigidbody>();


        float x = Random.Range(-1, 1);
        float y = Random.Range(-1, 1);
        rb.linearVelocity = new Vector3(x, 1, y);
        
        
    }

    void Update()
    {
        onBlock = Physics.Raycast(transform.position, Vector3.down, 0.25f, blockLayer);
        onGround = Physics.Raycast(transform.position, Vector3.down, 0.25f, groundLayer);
        if(onBlock || onGround)
        {
            rb.linearVelocity = new Vector3(0, 1f, 0);
        }

        transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))//대소문자 주의
        {
            //점수 추가 함수 넣어주세요
            gameObject.SetActive(false);
        }
    }
}
