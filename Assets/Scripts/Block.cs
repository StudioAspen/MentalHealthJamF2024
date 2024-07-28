using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Block : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D Rigidbody;
    public Transform DragPoint;

    [HideInInspector] public bool IsBeingDragged;
    [HideInInspector] public bool IsWaiting = true;
    [HideInInspector] public bool IsFrozen;
    [HideInInspector] public bool HasStartedPhysics;
    [HideInInspector] public bool HasTouchedLava;

    private BlockCreator blockCreator;
    private bool prevIsWaiting = true;

    public enum ResourceType
    {
        Physical,
        Mental,
        Financial
    }
    public ResourceType Resource;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        InitWithRandomResourceType();

        
    }

    private void Update()
    {
        if(IsWaiting != prevIsWaiting)
        {
            prevIsWaiting = IsWaiting;
            blockCreator.RemoveBlockFromSlot(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == PlayAreaManager.Instance.DeleteTriggerGameObject)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == PlayAreaManager.Instance.StartPhysicsTriggerGameObject)
        {
            gameObject.layer = LayerMask.NameToLayer("PhysicsBlock");
            Rigidbody.constraints = RigidbodyConstraints2D.None;
            HasStartedPhysics = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == PlayAreaManager.Instance.FreezeTriggerGameObject && !HasTouchedLava)
        {
            HasTouchedLava = true;
            StartCoroutine(FreezeAfterStopCoroutine());
        }

        if (Rigidbody.velocity.magnitude > 0.15f) return;
        if (IsWaiting) return;
        if (!HasStartedPhysics) return;
        if (IsBeingDragged) return;

        if (collision.gameObject == PlayAreaManager.Instance.OverfillTriggerGameObject)
        {
            PlayAreaManager.Instance.OnLoseGame.Invoke();
        }
    }

    private IEnumerator FreezeAfterStopCoroutine()
    {
        while(Rigidbody.velocity.magnitude > 0.25f)
        {
            yield return null;
        }

        if(!GetComponent<Collider2D>().IsTouching(PlayAreaManager.Instance.FreezeTriggerGameObject.GetComponent<Collider2D>())) // not touching floor
        {
            HasTouchedLava = false;
            yield break;
        }

        IsFrozen = true;
        Rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        PlayAreaManager.Instance.ChildBlockToShiftingPlatform(transform);

        switch (Resource)
        {
            case ResourceType.Physical:
                ResourceManager.Instance.AddPhysical(5);
                break;
            case ResourceType.Mental:
                ResourceManager.Instance.AddMental(5);
                break;
            case ResourceType.Financial:
                ResourceManager.Instance.AddFinancial(5);
                break;
            default:
                break;
        }

        ResourceManager.Instance.AddGoal(3);
    }

    public void InitWithRandomResourceType()
    {
        int randomIndex = Random.Range(0, 3);

        Resource = (ResourceType)randomIndex;
        AssignColor();
    }

    public void Init(BlockCreator creator)
    {
        blockCreator = creator;
    }

    public void AssignColor()
    {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();

        foreach(SpriteRenderer sprite in sprites)
        {
            if (Resource == ResourceType.Mental) sprite.color = Color.blue;
            if (Resource == ResourceType.Physical) sprite.color = Color.red;
            if (Resource == ResourceType.Financial) sprite.color = Color.green;
        }
    }
}
