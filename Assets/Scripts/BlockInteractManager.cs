using UnityEngine;

public class BlockInteractManager : MonoBehaviour
{
    private LineRenderer lineRenderer;
    
    [Header("Pointers")]
    [SerializeField] private Transform mousePointerTransform;
    [SerializeField] private Transform blockPointerTransform;

    [Header("Block")]
    [SerializeField] private LayerMask blockLayer;
    private Block hoveredBlock;
    private Block draggedBlock;

    private Vector3 mouseWorldPos;
    private bool isDragging;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        mousePointerTransform.position = mouseWorldPos;

        HandleInput();
        AssignHoveredBlock();
    }

    private void FixedUpdate()
    {
        HandleDrag();
    }

    private void HandleInput()
    {
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        if (Input.GetKey(KeyCode.Mouse0) && hoveredBlock != null && !isDragging)
        {
            StartDrag();
            isDragging = true;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && isDragging)
        {
            StopDrag();
            isDragging = false;
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
        draggedBlock.Rigidbody.gravityScale = 0;
        draggedBlock.DragPoint.position = mouseWorldPos;

        blockPointerTransform.position = draggedBlock.DragPoint.position;
        blockPointerTransform.gameObject.SetActive(true);
    }

    private void StopDrag()
    {
        draggedBlock.Rigidbody.gravityScale = 1;
        draggedBlock = null;

        blockPointerTransform.gameObject.SetActive(false);

        lineRenderer.enabled = false;
    }

    private void HandleDrag()
    {
        if (isDragging)
        {
            blockPointerTransform.position = draggedBlock.DragPoint.position;
            DrawLine(mouseWorldPos, draggedBlock.DragPoint.position);

            Vector3 dir = mouseWorldPos - draggedBlock.DragPoint.position;
            draggedBlock.Rigidbody.AddForceAtPosition(dir, draggedBlock.DragPoint.position);
        }
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        lineRenderer.enabled = true;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
