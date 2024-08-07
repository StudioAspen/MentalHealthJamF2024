using UnityEngine;

public class BlockInteractManager : MonoBehaviour
{
    public static BlockInteractManager Instance;
    
    [Header("Pointers")]
    [SerializeField] private Transform mousePointerTransform;

    [Header("Block")]
    [SerializeField] private LayerMask blockLayer;
    [SerializeField] private float blockSnapToCursorSpeed = 10f;
    [SerializeField] private float blockRotateSpeed = 5f;
    private float targetBlockRotateAngle;
    private Block hoveredBlock;
    private Block draggedBlock;

    private Vector3 mouseWorldPos;
    private bool isDragging;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    void Update()
    {
        mousePointerTransform.position = mouseWorldPos;

        HandleInput();
        AssignHoveredBlock();

        HandleDrag();
    }

    private void HandleInput()
    {
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        if (Input.GetKey(KeyCode.Mouse0) && hoveredBlock != null && !isDragging)
        {
            if (hoveredBlock.IsFrozen) return;

            StartDrag();
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && isDragging)
        {
            StopDrag();
        }
       
    }

    private void AssignHoveredBlock()
    {
        Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos, blockLayer);

        hoveredBlock = hit == null ? null : hit.gameObject.GetComponent<Block>();
    }

    private void StartDrag()
    {
        draggedBlock = hoveredBlock;

        draggedBlock.IsBeingDragged = true;
        draggedBlock.Rigidbody.gravityScale = 0;
        draggedBlock.Rigidbody.drag = 0f;
        draggedBlock.Rigidbody.angularDrag = 2f;
        draggedBlock.DragPoint.position = mouseWorldPos;
        draggedBlock.Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        targetBlockRotateAngle = draggedBlock.transform.rotation.eulerAngles.z;

        isDragging = true;
    }

    private void StopDrag()
    {
        draggedBlock.IsBeingDragged = false;
        draggedBlock.Rigidbody.gravityScale = 1;
        draggedBlock = null;

        isDragging = false;
    }

    private void HandleDrag()
    {
        if (isDragging)
        {
            if (draggedBlock.IsFrozen)
            {
                StopDrag();
                return;
            }

            HandleRotate();

            draggedBlock.CancelFreezing();

            draggedBlock.Rigidbody.velocity = Vector3.zero;

            if(!Physics.Raycast(draggedBlock.transform.position, mouseWorldPos - draggedBlock.transform.position, blockSnapToCursorSpeed * Time.deltaTime))
            {
                draggedBlock.transform.position = Vector3.Lerp(draggedBlock.transform.position, mouseWorldPos, blockSnapToCursorSpeed * Time.deltaTime);
            }
        }
    }

    private void HandleRotate()
    {
        draggedBlock.transform.rotation = Quaternion.Lerp(draggedBlock.transform.rotation, Quaternion.Euler(0, 0, targetBlockRotateAngle), blockRotateSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            targetBlockRotateAngle += 90f;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            targetBlockRotateAngle -= 90f;
        }
    }
}
