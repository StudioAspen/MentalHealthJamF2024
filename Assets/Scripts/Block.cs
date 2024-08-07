using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;

public class Block : MonoBehaviour
{
    private ResourceManager resourceManager;

    [HideInInspector] public Rigidbody2D Rigidbody;
    public Transform DragPoint;

    [HideInInspector] public bool IsBeingDragged;
    [HideInInspector] public bool IsFrozen;
    [HideInInspector] public bool HasStartedPhysics;
    [HideInInspector] public bool HasTouchedLava;

    public ResourceType ResourceType;
    private SpriteRenderer spriteRenderer;

    private Coroutine freezeAfterStopCoroutine;

    private void Awake()
    {
        resourceManager = FindObjectOfType<ResourceManager>();

        Rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        ResourceType = (ResourceType)Random.Range(0, 3);
        if (ResourceType == ResourceType.Physical) spriteRenderer.color = Color.red;
        if (ResourceType == ResourceType.Mental) spriteRenderer.color = Color.blue;
        if (ResourceType == ResourceType.Financial) spriteRenderer.color = Color.green;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == PlayAreaManager.Instance.DeleteTriggerGameObject)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        HandleOverfillCollider(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        HandleFrozenBlockTouch(collision);
        HandleLavaTouch(collision);
    }

    private void HandleOverfillCollider(Collider2D collision)
    {
        if (!IsFrozen) return;

        if(collision.gameObject == PlayAreaManager.Instance.OverfillTriggerGameObject)
        {
            PlayAreaManager.Instance.OnBoardFilled?.Invoke();
        }
    }

    private void HandleFrozenBlockTouch(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Block otherBlock))
        {
            if (otherBlock.IsFrozen)
            {
                if (freezeAfterStopCoroutine == null) freezeAfterStopCoroutine = StartCoroutine(FreezeAfterStopCoroutine()); 
            }
        }
    }

    private void HandleLavaTouch(Collision2D collision)
    {
        if (collision.gameObject == PlayAreaManager.Instance.FreezeTriggerGameObject && !HasTouchedLava)
        {
            HasTouchedLava = true;
            freezeAfterStopCoroutine = StartCoroutine(FreezeAfterStopCoroutine());
        }

    }

    private void HandleLose(Collision2D collision)
    {
        if (Rigidbody.velocity.magnitude > 0.15f) return;
        if (!HasStartedPhysics) return;
        if (IsBeingDragged) return;

        if (collision.gameObject == PlayAreaManager.Instance.OverfillTriggerGameObject)
        {
            PlayAreaManager.Instance.OnBoardFilled.Invoke();
        }
    }

    private IEnumerator FreezeAfterStopCoroutine()
    {
        while (Rigidbody.velocity.magnitude > 0.05f)
        {
            yield return null;
        }

        if (IsBeingDragged)
        {
            HasTouchedLava = false;
            yield break;
        }

        IsFrozen = true;
        Rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        PlayAreaManager.Instance.ChildBlockToShiftingPlatform(transform);

        if (ResourceType == ResourceType.Physical) resourceManager.AddPhysical(5);
        if (ResourceType == ResourceType.Mental) resourceManager.AddMental(5);
        if (ResourceType == ResourceType.Financial) resourceManager.AddFinancial(5);

        resourceManager.AddGoal(1);

        Color startColor = spriteRenderer.color;
        for(float t = 0; t < 0.5f; t += Time.deltaTime)
        {
            spriteRenderer.color = Color.Lerp(startColor, Color.gray, t/0.5f);
            yield return null;
        }
        spriteRenderer.color = Color.gray;
    }

    public void CancelFreezing()
    {
        if (freezeAfterStopCoroutine != null) StopCoroutine(freezeAfterStopCoroutine);

        HasTouchedLava = false;
    }
}

public enum ResourceType
{
    Physical,
    Mental,
    Financial
}