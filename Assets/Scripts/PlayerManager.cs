using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    [Header("오브젝트 연결")]
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private GoldUI goldUI;
    [SerializeField] private PickaxeController pickaxeController;
    [SerializeField] private Camera mainCamera;

    [Header("플레이어 조작")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float mouseSensitivity = 100f;

    [Header("플레이어 상태")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.2f;
    private bool isGrounded;

    private Rigidbody rb;
    private Animator animator;

    private float leftRight;
    private float frontBack;
    private float xRotation = 0f;

    [Header("채굴 관련")]
    [SerializeField] private float miningDistance = 3f;
    [SerializeField] private LayerMask blockLayer;
    public PickaxeData currentPickaxe;

    [Header("강화 데이터")]
    [SerializeField] private PickaxeTierList pickaxeTiers;

    [Header("부가 능력")]
    public int inventorySize = 2;
    public int bagLevel = 0;
    public float miningSpeedModifier = 1.0f;
    public int speedLevel = 0;

    private int _currentGold = 0;
    public int currentGold
    {
        get { return _currentGold; }
        set
        {
            _currentGold = value;
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
        currentGold = 30000;
    }

    void Update()
    {
        leftRight = Input.GetAxis("Horizontal");
        frontBack = Input.GetAxis("Vertical");

        HandleMouseLook();
        CheckGrounded();
        HandleJump();
        HandleMining();
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    // [수정] 채굴 속도 강화가 실제 애니메이션 속도에 반영되도록 수정
    private void HandleMining()
    {
        if (Input.GetMouseButton(0))
        {
            animator.SetBool("isMining", true);
            // 애니메이터의 재생 속도를 강화된 속도로 설정합니다.
            animator.speed = miningSpeedModifier;
        }
        else
        {
            animator.SetBool("isMining", false);
            // 채굴을 멈추면 애니메이터의 재생 속도를 원래대로(1) 되돌립니다.
            animator.speed = 1f;
        }
    }

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

    // --- 나머지 모든 함수는 기존과 동일 ---
    public void AddItem(BlockData blockData, int amount)
    {
        if (inventory.Count >= inventorySize && !inventory.ContainsKey(blockData))
        {
            Debug.Log("가방이 가득 찼습니다!");
            return;
        }
        if (inventory.ContainsKey(blockData)) { inventory[blockData] += amount; }
        else { inventory.Add(blockData, amount); }
        if (inventoryUI != null) { inventoryUI.UpdateInventoryUI(inventory); }
    }

    public int SellAllItems()
    {
        int totalSaleValue = 0;
        foreach (var item in inventory) { totalSaleValue += item.Key.value * item.Value; }
        if (totalSaleValue > 0)
        {
            currentGold += totalSaleValue;
            inventory.Clear();
            if (inventoryUI != null) { inventoryUI.UpdateInventoryUI(inventory); }
        }
        return totalSaleValue;
    }

    public PickaxeData GetNextPickaxeTier()
    {
        for (int i = 0; i < pickaxeTiers.tiers.Count - 1; i++)
        {
            if (pickaxeTiers.tiers[i] == currentPickaxe) { return pickaxeTiers.tiers[i + 1]; }
        }
        return null;
    }

    public void UpgradePickaxe()
    {
        PickaxeData nextTier = GetNextPickaxeTier();
        if (nextTier != null && currentGold >= nextTier.cost)
        {
            currentGold -= nextTier.cost;
            currentPickaxe = nextTier;
        }
    }

    public void UpgradeBag(UpgradeData data)
    {
        int cost = (int)(data.baseCost * Mathf.Pow(data.costIncreaseRate, bagLevel));
        if (currentGold >= cost)
        {
            currentGold -= cost;
            inventorySize += (int)data.valueIncrease;
            bagLevel++;

            // [추가] 가방 강화 후 UI를 즉시 갱신하여 슬롯이 열리는 것을 보여줍니다.
            if (inventoryUI != null)
            {
                inventoryUI.UpdateInventoryUI(inventory);
            }
        }
    }

    public void UpgradeSpeed(UpgradeData data)
    {
        int cost = (int)(data.baseCost * Mathf.Pow(data.costIncreaseRate, speedLevel));
        if (currentGold >= cost)
        {
            currentGold -= cost;
            miningSpeedModifier += data.valueIncrease;
            speedLevel++;
        }
    }

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
