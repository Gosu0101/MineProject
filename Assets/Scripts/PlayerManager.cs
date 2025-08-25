using UnityEngine;
using System.Collections.Generic; // [추가된 코드] Dictionary를 사용하기 위해 추가

public class PlayerManager : MonoBehaviour
{
    // --- [추가된 코드] 싱글톤 설정 ---
    // 다른 스크립트에서 PlayerManager.Instance 로 쉽게 접근할 수 있게 해줍니다.
    public static PlayerManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    // ---------------------------------

    // --- 변수 선언부 ---
    [Header("오브젝트 연결")]
    [SerializeField] private PickaxeController pickaxeController; // 곡괭이 컨트롤러 연결
    [SerializeField] private InventoryUI inventoryUI; // [추가된 코드] 인벤토리 UI 컨트롤러 연결

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

    // --- [추가된 코드] 재화 및 인벤토리 데이터 ---
    [Header("재화 및 인벤토리")]
    public int currentGold = 0;
    // 어떤 아이템(BlockData)을 몇 개(int) 가지고 있는지 저장합니다.
    public Dictionary<BlockData, int> inventory = new Dictionary<BlockData, int>();
    // -------------------------------------------


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
        Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * miningDistance, Color.red);
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    // --- [추가된 코드] 아이템 획득 및 UI 갱신 함수 ---
    public void AddItem(BlockData blockData, int amount)
    {
        // 인벤토리에 이미 해당 아이템이 있는지 확인
        if (inventory.ContainsKey(blockData))
        {
            inventory[blockData] += amount; // 있으면 개수만 추가
        }
        else
        {
            inventory.Add(blockData, amount); // 없으면 새로 추가
        }

        Debug.Log(blockData.blockName + " " + amount + "개 획득! 현재: " + inventory[blockData] + "개");

        // 인벤토리 UI가 연결되어 있다면 UI 갱신을 요청합니다.
        if (inventoryUI != null)
        {
            inventoryUI.UpdateInventoryUI(inventory);
        }
    }
    // ------------------------------------------------

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

    private void CheckGrounded()//블럭 위에 올라갔는지 체크하는것도 추가해주세요
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
            Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);//카메라 위치로 이동
            RaycastHit hit;

            // Ray가 블럭 레이어에 닿았는지 확인
            if (Physics.Raycast(ray, out hit, miningDistance, blockLayer))//3번째 매서드가 거리
            {
                BlockController block = hit.collider.GetComponent<BlockController>();//레이에 닿은 블럭 컨트롤러 가져오기

                if (block != null)//블럭에 블럭컨트로럴가 없지 않을 경우
                {
                    block.takeDamage(currentPickaxe.power, hit);//첫번째 매서드가 데미지
                }
            }
        }
    }
}
