using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAreaManager : MonoBehaviour
{
    public static PlayAreaManager Instance;

    public GameObject StartPhysicsTriggerGameObject;
    public GameObject FreezeTriggerGameObject;
    public GameObject DeleteTriggerGameObject;

    [SerializeField] private Transform shiftingPlatformTransform;
    [SerializeField] private float shiftDownAmount = 1f;
    [SerializeField] private float shiftDownDuration = 1f;
    [SerializeField] private float shiftDownInterval = 2f;
    private Coroutine shiftCoroutine;

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
        StartWarnEditors();

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
    private void StartWarnEditors()
    {
        if (StartPhysicsTriggerGameObject == null)
        {
            Debug.LogError("Please assign a Start Physics Trigger Object to the Play Area Manager");
        }

        if (FreezeTriggerGameObject == null)
        {
            Debug.LogError("Please assign a Freeze Trigger Object to the Play Area Manager");
        }

        if (DeleteTriggerGameObject == null)
        {
            Debug.Log("Please assign a Delete Trigger Object to the Play Area Manager");
        }

        if(shiftingPlatformTransform == null)
        {
            Debug.LogError("Please assign a Shifting Platform Transform to the Play Area Manager");
        }
    }
}
