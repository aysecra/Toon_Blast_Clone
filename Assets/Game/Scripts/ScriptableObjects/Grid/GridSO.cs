using System.Collections.Generic;
using System.Linq;
using ToonBlastClone.Data;
using UnityEngine;

namespace ToonBlastClone.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Grid Element")]
    public class GridSO : ScriptableObject
    {
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private Vector2Int cellAmount = Vector2Int.one;
        [SerializeField] private Vector2 gridCellSize = Vector2.one;
        [SerializeField] private GameObject cellPrefab;
        private LevelData _levelData;
        private GameData _gameData;

        public Vector2Int CellAmount => cellAmount;

        public GameObject CellPrefab => cellPrefab;

        public Vector2 GridCellSize => gridCellSize;

        public void SetCellAmount(Vector2Int amount)
        {
            cellAmount = amount;

            if (_levelData == null)
            {
                LoadData();
            }

            bool isContain = false;
            for (int i = 0; i < _gameData.LevelDataList.Count; i++)
            {
                if (_gameData.LevelDataList[i].GridSoName.Equals(this.name))
                {
                    _gameData.LevelDataList[i].CellAmount = cellAmount;
                    _gameData.LevelDataList[i].GridData = new List<BlockIndexData>();
                }
            }

            if (!isContain)
            {
                _levelData = new LevelData()
                {
                    CellAmount = cellAmount,
                    GridData = new List<BlockIndexData>(),
                    GridSoName = this.name
                };

                _gameData.LevelDataList ??= new List<LevelData>();
                
                _gameData.LevelDataList.Add(_levelData);
                SaveData();
            }

            LoadData();
        }

        public void SetGridData(List<BlockIndexData> gridData)
        {
            if (_levelData == null)
            {
                LoadData();
            }
            
            bool isContain = false;
            for (int i = 0; i < _gameData.LevelDataList.Count; i++)
            {
                if (_gameData.LevelDataList[i].GridSoName.Equals(this.name))
                {
                    _gameData.LevelDataList[i].GridData = gridData;
                }
            }

            if (!isContain)
            {
                _levelData = new LevelData()
                {
                    CellAmount = cellAmount,
                    GridData = gridData,
                    GridSoName = this.name
                };

                _gameData.LevelDataList ??= new List<LevelData>();
                
                _gameData.LevelDataList.Add(_levelData);
                SaveData();
            }

            LoadData();
        }

        public List<BlockIndexData> GetGridData()
        {
            if (_levelData == null)
                LoadData();
            return _levelData.GridData;
        }

        private void SaveData()
        {
            gameSettings.SaveToJSON(_gameData);
        }

        private void LoadData()
        {
            GameData gameData = gameSettings.LoadFromJSON<GameData>();

            if (gameData == default)
            {
                _gameData = new GameData()
                {
                    LevelDataList = new List<LevelData>()
                };
                _levelData = new LevelData()
                {
                    CellAmount = cellAmount,
                    GridData = new List<BlockIndexData>(),
                    GridSoName = this.name
                };

                _gameData.LevelDataList.Add(_levelData);
                SaveData();
            }

            bool isContain = false;
            foreach (var levelData in gameData.LevelDataList)
            {
                if (levelData.GridSoName.Equals(this.name))
                {
                    _levelData = levelData;
                    isContain = true;
                    break;
                }
            }

            if (!isContain)
            {
                _levelData = new LevelData()
                {
                    CellAmount = cellAmount,
                    GridData = new List<BlockIndexData>(),
                    GridSoName = this.name
                };

                _gameData.LevelDataList ??= new List<LevelData>();
                
                _gameData.LevelDataList.Add(_levelData);
                SaveData();
            }
        }

        private void SaveSO()
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}