using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // --- 변수 선언부 ---
    [Header("오브젝트 연결")]
    [SerializeField] private PickaxeController pickaxeController; // 곡괭이 컨트롤러 연결

    [Header("플레이어 조작")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float mouseSensitivity = 100f;

    [Header("플레이어 상태")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.2f;
    private bool isGrounded;

    [Header("오브젝트 연결")]
    [SerializeField] private Camera mainCamera;
    private Rigidbody rb;
    private Animator animator;

    // 이동 및 시점 관련 내부 변수
    private float leftRight;
    private float frontBack;
    private float xRotation = 0f; // <<-- 카메라의 상하 회전값을 저장할 변수 (추가)

    [Header("채굴 관련")]
    [SerializeField] private float miningDistance = 3f;
    [SerializeField] private LayerMask blockLayer;
    public PickaxeData currentPickaxe;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        leftRight = Input.GetAxis("Horizontal");
        frontBack = Input.GetAxis("Vertical");

        HandleMouseLook(); // 시점 처리
        CheckGrounded();   // 지면 체크
        HandleJump();      // 점프 처리
        HandleMining();    // 채굴 처리

        //ForTesting
        
        Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward, Color.red);
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    // --- 함수 구현부 ---

    // 애니메이션 이벤트가 호출할 함수 1: 공격 시작 신호 전달
    public void Event_StartMining()
    {
        pickaxeController.StartMining();
    }

    // 애니메이션 이벤트가 호출할 함수 2: 공격 끝 신호 전달
    public void Event_EndMining()
    {
        pickaxeController.EndMining();
    }

    private void HandleMouseLook()
    {
        // 마우스 입력값을 받습니다.
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // ▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
        // --- 상하 시점 이동 (수정 및 추가된 부분) ---
        // 마우스 Y축 움직임을 xRotation 변수에 누적합니다.
        xRotation -= mouseY;
        // 카메라가 90도 이상으로 고개를 꺾지 못하도록 회전값을 -90도와 90도 사이로 제한합니다.
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        // 제한된 회전값을 카메라의 로컬 회전값에 적용합니다. (몸 전체가 아닌 카메라만 회전)
        mainCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // ▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

        // --- 좌우 시점 이동 (기존과 동일) ---
        // 마우스 X축 움직임으로 플레이어 몸 전체를 회전시킵니다.
        transform.Rotate(Vector3.up * mouseX);
    }

    private void PlayerMove()
    {
        Vector3 moveDirection = transform.right * leftRight + transform.forward * frontBack;
        moveDirection.Normalize();
        rb.MovePosition(transform.position + moveDirection * speed * Time.fixedDeltaTime);

        animator.SetFloat("Speed", moveDirection.magnitude);
        animator.SetFloat("DirectionX", leftRight);
        animator.SetFloat("DirectionZ", frontBack);
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
        animator.SetBool("isGrounded", isGrounded);
    }

    private void HandleJump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // PlayerManager.cs의 일부

    private void HandleMining()
    {
        // 마우스 왼쪽 버튼을 누르면 "Mining" Trigger를 발동시킵니다.
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Mining");
            Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
            RaycastHit hit;

            // Ray가 적 레이어에 닿았는지 확인
            if (Physics.Raycast(ray, out hit, 2.0f, blockLayer))
            {
                
                BlockController block = hit.collider.GetComponent<BlockController>();

                
                if (block != null)
                {
                    block.takeDamage(2,hit);
                }
            }


        }
    }

}
