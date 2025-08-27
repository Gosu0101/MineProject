using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class PlayerManager : MonoBehaviour
{
    // --- 싱글톤 설정 ---
    public static PlayerManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    // --------------------

    [Header("오브젝트 연결")]
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private GoldUI goldUI; // [추가] GoldUI 스크립트 연결

    [Header("플레이어 조작")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float mouseSensitivity = 110f;

    [Header("플레이어 상태")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.2f;
    private bool isGrounded;

    [Header("오브젝트 연결")]
    [SerializeField] private Camera mainCamera;
    private Rigidbody rb;
    private Animator animator;

    private float leftRight;
    private float frontBack;
    private float xRotation = 0f;

    [Header("채굴 관련")]
    [SerializeField] private float miningDistance = 3f;
    [SerializeField] private LayerMask blockLayer;
    public PickaxeData currentPickaxe;

    [Header("재화 및 인벤토리")]
    private int _currentGold = 0;
    public int currentGold
    {
        get { return _currentGold; } // 값을 가져갈 때
        set // 값을 할당할 때
        {
            _currentGold = value;

            // 값이 바뀔 때마다 자동으로 UI 업데이트 함수를 호출
            if (goldUI != null)
            {
                goldUI.UpdateGoldText(_currentGold);
            }
        }
    }
    public Dictionary<BlockData, int> inventory = new Dictionary<BlockData, int>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // 게임 시작 시 골드를 0으로 설정 (이때 UI도 자동으로 0으로 업데이트됨)
        currentGold = 0;
    }

    void Update()
    {
        leftRight = Input.GetAxis("Horizontal");
        frontBack = Input.GetAxis("Vertical");

        HandleMouseLook();
        CheckGrounded();
        HandleJump();
        HandleMining();

        Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * miningDistance, Color.red);
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    // [수정] 꾹 누르는 동안 애니메이션을 반복하도록 로직 변경
    private void HandleMining()
    {
        // 마우스 왼쪽 버튼을 '누르고 있는 동안' true
        if (Input.GetMouseButton(0))
        {
            // isMining 파라미터를 true로 설정하여 채굴 애니메이션을 재생
            animator.SetBool("isMining", true);
        }
        else // 마우스 버튼을 떼면
        {
            // isMining 파라미터를 false로 설정하여 채굴 애니메이션을 중단
            animator.SetBool("isMining", false);
        }
    }

    // 애니메이션 이벤트가 호출할 실제 데미지를 주는 함수 (기존과 동일)
    public void ApplyMiningDamage()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, miningDistance, blockLayer))
        {
            BlockController block = hit.collider.GetComponent<BlockController>();
            if (block != null)
            {
                block.takeDamage(currentPickaxe.power, hit);
            }
        }
    }

    public void AddItem(BlockData blockData, int amount)
    {
        if (inventory.ContainsKey(blockData))
        {
            inventory[blockData] += amount;
        }
        else
        {
            inventory.Add(blockData, amount);
        }

        if (inventoryUI != null)
        {
            inventoryUI.UpdateInventoryUI(inventory);
        }
    }

    // --- 나머지 이동, 점프 관련 함수들 (기존과 동일) ---
    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        mainCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
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
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer | blockLayer);
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
    // --- [추가] 판매 관련 함수 ---
    public int SellAllItems()
    {
        int totalSaleValue = 0;

        // 인벤토리에 있는 모든 아이템을 순회합니다.
        foreach (var item in inventory)
        {
            BlockData data = item.Key;
            int count = item.Value;

            // (아이템 가치 * 아이템 개수)를 총 판매 금액에 더합니다.
            totalSaleValue += data.value * count;
        }

        // 만약 판매할 아이템이 있다면
        if (totalSaleValue > 0)
        {
            currentGold += totalSaleValue; // 번 돈을 현재 골드에 추가
            inventory.Clear(); // 인벤토리 비우기

            // 인벤토리 UI와 골드 UI를 즉시 갱신합니다.
            if (inventoryUI != null)
            {
                inventoryUI.UpdateInventoryUI(inventory);
            }
            // TODO: 골드 UI 갱신 로직도 여기에 추가하면 좋습니다.
        }

        return totalSaleValue; // 총 얼마를 벌었는지 반환
    }

    // ... (기존 코드 하단은 동일) ...

}
