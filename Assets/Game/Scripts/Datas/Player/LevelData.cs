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
        public List<BlockIndexData> GridData;
    }
}
