using System;
using System.Collections.Generic;
using UnityEngine;

namespace ToonBlastClone.Data
{
    [Serializable]
    public class LevelData
    {
        public string GridSoName;
        public Vector2Int CellAmount;
        public int MoveCount;
        public List<GoalBlockData> Goals;
        public List<BlockIndexData> GridData;
    }
}
