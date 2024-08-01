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

    [Header("Blocks Spawn Settings")]
    [SerializeField] private Block[] gameBlockPrefabs;
    [SerializeField] private Transform spawnAreaStartTransform;
    [SerializeField] private Transform spawnAreaStopTransform;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float spawnStartForce = 1f;
    private Coroutine spawnBlocksCoroutine;

    [Header("Events")]
    public UnityEvent OnBoardFilled = new UnityEvent();

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
        spawnBlocksCoroutine = StartCoroutine(SpawnBlocksCoroutine());
    }

    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(spawnAreaStartTransform.position, spawnAreaStopTransform.position);
    }

    private IEnumerator SpawnBlocksCoroutine()
    {
        while (true)
        {
            Vector3 dirBetweenBounds = spawnAreaStopTransform.position - spawnAreaStartTransform.position;

            float randomPercent = Random.Range(0f, 1f);
            Vector3 randomPosBetweenBounds = spawnAreaStartTransform.position + randomPercent * dirBetweenBounds;

            float randomStartAngle = Random.Range(0, 4) * 90f;
            Quaternion randomStartRotation = Quaternion.Euler(0, 0, randomStartAngle);

            int randomIndex = Random.Range(0, gameBlockPrefabs.Length);
            Block spawnedBlock = Instantiate(gameBlockPrefabs[randomIndex], randomPosBetweenBounds, randomStartRotation);

            float randomShootAngle = Random.Range(-45f, 45f);
            Quaternion randomShootRotation = Quaternion.Euler(0, 0, randomShootAngle);
            Vector3 randomShootDirection = randomShootRotation * Vector2.down;

            spawnedBlock.Rigidbody.velocity += spawnStartForce * (Vector2)randomShootDirection;

            yield return new WaitForSeconds(spawnInterval);
        }
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
