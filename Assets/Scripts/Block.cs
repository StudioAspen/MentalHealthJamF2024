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

    public ResourceType ResourceType;
    private SpriteRenderer spriteRenderer;

    private Coroutine freezeAfterStopCoroutine;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        ResourceType = (ResourceType)Random.Range(0, 3);
        if (ResourceType == ResourceType.Physical) spriteRenderer.color = Color.blue;
        if (ResourceType == ResourceType.Mental) spriteRenderer.color = Color.red;
        if (ResourceType == ResourceType.Financial) spriteRenderer.color = Color.green;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == PlayAreaManager.Instance.DeleteTriggerGameObject)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        HandleFrozenBlockTouch(collision);
        HandleLavaTouch(collision);
    }

    private void HandleFrozenBlockTouch(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Block otherBlock))
        {
            if (otherBlock.IsFrozen)
            {
                Debug.Log("Tooching");
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
        if (IsWaiting) return;
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
            if (Rigidbody.transform.InverseTransformDirection(Rigidbody.velocity).y < 0.05f && Rigidbody.transform.InverseTransformDirection(Rigidbody.velocity).y > -0.05f)
            {
                print("rigid body is nearly stationary.");
                break;
            }

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