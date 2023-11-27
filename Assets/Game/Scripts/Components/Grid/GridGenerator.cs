using System;
using System.Collections.Generic;
using ToonBlastClone.Data;
using ToonBlastClone.Logic;
using ToonBlastClone.ScriptableObjects;
using ToonBlastClone.Structs.Event;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;

namespace ToonBlastClone.Components
{
    public abstract class GridGenerator : MonoBehaviour
    {
        [SerializeField] protected GridSO gridSO;
        [SerializeField] private Grid grid;
        [SerializeField] protected Transform cellParent;
        [SerializeField] protected Transform cellDownLeftPoint;

        protected GridModel<GridGenerator> _gridModel;
        private GameObject _cellPrefab;

        public GridModel<GridGenerator> GridModel => _gridModel;

        public GridSO GridSo => gridSO;

        protected abstract void SetCell();

        public abstract List<BlockIndexData> SetRandomCell(List<GoalBlockData> goalList, out List<GoalBlockData> resultGoalList);

        protected abstract void SetNeigbors();

        public abstract List<CellData> GetSpawnArea(Vector2Int size, out Vector3 position);

        protected virtual void Start()
        {
            OpenGrid();
        }

        private void GetData()
        {
            _gridModel ??= new GridModel<GridGenerator>(this);

            _gridModel?.SetCellAmount(this, gridSO.CellAmount);
            _cellPrefab = gridSO.CellPrefab;
            _gridModel?.SetGridCellSize(this, gridSO.GridCellSize);
            grid.cellSize = _gridModel!.GridCellSize;

            int target = _gridModel.CellAmount.x * _gridModel.CellAmount.y * 2;

            InstantiateObjects(target);
            ClearCell(target);
        }

        private void InstantiateObjects(int target)
        {
            while (cellParent.childCount < target)
            {
                GameObject cellObject = Instantiate(_cellPrefab, cellParent);

                if (cellObject.TryGetComponent(out SpriteRenderer spriteRenderer))
                {
                    _gridModel.AddPoolObjects(this, spriteRenderer);
                }

                cellObject.SetActive(false);
            }
        }

        private void SetModel()
        {
            if (_gridModel.SpriteRendererPool.Count == 0)
            {
                foreach (Transform child in cellParent)
                {
                    child.gameObject.SetActive(false);
                    if (child.TryGetComponent(out SpriteRenderer spriteRenderer))
                    {
                        _gridModel.AddPoolObjects(this, spriteRenderer);
                    }
                }
            }
            else
            {
                foreach (var spriteRenderer in _gridModel.SpriteRendererPool)
                {
                    spriteRenderer.gameObject.SetActive(false);
                }
            }
        }

        protected Vector3 GetWorldToCellsWorldPosition(Vector3 position)
        {
            Vector3Int cellPos = grid.WorldToCell(position);
            return grid.CellToWorld(cellPos);
        }

        private void OpenGrid()
        {
            List<BlockIndexData> gridData = gridSO.GetGridData();
            if (gridData is {Count: > 0})
            {
                GetData();
                SetModel();
                GridModel?.SetCellArray(this, new CellData[GridModel!.CellAmount.x, GridModel!.CellAmount.y]);
                SetCell();
                SetNeigbors();
            }
            else
            {
                RandomGenerate();
            }
        }

        public void Generate(List<BlockIndexData> generatedArray, Vector2Int cellAmount, List<GoalBlockData> goalList, int moveCount = 25)
        {
            gridSO.SetGridData(cellAmount, generatedArray, goalList, moveCount);
            GetData();
            SetModel();
            GridModel?.SetCellArray(this, new CellData[GridModel!.CellAmount.x, GridModel!.CellAmount.y]);
            SetCell();
            SetNeigbors();
            EventManager.TriggerEvent(new LevelUIDataEvent()
            {
                LevelData = gridSO.GetLevelData()
            });

#if UNITY_EDITOR
            if (!Application.isPlaying)
                EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            // AssetDatabase.SaveAssets();
            // AssetDatabase.Refresh();
#endif
        }

        public List<BlockIndexData> RandomGenerate(List<GoalBlockData> goalList = null, int moveCount = 25)
        {
            GetData();
            SetModel();
            GridModel?.SetCellArray(this, new CellData[GridModel!.CellAmount.x, GridModel!.CellAmount.y]);
            List<BlockIndexData> result = SetRandomCell(goalList, out goalList);
            
            gridSO.SetGridData(GridModel!.CellAmount, result, goalList, moveCount);
            SetNeigbors();
            
            EventManager.TriggerEvent(new LevelUIDataEvent()
            {
                LevelData = gridSO.GetLevelData()
            });

#if UNITY_EDITOR
            if (!Application.isPlaying)
                EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            // AssetDatabase.SaveAssets();
            // AssetDatabase.Refresh();
#endif

            return result;
        }
        
        public void ClearCell(int target = 0)
        {
            while (cellParent.childCount > target)
            {
                foreach (Transform child in cellParent)
                {
                    DestroyImmediate(child.gameObject);
                }
            }

            _gridModel?.SetCellArray(this, new CellData[0, 0]);
            _gridModel?.ClearPool(this);
            GC.Collect();
        }
    }
}