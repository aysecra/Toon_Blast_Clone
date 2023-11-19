using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToonBlastClone.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Merge Result Elements")]

    public class MergeResultElementsSO : ScriptableObject
    {
        [SerializeField] private List<MergeElementSO> blockList;

        public BlockSO GetMergeResult(int count)
        {
            foreach (var block in blockList)
            {
                if (count >= block.MinAmount && count <= block.MaxAmount)
                {
                    return block;
                }
            }

            return null;
        }
        
    }
}
