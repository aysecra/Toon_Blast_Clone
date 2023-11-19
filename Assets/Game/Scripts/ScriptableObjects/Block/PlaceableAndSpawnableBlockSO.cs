using System.Collections.Generic;
using ToonBlastClone.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace ToonBlastClone.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Block/Placeable & Spawnable Blocks")]
    public class PlaceableAndSpawnableBlockSO : ScriptableObject
    {
        [SerializeField] private List<BlockSO> blockList;
        [SerializeField] private int randomSeed;

        public List<BlockSO> BlockList => blockList;

        private bool isBeginRandom = false;
        private int seedIndex = 0;

        public void InitRandom()
        {
            Random.InitState(seedIndex);
        }

        public void ChangeRandomSeed()
        {
            seedIndex++;
        }

        public BlockSO GetRandomBlock()
        {
            if (!isBeginRandom)
            {
                isBeginRandom = true;
            }

            if (blockList.Count > 0)
                return blockList[Random.Range(0, blockList.Count)];
            return null;
        }

        public void SetGridGeneratorForAllGenerator(GridGenerator gridGenerator)
        {
            foreach (var block in blockList)
            {
                block.GridGenerator = gridGenerator;
            }
        }

        public int GetListIndex(BlockSO block)
        {
            for (int i = 0; i < blockList.Count; i++)
            {
                if (blockList[i].IsEqual(block))
                    return i;
            }

            return -1;
        }
    }
}