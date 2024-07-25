using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayAreaManager : MonoBehaviour
{
    public static PlayAreaManager Instance;

    [Header("Triggers and Colliders")]
    public GameObject StartPhysicsTriggerGameObject;
    public GameObject FreezeTriggerGameObject;
    public GameObject DeleteTriggerGameObject;
    public GameObject OverfillTriggerGameObject;

    [Header("Shifting Settings")]
    [SerializeField] private Transform shiftingPlatformTransform;
    [SerializeField] private float shiftDownAmount = 1f;
    [SerializeField] private float shiftDownDuration = 1f;
    [SerializeField] private float shiftDownInterval = 2f;
    private Coroutine shiftCoroutine;

    [Header("Events")]
    public UnityEvent OnLoseGame;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    void Start()
    {
        shiftCoroutine = StartCoroutine(ShiftDownCoroutine());
    }

    void Update()
    {
        
    }

    private IEnumerator ShiftDownCoroutine()
    {
        while (true)
        {
            Vector3 startPos = shiftingPlatformTransform.position;
            Vector3 endPos = shiftingPlatformTransform.position + shiftDownAmount * Vector3.down;

            for(float t = 0; t < shiftDownDuration; t += Time.deltaTime)
            {
                shiftingPlatformTransform.position = Vector3.Lerp(startPos, endPos, t / shiftDownDuration);
                yield return null;
            }
            shiftingPlatformTransform.position = endPos;

            yield return new WaitForSeconds(shiftDownInterval);
        }
    }

    public void ChildBlockToShiftingPlatform(Transform child)
    {
        child.parent = shiftingPlatformTransform;
    }
}
