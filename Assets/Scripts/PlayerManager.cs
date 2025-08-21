using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator animator; // 애니메이터 컴포넌트를 담을 변수
    //이동변수
    private float LeftRight;
    private float FrontBack;
    private Rigidbody rb;
    [SerializeField] private float speed = 2f;  //이동속도

    //점프 변수
    [SerializeField] private LayerMask groundLayer;
    //[SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private float jumpForce = 5.0f;
    private bool isGrounded;
    


    // 게임이 시작될 때 한 번만 호출됩니다.
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // 내 게임 오브젝트에 붙어있는 Animator 컴포넌트를 찾아서 가져옵니다.
        animator = GetComponent<Animator>();
    }

    // 매 프레임마다 호출됩니다.
    void Update()
    {

        LeftRight = Input.GetAxis("Horizontal");
        FrontBack = Input.GetAxis("Vertical");
        Mine();

        //점프
        // Raycast 시작점을 현재 위치보다 10cm 위로 옮깁니다.
        Vector3 rayStartPoint = transform.position + Vector3.up * 0.1f;

        // 수정된 시작점에서 Raycast를 실행합니다.
        isGrounded = Physics.Raycast(rayStartPoint, Vector3.down, groundCheckDistance, groundLayer);

        //애니메이터 값 전달
        animator.SetBool("isGrounded", isGrounded);

        // Debug Ray도 수정된 시작점에서 그리도록 변경합니다.
        Debug.DrawRay(rayStartPoint, Vector3.down * groundCheckDistance, Color.red);
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Jump");
            Jump();
            
            
        }
    }

    private void FixedUpdate()
    {
        PlayerMove();
        
        
    }
    private void PlayerMove()
    {
        //이동 방향 및 속도 계산
        Vector3 playerVector;
        playerVector = new Vector3(LeftRight, 0f, FrontBack).normalized;
        float playerSpeed = playerVector.magnitude;

        //애니메이션 속도 업데이트
        animator.SetFloat("Speed", playerSpeed);

        // 월드 이동 방향(movement)을 캐릭터 기준의 로컬 방향으로 변환
        Vector3 localMovement = transform.InverseTransformDirection(playerVector);

        // 로컬 방향의 x축과 z축 값을 각각 애니메이터에 전달
        animator.SetFloat("DirectionX", localMovement.x);
        animator.SetFloat("DirectionZ", localMovement.z);

        //캐릭터 회전 및 이동 처리
        if (playerSpeed > 0.1f)
        {
            //회전처리
            Quaternion newRotation = Quaternion.LookRotation(playerVector);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, newRotation, 0.2f));
            
            //이동처리
            rb.MovePosition(transform.position + playerVector * speed * Time.fixedDeltaTime);
        }

    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);   
    }
    private void Mine()
    {

            if (Input.GetMouseButtonDown(0))
            {
                animator.SetBool("Mining", true);
            }
            if (Input.GetMouseButtonUp(0))
            {
                {
                    animator.SetBool("Mining", false);
                }
            }
    }
}
