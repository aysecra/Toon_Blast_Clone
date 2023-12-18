using System;
using ToonBlastClone.ScriptableObjects;
using UnityEngine;

namespace ToonBlastClone.Data
{
    [Serializable]
    public class GoalBlockData
    {
        [SerializeField] public int Index;
        [SerializeField] public BlockSO BlockType;
        [SerializeField] public int BlockCount;
    }
}
