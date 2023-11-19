using System.Collections.Generic;
using ToonBlastClone.Data;
using ToonBlastClone.Structs;
using UnityEngine;

namespace ToonBlastClone.Logic
{
    public class GridModel<T> where T : class
    {
        private CellData[,] _cellArray;
        private Vector2Int _cellAmount;
        private Vector2 _gridCellSize;

        private readonly T _generatedClass;
        private List<SpriteRenderer> _spriteRendererPool = new List<SpriteRenderer>();

        public CellData[,] CellArray => _cellArray;
        public Vector2Int CellAmount => _cellAmount;
        public Vector2 GridCellSize => _gridCellSize;
        public List<SpriteRenderer> SpriteRendererPool => _spriteRendererPool;

        public GridModel(T generatedClass)
        {
            _generatedClass = generatedClass;
        }

        public void SetCellArray(T calledClass, CellData[,] cellArray)
        {
            if (_generatedClass.Equals(calledClass))
                _cellArray = cellArray;
        }

        public void SetCellArrayData(T calledClass, Vector2Int index, CellData cell)
        {
            if (_generatedClass.Equals(calledClass))
                _cellArray[index.x, index.y] = cell;
        }

        public void SetCellAmount(T calledClass, Vector2Int cellAmount)
        {
            if (_generatedClass.Equals(calledClass))
                _cellAmount = cellAmount;
        }

        public void SetGridCellSize(T calledClass, Vector2 gridCellSize)
        {
            if (_generatedClass.Equals(calledClass))
                _gridCellSize = gridCellSize;
        }

        public void AddPoolObjects(T calledClass, SpriteRenderer spriteRenderer)
        {
            if (_generatedClass.Equals(calledClass))
            {
                _spriteRendererPool.Add(spriteRenderer);
            }
        }
        
        public void ClearPool(T calledClass)
        {
            if (_generatedClass.Equals(calledClass))
            {
                _spriteRendererPool.Clear();
            }
        }

        public SpriteRenderer GetSpriteRendererPoolObject()
        {
            for (int i = 0; i < _spriteRendererPool.Count; i++)
            {
                if (!_spriteRendererPool[i].gameObject.activeInHierarchy)
                {
                    return _spriteRendererPool[i];
                }
            }


            return null;
        }

        public CellData GetCell(Vector2Int index)
        {
            return _cellArray[index.x, index.y];
        }

        public CellData TouchedCell(ScreenToWorldPointData data)
        {
            int x = 0;
            int y = 0;

            while (true)
            {
                if (x < 0 || y < 0 || x >= _cellAmount.x || y >= _cellAmount.y)
                    break;
                CellData cell = _cellArray[x, y];

                Vector2Int dist = cell.Border.PlaneIntersect(data);
                if (dist.Equals(Vector2Int.zero)) return cell;

                x += dist.x;
                y += dist.y;
            }

            return null;
        }
        
        public CellData GetCellFromPosition(Vector3 position)
        {
            int x = 0;
            int y = 0;

            while (true)
            {
                if (x < 0 || y < 0 || x >= _cellAmount.x || y >= _cellAmount.y)
                    break;
                CellData cell = _cellArray[x, y];

                Vector2Int dist = cell.Border.IsInside(position);
                if (dist.Equals(Vector2Int.zero)) return cell;

                x += dist.x;
                y += dist.y;
            }

            return null;
        }
    }
}