using UnityEngine;

public class BlockInteractManager : MonoBehaviour
{
    public static BlockInteractManager Instance;

    private LineRenderer lineRenderer;
    
    [Header("Pointers")]
    [SerializeField] private Transform mousePointerTransform;
    [SerializeField] private Transform blockPointerTransform;

    [Header("Block")]
    [SerializeField] private LayerMask blockLayer;
    [SerializeField] private float blockSnapToCursorSpeed = 1f;
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

        draggedBlock.IsWaiting = false;
        draggedBlock.IsBeingDragged = true;
        draggedBlock.Rigidbody.gravityScale = 0;
        draggedBlock.Rigidbody.drag = 0f;
        draggedBlock.Rigidbody.angularDrag = 2f;
        draggedBlock.DragPoint.position = mouseWorldPos;
        draggedBlock.Rigidbody.constraints = RigidbodyConstraints2D.None;

        blockPointerTransform.position = draggedBlock.DragPoint.position;
        blockPointerTransform.gameObject.SetActive(true);

        isDragging = true;
    }

    private void StopDrag()
    {
        draggedBlock.IsBeingDragged = false;
        draggedBlock.Rigidbody.gravityScale = 1;
        draggedBlock = null;

        blockPointerTransform.gameObject.SetActive(false);

        lineRenderer.enabled = false;

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

            draggedBlock.CancelFreezing();

            blockPointerTransform.position = draggedBlock.DragPoint.position;
            DrawLine(mouseWorldPos, draggedBlock.DragPoint.position);

            Vector3 dir = mouseWorldPos - draggedBlock.DragPoint.position;

            Vector3 perimeterPointClosestToDragPoint = draggedBlock.GetComponent<Collider2D>().ClosestPoint(draggedBlock.DragPoint.position);
            Debug.DrawLine(perimeterPointClosestToDragPoint, perimeterPointClosestToDragPoint + dir.normalized * blockSnapToCursorSpeed * Time.deltaTime);
            if(!Physics.Raycast(perimeterPointClosestToDragPoint, dir, 5f * blockSnapToCursorSpeed * Time.deltaTime))
            {
                draggedBlock.Rigidbody.AddForceAtPosition(dir, draggedBlock.DragPoint.position);
                draggedBlock.Rigidbody.MovePosition(Vector3.Lerp(draggedBlock.transform.position, mouseWorldPos, blockSnapToCursorSpeed * Time.deltaTime));
            }
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
