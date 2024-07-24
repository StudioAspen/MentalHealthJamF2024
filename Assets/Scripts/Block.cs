using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D Rigidbody;
    public Transform DragPoint;

    public bool IsWaiting = true;
    public bool IsFrozen;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == PlayAreaManager.Instance.StartPhysicsTriggerGameObject)
        {
            Rigidbody.constraints = RigidbodyConstraints2D.None;
        }

        if(collision.gameObject == PlayAreaManager.Instance.FreezeTriggerGameObject)
        {
            IsFrozen = true;
            Rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            PlayAreaManager.Instance.ChildBlockToShiftingPlatform(transform);
        }

        if (collision.gameObject == PlayAreaManager.Instance.DeleteTriggerGameObject)
        {
            Destroy(gameObject);
        }
    }
}
