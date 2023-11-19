using System.Collections.Generic;
using ToonBlastClone.ScriptableObjects;
using UnityEngine;

namespace ToonBlastClone.Data
{
    public class CellData
    {
        public bool IsEmpty;
        public BlockSO BlockSO;
        public GameObject CellObject;
        public PlaneBorderData Border;
        public Vector2Int Index;
        public List<CellData> Neighbors = new List<CellData>();

        public void AddNeighbor(CellData cell)
        {
            Neighbors.Add(cell);
        }
    }
}