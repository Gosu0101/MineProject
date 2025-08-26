using UnityEngine;
using System.Collections.Generic;

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

    private float leftRight;
    private float frontBack;
    private float xRotation = 0f;

    [Header("채굴 관련")]
    [SerializeField] private float miningDistance = 3f;
    [SerializeField] private LayerMask blockLayer;
    public PickaxeData currentPickaxe;

    [Header("재화 및 인벤토리")]
    public int currentGold = 0;
    public Dictionary<BlockData, int> inventory = new Dictionary<BlockData, int>();

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
}
