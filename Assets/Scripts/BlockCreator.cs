using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCreator : MonoBehaviour
{
    [SerializeField] private List<Block> gameBlocks = new List<Block>();
    [SerializeField] private List<Transform> slotPositions = new List<Transform>();
    private List<Block> slotBlocks = new List<Block>();

    [SerializeField] private float blockAddInterval = 3f;
    private List<Block> blockQueue = new List<Block>();

    private void Awake()
    {
        
    }

    private void Start()
    {
        foreach(var pos in slotPositions)
        {
            slotBlocks.Add(null);
        }

        FillSlots();

        StartCoroutine(BlockQueueCoroutine());
    }

    private void Update()
    {
        if(blockQueue.Count > 0)
        {
            for(int i = 0; i < slotBlocks.Count; i++)
            {
                if (slotBlocks[i] != null) continue;

                AddNextBlockToSlot(i);
            }
        }
    }

    private void AddRandomBlockToSlot(int index)
    {
        if (slotBlocks[index] != null)
        {
            Debug.Log($"Slot {index} already occupied");
            return;
        }

        int randomIndex = Random.Range(0, gameBlocks.Count);
        slotBlocks[index] = Instantiate(gameBlocks[randomIndex], slotPositions[index].position, Quaternion.identity);
        slotBlocks[index].Init(this);
    }

    private void AddNextBlockToSlot(int index)
    {
        if (blockQueue.Count == 0) return;

        Block block = blockQueue[0];

        StartCoroutine(DelayedAddBlockToSlot(block, index));
    }

    private IEnumerator DelayedAddBlockToSlot(Block block, int index)
    {
        blockQueue.Remove(block);
        slotBlocks[index] = gameBlocks[0];

        yield return new WaitForSeconds(1.5f);

        slotBlocks[index] = Instantiate(block, slotPositions[index].position, Quaternion.identity);
        slotBlocks[index].Init(this);
    }

    private Block GenerateRandomBlock()
    {
        int randomIndex = Random.Range(0, gameBlocks.Count);
        return gameBlocks[randomIndex];
    }

    private void FillSlots()
    {
        for(int i = 0; i < slotPositions.Count; i++)
        {
            AddRandomBlockToSlot(i);
        }
    }

    public void RemoveBlockFromSlot(Block block)
    {
        slotBlocks[slotBlocks.IndexOf(block)] = null;
    }

    private IEnumerator BlockQueueCoroutine()
    {
        while (true)
        {
            blockQueue.Add(GenerateRandomBlock());

            yield return new WaitForSeconds(blockAddInterval);
        }
    }
}
