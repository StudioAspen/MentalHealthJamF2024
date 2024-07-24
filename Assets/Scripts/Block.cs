using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D Rigidbody;
    public Transform DragPoint;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }
}
